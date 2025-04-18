using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCredits : MonoBehaviour{
    [SerializeField] GameObject creditsMenu = default;

    public void CreditsOnClick() {
        PlayClickSound();
        creditsMenu.GetComponent<Animator>().SetBool("CreditsDropDown", true);
    }
    
    public void CreditsOnCancel() {
        PlayClickSound();
        creditsMenu.GetComponent<Animator>().SetBool("CreditsDropDown", false);
    }

    private void PlayClickSound() {
        FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
    }

}
