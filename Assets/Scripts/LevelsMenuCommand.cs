using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsMenuCommand : MonoBehaviour {

    private int tutorial = 0;

    private void Awake() {
        tutorial = PlayerPrefs.GetInt("FirstTutorialComplete", 0);
        if (tutorial == 0) {
            gameObject.GetComponent<Button>().interactable = false;
            gameObject.GetComponent<Image>().raycastTarget = false;
        }
    }

    public void LevelsMenuOnClick() {
        PlayClickSound();
        SceneManager.LoadScene("Menu_Level");
    }

    private void PlayClickSound() {
        FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
    }
}



