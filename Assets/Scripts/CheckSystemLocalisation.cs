using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSystemLocalisation : MonoBehaviour{

    public string currentLanguage = "";

    private void Start() {
        currentLanguage = PlayerPrefs.GetString("Language", "");

        if (currentLanguage == "") {
            Debug.Log("No Current Language");
            CheckSystemLanguage();
        }
        else {
            Debug.Log("Current Language: " + currentLanguage);
        }

    }


    private void CheckSystemLanguage() {
        Debug.Log("System Language: "+ Application.systemLanguage);

        switch (Application.systemLanguage) {
            case SystemLanguage.French:
                currentLanguage = "French";
                PlayerPrefs.SetString("Language", "French");
                break;
            case SystemLanguage.Spanish:
                currentLanguage = "Spanish";
                PlayerPrefs.SetString("Language", "Spanish");
                break;
            case SystemLanguage.German:
                currentLanguage = "German";
                PlayerPrefs.SetString("Language", "German");
                break;
            case SystemLanguage.Dutch:
                currentLanguage = "Dutch";
                PlayerPrefs.SetString("Language", "Dutch");
                break;
            case SystemLanguage.Russian:
                currentLanguage = "Russian";
                PlayerPrefs.SetString("Language", "Russian");
                break;
            case SystemLanguage.Thai:
                currentLanguage = "Thai";
                PlayerPrefs.SetString("Language", "Thai");
                break;
            case SystemLanguage.Japanese:
                currentLanguage = "Japan";
                PlayerPrefs.SetString("Language", "Japan");
                break;
            case SystemLanguage.Portuguese:
                currentLanguage = "Portueguese";
                PlayerPrefs.SetString("Language", "Portueguese");
                break;
            case SystemLanguage.Chinese:
                currentLanguage = "Chinese";
                PlayerPrefs.SetString("Language", "Chinese");
                break;
            case SystemLanguage.ChineseSimplified:
                currentLanguage = "Chinese";
                PlayerPrefs.SetString("Language", "Chinese");
                break;
            case SystemLanguage.ChineseTraditional:
                currentLanguage = "Chinese";
                PlayerPrefs.SetString("Language", "Chinese");
                break;
            case SystemLanguage.Italian:
                currentLanguage = "Italian";
                PlayerPrefs.SetString("Language", "Italian");
                break;
            case SystemLanguage.Indonesian:
                currentLanguage = "Indonesian";
                PlayerPrefs.SetString("Language", "Indonesian");
                break;
            case SystemLanguage.Turkish:
                currentLanguage = "Turkish";
                PlayerPrefs.SetString("Language", "Turkish");
                break;
            case SystemLanguage.Korean:
                currentLanguage = "Korean";
                PlayerPrefs.SetString("Language", "Korean");
                break;
            default:
                currentLanguage = "English";
                PlayerPrefs.SetString("Language", "English");
                break;

        }
    }
}
