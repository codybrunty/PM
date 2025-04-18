using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine.UI;
using TMPro;

public class PlayfabMenus : MonoBehaviour{

    private string userEmail;
    private string userPassword;
    private string username;

    public GameObject setupAccountButton;
    public TextMeshProUGUI setupText;
    public GameObject recoverAccountButton;
    public TextMeshProUGUI recoverText;



    [SerializeField] GameObject usernameError = default;
    [SerializeField] GameObject emailError = default;
    [SerializeField] GameObject passwordError = default;
    [SerializeField] ScreenShake screenShake = default;
    [SerializeField] GameObject connectionError = default;


    public void OnClickSetup() {
        PlayFabControler.PFC.username = username;
        PlayFabControler.PFC.userEmail = userEmail;
        PlayFabControler.PFC.userPassword = userPassword;
        PlayFabControler.PFC.OnClickSetupAccount();
    }

    public void OnClickRecover() {
        PlayFabControler.PFC.username = username;
        PlayFabControler.PFC.userEmail = userEmail;
        PlayFabControler.PFC.userPassword = userPassword;
        PlayFabControler.PFC.OnClickRecoverAccount();
    }

    public void OnSetupRecoverSuccess() {
        gameObject.GetComponent<ProfileButtonCommand>().Profile_Cancel();
    }

    public void ErrorFeedback() {
        FindObjectOfType<SoundManager>().PlaySound("neg1");
        StartCoroutine(screenShake.Shake(.1f, 10f));
    }

    public void ClearErrors() {
        usernameError.SetActive(false);
        emailError.SetActive(false);
        passwordError.SetActive(false);
    }
    public void ShowConnectionError() {
        connectionError.SetActive(true);
        recoverAccountButton.GetComponent<Button>().interactable = false;
        recoverAccountButton.GetComponent<Image>().raycastTarget = false;
        setupAccountButton.GetComponent<Button>().interactable = false;
        setupAccountButton.GetComponent<Image>().raycastTarget = false;
        setupText.text = "";
        recoverText.text = "";

    }
    public void ShowUsernameError() {
        usernameError.SetActive(true);
    }
    public void ShowEmailError() {
        emailError.SetActive(true);
    }
    public void ShowPasswordError() {
        passwordError.SetActive(true);
    }

    public void GetUserEmail(string emailIn) {
        userEmail = emailIn;
    }

    public void GetUserPassword(string passwordIn) {
        userPassword = passwordIn;
    }

    public void GetUserName(string usernameIn) {
        username = usernameIn;
    }

}
