using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsCommand : MonoBehaviour {
    [SerializeField] GameObject settingsMenu = default;
    [SerializeField] GameObject languageMenu = default;
    [SerializeField] GameObject creditsMenu = default;
    [SerializeField] Button settingsButton = default;
    [SerializeField] Image BG_Fade_IMG = default;
    private bool isMoving = false;
    [SerializeField] GameObject AlertSign = default;
    [SerializeField] GameObject AlertEffect = default;
    [SerializeField] GameObject profileSparks = default;

    public void SettingsOnClick() {
        if(!isMoving) {
            isMoving = true;
            PlayClickSound();
            settingsMenu.GetComponent<SettingsPopUp>().visible = true;
            settingsButton.interactable = false;
            BG_Fade_IMG.gameObject.SetActive(true);
            StartCoroutine(FadeTo(75 / 255f, 0.25f));
            settingsMenu.GetComponent<Animator>().SetBool("SettingsDropDown", true);
            AlertOff();
            SparksOff();
        }
    }

    private void SparksOff() {
        if (profileSparks != null) {
            profileSparks.SetActive(false);
        }
    }
    private void SparksOn() {
        if (profileSparks != null) {
            if (Application.internetReachability != NetworkReachability.NotReachable) {
                if (!PlayerPrefs.HasKey("USERNAME")) {
                    profileSparks.SetActive(true);
                }
            }
        }
    }

    private void AlertOn() {
        if (AlertSign != null) {
            AlertSign.SetActive(true);
            if (AlertEffect.activeInHierarchy) {
                StartCoroutine(HoldPlaySFX());
            }
        }
    }

    private IEnumerator HoldPlaySFX() {
        yield return new WaitForSeconds(.3f);
        FindObjectOfType<SoundManager>().PlaySound("Alert");
    }

    private void AlertOff() {
        if (AlertSign != null) {
            AlertSign.SetActive(false);
        }
    }

    private void PlayClickSound() {
        //SoundManager.PlaySound("selectSFX1");
        FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
    }

    public void Settings_Cancel() {
        if (!isMoving) {
            isMoving = true;
            PlayClickSound();
            settingsMenu.GetComponent<SettingsPopUp>().visible = false;
            //settingsButton.interactable = true;
            StartCoroutine(FadeTo(0 / 255f, 0.25f));
            settingsMenu.GetComponent<Animator>().SetBool("SettingsDropDown", false);

            if (languageMenu.GetComponent<Animator>().GetBool("LanguageDropDown")) {
                languageMenu.GetComponent<Animator>().SetBool("LanguageDropDown", false);
            }
            if (creditsMenu.GetComponent<Animator>().GetBool("CreditsDropDown")) {
                creditsMenu.GetComponent<Animator>().SetBool("CreditsDropDown", false);
            }
            AlertOn();
            SparksOn();
        }
    }


    IEnumerator FadeTo(float aValue, float aTime) {
        float alpha = BG_Fade_IMG.color.a;
        bool off = false;
        if (aValue == 0) {
            off = true;
        }
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha, aValue, t));
            BG_Fade_IMG.color = newColor;
            yield return null;
        }

        if (off) {
            BG_Fade_IMG.gameObject.SetActive(false);
            settingsButton.interactable = true;
        }
        isMoving = false;
    }
}
