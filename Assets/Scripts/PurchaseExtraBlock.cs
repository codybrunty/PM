using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PurchaseExtraBlock : MonoBehaviour { 
    public int purchasePrice = 75;
    [SerializeField] TextMeshProUGUI purchasePriceText = default;
    private bool isScrolling = false;
    public ScrollClickPrevention scrollClickPrevention;
    public CoinTotal bank;
    public GameObject buttonList_content;
    public float totalShakeTime = .2f;
    public float totalShakeMagnitude = 15f;

    public void SetPriceForBlock() {
        purchasePriceText.text = purchasePrice.ToString();
    }

    public void OnBlockPriceClick() {
        CheckIsScrolling();
        if (!isScrolling) {
            int coinsTotal = GameDataControl.gdControl.coinsTotal;

            if (purchasePrice <= coinsTotal) {

                if (GameDataControl.gdControl.extraBlock1Unlocked == false) {
                    
                    FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
                    GameDataControl.gdControl.extraBlock1Unlocked = true;
                    GameDataControl.gdControl.coinsSpent += purchasePrice;
                    GameDataControl.gdControl.coinsTotal -= purchasePrice;

                    Debug.Log("Extra Block unlocked for " + purchasePrice + " coins.");
                    GameDataControl.gdControl.SavePlayerData();
                    UnlockExtraBlock1();
                    UpdateBankPrice();
                    ExpandBlock();
                }
                else {
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

    private void UnlockExtraBlock1() {
        buttonList_content.GetComponent<DailyPuzzles>().UnlockExtraBlock(0);
    }

    private void UpdateBankPrice() {
        bank.SubtractFromBank(purchasePrice);
    }

    private void CheckIsScrolling() {
        isScrolling = scrollClickPrevention.isScrolling;
    }
}
