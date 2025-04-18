using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Theme {

    [Header("Theme")]
    public string name;
    public int price;
    public int status;

    [Header("Theme Menu GameObjects")]
    public Sprite sprite_buy;
    public Sprite sprite_owned;
    public Sprite sprite_selected;


}