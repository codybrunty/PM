using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Theme : MonoBehaviour{
    
    private void Start() {
        gameObject.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = ThemeManager.TM.GetStartSignSprite();
        gameObject.transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().sprite = ThemeManager.TM.GetStartMiddleSprite();
    }


}
