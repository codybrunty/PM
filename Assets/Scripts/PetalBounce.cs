using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetalBounce : MonoBehaviour {

    public float speed = 3f;
    public float maxRotation = 20f;

    // Update is called once per frame
    void Update () {
        transform.rotation = Quaternion.Euler(0f, 0f, maxRotation * Mathf.Sin(Time.time * speed));
    }
}
