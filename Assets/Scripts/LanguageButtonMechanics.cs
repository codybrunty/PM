using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguageButtonMechanics : MonoBehaviour{
    [SerializeField] GameObject languageMenu = default;

    private void PlayClickSound() {
        FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
    }

    public void LanguageMenu_Activate() {
        PlayClickSound();
        languageMenu.GetComponent<Animator>().SetBool("LanguageDropDown", true);
    }

    public void LanguageMenu_Cancel() {
        PlayClickSound();
        languageMenu.GetComponent<Animator>().SetBool("LanguageDropDown", false);
    }


}
