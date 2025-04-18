using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LangaugeButtonSpecificMechanics : MonoBehaviour{

    public string languageKey = default;
    private string currentLanguage = default;

    private void Start() {
        currentLanguage = PlayerPrefs.GetString("Language", "English");
        if (languageKey == currentLanguage) {
            gameObject.GetComponent<Button>().interactable = false;
            gameObject.GetComponent<Image>().raycastTarget = false;
            gameObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color(47f / 255f, 58f / 255f, 63f / 255f, 255f);
        }
    }

    //for buttons inside the drop down
    public void LoadSpecificLanguage() {
        if (languageKey != "") {
            FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
            PlayerPrefs.SetString("Language", languageKey);
            //SceneManager.LoadScene("Menu_Level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
