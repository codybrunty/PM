using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReultsHeader : MonoBehaviour {
    [SerializeField] TextMeshProUGUI resultsTitleText = default;
    private int index = 0;
    private List<string> funResultTitles = new List<string> { "resultMenu_Title_0", "resultMenu_Title_1", "resultMenu_Title_2", "resultMenu_Title_3", "resultMenu_Title_4", "resultMenu_Title_5" };

    public void ResultsHeaderOnClick() {
        resultsTitleText.text = LocalisationSystem.GetLocalisedValue(funResultTitles[index]);
        index++;
        if (index > funResultTitles.Count - 1) {
            index = 0;
        }
    }


}