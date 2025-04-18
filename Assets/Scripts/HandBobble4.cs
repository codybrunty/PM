using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBobble4 : MonoBehaviour{
    private Vector3 startPosition;
    private Vector3 endPosition;
    public float moveTimer = 1f;
    public bool moving = false;
    [SerializeField] ThemesSwipeMenu themesSwipeMenu = default;

    private void Start() {
        CheckIfFirstTime();
        startPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
        endPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - 75, gameObject.transform.localPosition.z);
    }

    private void CheckIfFirstTime() {
        int firstTimeThemeMenu = PlayerPrefs.GetInt("FirstTimeThemeMenu", 0);
        int themesOwned = GetThemesOwned();
        if (firstTimeThemeMenu != 0 || themesOwned > 1) {
            themesSwipeMenu.SwipeThemeMenuToSomethingPurchaseable();
            gameObject.SetActive(false);
            if (firstTimeThemeMenu == 0) {
                PlayerPrefs.SetInt("FirstTimeThemeMenu", 1);
            }
        }

    }

    private int GetThemesOwned() {
        int counter = 0;
        for (int i = 0; i < GameDataControl.gdControl.themes.Count; i++) {
            if (GameDataControl.gdControl.themes[i] != 0) {
                counter++;
            }
        }
        return counter;
    }

    private void Update() {
        if (!moving) {
            if (gameObject.transform.localPosition == startPosition) {
                StartCoroutine(MoveOverTime(startPosition, endPosition, moveTimer));
                moving = true;
            }
            if (gameObject.transform.localPosition == endPosition) {
                StartCoroutine(MoveOverTime(endPosition, startPosition, moveTimer));
                moving = true;
            }
        }

    }

    IEnumerator MoveOverTime(Vector3 currentPosition, Vector3 targetPosition, float duration) {

        for (float t = 0f; t < duration; t += Time.deltaTime) {
            float normalizedTime = t / duration;
            gameObject.transform.localPosition = Vector3.Lerp(currentPosition, targetPosition, normalizedTime);
            yield return null;
        }

        gameObject.transform.localPosition = targetPosition;
        moving = false;
    }

}
