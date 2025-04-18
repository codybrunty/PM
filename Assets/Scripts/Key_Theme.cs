using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Theme : MonoBehaviour{


    private void Start() {
        SetKeySprite();
    }

    private void SetKeySprite() {
        gameObject.transform.GetChild(2).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ThemeManager.TM.GetKeySprite();
        gameObject.transform.GetChild(2).GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = ThemeManager.TM.GetKeyShadowSprite();
    }


}
