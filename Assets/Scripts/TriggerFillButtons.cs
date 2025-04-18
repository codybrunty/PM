using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerFillButtons : MonoBehaviour{

    public ScrollClickPrevention scrollPrevent;
    public ScrollRect scrollView;
    public ScrollRect scrollViewVertical;
    public CoinTotal bank;

    private void Awake() {
        FillOutButtons[] fills;
        fills = gameObject.GetComponentsInChildren<FillOutButtons>(true);

        for (int i = 0; i < fills.Length; i++) {
            fills[i].scrollview = scrollView;
            fills[i].scrollviewVertical = scrollViewVertical;
            fills[i].scrollPrevent = scrollPrevent;
            fills[i].bank = bank;
            fills[i].buttonListContent = gameObject;
            fills[i].FillOutChildren();
        }
    }
}
