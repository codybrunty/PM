using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdBannerManager : MonoBehaviour {
#if UNITY_IOS
    private string gameId = "3194456";
#elif UNITY_ANDROID
    private string gameId = "3194457";
#endif
    
    public static AdBannerManager ABM;

    private void Awake() {
        if (ABM == null) {
            DontDestroyOnLoad(gameObject);
            ABM = this;
        }
        else if (ABM != this) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        CheckPuzzlesNumber();
    }

    private void CheckPuzzlesNumber() {
        //we want to make sure the add number under 5 (which is the interstitial number so we dont go straight into an ad from the main menu)
        int puzzlesComplete = PlayerPrefs.GetInt("puzzlesCompletedThisSession", 0);
        if (puzzlesComplete > 3) {
            PlayerPrefs.SetInt("puzzlesCompletedThisSession", 3);
        }
    }

}
