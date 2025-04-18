using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RateMeManager : MonoBehaviour{

    [SerializeField] Button rateMeButton = default;
    public int hasRated = 0;
    public int rateCount = 0;
    public bool online = false;

    void Start(){
        hasRated = PlayerPrefs.GetInt("hasRated", 0);
        if (hasRated == 0) {
            RateMeCheck();
        }
    }

    private void RateMeCheck() {
        IterateRateMeCounter();

        if(rateCount % 4 == 0 && rateCount > 16) {
            CheckIfOnline();
            if (online) {
                rateMeButton.gameObject.SetActive(true);
            }
            else {
                DeIterateRateMeCounter();
            }
        }
    }

    private void CheckIfOnline() {
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            Debug.Log("Rate Me Button Will Not Show, No Internet!");
            online = false;
        }
        else {
            Debug.Log("Rate Me Button Will Show, We Have Internet Access!");
            online = true;
        }
    }

    private void IterateRateMeCounter() {
        rateCount = PlayerPrefs.GetInt("mainMenuCount", 0);
        rateCount++;
        PlayerPrefs.SetInt("mainMenuCount", rateCount);
    }

    private void DeIterateRateMeCounter() {
        rateCount = PlayerPrefs.GetInt("mainMenuCount", 0);
        rateCount--;
        PlayerPrefs.SetInt("mainMenuCount", rateCount);

    }
}
