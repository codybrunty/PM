using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAdjustment : MonoBehaviour{

    private string currentLanguage;

    private void Start() {
        currentLanguage = PlayerPrefs.GetString("Language", "English");

        switch (currentLanguage) {
            case "Portueguese":
                MoveBulletDown(-39);
                break;
            case "Spanish":
                MoveBulletDown(-118);
                break;
            case "Russian":
                MoveBulletDown(-115);
                break;
            case "Dutch":
                MoveBulletDown(-115);
                break;
            case "German":
                MoveBulletDown(-115);
                break;
            case "Thai":
                MoveBulletDown(-45);
                break;
            case "Italian":
                MoveBulletDown(-115);
                break;
            case "Chinese":
                MoveBulletDown(10);
                break;
            case "Japan":
                MoveBulletDown(-80);
                break;
            case "French":
                MoveBulletDown(-123);
                break;
            default:
                break;
        }
    }

    private void MoveBulletDown(int yposition) {
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, yposition, gameObject.transform.localPosition.z);
    }
}
