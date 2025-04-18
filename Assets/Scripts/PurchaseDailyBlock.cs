using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PurchaseDailyBlock : MonoBehaviour{
    public int purchasePrice = 50;
    [SerializeField] TextMeshProUGUI purchasePriceText = default;
    private bool isScrolling = false;
    public ScrollClickPrevention scrollClickPrevention;
    public CoinTotal bank;
    public GameObject buttonList_content;
    public float totalShakeTime = .2f;
    public float totalShakeMagnitude = 15f;

    public void SetPriceForBlock() {
        //purchasePriceText.text = purchasePrice.ToString();
        purchasePriceText.text = LocalisationSystem.GetLocalisedValue("storeMenu_free");
    }
    public void OnBlockPriceClickHard() {
        GameDataControl.gdControl.hardUnlocked = true;
        GameDataControl.gdControl.SavePlayerData();
        UnlockBlock();
        ExpandBlock();
    }

    public void OnBlockPriceClick() {
        CheckIsScrolling();
        if (!isScrolling) {
            int coinsTotal = GameDataControl.gdControl.coinsTotal;

            if (purchasePrice <= coinsTotal) {

                if (GameDataControl.gdControl.hardUnlocked == false) {

                    //SoundManager.PlaySound("selectSFX1");
                    FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
                    GameDataControl.gdControl.hardUnlocked = true;
                    GameDataControl.gdControl.coinsSpent += purchasePrice;
                    GameDataControl.gdControl.coinsTotal -= purchasePrice;

                    Debug.Log("Daily Hard Block unlocked for " + purchasePrice + " coins.");
                    GameDataControl.gdControl.SavePlayerData();
                    UnlockBlock();
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

    public void ExpandBlock() {
        if (!gameObject.transform.parent.GetComponentInChildren<ExpandableButton>().expanded) {
            gameObject.transform.parent.GetComponentInChildren<ExpandableButton>().OnExpandClick();
        }
    }

    private void NegativeFeedBack() {
        StartCoroutine(FindObjectOfType<ScreenShake>().Shake(totalShakeTime, totalShakeMagnitude));
        FindObjectOfType<SoundManager>().PlaySound("neg1");
        FindObjectOfType<BlinkingText>().BlinkLimit(3);
    }

    public void UnlockBlock() {
        buttonList_content.GetComponent<DailyPuzzles>().UnlockHardBlock();
    }

    private void UpdateBankPrice() {
        bank.SubtractFromBank(purchasePrice);
    }

    private void CheckIsScrolling() {
        isScrolling = scrollClickPrevention.isScrolling;
    }

}
