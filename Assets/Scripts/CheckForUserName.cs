using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForUserName : MonoBehaviour{
    // Start is called before the first frame update
    void Start(){
        if (PlayerPrefs.HasKey("USERNAME")) {
            gameObject.SetActive(false);
        }
    }


}
