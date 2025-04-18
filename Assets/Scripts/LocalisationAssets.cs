using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LocalisationAssets : MonoBehaviour{

    [SerializeField] TMP_FontAsset font_RU = default;
    [SerializeField] TMP_FontAsset font_TH = default;
    [SerializeField] TMP_FontAsset font_JP = default;
    [SerializeField] TMP_FontAsset font_CN = default;
    [SerializeField] TMP_FontAsset font_KO = default;
    public string currentLanguage;

    private void Awake() {
        currentLanguage = PlayerPrefs.GetString("Language", "English");
        LocalisationSystem.SetLocalisedLanguage(currentLanguage);
    }
    
    private void Start() {
        string currentLanguage = PlayerPrefs.GetString("Language", "English");

        if (currentLanguage == "Thai") {
            foreach (GameObject root in GameObject.FindObjectsOfType(typeof(GameObject))) {
                if (root.transform.parent == null) {
                    TextMeshProUGUI[] allTexts = root.GetComponentsInChildren<TextMeshProUGUI>(true);
                    foreach (TextMeshProUGUI text in allTexts) {
                        if (text.GetComponent<TextLocaliserUI>() != null) {
                            text.font = font_TH;
                        }
                    }
                }
            }
        }

        if (currentLanguage == "Russian") {
            foreach (GameObject root in GameObject.FindObjectsOfType(typeof(GameObject))) {
                if (root.transform.parent == null) {
                    TextMeshProUGUI[] allTexts = root.GetComponentsInChildren<TextMeshProUGUI>(true);
                    foreach (TextMeshProUGUI text in allTexts) {
                        if (text.GetComponent<TextLocaliserUI>() != null) {
                            text.font = font_RU;
                        }
                    }
                }
            }
        }

        if (currentLanguage == "Japan") {
            foreach (GameObject root in GameObject.FindObjectsOfType(typeof(GameObject))) {
                if (root.transform.parent == null) {
                    TextMeshProUGUI[] allTexts = root.GetComponentsInChildren<TextMeshProUGUI>(true);
                    foreach (TextMeshProUGUI text in allTexts) {
                        if (text.GetComponent<TextLocaliserUI>() != null) {
                            text.font = font_JP;
                            //if (text.name == "Description") {
                            //    text.alignment = TextAlignmentOptions.Left;
                            //}
                        }
                    }
                }
            }
        }

        if (currentLanguage == "Chinese") {
            foreach (GameObject root in GameObject.FindObjectsOfType(typeof(GameObject))) {
                if (root.transform.parent == null) {
                    TextMeshProUGUI[] allTexts = root.transform.GetComponentsInChildren<TextMeshProUGUI>(true);
                    foreach (TextMeshProUGUI text in allTexts) {
                        if (text.GetComponent<TextLocaliserUI>() != null) {
                            text.font = font_CN;
                            //if (text.name == "Description") {
                            //    text.alignment = TextAlignmentOptions.Left;
                            //}
                        }
                    }
                }
            }
        }

        if (currentLanguage == "Korean") {
            foreach (GameObject root in GameObject.FindObjectsOfType(typeof(GameObject))) {
                if (root.transform.parent == null) {
                    TextMeshProUGUI[] allTexts = root.GetComponentsInChildren<TextMeshProUGUI>(true);
                    foreach (TextMeshProUGUI text in allTexts) {
                        if (text.GetComponent<TextLocaliserUI>() != null) {
                            text.font = font_KO;
                            //if (text.name == "Description") {
                            //    text.alignment = TextAlignmentOptions.Left;
                            //}
                        }
                    }
                }
            }
        }
    }

}
