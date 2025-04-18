using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_Theme : MonoBehaviour{

    public void SetEmptySign(GameObject endSign) {
        endSign.GetComponent<SpriteRenderer>().sprite = ThemeManager.TM.GetEndSignEmpty();
    }

    public void SetFullSign(GameObject endSign) {
        endSign.GetComponent<SpriteRenderer>().sprite = ThemeManager.TM.GetEndSignFull();
    }

}
