using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TitleMechanics : MonoBehaviour {

    [SerializeField] TextMeshProUGUI titleText = default;
    [SerializeField] Button leftArrow = default;
    [SerializeField] Button rightArrow = default;
    private List<string> titleTexts = new List<string> { "menuTitle_profile", "menuTitle_campaign", "menuTitle_bonus" };


    public void RightArrowOnClick() {
        Debug.Log("right arrow click");
        GameObject swipeScrollCTRL = transform.parent.GetChild(1).gameObject;
        swipeScrollCTRL.GetComponent<ScrollBarFix>().FlickRight();
    }

    public void LeftArrowOnClick() {
        Debug.Log("left arrow click");
        GameObject swipeScrollCTRL = transform.parent.GetChild(1).gameObject;
        swipeScrollCTRL.GetComponent<ScrollBarFix>().FlickLeft();
    }

    private void DisableRightArrow() {
        rightArrow.interactable = false;
        rightArrow.gameObject.GetComponent<Image>().raycastTarget = false;
    }

    private void EnableRightArrow() {
        rightArrow.interactable = true;
        rightArrow.gameObject.GetComponent<Image>().raycastTarget = true;
    }

    private void DisableLeftArrow() {
        leftArrow.interactable = false;
        leftArrow.gameObject.GetComponent<Image>().raycastTarget = false;
    }

    private void EnableLeftArrow() {
        leftArrow.interactable = true;
        leftArrow.gameObject.GetComponent<Image>().raycastTarget = true;
    }

    public void UpdateTitle(int index) {
        Debug.Log("titleChange");

        if (PlayerPrefs.HasKey("USERNAME") && index == 0) {
            titleText.text = PlayerPrefs.GetString("USERNAME");
        }

        else{
            titleText.text = LocalisationSystem.GetLocalisedValue(titleTexts[index]);
        }


        UpdateArrows(index);
    }

    private void UpdateArrows(int index) {
        if (index == 0) {
            DisableLeftArrow();
        }
        if (index == 1) {
            EnableLeftArrow();
            EnableRightArrow();
        }
        if (index == 2) {
            DisableRightArrow();
        }

    }
}
