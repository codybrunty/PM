using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataDebugGUI : MonoBehaviour {
    void OnGUI() {
        
        GUIStyle guiStyle = new GUIStyle("button");
        guiStyle.fontSize = 38; //change the font size

        if (GUI.Button(new Rect(350, 20, 200, 100), "Reset Data", guiStyle)) {
            GameDataControl.gdControl.ResetPlayerData();
            PlayerPrefs.DeleteAll();
            PlayTimerMechanics.instance.ResetTimePlayed();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
    }
}
