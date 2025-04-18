using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecretFlowerTest : MonoBehaviour {

    public void SecretFlowerOnClick() {
        PlayClickSound();
        DiscoverNextSecretFlower();
        GameDataControl.gdControl.SavePlayerData();
        //GameDataControl.gdControl.PrintSecretResults();
        SceneManager.LoadScene("Menu_Level");
    }

    private void DiscoverNextSecretFlower() {

        for (int i = 0; i < 20; i++) {
            int counter = 0;
            for (int x = 0; x < 4; x++) {
                if (GameDataControl.gdControl.blockSecretsUnlocked[i][x] == 0) {
                    GameDataControl.gdControl.blockSecretsUnlocked[i][x] = 1;
                    GameDataControl.gdControl.blockSecretsUnlocked[i][x+4] = x+1;
                    counter++;
                    break;
                }
            }
            if(counter > 0) {
                break;
            }
        }
    }

    private void PlayClickSound() {
        //SoundManager.PlaySound("selectSFX1");
        FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
    }

}
