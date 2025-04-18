using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TurnDebugOnAndOff : MonoBehaviour{
    public int DebugOn = 0;

    private void Awake() {
        DebugOn = PlayerPrefs.GetInt("DebugOn", 0);
    }

    private void Start() {
        SetDebugButtonColor();
    }

    private void SetDebugButtonColor() {
        if (DebugOn == 1) {
            SwitchButtonColorGreen();
        }
        else {
            SwitchButtonColorRed();
        }
    }

    private void SwitchButtonColorGreen() {
        gameObject.GetComponent<Image>().color = new Color(0f, 255f / 255f, 0f, 1f);
    }

    private void SwitchButtonColorRed() {
        gameObject.GetComponent<Image>().color = new Color(255f / 255f, 0f, 0f, 1f);
    }

    public void SwitchDebugOnOrOff() {
        if (DebugOn == 1) {
            TurnDebugOff();
        }
        else {
            TurnDebugOn();
        }
        SetDebugButtonColor();
    }

    private void TurnDebugOff() {
        DebugOn = 0;
        PlayerPrefs.SetInt("DebugOn", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void TurnDebugOn() {
        DebugOn = 1;
        PlayerPrefs.SetInt("DebugOn", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
