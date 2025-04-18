using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System;
using TMPro;

[RequireComponent(typeof(Button))]
public class Hard_AdManager : MonoBehaviour {

#if UNITY_IOS
    private string gameId = "3194456";
#elif UNITY_ANDROID
    private string gameId = "3194457";
#endif
    
    public string myPlacementId = "rewardedVideo";
    private bool isScrolling = false;
    public ScrollClickPrevention scrollClickPrevention;
    public PurchaseDailyBlock purchaseDailyBlock;
    public Button hardButton;
    public Button hardFooterButton;

    void Start() {

        // Set interactivity to be dependent on the Placement’s status:
        hardButton.interactable = true;
        hardFooterButton.interactable = true;

    }

    // Implement a function for showing a rewarded video ad:
    public void ShowVideo() {
        CheckIsScrolling();
        if (!isScrolling) {
            FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
            Hard_AdFinished();
        }
    }

    private void Hard_AdFinished() {
        Debug.Log("Player watched ad.");
        purchaseDailyBlock.OnBlockPriceClickHard();
    }

    private void CheckIsScrolling() {
        isScrolling = scrollClickPrevention.isScrolling;
    }
}
