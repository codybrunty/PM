using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwipeText : MonoBehaviour{

    TextMeshProUGUI text;
    public float textMoveTime = 0f;
    public float holdTimer = 0f;
    [SerializeField] AnimationCurve easeCurve=default;
    private Vector3 startPosition;
    private bool reachedCenter = false;
    [SerializeField] Transform destination = default;

    private void Start() {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        startPosition = text.transform.localPosition;
        Vector3 middlePosition = new Vector3(0f,0f,0f);
        if (destination == null) {
            middlePosition = new Vector3(0f, text.gameObject.transform.localPosition.y, text.gameObject.transform.localPosition.z);
        }
        else {
            middlePosition = destination.transform.localPosition;
        }
        Debug.Log(middlePosition);
        StartCoroutine(MoveOverTime(middlePosition, textMoveTime, true));
    }

    private void Update() {
        if (reachedCenter) {
            reachedCenter = false;
            StartCoroutine(HoldTextInCenter(holdTimer));
        }
    }

    IEnumerator HoldTextInCenter(float duration) {
        yield return new WaitForSeconds(duration);
        Vector3 endPosition = new Vector3(startPosition.x*-1, text.gameObject.transform.localPosition.y, text.gameObject.transform.localPosition.z);
        StartCoroutine(MoveOverTime(endPosition, textMoveTime, false));
    }

    IEnumerator MoveOverTime(Vector3 destination, float duration, bool center) {

        Vector3 start = text.transform.localPosition;
        Vector3 end = destination;

        for (float t = 0f; t < duration; t += Time.deltaTime) {
            float normalizedTime = t / duration;
            text.transform.localPosition = Vector3.Lerp(start, end, easeCurve.Evaluate(normalizedTime));
            yield return null;
        }

        text.transform.localPosition = end;
        reachedCenter = center;
        if (!center) {
            gameObject.SetActive(false);
        }
    }


}
