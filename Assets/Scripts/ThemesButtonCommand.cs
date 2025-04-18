using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemesButtonCommand : MonoBehaviour{

    [SerializeField] GameObject themesMenu = default;
    [SerializeField] Button themesButton = default;
    [SerializeField] Image BG_Fade_IMG = default;
    [SerializeField] GameObject AlertSign = default;
    [SerializeField] GameObject AlertEffect = default;
    [SerializeField] GameObject profileSparks = default;
    private bool isMoving = false;

    public void ThemesOnClick() {
        if (!isMoving) {
            isMoving = true;
            PlayClickSound();
            themesButton.interactable = false;
            BG_Fade_IMG.gameObject.SetActive(true);
            StartCoroutine(FadeTo(75 / 255f, 0.25f));
            themesMenu.GetComponent<Animator>().SetBool("ThemesDropDown", true);
            AlertOff();
            SparksOff();
        }
    }

    public void Themes_Cancel() {
        if (!isMoving) {
            isMoving = true;
            PlayClickSound();
            StartCoroutine(FadeTo(0 / 255f, 0.25f));
            themesMenu.GetComponent<Animator>().SetBool("ThemesDropDown", false);
            AlertOn();
            SparksOn();
        }
    }

    private void AlertOn() {
        AlertSign.SetActive(true);

        if (AlertEffect.activeInHierarchy) {
            StartCoroutine(HoldPlaySFX());
        }
    }

    private IEnumerator HoldPlaySFX() {
        yield return new WaitForSeconds(.3f);
        FindObjectOfType<SoundManager>().PlaySound("Alert");
    }

    private void AlertOff() {
        AlertSign.SetActive(false);
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

    private void PlayClickSound() {
        FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
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
            themesButton.interactable = true;
        }
        isMoving = false;
    }

}
