using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBobble2 : MonoBehaviour {
    private Vector3 startPosition;
    private Vector3 endPosition;
    public float moveTimer = 1f;
    public bool moving = true;
    public bool moveToStartingPosition = false;

    private void Start() {
        StartCoroutine(HoldHandBobble());
    }

    IEnumerator HoldHandBobble() {
        yield return new WaitForSeconds(0.4f);
        moving = false;
        startPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
        endPosition = new Vector3(gameObject.transform.localPosition.x-50, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
        ;
    }

    private void Update() {
        if (!moving) {
            //if (moveToStartingPosition) {
                if (gameObject.transform.localPosition.x == startPosition.x) {
                    StartCoroutine(MoveOverTime(startPosition, endPosition, moveTimer));
                    moving = true;
                }
                if (gameObject.transform.localPosition.x == endPosition.x) {
                    StartCoroutine(MoveOverTime(endPosition, startPosition, moveTimer));
                    moving = true;
                }
            //}
            //else {
            //    Vector2 currentPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
            //    StartCoroutine(MoveOverTime(currentPosition, startPosition, moveTimer));
            //    moving = true;
           // }
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
        if (!moveToStartingPosition) {
            moveToStartingPosition = true;
        }
    }

}
