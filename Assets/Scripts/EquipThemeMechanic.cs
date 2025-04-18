using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipThemeMechanic : MonoBehaviour{

    [SerializeField] ThemesSwipeMenu themeSwipeMenu = default;
    [SerializeField] ThemeMenuMechanics themeMenu = default;
    private int themeIndex;

    public void EquipOnClick() {
        Debug.Log("Equip Button Clicked");
        GetThemeIndex();
        PlayClickSound();
        UpdateThemesGameData();
        UpdateThemeManager();
        GameDataControl.gdControl.SavePlayerData();
        themeMenu.UpdateThemeMenu();
        themeSwipeMenu.UpdateThemePreviewAndMasterButton();
    }

    private void UpdateThemeManager() {
        ThemeManager.TM.UpdateActiveTheme(themeIndex);
    }

    private void GetThemeIndex() {
        themeIndex = themeSwipeMenu.index;
    }

    private void PlayClickSound() {
        FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
    }

    private void UpdateThemesGameData() {
        Debug.Log("Equipping Theme "+ themeIndex);
        //make all owned themes 1 or owned status
        for(int i = 0; i< GameDataControl.gdControl.themes.Count; i++) {
            if(GameDataControl.gdControl.themes[i] == 2) {
                GameDataControl.gdControl.themes[i] = 1;
            }
        }
        //after making all owned swap index to equipped
        GameDataControl.gdControl.themes[themeIndex] = 2;
    }
}
