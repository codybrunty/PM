using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemePreviewMechanics : MonoBehaviour{

    private int themeIndex = 0;
    public List<Sprite> Grass1 = new List<Sprite>();
    public List<Sprite> Grass2 = new List<Sprite>();
    public List<Sprite> Grass3 = new List<Sprite>();
    public List<Sprite> Dirt = new List<Sprite>();
    public List<Sprite> Start = new List<Sprite>();
    public List<Sprite> StartRoad = new List<Sprite>();
    public List<Sprite> End = new List<Sprite>();
    public List<Sprite> EndRoad = new List<Sprite>();
    public List<Sprite> RedHouse = new List<Sprite>();
    public List<Sprite> BlueHouse = new List<Sprite>();
    public List<Sprite> Water = new List<Sprite>();
    public List<Sprite> Tree1 = new List<Sprite>();
    public List<Sprite> Tree2 = new List<Sprite>();
    public List<Sprite> HorizontalRoad = new List<Sprite>();
    public List<Sprite> HorizontalBridge = new List<Sprite>();
    public List<Sprite> TurnUpRoad = new List<Sprite>();
    public List<Sprite> VerticalRoad = new List<Sprite>();
    public List<Sprite> TurnRightRoad = new List<Sprite>();
    public List<Sprite> Trim_G1 = new List<Sprite>();
    public List<Sprite> Trim_G2 = new List<Sprite>();
    public List<Sprite> Trim_G3 = new List<Sprite>();
    public List<Sprite> Debri1 = new List<Sprite>();
    public List<Sprite> Debri2 = new List<Sprite>();
    public List<Sprite> Debri3 = new List<Sprite>();

    public void UpdateThemePreview(int index) {
        Debug.Log("Set Preview Index");
        themeIndex = index;
        UpdateAllPreviewImages();
    }

    private void UpdateAllPreviewImages() {
        Debug.Log("Update All Preview Images");
        ThemePreviewLookup[] images;
        images = gameObject.GetComponentsInChildren<ThemePreviewLookup>(true);

        for (int i = 0; i<images.Length; i++) {
            images[i].UpdateImageSprite(GetPreviewSprite(images[i].key));
        }

    }

    public Sprite GetPreviewSprite(string key) {
        switch (key) {
            case "Grass1":
                return Grass1[themeIndex];
            case "Grass2":
                return Grass2[themeIndex];
            case "Grass3":
                return Grass3[themeIndex];
            case "Dirt":
                return Dirt[themeIndex];
            case "Start":
                return Start[themeIndex];
            case "StartRoad":
                return StartRoad[themeIndex];
            case "End":
                return End[themeIndex];
            case "EndRoad":
                return EndRoad[themeIndex];
            case "RedHouse":
                return RedHouse[themeIndex];
            case "BlueHouse":
                return BlueHouse[themeIndex];
            case "Water":
                return Water[themeIndex];
            case "Tree1":
                return Tree1[themeIndex];
            case "Tree2":
                return Tree2[themeIndex];
            case "HorizontalRoad":
                return HorizontalRoad[themeIndex];
            case "HorizontalBridge":
                return HorizontalBridge[themeIndex];
            case "TurnUpRoad":
                return TurnUpRoad[themeIndex];
            case "VerticalRoad":
                return VerticalRoad[themeIndex];
            case "TurnRightRoad":
                return TurnRightRoad[themeIndex];
            case "Trim_G1":
                return Trim_G1[themeIndex];
            case "Trim_G2":
                return Trim_G2[themeIndex];
            case "Trim_G3":
                return Trim_G3[themeIndex];
            case "Debri1":
                return Debri1[themeIndex];
            case "Debri2":
                return Debri2[themeIndex];
            case "Debri3":
                return Debri3[themeIndex];
            default:
                Debug.LogWarning("Preview Lookup Error: "+ key);
                return null;
        }
    }

}
