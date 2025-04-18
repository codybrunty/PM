using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CloudOnce;
using System;

public class AchievementsCommand : MonoBehaviour{

    public void AchievementsOnClick() {
        PlayClickSFX();

        if (Cloud.IsSignedIn) {
            AchievementUnlockedCheck();
            Cloud.Achievements.ShowOverlay();
        }
        else {
            Cloud.SignIn();
        }
    }

    private void PlayClickSFX() {
        FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
    }

    private void AchievementUnlockedCheck() {
        Debug.Log("Checking to see if all achievements have been properly unlocked");
        for (int i = 0; i < GameDataControl.gdControl.blockSecretsUnlocked.Count; i++) {
            int firstSecret = GameDataControl.gdControl.blockSecretsUnlocked[i][0];
            int secondSecret = GameDataControl.gdControl.blockSecretsUnlocked[i][1];
            int thirdSecret = GameDataControl.gdControl.blockSecretsUnlocked[i][2];
            int fourthSecret = GameDataControl.gdControl.blockSecretsUnlocked[i][3];
            if (firstSecret == 1 && secondSecret == 1 && thirdSecret == 1 && fourthSecret == 1) {
                UnlockFlowerAchievmentSwitch(i + 1);
            }
        }
    }

    private void UnlockFlowerAchievmentSwitch(int flower) {
        switch (flower) {
            case 1:
                Achievements.Flower1Completed.Unlock();
                break;
            case 2:
                Achievements.Flower2Completed.Unlock();
                break;
            case 3:
                Achievements.Flower3Completed.Unlock();
                break;
            case 4:
                Achievements.Flower4Completed.Unlock();
                break;
            case 5:
                Achievements.Flower5Completed.Unlock();
                break;
            case 6:
                Achievements.Flower6Completed.Unlock();
                break;
            case 7:
                Achievements.Flower7Completed.Unlock();
                break;
            case 8:
                Achievements.Flower8Completed.Unlock();
                break;
            case 9:
                Achievements.Flower9Completed.Unlock();
                break;
            case 10:
                Achievements.Flower10Completed.Unlock();
                break;
            case 11:
                Achievements.Flower11Completed.Unlock();
                break;
            case 12:
                Achievements.Flower12Completed.Unlock();
                break;
            case 13:
                Achievements.Flower13Completed.Unlock();
                break;
            case 14:
                Achievements.Flower14Completed.Unlock();
                break;
            case 15:
                Achievements.Flower15Completed.Unlock();
                break;
            case 16:
                Achievements.Flower16Completed.Unlock();
                break;
            case 17:
                Achievements.Flower17Completed.Unlock();
                break;
            case 18:
                Achievements.Flower18Completed.Unlock();
                break;
            case 19:
                Achievements.Flower19Completed.Unlock();
                break;
            case 20:
                Achievements.Flower20Completed.Unlock();
                break;
        }
    }

}
