using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour{

    TextMeshProUGUI text;
    public float fadetimer = 1f;
    public bool blink = false;
    public float blinkAlphaMin = 0f;
    public float blinkAlphMax = 1f;
    public bool blinkLimit = false;
    public int numberOfTimes = 0;
     
    private void Start(){
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update() {

        if (blink) {
            if (text.color.a == blinkAlphaMin) {
                StartCoroutine(FadeTextOverTime(blinkAlphMax, fadetimer));
            }
            if (text.color.a == blinkAlphMax) {
                StartCoroutine(FadeTextOverTime(blinkAlphaMin, fadetimer));
            }
        }

        if (blinkLimit && numberOfTimes > 0) {
            if (text.color.a == blinkAlphaMin) {
                StartCoroutine(FadeTextOverTime(blinkAlphMax, fadetimer));
                blinkLimit = false;
            }
            if (text.color.a == blinkAlphMax) {
                StartCoroutine(FadeTextOverTime(blinkAlphaMin, fadetimer));
                blinkLimit = false;
            }
        }
        if (numberOfTimes == 0) {
            blinkLimit = false;
        }
    }

    public void BlinkLimit(int num) {
        blinkLimit = true;
        numberOfTimes = num;
    }

    IEnumerator FadeTextOverTime( float alphaEnd, float duration) {
        if (alphaEnd == blinkAlphMax) {
            numberOfTimes--;
        }

        Color start = text.color;
        Color end = new Color(text.color.r, text.color.g, text.color.b, alphaEnd);

        for (float t = 0f; t < duration; t += Time.deltaTime) {
            float normalizedTime = t / duration;
            text.color = Color.Lerp(start, end, normalizedTime);
            yield return null;
        }

        text.color = end;
        if (blink==false) {
            blinkLimit = true;
        }
    }
    

}
