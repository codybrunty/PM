using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileButtonCommand : MonoBehaviour{

    [SerializeField] GameObject profileMenu = default;
    [SerializeField] Button profileButton = default;
    private bool isMoving = false;
    [SerializeField] Image BG_Fade_IMG = default;
    [SerializeField] GameObject sparks = default;
    [SerializeField] GameObject AlertSign = default;
    [SerializeField] GameObject AlertEffect = default;

    private void Start() {
        if (PlayerPrefs.HasKey("EMAIL")) {
            gameObject.SetActive(false);
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

    public void ProfileOnClick() {
        if (!isMoving) {
            isMoving = true;
            PlayClickSound();
            profileButton.interactable = false;
            BG_Fade_IMG.gameObject.SetActive(true);
            StartCoroutine(FadeTo(75 / 255f, 0.25f,false));
            profileMenu.GetComponent<Animator>().SetBool("ProfileDropDown", true);
            sparks.SetActive(false);
            AlertOff();
        }
    }

    public void Profile_Cancel() {
        if (!isMoving) {
            isMoving = true;
            PlayClickSound();
            bool turnOff = false;
            if (PlayerPrefs.HasKey("EMAIL")) {
                turnOff = true;
            }
            StartCoroutine(FadeTo(0 / 255f, 0.25f, turnOff));
            profileMenu.GetComponent<Animator>().SetBool("ProfileDropDown", false);
            if (!PlayerPrefs.HasKey("USERNAME")) {
                sparks.SetActive(true);
            }

            AlertOn();
        }
    }


    private void PlayClickSound() {
        FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
    }

    IEnumerator FadeTo(float aValue, float aTime, bool turnOff) {
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
            profileButton.interactable = true;
        }
        isMoving = false;
        if (turnOff == true) {
            gameObject.SetActive(false);
        }
    }
}
