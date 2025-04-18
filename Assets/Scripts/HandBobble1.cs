using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBobble1 : MonoBehaviour {
    private Vector3 startPosition;
    private Vector3 endPosition;
    public float moveTimer = 1f;
    public bool moving = false;

    private void Start() {
        startPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
        endPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - 75, gameObject.transform.localPosition.z);
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
