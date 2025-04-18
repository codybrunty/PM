using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEngine.iOS;

public class RateMeButtonCommand : MonoBehaviour{

    public void RateButtonOnClick() {
        PlayClickSFX();
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + Application.identifier);
        PlayerPrefs.SetInt("hasRated", 1);
        TurnOffImageAndText();

#elif UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.apple.com/app/id1503400446");
        PlayerPrefs.SetInt("hasRated", 1);
        TurnOffImageAndText();
#endif
    }

    private void TurnOffImageAndText() {
        gameObject.GetComponent<Image>().enabled = false;
        TextMeshProUGUI[] texts = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in texts) {
            text.gameObject.SetActive(false);
        }
    }

    public void PlayClickSFX() {
        FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
    }
}
