using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoWin : MonoBehaviour {

    [SerializeField] CreateBoard gameboard = default;

    public void AutoWinOnClick() {
        gameboard.NormalWin();
        gameObject.GetComponent<Button>().interactable = false;
        gameObject.GetComponent<Image>().raycastTarget = false;
    }
    
}
