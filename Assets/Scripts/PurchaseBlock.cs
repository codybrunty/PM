using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PurchaseBlock : MonoBehaviour {
    public int blockNumber;
    private List<int> purchasePrices = new List<int> { 0, 75, 150, 275, 350, 475, 550, 550, 550, 675, 675, 675, 750, 750, 750, 875, 875, 875, 950, 950, 1000 };
    //private List<int> purchasePrices = new List<int> { 0, 75, 150, 275, 350, 350, 475, 475, 475, 550, 550, 550, 675, 675, 675, 750, 750, 750, 875, 875, 1000 };
    //private List<int> purchasePrices = new List<int> { 0, 75, 150, 275, 350, 475, 550, 675, 750, 750, 750, 800, 800, 800, 900, 900, 900, 1000, 1000, 1000, 1000 };
    public int purchasePrice;
    [SerializeField] TextMeshProUGUI purchasePriceText = default;
    private bool isScrolling = false;
    private bool isSwiping = false;
    public ScrollClickPrevention scrollClickPrevention;
    public GameObject buttonList_content;
    public CoinTotal bank;
    public bool tutorialBuyButton = false;
    [SerializeField] MainMenuTutorial tutorial = default;
    public float totalShakeTime = .2f;
    public float totalShakeMagnitude = 15f;


    public void SetPriceForBlock() {
        purchasePrice = purchasePrices[blockNumber - 1];
        purchasePriceText.text = purchasePrice.ToString();
    }

    public void OnBlockPriceClick() {
        CheckIsScrolling();
        CheckIsSwiping();
        if (!isScrolling && !isSwiping) {
            int coinsTotal = GameDataControl.gdControl.coinsTotal;

            if (purchasePrice <= coinsTotal) {

                if (GameDataControl.gdControl.blocksUnlocked[blockNumber - 1] == 0) {

                    //SoundManager.PlaySound("selectSFX1");
                    FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
                    GameDataControl.gdControl.blocksUnlocked[blockNumber - 1] = 1;
                    GameDataControl.gdControl.coinsSpent += purchasePrice;
                    GameDataControl.gdControl.coinsTotal -= purchasePrice;
                    Debug.Log("Block " + blockNumber + " unlocked for " + purchasePrice + " coins.");
                    GameDataControl.gdControl.SavePlayerData();
                    UnlockBlock();
                    UpdateBankPrice();

                    if (tutorialBuyButton) {
                        SendTutorialBuyConfirmation();
                    }
                    else {
                        ExpandBlock();
                    }

                }
                else {
                    Debug.Log(GameDataControl.gdControl.blocksUnlocked[blockNumber - 1]);
                    Debug.Log("Something went wrong this block has already been payed for!");
                }
            }
            else {
                Debug.Log("You don't have enough coins. It costs " + purchasePrice + " coins. You have " + coinsTotal + ".");
                NegativeFeedBack();
            }
        }
    }

    private void ExpandBlock() {
        if (!gameObject.transform.parent.GetComponentInChildren<ExpandableButton>().expanded) {
            gameObject.transform.parent.GetComponentInChildren<ExpandableButton>().OnExpandClick();
        }
    }

    private void NegativeFeedBack() {
        StartCoroutine(FindObjectOfType<ScreenShake>().Shake(totalShakeTime, totalShakeMagnitude));
        FindObjectOfType<SoundManager>().PlaySound("neg1");
        FindObjectOfType<BlinkingText>().BlinkLimit(3);
    }

    private void SendTutorialBuyConfirmation() {
        if (tutorial != null) {
            tutorial.BuyConfirm();
        }
    }

    private void CheckIsSwiping() {
        isSwiping = scrollClickPrevention.isSwiping;
    }

    private void UpdateBankPrice() {
        bank.SubtractFromBank(purchasePrice);
    }

    private void CheckIsScrolling() {
        isScrolling = scrollClickPrevention.isScrolling;
    }

    private void UnlockBlock() {
        buttonList_content.GetComponent<UnlockBlocks>().UnlockSpecificBlock(blockNumber);
    }

}
