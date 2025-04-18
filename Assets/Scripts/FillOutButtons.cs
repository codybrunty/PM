using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillOutButtons : MonoBehaviour{
    public ScrollRect scrollview;
    public ScrollRect scrollviewVertical;
    public ScrollClickPrevention scrollPrevent;
    public CoinTotal bank;
    public GameObject buttonListContent;


    public void FillOutChildren() {
        SetUpPropogateDrag();
        SetUpPropogateDragVertical();
        SetUpPriceGRP();
        SetupExpandableButtons();
        SetupHardDailyAdButtons();
        SetupHardDailyAdButtons();
    }
    private void SetupExpandableButtons() {
        ExpandableButton[] expands;
        expands = gameObject.GetComponentsInChildren<ExpandableButton>(true);

        for (int i = 0; i < expands.Length; i++) {
            expands[i].scrollClickPrevention = scrollPrevent;
        }

    }

    private void SetupHardDailyAdButtons() {
        HardFooter_AdManager[] hfs;
        hfs = gameObject.GetComponentsInChildren<HardFooter_AdManager>(true);

        for (int i = 0; i < hfs.Length; i++) {
            hfs[i].scrollClickPrevention = scrollPrevent;
        }

        Hard_AdManager[] hs;
        hs = gameObject.GetComponentsInChildren<Hard_AdManager>(true);

        for (int i = 0; i < hs.Length; i++) {
            hs[i].scrollClickPrevention = scrollPrevent;
        }
    }

    private void SetUpPriceGRP() {
        PurchaseBlock[] purchases;
        purchases = gameObject.GetComponentsInChildren<PurchaseBlock>(true);

        for (int i = 0; i < purchases.Length; i++) {
            purchases[i].scrollClickPrevention = scrollPrevent;
            purchases[i].buttonList_content = buttonListContent;
            purchases[i].bank = bank;
        }

        PurchaseDailyBlock[] dailyPurchases;
        dailyPurchases = gameObject.GetComponentsInChildren<PurchaseDailyBlock>(true);
        for (int i = 0; i < dailyPurchases.Length; i++) {
            dailyPurchases[i].scrollClickPrevention = scrollPrevent;
            dailyPurchases[i].buttonList_content = buttonListContent;
            dailyPurchases[i].bank = bank;
        }

        PurchaseExtraBlock[] extraPurchases;
        extraPurchases = gameObject.GetComponentsInChildren<PurchaseExtraBlock>(true);
        for (int i = 0; i < extraPurchases.Length; i++) {
            extraPurchases[i].scrollClickPrevention = scrollPrevent;
            extraPurchases[i].buttonList_content = buttonListContent;
            extraPurchases[i].bank = bank;
        }
    }

    private void SetUpPropogateDrag() {
        PropogateDrag[] drags;
        drags = gameObject.GetComponentsInChildren<PropogateDrag>(true);

        for (int i = 0; i < drags.Length; i++) {
            drags[i].scrollView = scrollview;
        }
    }

    private void SetUpPropogateDragVertical() {
        PropogateDragVertical[] dragsVert;
        dragsVert = gameObject.GetComponentsInChildren<PropogateDragVertical>(true);

        for (int i = 0; i < dragsVert.Length; i++) {
            dragsVert[i].scrollViewVertical = scrollviewVertical;
        }
    }

}
