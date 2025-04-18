using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThemeMenuMechanics : MonoBehaviour{

    public List<Theme> themes = new List<Theme>();
    public List<Button> themeSwipeButtons = new List<Button>();
    [SerializeField] ThemesSwipeMenu themesSwipeMenu = default;

    private void Start() {
        UpdateThemeMenu();

    }

    public void UpdateThemeMenu() {
        Debug.Log("Updating Theme Menu");
        GetThemesStatusFromGameData();
        SetSwipeButtonDisplays();
        themesSwipeMenu.UpdateThemePreviewAndMasterButton();
    }

    private void GetThemesStatusFromGameData() {
        //updating the status of our themes in our themes menu with our gamedata of theme statuses 
        //only for the number of active buttons
        for (int i = 0; i < themeSwipeButtons.Count; i++) {
            themes[i].status = GameDataControl.gdControl.themes[i];
            //Debug.Log(i + " theme status " + GameDataControl.gdControl.themes[i]);
        }
    }

    private void SetSwipeButtonDisplays() {
        for (int i = 0; i < themeSwipeButtons.Count; i++) {
            //0 = buy, 1 = owned, 2 = selected
            int status = themes[i].status;
            if (status == 0) {
                themeSwipeButtons[i].GetComponent<Image>().sprite = themes[i].sprite_buy;
                //set price text
                themeSwipeButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = themes[i].price.ToString();
            }
            else if (status == 1) {
                themeSwipeButtons[i].GetComponent<Image>().sprite = themes[i].sprite_owned;
                //hide price text
                themeSwipeButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            else {
                themeSwipeButtons[i].GetComponent<Image>().sprite = themes[i].sprite_selected;
                //hide price text
                themeSwipeButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            
        }
    }


}


