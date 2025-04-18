using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateAllPriceGrpShakeAttributes : MonoBehaviour{

    public float totalShakeTime = .2f;
    public float totalShakeMagnitude = 15f;
    private List<Transform> block_priceGRPs = new List<Transform>();
    private List<Transform> daily_priceGRPs = new List<Transform>();
    private List<Transform> extra_priceGRPs = new List<Transform>();

    private void Start() {
        FillPriceGRP();
        UpdateShakeAttributes();
    }

    private void UpdateShakeAttributes() {
        for (int i = 0; i < block_priceGRPs.Count; i++) {
            block_priceGRPs[i].gameObject.GetComponent<PurchaseBlock>().totalShakeMagnitude = totalShakeMagnitude;
            block_priceGRPs[i].gameObject.GetComponent<PurchaseBlock>().totalShakeTime = totalShakeTime;
        }
        for (int i = 0; i < daily_priceGRPs.Count; i++) {
            daily_priceGRPs[i].gameObject.GetComponent<PurchaseDailyBlock>().totalShakeMagnitude = totalShakeMagnitude;
            daily_priceGRPs[i].gameObject.GetComponent<PurchaseDailyBlock>().totalShakeTime = totalShakeTime;
        }
        for (int i = 0; i < extra_priceGRPs.Count; i++) {
            extra_priceGRPs[i].gameObject.GetComponent<PurchaseExtraBlock>().totalShakeMagnitude = totalShakeMagnitude;
            extra_priceGRPs[i].gameObject.GetComponent<PurchaseExtraBlock>().totalShakeTime = totalShakeTime;
        }

    }

    private void FillPriceGRP() {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < transforms.Length; i++) {
            if (transforms[i].name == "Price_GRP") {
                if (transforms[i].GetComponent<PurchaseBlock>() != null) {
                    block_priceGRPs.Add(transforms[i]);
                }
                else if (transforms[i].GetComponent<PurchaseDailyBlock>() != null) {
                    daily_priceGRPs.Add(transforms[i]);
                }
                else {
                    extra_priceGRPs.Add(transforms[i]);
                }
            }
        }
    }
}
