using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyThemeMechanic : MonoBehaviour{

    [SerializeField] ThemesSwipeMenu themeSwipeMenu = default;
    [SerializeField] ThemeMenuMechanics themeMenu = default;
    [SerializeField] CoinTotal bank = default;
    [SerializeField] ThemesButtonCommand themeButton = default;
    [SerializeField] StoreButtonCommand storeButton = default;
    private int themeIndex;


    public void BuyThemeOnClick() {
        Debug.Log("Buy Button Clicked");
        GetThemeIndex();

        int price = themeMenu.themes[themeIndex].price;

        if (GameDataControl.gdControl.coinsTotal >= price) {
            Debug.Log("You have enough coins.");
            PlayClickSound();
            RemoveCoinsFromTotal(price);
            BuyThemeAndEquip();
        }
        else {
            Debug.Log("Not enough coins");
            NegativeFeedBack();
        }

    }

    private void PlayClickSound() {
        FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
    }

    private void NegativeFeedBack() {
        FindObjectOfType<BlinkingText>().BlinkLimit(3);
        themeButton.Themes_Cancel();
        storeButton.StoreOnClick();
    }

    private void RemoveCoinsFromTotal(int price) {
        Debug.Log("Removing " + price+ " Coins");
        GameDataControl.gdControl.coinsSpent += price;
        GameDataControl.gdControl.coinsTotal -= price;
        bank.SubtractFromBank(price);

    }

    private void GetThemeIndex() {
        themeIndex = themeSwipeMenu.index;
    }


    private void BuyThemeAndEquip() {
        UpdateThemesGameDataAfterBuy();
        UpdateThemeManager_Buy();
        GameDataControl.gdControl.SavePlayerData();
        themeMenu.UpdateThemeMenu();
        themeSwipeMenu.UpdateThemePreviewAndMasterButton();
    }

    private void UpdateThemeManager_Buy() {
        ThemeManager.TM.UpdateActiveTheme(themeIndex);
    }

    private void UpdateThemesGameDataAfterBuy() {
        
        Debug.Log("Buying and Equiping Theme " + themeIndex);

        //make index owned
        GameDataControl.gdControl.themes[themeIndex] = 1;
        //make all owned themes 1 or owned status
        for (int i = 0; i < GameDataControl.gdControl.themes.Count; i++) {
            if (GameDataControl.gdControl.themes[i] == 2) {
                GameDataControl.gdControl.themes[i] = 1;
            }
        }
        //after making all owned swap index to equipped
        GameDataControl.gdControl.themes[themeIndex] = 2;
    }


}
