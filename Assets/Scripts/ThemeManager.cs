using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour{
    public static ThemeManager TM;
    public int activeThemeIndex = 0;

    #region variables
    [Header("Sign Theme Pieces")]
    [SerializeField] List<Sprite> endSignFull = new List<Sprite>();
    [SerializeField] List<Sprite> endSignEmpty = new List<Sprite>();
    [SerializeField] List<Sprite> startSign = new List<Sprite>();
    [SerializeField] List<Sprite> key = new List<Sprite>();
    [SerializeField] List<Sprite> keyShadow = new List<Sprite>();

    [Header("Obstacle Theme Pieces")]
    [SerializeField] List<GameObject> redHouses = new List<GameObject>();
    [SerializeField] List<GameObject> blueHouses = new List<GameObject>();
    [SerializeField] List<Sprite> water = new List<Sprite>();

    [Header("Ground Theme Pieces")]
    [SerializeField] List<GameObject> tree1 = new List<GameObject>();
    [SerializeField] List<GameObject> tree2 = new List<GameObject>();

    [SerializeField] List<GameObject> GrassBurst = new List<GameObject>();
    [SerializeField] List<GameObject> WaterBurst = new List<GameObject>();

    [SerializeField] List<Sprite> grass1 = new List<Sprite>();
    [SerializeField] List<Sprite> grass1Trim = new List<Sprite>();

    [SerializeField] List<Sprite> grass2 = new List<Sprite>();
    [SerializeField] List<Sprite> grass2Trim = new List<Sprite>();

    [SerializeField] List<Sprite> grass3 = new List<Sprite>();
    [SerializeField] List<Sprite> grass3Trim = new List<Sprite>();

    [SerializeField] List<Sprite> dirt = new List<Sprite>();


    [Header("Road Theme Pieces")]
    [SerializeField] List<Sprite> StartMiddle = new List<Sprite>();
    [SerializeField] List<Sprite> StartRightMiddle = new List<Sprite>();
    [SerializeField] List<Sprite> StartTopMiddle = new List<Sprite>();
    [SerializeField] List<Sprite> StartBottomMiddle = new List<Sprite>();
    [SerializeField] List<Sprite> StartLeftMiddle = new List<Sprite>();

    [SerializeField] List<Sprite> BottomMiddle = new List<Sprite>();
    [SerializeField] List<Sprite> BottomLeft = new List<Sprite>();
    [SerializeField] List<Sprite> BottomRight = new List<Sprite>();
    [SerializeField] List<Sprite> BottomTop = new List<Sprite>();
    [SerializeField] List<Sprite> BottomTop_alt = new List<Sprite>();

    [SerializeField] List<Sprite> LeftMiddle = new List<Sprite>();
    [SerializeField] List<Sprite> LeftBottom = new List<Sprite>();
    [SerializeField] List<Sprite> LeftRight = new List<Sprite>();
    [SerializeField] List<Sprite> LeftRight_alt = new List<Sprite>();
    [SerializeField] List<Sprite> LeftTop = new List<Sprite>();

    [SerializeField] List<Sprite> RightMiddle = new List<Sprite>();
    [SerializeField] List<Sprite> RightBottom = new List<Sprite>();
    [SerializeField] List<Sprite> RightLeft = new List<Sprite>();
    [SerializeField] List<Sprite> RightLeft_alt = new List<Sprite>();
    [SerializeField] List<Sprite> RightTop = new List<Sprite>();

    [SerializeField] List<Sprite> TopMiddle = new List<Sprite>();
    [SerializeField] List<Sprite> TopLeft = new List<Sprite>();
    [SerializeField] List<Sprite> TopRight = new List<Sprite>();
    [SerializeField] List<Sprite> TopBottom = new List<Sprite>();
    [SerializeField] List<Sprite> TopBottom_alt = new List<Sprite>();

    [Header("Bridge Theme Pieces")]
    [SerializeField] List<Sprite> BottomMiddle_Bridge = new List<Sprite>();
    [SerializeField] List<Sprite> BottomLeft_Bridge = new List<Sprite>();
    [SerializeField] List<Sprite> BottomRight_Bridge = new List<Sprite>();
    [SerializeField] List<Sprite> BottomTop_Bridge = new List<Sprite>();

    [SerializeField] List<Sprite> LeftMiddle_Bridge = new List<Sprite>();
    [SerializeField] List<Sprite> LeftBottom_Bridge = new List<Sprite>();
    [SerializeField] List<Sprite> LeftRight_Bridge = new List<Sprite>();
    [SerializeField] List<Sprite> LeftTop_Bridge = new List<Sprite>();

    [SerializeField] List<Sprite> RightMiddle_Bridge = new List<Sprite>();
    [SerializeField] List<Sprite> RightBottom_Bridge = new List<Sprite>();
    [SerializeField] List<Sprite> RightLeft_Bridge = new List<Sprite>();
    [SerializeField] List<Sprite> RightTop_Bridge = new List<Sprite>();

    [SerializeField] List<Sprite> TopMiddle_Bridge = new List<Sprite>();
    [SerializeField] List<Sprite> TopLeft_Bridge = new List<Sprite>();
    [SerializeField] List<Sprite> TopRight_Bridge = new List<Sprite>();
    [SerializeField] List<Sprite> TopBottom_Bridge = new List<Sprite>();

    [Header("Debri Theme Pieces")]
    [SerializeField] List<DebriLayoutByTheme> debriLayoutsByTheme = new List<DebriLayoutByTheme>();

    #endregion

    #region singleton  
    private void Awake() {
        if (TM == null) {
            DontDestroyOnLoad(gameObject);
            TM = this;
        }
        else if (TM != this) {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start() {
        GetActiveThemeFromGameData();
    }

    #region indexing
    public void GetActiveThemeFromGameData() {
        for (int i = 0; i<GameDataControl.gdControl.themes.Count; i++) {
            if (GameDataControl.gdControl.themes[i] == 2) {
                activeThemeIndex = i;
                break;
            }
        } 
    }

    public void UpdateActiveTheme(int index) {
        activeThemeIndex = index;
    }
    #endregion

    #region GetRoads

    public Sprite GetStartBottomMiddleSprite() {
        return StartBottomMiddle[activeThemeIndex];
    }
    public Sprite GetStartTopMiddleSprite() {
        return StartTopMiddle[activeThemeIndex];
    }
    public Sprite GetStartMiddleSprite() {
        return StartMiddle[activeThemeIndex];
    }
    public Sprite GetStartRightMiddleSprite() {
        return StartRightMiddle[activeThemeIndex];
    }
    public Sprite GetStartLeftMiddleSprite() {
        return StartLeftMiddle[activeThemeIndex];
    }

    public Sprite GetTopMiddleSprite() {
        return TopMiddle[activeThemeIndex];
    }
    public Sprite GetBottomMiddleSprite() {
        return BottomMiddle[activeThemeIndex];
    }
    public Sprite GetLeftMiddleSprite() {
        return LeftMiddle[activeThemeIndex];
    }
    public Sprite GetRightMiddleSprite() {
        return RightMiddle[activeThemeIndex];
    }

    public Sprite GetBottomTopSprite() {
        int randomNumber = UnityEngine.Random.Range(0,2);
        if (randomNumber == 0) {
            return BottomTop[activeThemeIndex];
        }
        else {
            return BottomTop_alt[activeThemeIndex];
        }
    }
    public Sprite GetBottomLeftSprite() {
        return BottomLeft[activeThemeIndex];
    }
    public Sprite GetBottomRightSprite() {
        return BottomRight[activeThemeIndex];
    }

    public Sprite GetRightTopSprite() {
        return RightTop[activeThemeIndex];
    }
    public Sprite GetRightLeftSprite() {
        int randomNumber = UnityEngine.Random.Range(0, 2);
        if (randomNumber == 0) {
            return RightLeft[activeThemeIndex];
        }
        else {
            return RightLeft_alt[activeThemeIndex];
        }
    }
    public Sprite GetRightBottomSprite() {
        return RightBottom[activeThemeIndex];
    }

    public Sprite GetLeftTopSprite() {
        return LeftTop[activeThemeIndex];
    }
    public Sprite GetLeftRightSprite() {
        int randomNumber = UnityEngine.Random.Range(0, 2);
        if (randomNumber == 0) {
            return LeftRight[activeThemeIndex];
        }
        else {
            return LeftRight_alt[activeThemeIndex];
        }
    }
    public Sprite GetLeftBottomSprite() {
        return LeftBottom[activeThemeIndex];
    }

    public Sprite GetTopBottomSprite() {
        int randomNumber = UnityEngine.Random.Range(0, 2);
        if (randomNumber == 0) {
            return TopBottom[activeThemeIndex];
        }
        else {
            return TopBottom_alt[activeThemeIndex];
        }
    }
    public Sprite GetTopRightSprite() {
        return TopRight[activeThemeIndex];
    }
    public Sprite GetTopLeftSprite() {
        return TopLeft[activeThemeIndex];
    }

    #endregion

    #region GetBridges

    public Sprite GetTopMiddleBridgeSprite() {
        return TopMiddle_Bridge[activeThemeIndex];
    }
    public Sprite GetBottomMiddleBridgeSprite() {
        return BottomMiddle_Bridge[activeThemeIndex];
    }
    public Sprite GetLeftMiddleBridgeSprite() {
        return LeftMiddle_Bridge[activeThemeIndex];
    }
    public Sprite GetRightMiddleBridgeSprite() {
        return RightMiddle_Bridge[activeThemeIndex];
    }

    public Sprite GetBottomTopBridgeSprite() {
        return BottomTop_Bridge[activeThemeIndex];
    }
    public Sprite GetBottomLeftBridgeSprite() {
        return BottomLeft_Bridge[activeThemeIndex];
    }
    public Sprite GetBottomRightBridgeSprite() {
        return BottomRight_Bridge[activeThemeIndex];
    }

    public Sprite GetRightTopBridgeSprite() {
        return RightTop_Bridge[activeThemeIndex];
    }
    public Sprite GetRightLeftBridgeSprite() {
        return RightLeft_Bridge[activeThemeIndex];
    }
    public Sprite GetRightBottomBridgeSprite() {
        return RightBottom_Bridge[activeThemeIndex];
    }

    public Sprite GetLeftTopBridgeSprite() {
        return LeftTop_Bridge[activeThemeIndex];
    }
    public Sprite GetLeftRightBridgeSprite() {
        return LeftRight_Bridge[activeThemeIndex];
    }
    public Sprite GetLeftBottomBridgeSprite() {
        return LeftBottom_Bridge[activeThemeIndex];
    }

    public Sprite GetTopBottomBridgeSprite() {
        return TopBottom_Bridge[activeThemeIndex];
    }
    public Sprite GetTopRightBridgeSprite() {
        return TopRight_Bridge[activeThemeIndex];
    }
    public Sprite GetTopLeftBridgeSprite() {
        return TopLeft_Bridge[activeThemeIndex];
    }

    #endregion

    #region GetGround

    public GameObject GetGrassBurstEffect() {
        return GrassBurst[activeThemeIndex];
    }
    public GameObject GetWaterBurstEffect() {
       return WaterBurst[activeThemeIndex];
    }
    public Sprite GetGrass1Sprite() {
     return grass1[activeThemeIndex];
    }
    public Sprite GetGrass2Sprite() {
        return grass2[activeThemeIndex];
    }
    public Sprite GetGrass3Sprite() {
        return grass3[activeThemeIndex];
    }
    public Sprite GetDirtSprite() {
        return dirt[activeThemeIndex];
    }
    public Sprite GetGrass1TrimSprite() {
        return grass1Trim[activeThemeIndex];
    }
    public Sprite GetGrass2TrimSprite() {
        return grass2Trim[activeThemeIndex];
    }
    public Sprite GetGrass3TrimSprite() {
        return grass3Trim[activeThemeIndex];
    }
    public GameObject GetTree() {
        int randomNumber = UnityEngine.Random.Range(0, 2);
        if (randomNumber == 0) {
            return tree1[activeThemeIndex];
        }
        else {
            return tree2[activeThemeIndex];
        }
    }
    #endregion

    #region GetSigns
    public Sprite GetEndSignEmpty() {
        return endSignEmpty[activeThemeIndex];
    }
    public Sprite GetEndSignFull() {
        return endSignFull[activeThemeIndex];
    }
    public Sprite GetStartSignSprite() {
        return startSign[activeThemeIndex];
    }
    public Sprite GetKeySprite() {
        return key[activeThemeIndex];
    }
    public Sprite GetKeyShadowSprite() {
        return keyShadow[activeThemeIndex];
    }


    #endregion

    #region GetObstacles

    public GameObject GetRedHouseGameObject() {
        return redHouses[activeThemeIndex];
    }
    public GameObject GetBlueHouseGameObject() {
        return blueHouses[activeThemeIndex];
    }
    public Sprite GetWaterSprite() {
        return water[activeThemeIndex];
    }

    #endregion

    #region GetDebri

    public GameObject GetGrassDebriLayout() {
        List<GameObject> grassDebriLayouts = debriLayoutsByTheme[activeThemeIndex].grassDebriLayouts;
        int randomIndex = UnityEngine.Random.Range(0, grassDebriLayouts.Count);
        return grassDebriLayouts[randomIndex];
    }

    public GameObject GetWaterDebriLayout() {
        List<GameObject> waterDebriLayouts = debriLayoutsByTheme[activeThemeIndex].waterDebriLayouts;
        int randomIndex = UnityEngine.Random.Range(0, waterDebriLayouts.Count);
        return waterDebriLayouts[randomIndex];
    }

    #endregion
}