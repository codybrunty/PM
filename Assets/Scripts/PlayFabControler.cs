using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using FSG.iOSKeychain;

public class PlayFabControler : MonoBehaviour {

    public static PlayFabControler PFC;


    public string myID;
    public string userEmail;
    public string userPassword;
    public string username;

    public bool loginStatus = false;




    private void Awake() { 

        if (PFC == null) {
            DontDestroyOnLoad(gameObject);
            PFC = this;
        }
        else if (PFC != this) {
            Destroy(gameObject);
        }
    }

    public void Start() {

        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) {
            PlayFabSettings.TitleId = "31704";
        }

        PlayfabsLogin();

    }

    private void PlayfabsLogin() {
        //PlayerPrefs.DeleteKey("USERNAME");
        //PlayerPrefs.DeleteKey("EMAIL");
        //PlayerPrefs.DeleteKey("PASSWORD");
        //PlayerPrefs.DeleteKey("firstTimePlayFabLogin");

        if (PlayerPrefs.HasKey("EMAIL")) {
            username = PlayerPrefs.GetString("USERNAME");
            userEmail = PlayerPrefs.GetString("EMAIL");
            userPassword = PlayerPrefs.GetString("PASSWORD");
            var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        }
        else {
#if UNITY_ANDROID
            var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginMobileSuccess, OnLoginMobileFailure);
#endif
#if UNITY_IOS
            var requestIOS = new LoginWithIOSDeviceIDRequest { DeviceId = ReturnMobileIOSID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, OnLoginMobileSuccess, OnLoginMobileFailure);
#endif
        }
    }

    private void OnLoginSuccess(LoginResult result) {
        loginStatus = true;
        Debug.Log("Congratulations, Login Success");
        //Debug.Log("Email: " + userEmail);
        //Debug.Log("Password: " + userPassword);
        PlayerPrefs.SetString("USERNAME", username);
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        myID = result.PlayFabId;
        GameDataControl.gdControl.GetPlayFabGameDataAndSync();
    }

    private void OnLoginMobileSuccess(LoginResult result) {
        loginStatus = true;
        Debug.Log("Congratulations, Login Mobile Success");
        myID = result.PlayFabId;
        CheckFirstTimePlayFabLogin(result);
    }

    private void OnLoginFailure(PlayFabError error) {
        Debug.Log("Email Login Failure try just logging into device");
        Debug.LogError(error.GenerateErrorReport());
#if UNITY_ANDROID
        var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginMobileSuccess, OnLoginMobileFailure);
#endif
#if UNITY_IOS
            var requestIOS = new LoginWithIOSDeviceIDRequest { DeviceId = ReturnMobileIOSID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, OnLoginMobileSuccess, OnLoginMobileFailure);
#endif
    }

    private void OnLoginMobileFailure(PlayFabError error) {
        Debug.Log("Login Mobile Failure");
        Debug.LogError(error.GenerateErrorReport());
    }

    public static string ReturnMobileID(){
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }

    public static string ReturnMobileIOSID(){
        string deviceID = Keychain.GetValue("iosDeviceID");
        if (deviceID == "") {
            deviceID = SystemInfo.deviceUniqueIdentifier;
            Keychain.SetValue("iosDeviceID", deviceID);
        }
        return deviceID;
    }


    public void OnClickSetupAccount() {
        if (username !=null && username != "") {
            if (loginStatus) {
                var addLoginRequest = new AddUsernamePasswordRequest { Email = userEmail, Password = userPassword, Username = username };
                PlayFabClientAPI.AddUsernamePassword(addLoginRequest, OnAddLoginSuccess, OnAddLoginFailure);
            }
            else {
                FindObjectOfType<PlayfabMenus>().ClearErrors();
                FindObjectOfType<PlayfabMenus>().ErrorFeedback();
                FindObjectOfType<PlayfabMenus>().ShowConnectionError();
            }
        }
        else {
            FindObjectOfType<PlayfabMenus>().ClearErrors();
            FindObjectOfType<PlayfabMenus>().ErrorFeedback();
            FindObjectOfType<PlayfabMenus>().ShowUsernameError();
        }
    }

    private void OnAddLoginSuccess(AddUsernamePasswordResult result) {
        Debug.Log("Congratulations, Add Login Success");
        PlayerPrefs.SetString("USERNAME", username);
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        FindObjectOfType<ProfileButtonCommand>().Profile_Cancel();
        FindObjectOfType<TitleMechanics>().UpdateTitle(0);
    }

    private void OnAddLoginFailure(PlayFabError error) {
        Debug.Log("Add Login Failure");

        FindObjectOfType<PlayfabMenus>().ClearErrors();
        FindObjectOfType<PlayfabMenus>().ErrorFeedback();

        if (error.ErrorDetails != null) {
            foreach (var x in error.ErrorDetails) {
                if (x.Key == "Username") {
                    FindObjectOfType<PlayfabMenus>().ShowUsernameError();
                }
                if (x.Key == "Email") {
                    FindObjectOfType<PlayfabMenus>().ShowEmailError();
                }
                if (x.Key == "Password") {
                    FindObjectOfType<PlayfabMenus>().ShowPasswordError();
                }
                if (x.Key != "Username" && x.Key != "Email" && x.Key != "Password") {
                    FindObjectOfType<PlayfabMenus>().ShowUsernameError();
                    FindObjectOfType<PlayfabMenus>().ShowEmailError();
                    FindObjectOfType<PlayfabMenus>().ShowPasswordError();
                }
            }
        }
        else {
            FindObjectOfType<PlayfabMenus>().ShowUsernameError();
            FindObjectOfType<PlayfabMenus>().ShowEmailError();
            FindObjectOfType<PlayfabMenus>().ShowPasswordError();

        }
    }

    public void OnClickRecoverAccount() {
        if (username != null && username != "") {
            if (loginStatus) {
                var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
                PlayFabClientAPI.LoginWithEmailAddress(request, OnRecoverLoginSuccess, OnRecoverLoginFail);
            }
            else {
                FindObjectOfType<PlayfabMenus>().ClearErrors();
                FindObjectOfType<PlayfabMenus>().ErrorFeedback();
                FindObjectOfType<PlayfabMenus>().ShowConnectionError();
            }
        }
        else {
            FindObjectOfType<PlayfabMenus>().ClearErrors();
            FindObjectOfType<PlayfabMenus>().ErrorFeedback();
            FindObjectOfType<PlayfabMenus>().ShowUsernameError();
        }
    }

    private void OnRecoverLoginSuccess(LoginResult result) {
        Debug.Log("Congratulations, Login Success");
        PlayerPrefs.SetString("USERNAME", username);
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        LinkNewDeviceID();
        myID = result.PlayFabId;
        FindObjectOfType<ProfileButtonCommand>().Profile_Cancel();
        FindObjectOfType<TitleMechanics>().UpdateTitle(0);
        GameDataControl.gdControl.GetPlayFabGameDataAndLoad();
    }

    private void OnRecoverLoginFail(PlayFabError error) {
        Debug.Log("Login Failed to Recover");

        FindObjectOfType<PlayfabMenus>().ClearErrors();
        FindObjectOfType<PlayfabMenus>().ErrorFeedback();

        Debug.Log(error.ErrorMessage);


        if (error.ErrorDetails != null) {
            foreach (var x in error.ErrorDetails) {
                Debug.Log(x.Key);
                if (x.Key == "Username") {
                    FindObjectOfType<PlayfabMenus>().ShowUsernameError();
                }
                if (x.Key == "Email") {
                    FindObjectOfType<PlayfabMenus>().ShowEmailError();
                }
                if (x.Key == "Password") {
                    FindObjectOfType<PlayfabMenus>().ShowPasswordError();
                }
                if (x.Key != "Username" && x.Key != "Email" && x.Key != "Password") {
                    FindObjectOfType<PlayfabMenus>().ShowEmailError();
                    FindObjectOfType<PlayfabMenus>().ShowPasswordError();
                }
            }
        }
        else {
            FindObjectOfType<PlayfabMenus>().ShowEmailError();
            FindObjectOfType<PlayfabMenus>().ShowPasswordError();
        }
    }

    private void LinkNewDeviceID() {
#if UNITY_ANDROID
        var linkAndroidRequest = new LinkAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), ForceLink = true };
        PlayFabClientAPI.LinkAndroidDeviceID(linkAndroidRequest, OnAndroidLinkSuccess, OnLinkFail);
#endif
#if UNITY_IOS
        var linkIOSRequest = new LinkIOSDeviceIDRequest { DeviceId = ReturnMobileIOSID(), ForceLink = true };
        PlayFabClientAPI.LinkIOSDeviceID(linkIOSRequest, OnIOSLinkSuccess, OnLinkFail);
#endif
    }

    private void OnAndroidLinkSuccess(LinkAndroidDeviceIDResult result) {
        Debug.Log("Android Link Success");
    }

    private void OnIOSLinkSuccess(LinkIOSDeviceIDResult result) {
        Debug.Log("IOS Link Success");
    }

    private void OnLinkFail(PlayFabError error) {
        Debug.Log("Link Failed");
        Debug.LogError(error.GenerateErrorReport());
    }

    private void CheckFirstTimePlayFabLogin(LoginResult result) {

        int firstTime = PlayerPrefs.GetInt("firstTimePlayFabLogin", 0);

        if (firstTime == 0) {
            Debug.Log("FirstTimeCheck");
            PlayerPrefs.SetInt("firstTimePlayFabLogin", 1);
            GameDataControl.gdControl.GetPlayFabGameDataAndSync();
        }
        else {
            Debug.Log("Not First PlayFab Login");
        }
    }

}

