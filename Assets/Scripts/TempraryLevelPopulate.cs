using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TempraryLevelPopulate : MonoBehaviour {
    [SerializeField] List<Transform> points = default;
    [SerializeField] GameObject lvlPrefab = default;

    private void Awake() {
        LevelManager.levelManager.level = 0;
        for (int i = 0; i < points.Count; i++) {
            GameObject newLevel = Instantiate(lvlPrefab, points[i].position, Quaternion.identity, points[i]);
            Button lvlButton = newLevel.GetComponent<Button>();
            int lvl = i + 1;
            newLevel.name = "Level_" + lvl;
            lvlButton.onClick.AddListener(() => OnButtonClick(lvl));

            lvlButton.GetComponentInChildren<Text>().text = lvl.ToString();
        }
    }

    public void OnButtonClick(int lvl) {
        Debug.Log("Loading level "+lvl);
        LevelManager.levelManager.level = lvl;
        SceneManager.LoadScene("Game");
    }

}
