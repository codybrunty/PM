using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeButtonMechanic : MonoBehaviour{

    [SerializeField] GameObject hand = default;
    public int buttonIndex = 0;

    public void SwipeButtonOnClick() {
        int firstTimeThemeMenu = PlayerPrefs.GetInt("FirstTimeThemeMenu",0);

        if(firstTimeThemeMenu == 0) {
            PlayerPrefs.SetInt("FirstTimeThemeMenu", 1);
            hand.SetActive(false);
        }

        gameObject.transform.GetComponentInParent<ThemesSwipeMenu>().MoveSwipeMenu(buttonIndex);
    }

}
