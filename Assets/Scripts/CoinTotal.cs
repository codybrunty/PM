using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CoinTotal : MonoBehaviour {

    int coins;
    [SerializeField] TextMeshProUGUI bankText = default;
    [SerializeField] float pos_X_min=-0.5f;
    [SerializeField] float pos_X_max=0.5f;
    [SerializeField] float pos_Y_min=-0.2f;
    [SerializeField] float pos_Y_max=0.1f;
    private Vector3 originalScale;
    private Vector3 destinationScale;
    private float currentTime = 0.0f;
    #pragma warning disable
    private bool last = false;
    #pragma warning restore
    private float duration = .22f;
    private bool coinSound = false;

    [SerializeField] GameObject bankEffect = default;
    [SerializeField] GameObject bankEffectHold = default;
    [SerializeField] GameObject bankEffectPosition = default;
    [SerializeField] AudioClip coinClip;

    private int byOneTotal = 0;


    private void Start() {
        UpdateCoinsTotalText();
        originalScale = gameObject.transform.localScale;
        destinationScale = new Vector3(originalScale.x + 1f, originalScale.y + 1f, originalScale.z + 1f);
    }

    public void UpdateCoinsTotalText() {
        Debug.Log("Update Bank to PlayerData");
        //FloatingTextEffect(purchasePrice, false, false);
        bankText.text = (GameDataControl.gdControl.coinsTotal).ToString();
    }

    public void UpdateCoinsTotalTextAndFlyingTextAnim(int coinsAdded) {
        Debug.Log("Update Bank to PlayerData");
        FloatingTextEffect(coinsAdded, true, false);
        bankText.text = (GameDataControl.gdControl.coinsTotal).ToString();
    }

    public void SubtractFromBank(int purchasePrice) {
        StartCoroutine(SubtractBankAnimation(purchasePrice));
    }

    public void AddToBank(int number) {
        StartCoroutine(AddBankAnimation(number));
    }
    
    IEnumerator AddBankAnimation(int number) {
        float time = 0.0f;
        float allTime = .1f;
        do {
            time += Time.deltaTime;
            gameObject.transform.localScale = Vector3.Lerp(originalScale, destinationScale, time);
            yield return null;
        }
        while (time <= allTime);

        bankText.text = (GameDataControl.gdControl.coinsTotal).ToString();
        Vector3 tempDestinationScale = gameObject.transform.localScale;
        FindObjectOfType<SoundManager>().PlayOneShotSound("coinSFX");

        FloatingTextEffect(number, true, false);

        float newTime = 0.0f;
        float newDuration = .1f;
        do {
            newTime += Time.deltaTime;
            gameObject.transform.localScale = Vector3.Lerp(tempDestinationScale, originalScale, newTime);
            yield return null;
        }
        while (newTime <= newDuration);
        gameObject.transform.localScale = originalScale;
    }

    IEnumerator SubtractBankAnimation(int purchasePrice) {
        float time = 0.0f;
        float allTime = .1f;
        do {
            time += Time.deltaTime;
            gameObject.transform.localScale = Vector3.Lerp(originalScale, destinationScale, time);
            yield return null;
        }
        while (time <= allTime);

        bankText.text = (GameDataControl.gdControl.coinsTotal).ToString();
        Vector3 tempDestinationScale = gameObject.transform.localScale;
        FindObjectOfType<SoundManager>().PlayOneShotSound("coinSFX");
        FloatingTextEffect(purchasePrice, false, false);

        float newTime = 0.0f;
        float newDuration = .1f;
        do {
            newTime += Time.deltaTime;
            gameObject.transform.localScale = Vector3.Lerp(tempDestinationScale, originalScale, newTime);
            yield return null;
        }
        while (newTime <= newDuration);
        gameObject.transform.localScale = originalScale;
    }

    public void UpdateCoinsTotalTextByOne(int rewardTotal) {
        //increment bank total instead of just 1s
        byOneTotal++;
        //FloatingTextEffect(byOneTotal, true, false);

        int total = int.Parse(bankText.text);
        total += 1;
        bankText.text = total.ToString();

        if (!coinSound) {
            coinSound = true;
            FindObjectOfType<SoundManager>().PlayOneShotSound("coinSFX");
            StartCoroutine(DelayNextCoinSFX());
        }

        if (rewardTotal==byOneTotal) {
            FloatingTextEffect(byOneTotal, true, true);
            //Debug.Log("LAST");
            ResetByOneTotal();
        }
        else {
            //Debug.Log("NOT LAST");
            FloatingTextEffect(byOneTotal, true, false);
        }
    }

    public void ResetByOneTotal() {
        byOneTotal = 0;
    }

    IEnumerator DelayNextCoinSFX() {
        yield return new WaitForSeconds(0.1f);
        coinSound = false;
    }

    IEnumerator BankAnim() {
        Debug.Log("BankAnim!");
        yield return new WaitForSeconds(.4f);

        do {
            currentTime += Time.deltaTime;
            gameObject.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime);
            yield return null;
        }
        while (currentTime <= duration);


        Vector3 tempDestinationScale = gameObject.transform.localScale;
        float newDuration = .15f;
        float newCurrentTime = 0.0f;

        do {
            newCurrentTime += Time.deltaTime;
            gameObject.transform.localScale = Vector3.Lerp(tempDestinationScale, originalScale, newCurrentTime);
            yield return null;
        }
        while (newCurrentTime <= newDuration);
        gameObject.transform.localScale = originalScale;
    }



    public void FloatingTextEffect(int number, bool positive, bool last) {
        Vector3 newPosition = bankEffectPosition.transform.position;
        newPosition = newPosition + new Vector3(UnityEngine.Random.Range(pos_X_min, pos_X_max), UnityEngine.Random.Range(pos_Y_min, pos_Y_max), 0);
        GameObject floatingText;
        if (last) {
            floatingText = Instantiate(bankEffectHold, newPosition, Quaternion.identity, bankEffectPosition.transform);
        }
        else {
            floatingText = Instantiate(bankEffect, newPosition, Quaternion.identity, bankEffectPosition.transform);
        }

        if (positive) {
            floatingText.GetComponent<TextMesh>().text = "+" + number;
        }
        else {
            floatingText.GetComponent<TextMesh>().text = "-" + number;
        }
    }

}
