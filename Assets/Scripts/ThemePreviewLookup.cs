using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ThemePreviewLookup : MonoBehaviour{
    
    public string key;

    public void UpdateImageSprite(Sprite newSprite){
        gameObject.GetComponent<Image>().sprite = newSprite; ;
    }

}