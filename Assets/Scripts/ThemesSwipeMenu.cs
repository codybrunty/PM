using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ThemesSwipeMenu : MonoBehaviour{
    public GameObject scrollbar;
    float scroll_pos = 0;
    float[] pos;
    public int index = 0;
    [SerializeField] ThemeMenuMechanics themesMenu = default;
    [SerializeField] Button buyButton = default;
    [SerializeField] Button equipButton = default;
    [SerializeField] ThemePreviewMechanics themePreview = default;
    [SerializeField] GameObject LeftArrow = default;
    [SerializeField] GameObject RightArrow = default;
    [SerializeField] GameObject hand = default;
    public bool moving = false;

    private void Start() {
        DisableAndEnableArrows();
    }

    private void Update() {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++) {
            pos[i] = distance * i;
        }
        //if arrows or clicking is moving it then mouse cant mess it up
        if (moving == false) {
            if (Input.touchCount > 0) {
                //if (Input.GetTouch(0).phase.Equals(TouchPhase.Moved)) {
                    scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
                //}
            }
            else {
                for (int i = 0; i < pos.Length; i++) {
                    if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                        scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                    }
                }
            }
        }
        else {
            for (int i = 0; i < pos.Length; i++) {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < pos.Length; i++) {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.3f);

                if (index != i) {
                    SetSwipeIndex(i);
                }

                for (int j = 0; j < pos.Length; j++) {
                    if (j != i) {
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.7f, 0.7f), 0.3f);
                    }
                }
            }
        }
    }

    public void DisableAndEnableArrows() {
        if (index == 0) {
            LeftArrow.GetComponent<Button>().interactable = false;
            LeftArrow.GetComponent<Image>().raycastTarget = false;
            RightArrow.GetComponent<Button>().interactable = true;
            RightArrow.GetComponent<Image>().raycastTarget = true;
        }
        else if(index == pos.Length-1) {
            LeftArrow.GetComponent<Button>().interactable = true;
            LeftArrow.GetComponent<Image>().raycastTarget = true;
            RightArrow.GetComponent<Button>().interactable = false;
            RightArrow.GetComponent<Image>().raycastTarget = false;
        }
        else {
            LeftArrow.GetComponent<Button>().interactable = true;
            LeftArrow.GetComponent<Image>().raycastTarget = true;
            RightArrow.GetComponent<Button>().interactable = true;
            RightArrow.GetComponent<Image>().raycastTarget = true;
        }
    } 

    public void RightArrowOnClick() {
        if (index != pos.Length - 1 && moving == false) {
            moving = true;
            StartCoroutine(MovingHold());
            scroll_pos = (float)((index + 1.0f) / (themesMenu.themeSwipeButtons.Count - 1.0f));
        }
    }

    public void LeftArrowOnClick() {
        if (index != 0 && moving == false) {
            moving = true;
            StartCoroutine(MovingHold());
            scroll_pos = (float)((index - 1.0f) / (themesMenu.themeSwipeButtons.Count - 1.0f));
        }
    }

    public void SwipeThemeMenuToSomethingPurchaseable() {
        Debug.Log("Swiping Themes Menu to something Purchasable");
        int index = 0;
        for (int i =0; i< themesMenu.themeSwipeButtons.Count; i++) {
            if (GameDataControl.gdControl.themes[i] == 0) {
                index = i;
                break;
            }
        }
        if (index == 0) {
            Debug.Log("Nothing Purchasable swiping to Current Theme");
            for (int i = 0; i < themesMenu.themeSwipeButtons.Count; i++) {
                if (GameDataControl.gdControl.themes[i] == 2) {
                    index = i;
                    break;
                }
            }
        }
        MoveSwipeMenu(index);
        
    }

    public void MoveSwipeMenu(int newIndex) {
        if (index != newIndex && moving == false) {
            moving = true;
            StartCoroutine(MovingHold());
            scroll_pos = (float)((newIndex) / (themesMenu.themeSwipeButtons.Count - 1.0f));
        }
    }

    IEnumerator MovingHold() {
        yield return new WaitForSeconds(0.5f);
        moving = false;
    }

    private void SetSwipeIndex(int i) {
        index = i;
        CheckFirstTimeInMenu();
        DisableAndEnableArrows();
        UpdateThemePreviewAndMasterButton();
    }

    private void CheckFirstTimeInMenu() {
        int firstTimeThemeMenu = PlayerPrefs.GetInt("FirstTimeThemeMenu", 0);
        if (index !=0 && firstTimeThemeMenu == 0) {
            PlayerPrefs.SetInt("FirstTimeThemeMenu", 1);
            hand.SetActive(false);
        }
    }

    public void UpdateThemePreviewAndMasterButton() {
        UpdateThemesMenuPreview();
        UpdateThemesMenuMasterButton();
    }

    private void UpdateThemesMenuMasterButton() {
        Debug.Log("Updating Master Button");
        //0 = buy, 1 = owned, 2 = selected
        int status = themesMenu.themes[index].status;
        Debug.Log(status);
        if (status == 0) {
            buyButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(false);
            buyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = themesMenu.themes[index].price.ToString();
        }
        else if (status == 1) {
            buyButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(true);
            equipButton.interactable = true;
            equipButton.GetComponent<Image>().raycastTarget = true;
            equipButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalisationSystem.GetLocalisedValue("themesMenu_equip");
        }
        else {
            buyButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(true);
            equipButton.interactable = false;
            equipButton.GetComponent<Image>().raycastTarget = false;
            equipButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalisationSystem.GetLocalisedValue("themesMenu_equipped");
        }
    }

    private void UpdateThemesMenuPreview() {
        Debug.Log("Updating Preview");
        themePreview.UpdateThemePreview(index);
    }
}
