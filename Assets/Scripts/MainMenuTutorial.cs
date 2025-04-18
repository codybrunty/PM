using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuTutorial : MonoBehaviour{

    [SerializeField] GameObject mainMenuTutorialBG = default;
    [SerializeField] GameObject handImage1 = default;
    [SerializeField] GameObject handImage2 = default;
    [SerializeField] CoinTotal bank = default;
    public int blockPrice = 75;
    [SerializeField] ExpandableButton expand = default;
    [SerializeField] ScrollBarFix scrollCTRL = default;
    [SerializeField] ScrollBarFix swipeCTRL = default;
    [SerializeField] Scrollbar scroll = default;

    private void Start() {
        int tutorialCheck = PlayerPrefs.GetInt("MainMenuTutorial",0);
        if (tutorialCheck == 0) {
            MoneyCheck();
            SetScrollBarToTop();
            DisableScrollingAndSwiping();
            DisableExpandButton();
            StartMainMenuTutorial();
        }
    }

    private void SetScrollBarToTop() {
        scroll.value = 1.0f;
    }

    private void DisableExpandButton() {
        expand.disableExpand = true;
    }

    private void DisableScrollingAndSwiping() {
        scrollCTRL.vertical = false;
        swipeCTRL.horizontal = false;
    }

    private void MoneyCheck() {
        int totalCoins = GameDataControl.gdControl.coinsTotal;

        if (totalCoins < blockPrice) {
            int difference = blockPrice - totalCoins;
            AddDifferenceToTotalCoins(difference);
        }
    }

    private void AddDifferenceToTotalCoins(int difference) {
        GameDataControl.gdControl.AddCoins(difference);
        GameDataControl.gdControl.SavePlayerData();
        bank.UpdateCoinsTotalText();
    }

    private void StartMainMenuTutorial() {
        Canvas newCanvas = gameObject.AddComponent<Canvas>();
        newCanvas.overrideSorting = true;
        newCanvas.sortingLayerName = "OverPopups";
        gameObject.AddComponent<GraphicRaycaster>();
        mainMenuTutorialBG.SetActive(true);
        handImage1.SetActive(true);
    }

    public void BuyConfirm() {
        PlayerPrefs.SetInt("MainMenuTutorial", 1);
        ExpandLevelBlock();
        handImage1.SetActive(false);
        handImage2.SetActive(true);
    }
    private void ExpandLevelBlock() {
        expand.disableExpand = false;
        expand.OnExpandClick();
        expand.disableExpand = true;
    }
}
