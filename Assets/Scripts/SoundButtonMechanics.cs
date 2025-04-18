using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundButtonMechanics : MonoBehaviour {

    public int soundOn;
    [SerializeField] Sprite soundOnImage = default;
    [SerializeField] Sprite soundOffImage = default;

    private void Awake() {
        soundOn = PlayerPrefs.GetInt("UserSoundOn", 1);
    }

    private void Start() {
        SetSoundImages();
    }

    public void SoundButtonOnClick() {
        if (soundOn == 1) {
            TurnOffSound();
        }
        else {
            TurnOnSound();
        }
        SetSoundImages();
    }

    private void TurnOnSound() {
        Debug.Log("SoundOn");
        PlayerPrefs.SetInt("UserSoundOn", 1);
        soundOn = PlayerPrefs.GetInt("UserSoundOn", 1);
        PlayClickSound();
        FindObjectOfType<SoundManager>().StartGameMusic();
    }

    private void TurnOffSound() {
        Debug.Log("SoundOff");
        PlayerPrefs.SetInt("UserSoundOn", 0);
        soundOn = PlayerPrefs.GetInt("UserSoundOn", 1);
        FindObjectOfType<SoundManager>().StopGameMusic();
    }

    private void PlayClickSound() {
        FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
    }

    private void SetSoundImages() {
        if (soundOn == 1) {
            gameObject.GetComponent<Image>().sprite = soundOnImage;
        }
        else {
            gameObject.GetComponent<Image>().sprite = soundOffImage;
        }
    }
}
