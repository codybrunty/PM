using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DailyReward : MonoBehaviour {
    public Button timerButton;
    public TextMeshProUGUI timer_text;
    public int hours;
    public int minutes;
    public int seconds;
    private bool _timerComplete = false;
    private bool _timerIsReady;
    private TimeSpan _startTime;
    private TimeSpan _endTime;
    private TimeSpan _remainingTime;
    private TimeSpan _remainingTimeDisplay;
    public CoinTotal coinTotal;
    private float _value = 1f;
    public int RewardToEarn;
    public GameObject alertImg;
    public Image footerBorder;
    public TextMeshProUGUI rewardText;
    bool reward = false;
    bool first = true;
    public GameObject flowerDisplayGroup = default;
    public GameObject popEffectPrefab = default;
    public List<GameObject> flowerDisplayFlowers = new List<GameObject>();
    private List<GameObject> unlockedFlowerDisplayFlowers = new List<GameObject>();
    public Sprite activeFooterBorderSprite = default;
    public Sprite inactiveFooterBorderSprite = default;
    public Sprite activeButton = default;
    public Sprite inactiveButton = default;
    public Image buttonShadow = default;
    public bool mainMenuTutorialPassed = false;
    public GameObject sparks = default;

    void Start() {
        CheckMainMenuTutorialStatus();
        SetRewardToEarn();
        StartReward();
    }

    private void CheckMainMenuTutorialStatus() {
        int tutorialCheck = PlayerPrefs.GetInt("MainMenuTutorial", 0);
        if (tutorialCheck == 0) {
            mainMenuTutorialPassed = false;
        }
        if (tutorialCheck == 1) {
            mainMenuTutorialPassed = true;
        }
    }

    private void SetRewardToEarn() {
        int secretFlowersDiscovered = GetSecretFlowersDiscovered();
        RewardToEarn += (15 * secretFlowersDiscovered);
        rewardText.text = RewardToEarn.ToString();
    }

    private int GetSecretFlowersDiscovered() {
        int secretFlowersCompleted = 0;

        for (int i = 0; i< GameDataControl.gdControl.blockSecretsUnlocked.Count; i++) {
            int secretOne = GameDataControl.gdControl.blockSecretsUnlocked[i][0];
            int secretTwo = GameDataControl.gdControl.blockSecretsUnlocked[i][1];
            int secretThree = GameDataControl.gdControl.blockSecretsUnlocked[i][2];
            int secretFour = GameDataControl.gdControl.blockSecretsUnlocked[i][3];
            if (secretOne == 1 && secretTwo == 1 && secretThree == 1 && secretFour == 1) {
                secretFlowersCompleted++;
            }
        }

        return secretFlowersCompleted;
    }

    private void StartReward() {
        if (PlayerPrefs.GetString("_timer") == "") {
            enableButton();
            StartCoroutine(EnableFlowerDisplay());
        }
        else {
            disableButton();
            StartCoroutine("CheckTime");
        }
    }

    private void EnableAlertImage() {
        if (mainMenuTutorialPassed) {
            alertImg.SetActive(true);
            StartCoroutine(HoldPlaySFX());
        }
    }

    private IEnumerator HoldPlaySFX() {
        yield return new WaitForSeconds(.3f);
        FindObjectOfType<SoundManager>().PlaySound("Alert");
    }

    private void DisableAlertImage() {
        alertImg.SetActive(false);
    }
    
    //update the time information with what we got from the internet
    private void updateTime() {
        if (PlayerPrefs.GetString("_timer") == "Standby") {
            PlayerPrefs.SetString("_timer", TimeManager.sharedInstance.getCurrentTimeNow());
            PlayerPrefs.SetInt("_date", TimeManager.sharedInstance.getCurrentDateNow());
        }
        else if (PlayerPrefs.GetString("_timer") != "" && PlayerPrefs.GetString("_timer") != "Standby") {
            int _old = PlayerPrefs.GetInt("_date");
            int _now = TimeManager.sharedInstance.getCurrentDateNow();
            
            //check if a day as passed
            if (_now > _old) {
                Debug.Log("Day has passed");
                enableButton();
                timer_text.text = LocalisationSystem.GetLocalisedValue("storeMenu_free");
                RewardEffectsEnabled();
                _timerIsReady = false;
                return;

            }
            else if (_now == _old) {//same day
                Debug.Log("Same Day - configuring now");
                _configTimerSettings();
                return;
            }
            else {
                Debug.Log("error with date");
                return;
            }
        }
        Debug.Log("Day had passed - configuring now");
        _configTimerSettings();
    }
    
    //update the time information with what we got some the internet
    private void _configTimerSettings() {
        _startTime = TimeSpan.Parse(PlayerPrefs.GetString("_timer"));
        _endTime = TimeSpan.Parse(hours + ":" + minutes + ":" + seconds);
        TimeSpan temp = TimeSpan.Parse(TimeManager.sharedInstance.getCurrentTimeNow());
        TimeSpan diff = temp.Subtract(_startTime);
        _remainingTime = _endTime.Subtract(diff);
        _remainingTimeDisplay = _endTime.Subtract(diff);

        //start timmer where we left off
        setProgressWhereWeLeftOff();
        if (diff >= _endTime) {
            _timerComplete = true;
            _timerIsReady = false;
            enableButton();
        }
        else {
            _timerComplete = false;
            disableButton();
            _timerIsReady = true;
        }
    }
    
    //initializing timer
    private void setProgressWhereWeLeftOff() {
        float ah = 1f / (float)_endTime.TotalSeconds;
        float bh = 1f / (float)_remainingTime.TotalSeconds;
        _value = (float)_remainingTime.TotalSeconds;
        timer_text.text = _value.ToString();
        RewardEffectsDisabled();
        //Debug.Log(_value);
        if (_value <= 0) {
            timer_text.text = LocalisationSystem.GetLocalisedValue("storeMenu_free");
            RewardEffectsEnabled();
        }
    }

    private void RewardEffectsEnabled() {
        if (!reward||first) {
            first = false;
            reward = true;
            //Debug.LogWarning("enable");
            rewardText.color = new Color(235f / 255f, 229f / 255f, 98f / 255f);
            EnableMainButton();
            StartCoroutine(EnableFlowerDisplay());
        }
    }

    private void EnableSparks() {
        sparks.SetActive(true);
    }

    private void EnableMainButton() {
        gameObject.GetComponent<Image>().sprite = activeButton;
        footerBorder.sprite = activeFooterBorderSprite;
        buttonShadow.enabled = true;
        EnableSparks();
    }

    private void DisableMainButton() {
        gameObject.GetComponent<Image>().sprite = inactiveButton;
        footerBorder.sprite = inactiveFooterBorderSprite;
        buttonShadow.enabled = false;
        DisableSparks();
    }

    IEnumerator DisableFlowerDisplay(){
        //Debug.Log("Flowers Display Off");
        int flowerTotal = 0;

        foreach (Transform child in flowerDisplayGroup.transform) {
            flowerTotal++;
            Transform flower = child.GetChild(0);
            flower.gameObject.GetComponent<Animator>().SetTrigger("Reverse");
            GameObject popEffect = Instantiate(popEffectPrefab, flower.position, Quaternion.identity, child);

            //50 flowers, should run 10 times
            if (flowerTotal % 5 == 0) {
                AddCoinsToPlayerData(RewardToEarn/10);
                coinTotal.AddToBank(RewardToEarn / 10);
            }
            

            Destroy(flower.gameObject, 1f);
            Destroy(popEffect, 1f);
            yield return new WaitForSeconds(.01f);
        }



        SaveCoinsAddedToPlayerData();
    }

    IEnumerator EnableFlowerDisplay(){
        //Debug.Log("Flowers Display On");

        CreateNewFlowerDisplayList();

        foreach (Transform child in flowerDisplayGroup.transform) {

            int flowerIndex = UnityEngine.Random.Range(0, unlockedFlowerDisplayFlowers.Count);
            Instantiate(unlockedFlowerDisplayFlowers[flowerIndex], child.position, Quaternion.identity, child);
            yield return new WaitForSeconds(.01f);
        }
    }

    private void CreateNewFlowerDisplayList(){
        unlockedFlowerDisplayFlowers.Clear();
        List<int> secretsFullyUnlocked = GameDataControl.gdControl.GetFullSecretsUnlocked();

        unlockedFlowerDisplayFlowers.Add(flowerDisplayFlowers[0]);
        for (int i=0; i < secretsFullyUnlocked.Count;i++){
            if (secretsFullyUnlocked[i] == 1){
                //Add +1 because 0 is the default coin image and not a secret flower.
                unlockedFlowerDisplayFlowers.Add(flowerDisplayFlowers[i+1]);
            }
        }

    }

    private void RewardEffectsDisabled() {
        if (reward||first) {
            first = false;
            reward = false;
            //Debug.LogWarning("disable");
            rewardText.color = new Color(167f / 255f, 163f / 255f, 85f / 255f);
            DisableMainButton();
        }
    }

    private void DisableSparks() {
        sparks.SetActive(false);
    }

    //enable button function
    private void enableButton() {
        EnableAlertImage();
        timerButton.interactable = true;
        //timeLabel.text = "25 Free Coins!";
    }

    //disable button function
    private void disableButton() {
        DisableAlertImage();
        timerButton.interactable = false;
        //timeLabel.text = "Free Coins Soon!";
    }
    
    //Check the current time before any task.
    private IEnumerator CheckTime() {
        disableButton();
        timer_text.text = LocalisationSystem.GetLocalisedValue("storeMenu_server");
        Debug.Log("Getting Time For Daily Reward");

        yield return StartCoroutine(TimeManager.sharedInstance.getTime());

        updateTime();
        Debug.Log("Time retrieved!");
    }
    
    //on button click
    public void rewardClicked() {
        Debug.Log("Daily Reward Claim Button Clicked");
        claimReward(RewardToEarn);
        PlayerPrefs.SetString("_timer", "Standby");
        StartCoroutine("CheckTime");
    }

    //validate
    private void validateTime() {
        Debug.Log("Validating time to make sure no speed hack!");
        StartCoroutine("CheckTime");
    }
    
    //countdown
    void Update() {
        if (_timerIsReady) {
            if (!_timerComplete && PlayerPrefs.GetString("_timer") != "") {
                //_value -= Time.deltaTime * 1f / (float)_endTime.TotalSeconds;
                //Debug.Log(_value);
                _value -= Time.deltaTime;
                float subtractNum = ((float)Time.deltaTime * 1f);
                string subtractNumString = subtractNum.ToString("0.0000");
                TimeSpan tmp = TimeSpan.Parse(0 + ":" + 0 + ":" + subtractNumString);
                _remainingTimeDisplay = _remainingTimeDisplay.Subtract(tmp);
                TimeSpan _remainingTimeDisplay_tmp = TimeSpan.Parse(_remainingTimeDisplay.Hours + ":" + _remainingTimeDisplay.Minutes + ":" + _remainingTimeDisplay.Seconds);
                string output = String.Format("{0:c}", _remainingTimeDisplay_tmp);
                //Debug.Log(output);
                timer_text.text = output;
                RewardEffectsDisabled();
                //thRewardEffectsEnabled();is is called once only
                if (_value <= 0 && !_timerComplete) {
                    //when the timer hits 0, let do a quick validation to make sure no speed hacks.
                    validateTime();
                    _timerComplete = true;
                    timer_text.text = LocalisationSystem.GetLocalisedValue("storeMenu_free");
                    //RewardEffectsEnabled();
                }
            }
        }
    }

    private void claimReward(int x) {
        Debug.Log("YOU Received " + x + " Coins");
        FreeCoinsCounter();
        StartCoroutine(DisableFlowerDisplay());
    }

    private void FreeCoinsCounter() {
        int freeCoinsCounter = PlayerPrefs.GetInt("FreeCoinsCounter", 0);
        freeCoinsCounter++;
        PlayerPrefs.SetInt("FreeCoinsCounter", freeCoinsCounter);
    }

    private static void AddCoinsToPlayerData(int x) {
        Debug.Log(x + " coins added to PlayerData.");
        GameDataControl.gdControl.coinsEarned += x;
        GameDataControl.gdControl.coinsTotal += x;
    }

    private void SaveCoinsAddedToPlayerData(){
        GameDataControl.gdControl.SavePlayerData();
        GameDataControl.gdControl.LoadPlayerData();
    }

    void OnApplicationFocus(bool hasFocus) {
        if (hasFocus) {
            if (_timerIsReady) {
                Debug.Log("Has Focus, Reconfiguring Reward Timer Display.");
                StartReward();
            }
        }
    }
}


