using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckForUserNameButton : MonoBehaviour{

    void Start() {
        if (PlayerPrefs.HasKey("USERNAME")) {
            gameObject.GetComponent<Button>().interactable = false;
            gameObject.GetComponent<Image>().raycastTarget = false;
        }
    }

}
