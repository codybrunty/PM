using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayTimerMechanics : MonoBehaviour{

    public static PlayTimerMechanics instance;
    public float startTime = 0f;
    public float currentTimePlayed = 0f;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        startTime = PlayerPrefs.GetFloat("timePlayed", 0f);
    }

    private void Update() {
        currentTimePlayed = startTime + Time.time;
    }
    
    public void UpdateTimer(float number) {
        Debug.Log("Timer Updated "+number);
        startTime = number;
    }

    private void SaveTimePlayed() {
        Debug.Log("TimePlayedSaved");
        PlayerPrefs.SetFloat("timePlayed", currentTimePlayed);
    }

    public void ResetTimePlayed() {
        Debug.Log("ResetPlayTime");
        PlayerPrefs.SetFloat("timePlayed", 0f);
        SaveTimePlayed();
    }

    void OnApplicationQuit() {
        Debug.Log("Quit");
        SaveTimePlayed();
    }

    void OnApplicationFocus(bool hasFocus) {
        if (!hasFocus) {
            Debug.Log("Lost Focus");
            SaveTimePlayed();
        }
    }

    void OnApplicationPause(bool pauseStatus) {
        if (pauseStatus) {
            Debug.Log("Pause");
            SaveTimePlayed();
        }
    }

}
