using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System;
using TMPro;

[RequireComponent(typeof(Button))]
public class SpinAgain_AdManager : MonoBehaviour {

#if UNITY_IOS
    private string gameId = "3194456";
#elif UNITY_ANDROID
    private string gameId = "3194457";
#endif

    Button myButton;
    public string myPlacementId = "rewardedVideo";
    [SerializeField] CreateBoard gameBoard = default;
    [SerializeField] GameObject prizewheel = default;
    [SerializeField] GameObject prizewheelarrow = default;
    [SerializeField] GameObject spinAgainText = default;
    [SerializeField] GameObject sparks = default;

    void Start() {

        myButton = GetComponent<Button>();

        // Set interactivity to be dependent on the Placement’s status:
        myButton.interactable = true;

        if (myButton.interactable == true) {
            //Spin wheel shows up starting on lvl 3
            int beatFirstThreeLevels = PlayerPrefs.GetInt("BeatFristTwo",0);

            if (beatFirstThreeLevels == 0) {
                CheckIfWeBeatFirstThreeYet();
                beatFirstThreeLevels = PlayerPrefs.GetInt("BeatFristTwo", 0);
            }

            if (beatFirstThreeLevels == 0) {
                Debug.Log("No spin wheel on first two levels");
            }
            else {
                Debug.Log("Show Spin Wheel");
                ButtonIsVisible();
            }

        }

    }

    private void CheckIfWeBeatFirstThreeYet() {
        int lvl_1_results = GameDataControl.gdControl.all_level_results[0][0];
        int lvl_2_results = GameDataControl.gdControl.all_level_results[0][1];
        int lvl_3_results = GameDataControl.gdControl.all_level_results[0][2];

        if (lvl_1_results == 1 && lvl_2_results == 1 && lvl_3_results == 1) {
            Debug.Log("all three levels have been completed unlock wheel spin");
            PlayerPrefs.SetInt("BeatFristTwo", 1);
        }
    }

    // Implement a function for showing a rewarded video ad:
    public void ShowRewardedVideo() {
        //DisableAllButtons();
        //reset to 0 since rewared ad being played
        PlayerPrefs.SetInt("puzzlesCompletedThisSession", 0);

        SpinAgain_AdFinished();
    }


    private void ButtonIsVisible() {
        myButton.gameObject.GetComponent<Image>().enabled = true;
        //watchAdText.gameObject.SetActive(true);
        myButton.gameObject.GetComponent<Animator>().SetBool("AdButtonGrow", true);
        spinAgainText.SetActive(true);
        FindObjectOfType<SoundManager>().PlayOneShotSound("MechanicalAdButton");
        //prizewheel.gameObject.GetComponent<Animator>().SetBool("FadeOut", true);
        //prizewheelarrow.gameObject.GetComponent<Animator>().SetBool("FadeOut", true);
        sparks.SetActive(true);
    }

    private void SpinAgain_AdFinished() {
        Debug.Log("Player watched ad.");
        SpinWheelCounter();
        sparks.SetActive(false);
        gameBoard.AssignImagesToButtons();
        prizewheel.GetComponent<Animator>().enabled = false; ;
        prizewheelarrow.GetComponent<Animator>().enabled = false;
        prizewheel.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        prizewheelarrow.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        gameBoard.AddRewardCoinsToPlayerDataAndUpdateFlowerDisplay();
        //EnableAllButtons();
        GameDataControl.gdControl.SavePlayerData();
        //watchAdText.SetActive(false);
        gameObject.SetActive(false);
    }

    private void SpinWheelCounter() {
        int wheelSpins = PlayerPrefs.GetInt("WheelSpins",0);
        wheelSpins++;
        PlayerPrefs.SetInt("WheelSpins", wheelSpins);
    }

}