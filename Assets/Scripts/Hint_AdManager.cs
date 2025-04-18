using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System;
using TMPro;

[RequireComponent(typeof(Button))]
public class Hint_AdManager : MonoBehaviour {

#if UNITY_IOS
    private string gameId = "3194456";
#elif UNITY_ANDROID
    private string gameId = "3194457";
#endif

    Button myButton;
    public string myPlacementId = "video";
    [SerializeField] CreateBoard gameBoard = default;
    public int hintCounter = 0;
    public int puzzlesCompletedThisSession = 0;
    public int puzzleCompleteThreshold = 6;
    private bool hintButtonPressed = false;
    [SerializeField] Sprite hint_online = default;
    [SerializeField] Sprite hint_offline = default;
    public int offlineHintPrice = 10;



    void Start() {
        //1.1.1 always reset to 0 get rid of interstital ads
        //PlayerPrefs.SetInt("puzzlesCompletedThisSession", 0);

        puzzlesCompletedThisSession = PlayerPrefs.GetInt("puzzlesCompletedThisSession", 0);
        //Debug.LogWarning("puzzles completed this sesh: "+puzzlesCompletedThisSession);

        //If the user hasn't completed the tutorial the puzzlesCompletedThisSession counter is reset to 0.
        int tutorialComplete = PlayerPrefs.GetInt("FirstTutorialComplete", 0);
        if (tutorialComplete == 0) {
            puzzlesCompletedThisSession = 0;
            PlayerPrefs.SetInt("puzzlesCompletedThisSession", 0);
        }

        if (Application.internetReachability != NetworkReachability.NotReachable) {
            if (puzzlesCompletedThisSession % puzzleCompleteThreshold == 0 && puzzlesCompletedThisSession > 0) {
                //reset to 0 after interstitial ad is played
                PlayerPrefs.SetInt("puzzlesCompletedThisSession", 0);
            }
        }

        myButton = GetComponent<Button>();

        UpdateHintButtonImage();

    }

    private void UpdateHintButtonImage() {

        UpdateImageOnline();
        
    }

    private void UpdateImageOnline() {
        gameObject.GetComponent<Image>().sprite = hint_online;
    }

    private void UpdateImageOffline() {
        gameObject.GetComponent<Image>().sprite = hint_offline;
    }

    private void Update() {
        if (gameObject.activeInHierarchy == true) {
            if (gameObject.GetComponent<Button>().interactable == false) {
                FixHintButtonFontColorFalse();
            }
            if (gameObject.GetComponent<Button>().interactable == true) {
                FixHintButtonFontColorTrue();
            }
        }


    }

    private void FixHintButtonFontColorFalse() {
        gameObject.GetComponent<Image>().raycastTarget = false;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().raycastTarget = false;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color(47f / 255f, 58f / 255f, 63f / 255f, 255f);
    }

    private void FixHintButtonFontColorTrue() {
        gameObject.GetComponent<Image>().raycastTarget = true;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().raycastTarget = true;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color(66f / 255f, 81f / 255f, 90f / 255f, 255f);
    }
    

    // Implement a function for showing a rewarded video ad:
    public void ShowVideo() {
        UpdateImageOnline(); 
        HintUsedPlayerPrefs();
        hintButtonPressed = true;
        Hint_AdFinished();
        hintButtonPressed = false;
    }


    private void Hint_AdFinished() {
        Debug.Log("Player watched ad.");
        if (hintButtonPressed) { 
            //EnableAllButtons();

            //second time show all trees
            if (hintCounter > 0) {
                gameBoard.ShowXsOnGameBoard(false);
                gameObject.GetComponent<Button>().interactable = false;
                gameObject.GetComponent<Image>().raycastTarget = false;
            }
            //first time show some trees
            else {
                gameBoard.HintShowSomeXs();
            }

            hintCounter++;
        }

    }

    private void HintUsedPlayerPrefs() {
        int hintsUsed = PlayerPrefs.GetInt("HintsUsed", 0);
        hintsUsed++;
        PlayerPrefs.SetInt("HintsUsed", hintsUsed);
    }

}