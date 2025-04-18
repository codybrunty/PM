using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckForUserNameText : MonoBehaviour{
    

    void Start(){
        if (PlayerPrefs.HasKey("USERNAME")) {
            gameObject.GetComponent<TextMeshProUGUI>().text = LocalisationSystem.GetLocalisedValue("profileMenu_recoverProfile");
        }
    }

}
