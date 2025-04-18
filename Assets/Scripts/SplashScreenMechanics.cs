using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenMechanics : MonoBehaviour{

    public float total_splashTime = 3f;
    public int tutorial = 0;
    public int tutorial_index = 1;
    [SerializeField] GameObject bomb_canvas = default;
    [SerializeField] LeafMovement leafMovement = default;

    private void Start() {
        GetTutorialStatus();
        StartCoroutine(HoldAndGo());
    }

    private void GetTutorialStatus() {
        tutorial = PlayerPrefs.GetInt("FirstTutorialComplete", 0);
        if (tutorial == 0) {
            Debug.Log("Tutorial Not Complete");
            GetTutorialLevelIndex();
        }
        else {
            Debug.Log("Tutorial Already Complete");
        }
    }

    private void GetTutorialLevelIndex() {
        Debug.Log("Getting Current Tutorial Progress");
        for (int i = 0; i < GameDataControl.gdControl.all_level_results[0].Count; i++) {
            if (GameDataControl.gdControl.all_level_results[0][i] == 1) {
                tutorial_index++;
            }
            else {
                break;
            }
        }
        if (tutorial_index > 10) {
            Debug.Log("Tutorial has been complete");
            PlayerPrefs.SetInt("FirstTutorialComplete", 1);
            tutorial = 1;
        }
        else {
            SetTutorialLevelIndex();
        }
    }

    private void SetTutorialLevelIndex() {
        Debug.Log("Tutorial current level " + tutorial_index);
        LevelManager.levelManager.scroll = 0;
        LevelManager.levelManager.block = 1;
        LevelManager.levelManager.level = tutorial_index;
    }

    IEnumerator HoldAndGo() {
        float half_splashTime = total_splashTime / 2f;
        yield return new WaitForSeconds(half_splashTime);
        bomb_canvas.SetActive(false);
        leafMovement.enabled = true;
        float secondHalf_splashTime = half_splashTime - 0.5f;
        yield return new WaitForSeconds(secondHalf_splashTime);
        if (tutorial == 0) {
            SceneManager.LoadSceneAsync("Game");
        }
        else {
            SceneManager.LoadSceneAsync("Menu_Level");
        }

    }
}
