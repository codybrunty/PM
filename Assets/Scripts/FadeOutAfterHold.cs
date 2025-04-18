using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeOutAfterHold : MonoBehaviour{

    public float holdTime = 1f;
    public float fadeTime = 1f;

    void Start(){
        StartCoroutine(HoldFade());
    }

    IEnumerator HoldFade() {
        yield return new WaitForSeconds(holdTime);
        StartCoroutine(FadeTo(0f, fadeTime));
    }

    IEnumerator FadeTo(float aValue, float aTime) {
        TextMeshProUGUI text = gameObject.GetComponent<TextMeshProUGUI>();
        float alpha = text.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
            Color newColor = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(alpha, aValue, t));
            text.color = newColor;
            yield return null;
        }

        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);

    }

}
