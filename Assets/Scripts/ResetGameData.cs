using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGameData : MonoBehaviour{

    private int debugOn = 0;

    public void ResetOnClick() {
        debugOn = PlayerPrefs.GetInt("DebugOn",0);
        GameDataControl.gdControl.ResetPlayerData();
        PlayerPrefs.DeleteAll();
        PlayTimerMechanics.instance.ResetTimePlayed();
        PlayerPrefs.SetInt("DebugOn", debugOn);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
