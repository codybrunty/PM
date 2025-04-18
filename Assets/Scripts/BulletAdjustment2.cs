using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAdjustment2 : MonoBehaviour{
    private string currentLanguage;

    private void Start() {
        currentLanguage = PlayerPrefs.GetString("Language", "English");

        switch (currentLanguage) {
            case "Portueguese":
                MoveBulletDown(-91);
                break;
            case "Spanish":
                MoveBulletDown(-91);
                break;
            case "Russian":
                MoveBulletDown(-91);
                break;
            case "Dutch":
                break;
            case "German":
                MoveBulletDown(-91);
                break;
            case "Thai":
                MoveBulletDown(-53);
                break;
            case "Italian":
                MoveBulletDown(-91);
                break;
            case "Chinese":
                MoveBulletDown(-91);
                break;
            case "Japan":
                MoveBulletDown(-80);
                break;
            case "Korean":
                MoveBulletDown(-40);
                break;
            case "Turkish":
                MoveBulletDown(-44);
                break;
            case "Indonesian":
                MoveBulletDown(-91);
                break;
            case "French":
                MoveBulletDown(-91);
                break;
            default:
                break;
        }
    }

    private void MoveBulletDown(int yposition) {
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, yposition, gameObject.transform.localPosition.z);
    }
}
