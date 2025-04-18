using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiImageButton : Button {

    protected override void DoStateTransition(SelectionState state, bool instant) {
        var targetColor =
            state == SelectionState.Disabled ? colors.disabledColor :
            state == SelectionState.Highlighted ? colors.highlightedColor :
            state == SelectionState.Normal ? colors.normalColor :
            state == SelectionState.Pressed ? colors.pressedColor :
            state == SelectionState.Selected ? colors.selectedColor : Color.white;

        foreach (var graphic in gameObject.transform.parent.GetComponentsInChildren<Graphic>()) {
            if (graphic.gameObject.name == "PrizeWheel" || graphic.gameObject.name == "Button_PlayAd") {
                graphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
            }
        }
    }
}

