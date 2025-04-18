using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafMovement : MonoBehaviour{


    public Leaf[] leafs;


    void Start() {

        for (int i = 0; i < leafs.Length; i++) {
            StartCoroutine(MoveLeafOverTime(leafs[i].leaf, leafs[i].start.transform.position, leafs[i].end.transform.position, leafs[i].duration, leafs[i].ease, leafs[i].degrees));
        }

    }

    private IEnumerator MoveLeafOverTime(GameObject leaf, Vector3 start, Vector3 end, float duration, AnimationCurve ease,float degrees) {
        float startRotation = UnityEngine.Random.Range(0,360);
        float endRotation = startRotation + degrees;
        leaf.transform.eulerAngles = new Vector3(0f,0f,startRotation);

        for (float t = 0f; t < duration; t += Time.deltaTime) {
            float normalizedTime = t / duration;
            leaf.transform.position = Vector3.Lerp(start, end, ease.Evaluate(normalizedTime));
            leaf.transform.eulerAngles = Vector3.Lerp(new Vector3(0f, 0f, startRotation), new Vector3(0f, 0f, endRotation), ease.Evaluate(normalizedTime));
            yield return null;
        }

        leaf.transform.eulerAngles = new Vector3(0f, 0f, endRotation);
        leaf.transform.position = end;
    }

}
