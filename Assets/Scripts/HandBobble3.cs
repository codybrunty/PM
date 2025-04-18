using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandBobble3 : MonoBehaviour { 
    private Vector3 startPosition;
    private Vector3 endPosition;
    public float moveTimer = 1f;
    public bool moving = false;
    public bool on = false;
    public int totalBobbles = 3;
    private int currentBobbles = 0;
    [SerializeField] GameObject hintButton = default;
    private bool hand = false;
    private Coroutine fade;

    private void Start() {
        startPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
        endPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - 35, gameObject.transform.localPosition.z);
    }

    private void Update() {
        CheckBobbleCount();
        if (on) {
            if (!moving) {
                if (gameObject.transform.localPosition == startPosition) {
                    StartCoroutine(MoveOverTime(startPosition, endPosition, moveTimer));
                    moving = true;
                }
                if (gameObject.transform.localPosition == endPosition) {
                    StartCoroutine(MoveOverTime(endPosition, startPosition, moveTimer));
                    moving = true;
                    currentBobbles++;
                }
            }
        }
    }

    private void CheckHintCounter() {
        if(hintButton.GetComponent<Hint_AdManager>().hintCounter < 2) {
            hand = true;
        }
        else {
            hand = false;
        }
    }

    public void TurnOnHelpingHand() {
        CheckHintCounter();
        if (hand) {
            //gameObject.GetComponent<Image>().enabled = true;
            on = true;
            FadeIn();
        }
    }

    private void CheckBobbleCount() {
        if (currentBobbles == totalBobbles) {
            currentBobbles = 0;
            on = false;
            //gameObject.GetComponent<Image>().enabled = false;
            FadeOut();
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

    private void FadeIn() {
        if (fade == null) {
            Color currentColor = gameObject.GetComponent<Image>().color;
            Color FadeInColor = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
            fade = StartCoroutine(FadeOverTime(FadeInColor, 0.5f));
        }
        else {
            StopCoroutine(fade);
            Color currentColor = gameObject.GetComponent<Image>().color;
            Color FadeInColor = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
            fade = StartCoroutine(FadeOverTime(FadeInColor, 0.5f));
        }
    }
    private void FadeOut() {
        if (fade == null) {
            Color currentColor = gameObject.GetComponent<Image>().color;
            Color FadeOutColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
            fade = StartCoroutine(FadeOverTime(FadeOutColor, 0.5f));
        }
        else {
            StopCoroutine(fade);
            Color currentColor = gameObject.GetComponent<Image>().color;
            Color FadeInColor = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
            fade = StartCoroutine(FadeOverTime(FadeInColor, 0.5f));
        }
    }

    IEnumerator FadeOverTime(Color targetColor, float duration) {

        Color currentColor = gameObject.GetComponent<Image>().color;

        for (float t = 0f; t < duration; t += Time.deltaTime) {
            float normalizedTime = t / duration;
            gameObject.GetComponent<Image>().color = Color.Lerp(currentColor, targetColor, normalizedTime);
            yield return null;
        }

        gameObject.GetComponent<Image>().color = targetColor;
        fade = null;
    }
}
