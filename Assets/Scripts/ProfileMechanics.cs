using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using CloudOnce;

public class ProfileMechanics : MonoBehaviour {
    [SerializeField] TextMeshProUGUI campaignPercentageText = default;
    [SerializeField] TextMeshProUGUI mysteryFlowersText = default;
    [SerializeField] TextMeshProUGUI puzzlesSolvedText = default;
    [SerializeField] TextMeshProUGUI totalGoldText = default;
    [SerializeField] TextMeshProUGUI timePlayedText = default;

    private void Start() {
        SetCampaignText();
        SetMysteryFlowerText();
        SetPuzzlesSolvedText();
        SetTotalGoldText();
        SetTimePlayedText();
    }

    private void Update() {
        SetTimePlayedText();
        SetTotalGoldText();
    }

    private void SetTimePlayedText() {
        float currentTimePlayed = PlayTimerMechanics.instance.currentTimePlayed;

        int seconds = ((int)currentTimePlayed) % 60;
        int minutes = (((int)currentTimePlayed) / 60) % 60;
        int hours = ((int)currentTimePlayed) / 3600;

        if (hours > 0) {
            timePlayedText.text = hours.ToString("00") + ":" + minutes.ToString("00");
        }
        else {
            timePlayedText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }


    }

    private void SetTotalGoldText() {
        totalGoldText.text = GameDataControl.gdControl.coinsEarned.ToString();
    }

    private void SetPuzzlesSolvedText() {
        int puzzlesSolved = GameDataControl.gdControl.profile_puzzlesSolved;
        puzzlesSolvedText.text = puzzlesSolved.ToString();
        Leaderboards.PuzzlesSolved.SubmitScore(puzzlesSolved, callbackCheck);
    }

    private void callbackCheck(CloudRequestResult<bool> result) {
        if (result.Result == false) {
            Debug.Log(result.Error);
        }
    }

    private void SetMysteryFlowerText() {
        int totalMysteryFlowersDiscovered = 0;
        int totalMysteryFlowers = 0;

        for (int i = 0; i < GameDataControl.gdControl.blockSecretsUnlocked.Count; i++) {
            int petal1 = GameDataControl.gdControl.blockSecretsUnlocked[i][0];
            int petal2 = GameDataControl.gdControl.blockSecretsUnlocked[i][1];
            int petal3 = GameDataControl.gdControl.blockSecretsUnlocked[i][2];
            int petal4 = GameDataControl.gdControl.blockSecretsUnlocked[i][3];

            if (petal1 == 1 && petal2 == 1 && petal3 ==1 && petal4 == 1) {
                totalMysteryFlowersDiscovered++;
            }
            totalMysteryFlowers++;
        }

        mysteryFlowersText.text = totalMysteryFlowersDiscovered.ToString() + "/" + totalMysteryFlowers.ToString();
    }

    private void SetCampaignText() {
        int campaignPuzzlesSolved = 0;
        int campaignTotalLevels = 0;

        for(int i = 0; i<GameDataControl.gdControl.all_level_results.Count;i++) {
            for (int j=0; j < GameDataControl.gdControl.all_level_results[i].Count; j++) {
                if (GameDataControl.gdControl.all_level_results[i][j] == 1) {
                    campaignPuzzlesSolved++;
                }
                campaignTotalLevels++;
            }
        }

        float campaignSolvedPercentage = (((float)campaignPuzzlesSolved) / ((float)campaignTotalLevels))*100;
        campaignPercentageText.text = campaignSolvedPercentage.ToString("F1")+"%";
    }
}
