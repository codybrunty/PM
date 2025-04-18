using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass_Theme : MonoBehaviour {

    private void Start() {
        SetGrassSprites();
    }

    private void SetGrassSprites() {
        SetGrass1();
        SetGrass2();
        SetGrass3();
    }

    private void SetGrass3() {
        gameObject.transform.GetChild(0).GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite = ThemeManager.TM.GetGrass3Sprite();
        gameObject.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ThemeManager.TM.GetDirtSprite();
        Sprite trim = ThemeManager.TM.GetGrass3TrimSprite();
        gameObject.transform.GetChild(0).GetChild(2).GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = trim;
        gameObject.transform.GetChild(0).GetChild(2).GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite = trim;
        gameObject.transform.GetChild(0).GetChild(2).GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite = trim;
    }

    private void SetGrass2() {
        gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = ThemeManager.TM.GetGrass2Sprite();
        gameObject.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ThemeManager.TM.GetDirtSprite();
        Sprite trim = ThemeManager.TM.GetGrass2TrimSprite();
        gameObject.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = trim;
        gameObject.transform.GetChild(0).GetChild(1).GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite = trim;
        gameObject.transform.GetChild(0).GetChild(1).GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite = trim;
    }

    private void SetGrass1() {
        gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ThemeManager.TM.GetGrass1Sprite();
        gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ThemeManager.TM.GetDirtSprite();
        Sprite trim = ThemeManager.TM.GetGrass1TrimSprite();
        gameObject.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = trim;
        gameObject.transform.GetChild(0).GetChild(0).GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite = trim;
        gameObject.transform.GetChild(0).GetChild(0).GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite = trim;
    }

}
