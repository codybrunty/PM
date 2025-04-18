using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEffects : MonoBehaviour{

    [SerializeField] CoinTotal bank = default;
    public ScreenShake screenShake;
    public float duration = .15f;
    public float magnitude = .4f;

    public void EffectsOnClick() {
        //SoundManager.PlaySound("coinSFX");
        FindObjectOfType<SoundManager>().PlaySound("coinSFX");
        bank.FloatingTextEffect(5, true,false );
        StartCoroutine(screenShake.Shake(duration, magnitude));
    }

}
