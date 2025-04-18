using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Theme : MonoBehaviour{

    private void Start() {
        SetWaterSprite();
    }

    private void SetWaterSprite() {
        gameObject.transform.GetChild(2).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ThemeManager.TM.GetWaterSprite();
    }
}
