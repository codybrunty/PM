using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyMechanics : MonoBehaviour {


    public int display = 1;
    [SerializeField] GameObject key = default;
    [SerializeField] Image arrow = default;
    //[SerializeField] Sprite leftArrow = default;
    //[SerializeField] Sprite rightArrow = default;
    [SerializeField] HelpMenuMechanics helpMenu = default;
    private float keyLength= 0f;
    public bool moving = false;
    public float movespeed = 0.5f;
    public AnimationCurve easeCurce;

    private void Start() {
        SetKeyLength();
        display = PlayerPrefs.GetInt("KeyDisplay",0);
        UpdateKeyDisplay();
    }

    private void SetKeyLength() {
        int scroll = LevelManager.levelManager.scroll;
        int block = LevelManager.levelManager.block;
        int level = LevelManager.levelManager.level;

        if (scroll == 0) {
            if (block==1 && level == 1) {
                key.SetActive(false);
            }
            else if (block == 1 && level < 7) {
                keyLength = -134f;
            }
            else if(block > 3) {
                keyLength = -358f;
            }
            else{
                keyLength = -245f;
            }
        }

        else {
            keyLength = -358f;
        }

    }

    private void UpdateKeyDisplay() {
        if (display == 1) {
            ShowKeyDisplay();
        }
        else {
            HideKeyDisplay();
        }
    }

    private void MoveKey() {
        moving = true;
        if (display == 1) {
            ShowKey();
        }
        else {
            HideKey();
        }
    }

    private void ShowKeyDisplay() {
        RectTransform rt = key.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector3(keyLength, 0f, 0f);
    }
    private void HideKeyDisplay() {
        RectTransform rt = key.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector3(0f, 0f, 0f);
    }

    private void ShowKey() {
        PlayClickSFX();
        StartCoroutine(MoveOverTime(new Vector3(keyLength, 0f, 0f), movespeed));
    }
    private void HideKey() {
        PlayClickSFX();
        StartCoroutine(MoveOverTime(new Vector3(0f, 0f, 0f),movespeed));
    }

    public void ChangeKeyStatus() {
        if (moving == false) {
            if (display == 1) {
                display = 0;
                PlayerPrefs.SetInt("KeyDisplay", display);
            }
            else {
                display = 1;
                PlayerPrefs.SetInt("KeyDisplay", display);
            }
            MoveKey();
        }
    }

    private void PlayClickSFX() {
        FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
    }

    IEnumerator MoveOverTime(Vector3 targetPosition, float duration) {
        RectTransform rt = key.GetComponent<RectTransform>();
        Vector3 currentPosition = rt.anchoredPosition;

        for (float t = 0f; t < duration; t += Time.deltaTime) {
            float normalizedTime = t / duration;
            rt.anchoredPosition = Vector3.Lerp(currentPosition, targetPosition, easeCurce.Evaluate(normalizedTime));
            yield return null;
        }

        rt.anchoredPosition = targetPosition;
        moving = false;
    }
}
