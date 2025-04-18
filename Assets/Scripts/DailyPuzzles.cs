using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class DailyPuzzles : MonoBehaviour{

    [SerializeField] List<GameObject> dailyPuzzle_BlockButtons = new List<GameObject>();
    [SerializeField] List<GameObject> extras_BlockButtons = new List<GameObject>();
    [SerializeField] Sprite unlockedSprite = default;
    [SerializeField] Sprite lockedSprite = default;
    [SerializeField] Sprite lockedSprite_Ad = default;
    [SerializeField] TextMeshProUGUI dateText = default;
    public int seed;
    private bool isScrolling = false;
    private bool isSwiping = false;
    public ScrollClickPrevention scrollClickPrevention;
    List<int> easyLevelsIndex = new List<int>();
    List<int> hardLevelsIndex = new List<int>();
    private int checkDateCounter = 0;
    public int numberOfEasyLevels;
    public int numberOfHardLevels;


    void Start() {
        scrollClickPrevention = gameObject.GetComponent<TriggerFillButtons>().scrollPrevent;
        SetupDailyBlocksWithGameData();
        NewDayCheck();
        SetUpExtras();
    }


    private void SetUpExtras() {
        UnlockExtraBlock1();
        SetExtraLevelOnClickNumbers();
        DisplayExtraBlock1LevelsWon();
    }

    public void UnlockExtraBlock(int extraBlockIndex) {
        UnlockExtraBlockLevels(extraBlockIndex);
        ChangeExtraBlockHeaderColor(extraBlockIndex);
        ChangeExtraBlockGameObjectIconsColor(extraBlockIndex);
        HideExtraBlockPriceGRP(extraBlockIndex);
        UnHideExtraBlockProgressGRP(extraBlockIndex);
        UpdateExtraBlockExpandButton(extraBlockIndex);
    }

    private void UnlockExtraBlock1() {
        bool extraBlock1Unlocked = GameDataControl.gdControl.extraBlock1Unlocked;
        if (!extraBlock1Unlocked) {
            SetExtraPricesText();
        }

        else {
            UnlockExtraBlockLevels(0);
            ChangeExtraBlockHeaderColor(0);
            ChangeExtraBlockGameObjectIconsColor(0);
            HideExtraBlockPriceGRP(0);
            UnHideExtraBlockProgressGRP(0);
            UpdateExtraBlockExpandButton(0);
        }
    }

    private void UpdateExtraBlockExpandButton(int blockIndex) {
        Transform[] transforms;
        transforms = extras_BlockButtons[blockIndex].GetComponentsInChildren<Transform>(true);

        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "Expand_Button") {
                transforms[x].gameObject.GetComponent<ExpandableButton>().locked = false;
            }
            if (transforms[x].name == "Footer") {
                transforms[x].gameObject.SetActive(false);
            }
        }
    }

    private void UnHideExtraBlockProgressGRP(int blockIndex) {
        Transform[] transforms;
        transforms = extras_BlockButtons[blockIndex].GetComponentsInChildren<Transform>(true);
        int progressGRPIndex = 0;
        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "levelBlock_Progress") {
                progressGRPIndex = x;
            }
        }
        GameObject progressGRP = transforms[progressGRPIndex].gameObject;
        progressGRP.SetActive(true);
    }

    private void HideExtraBlockPriceGRP(int blockIndex) {
        Transform[] transforms;
        transforms = extras_BlockButtons[blockIndex].GetComponentsInChildren<Transform>();

        int priceGRPIndex = 0;
        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "Price_GRP") {
                priceGRPIndex = x;
            }
        }

        GameObject priceGRP = transforms[priceGRPIndex].gameObject;
        priceGRP.SetActive(false);
    }

    private void ChangeExtraBlockGameObjectIconsColor(int blockIndex) {
        Transform[] transforms;
        transforms = extras_BlockButtons[blockIndex].GetComponentsInChildren<Transform>(true);

        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "RedHouse" || transforms[x].name == "BlueHouse" || transforms[x].name == "Water" || transforms[x].name == "Key" || transforms[x].name == "SecretFlower_QuestionMark") {
                transforms[x].gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        }
    }

    private void ChangeExtraBlockHeaderColor(int blockIndex) {
        Transform[] transforms;
        transforms = extras_BlockButtons[blockIndex].GetComponentsInChildren<Transform>(true);

        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "Expand_Button") {
                transforms[x].gameObject.GetComponent<Image>().sprite = unlockedSprite;
            }
        }
    }

    private void UnlockExtraBlockLevels(int blockIndex) {
        Button[] buttons;
        buttons = extras_BlockButtons[blockIndex].GetComponentsInChildren<Button>(true);

        for (int i = 0; i < buttons.Length; i++) {
            if (buttons[i].name.Contains("LevelButton")) {
                buttons[i].interactable = true;
                buttons[i].gameObject.GetComponent<Image>().raycastTarget = true;
            }
        }
    }

    private void DisplayExtraBlock1LevelsWon() {
        bool extraBlock1Unlocked = GameDataControl.gdControl.extraBlock1Unlocked;
        CheckExtraBlock1LevelResultsAndUpdateDisplay();
    }

    private void CheckExtraBlock1LevelResultsAndUpdateDisplay() {
        Button[] buttons;
        buttons = extras_BlockButtons[0].GetComponentsInChildren<Button>(true);
        int lvlsWonInBlock = 0;
        int totalLevelButtons = 0;

        for (int x = 0; x < buttons.Length; x++) {
            if (buttons[x].name.Contains("LevelButton")) {
                int levelIndex = totalLevelButtons;
                totalLevelButtons++;
                int levelResults = GameDataControl.gdControl.extra_level_block_1_results[levelIndex];
                if (levelResults == 1) {
                    lvlsWonInBlock++;
                    buttons[x].GetComponent<Image>().color = new Color(179f / 255f, 246f / 255f, 171f / 255f, 1);
                }
            }
        }

        TextMeshProUGUI[] texts;
        texts = extras_BlockButtons[0].GetComponentsInChildren<TextMeshProUGUI>(true);
        for (int x = 0; x < texts.Length; x++) {
            if (texts[x].name.Contains("progressText")) {
                texts[x].text = lvlsWonInBlock.ToString() + "/" + totalLevelButtons.ToString();
            }
        }

        if (lvlsWonInBlock == totalLevelButtons) {
            UnhideExtraBlockCheckMark(0);
        }
    }

    private void UnhideExtraBlockCheckMark(int blockIndex) {
        Image[] images;
        images = extras_BlockButtons[blockIndex].GetComponentsInChildren<Image>(true);

        for (int x = 0; x < images.Length; x++) {
            if (images[x].name.Contains("CompletedCheckMark")) {
                images[x].gameObject.SetActive(true);
            }
        }
    }

    private void SetExtraLevelOnClickNumbers() {
        for (int i = 0; i < extras_BlockButtons.Count; i++) {
            Button[] buttons;
            buttons = extras_BlockButtons[i].GetComponentsInChildren<Button>(true);

            int levelIndex = 1;
            for (int x = 0; x < buttons.Length; x++) {
                if (buttons[x].name.Contains("LevelButton")) {
                    int block = i + 3;
                    int lvl = levelIndex;
                    levelIndex++;

                    EventTrigger trigger = buttons[x].GetComponent<EventTrigger>();
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerClick;
                    entry.callback.AddListener((eventData) => { OnButtonClick(block, lvl); });
                    trigger.triggers.Add(entry);
                }
            }
        }
    }

    private void SetExtraPricesText() {

        for (int i = 0; i < extras_BlockButtons.Count; i++) {
            Transform[] transforms;
            transforms = extras_BlockButtons[i].GetComponentsInChildren<Transform>(true);
            int price = 0;
            for (int x = 0; x < transforms.Length; x++) {
                if (transforms[x].name == "Price_GRP") {
                    transforms[x].gameObject.GetComponent<PurchaseExtraBlock>().SetPriceForBlock();
                    price = transforms[x].GetComponent<PurchaseExtraBlock>().purchasePrice;
                }
            }

            for (int x = 0; x < transforms.Length; x++) {
                if (transforms[x].name == "UnlockText") {
                    transforms[x].gameObject.GetComponent<TextMeshProUGUI>().text = LocalisationSystem.GetLocalisedValue("campaignMenu_footer") + " " + price;
                }
            }
        }
    }
    
    private void SetupDailyBlocksWithGameData() {
        GetDailysFromGameData();
        UnlockDailyBlocks();
        SetAllLevelOnClickNumbers();
        DisplayLevelsWon();
    }

    private void GetDailysFromGameData() {
        easyLevelsIndex = GameDataControl.gdControl.daily_easyLevel_Indexes;
        hardLevelsIndex = GameDataControl.gdControl.daily_hardLevel_Indexes;
    }

    public void UnlockDailyBlocks() {
        bool hardUnlocked = GameDataControl.gdControl.hardUnlocked;
        int buttons = dailyPuzzle_BlockButtons.Count;
        if (!hardUnlocked) {
            buttons--;
            SetHardBlockPricesText();
            SetHardFooterTextWithPrice();
        }

        for (int i = 0; i < buttons; i++) {
            UnlockSpecificBlockLevels(i);
            ChangeHeaderColor(i);
            ChangeGameObjectIconsColor(i);
            HidePriceGRP(i);
            UnHideProgressGRP(i);
            UpdateExpandButton(i);
        }
    }

    private void SetHardBlockPricesText() {
        Transform[] transforms;
        transforms = dailyPuzzle_BlockButtons[1].GetComponentsInChildren<Transform>();

        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "Price_GRP") {
                transforms[x].gameObject.GetComponent<PurchaseDailyBlock>().SetPriceForBlock();
            }
        }

    }

    private void SetHardFooterTextWithPrice() {
        Transform[] transforms;
        transforms = dailyPuzzle_BlockButtons[1].GetComponentsInChildren<Transform>(true);
        int price = 0;

        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "Price_GRP") {
                price = transforms[x].GetComponent<PurchaseDailyBlock>().purchasePrice;
            }
        }

        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "UnlockText") {
                //transforms[x].gameObject.GetComponent<TextMeshProUGUI>().text = LocalisationSystem.GetLocalisedValue("campaignMenu_footer") + " " + price;

                //new hard dailys unlocked for free with ad watch
                transforms[x].gameObject.GetComponent<TextMeshProUGUI>().text = LocalisationSystem.GetLocalisedValue("campaignMenu_footer") + " " + LocalisationSystem.GetLocalisedValue("storeMenu_free");
            }
        }
    }


    private void RemoveOnClickListeners() {
        for (int i = 0; i < dailyPuzzle_BlockButtons.Count; i++) {
            Button[] buttons;
            buttons = dailyPuzzle_BlockButtons[i].GetComponentsInChildren<Button>(true);
            
            for (int x = 0; x < buttons.Length; x++) {
                if (buttons[x].name.Contains("LevelButton")) {
                    EventTrigger trigger = buttons[x].GetComponent<EventTrigger>();
                    trigger.triggers.RemoveRange(0, trigger.triggers.Count);
                    buttons[x].GetComponent<PropogateDrag>().PropogateAgain();
                    buttons[x].GetComponent<PropogateDragVertical>().ProprogateAgain();
                }
            }
        }
    }

    private void SetAllLevelOnClickNumbers() {
        for (int i = 0; i < dailyPuzzle_BlockButtons.Count; i++) {
            Button[] buttons;
            buttons = dailyPuzzle_BlockButtons[i].GetComponentsInChildren<Button>(true);

            int levelIndex = 1;
            for (int x = 0; x < buttons.Length; x++) {
                if (buttons[x].name.Contains("LevelButton")) {
                    int block = i + 1;
                    int lvl = levelIndex;
                    levelIndex++;

                    EventTrigger trigger = buttons[x].GetComponent<EventTrigger>();
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerClick;
                    entry.callback.AddListener((eventData) => { OnButtonClick(block, lvl); });
                    trigger.triggers.Add(entry);
                }
            }
        }
    }

    private void DisplayLevelsWon() {
        bool hardUnlocked = GameDataControl.gdControl.hardUnlocked;
        int buttons = dailyPuzzle_BlockButtons.Count;
        if (!hardUnlocked) {
            buttons--;
        }

        for (int i = 0; i < buttons; i++) {
            CheckLevelResultsAndUpdateDisplay(i);
        }
    }

    //User switches apps. When our app regains focus we run the day/time check.
    void OnApplicationFocus(bool hasFocus) {
        if (hasFocus && checkDateCounter > 0) {
            Debug.Log("Has Focus, Checking Daily Puzzle Date.");
            NewDayCheck();
        }
    }

    private void NewDayCheck() {
        GetSeedFromPrefs();
        StartCoroutine(CheckDate());
    }



    private void SetupDailyBlocksFromNewSeed() {
        Debug.Log("Reseting Daily Blocks");
        RemoveOnClickListeners();
        ResetDailyBlocks();
        SetupDailyBlocksWithGameData();
    }

    private void ResetDailyBlocks() {
        for (int i = 0; i < dailyPuzzle_BlockButtons.Count; i++) {
            LockSpecificBlockLevels(i);
            LockHeaderColor(i);
            LockGameObjectIconsColor(i);
            ResetLevelColors(i);
            UnHidePriceGRP(i);
            HideCheckMarks(i);
            HideProgressGRP(i);
            LockExpandButton(i);
        }
    }

    private void HideCheckMarks(int blockIndex) {
        Image[] images;
        images = dailyPuzzle_BlockButtons[blockIndex].GetComponentsInChildren<Image>(true);

        for (int x = 0; x < images.Length; x++) {
            if (images[x].name.Contains("CompletedCheckMark")) {
                images[x].gameObject.SetActive(false);
            }
        }
    }

    private void ResetLevelColors(int blockIndex) {
        Button[] buttons;
        buttons = dailyPuzzle_BlockButtons[blockIndex].GetComponentsInChildren<Button>(true);

        for (int x = 0; x < buttons.Length; x++) {
            if (buttons[x].name.Contains("LevelButton")) {
                buttons[x].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        }
    }

    private void LockExpandButton(int blockIndex) {
        Transform[] transforms;
        transforms = dailyPuzzle_BlockButtons[blockIndex].GetComponentsInChildren<Transform>(true);

        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "Expand_Button") {
                transforms[x].gameObject.GetComponent<ExpandableButton>().locked = true;
            }
            if (transforms[x].name == "Footer") {
                transforms[x].gameObject.SetActive(true);
            }
        }
    }

    private void HideProgressGRP(int blockIndex) {
        Transform[] transforms;
        transforms = dailyPuzzle_BlockButtons[blockIndex].GetComponentsInChildren<Transform>(true);

        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "levelBlock_Progress") {
                transforms[x].gameObject.SetActive(false);
            }
        }
    }

    private void UnHidePriceGRP(int blockIndex) {
        Transform[] transforms;
        transforms = dailyPuzzle_BlockButtons[blockIndex].GetComponentsInChildren<Transform>(true);
        
        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "Price_GRP") {
                transforms[x].gameObject.SetActive(true);
            }
        }
    }

    private void LockGameObjectIconsColor(int blockIndex) {
        Transform[] transforms;
        transforms = dailyPuzzle_BlockButtons[blockIndex].GetComponentsInChildren<Transform>(true);

        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "RedHouse" || transforms[x].name == "BlueHouse" || transforms[x].name == "Water" || transforms[x].name == "Key" || transforms[x].name == "SecretFlower_QuestionMark") {
                transforms[x].gameObject.GetComponent<Image>().color = new Color(185f/255f, 185f/255f, 185f/255f, 1);
            }
        }
    }

    private void LockHeaderColor(int blockIndex) {
        Transform[] transforms;
        transforms = dailyPuzzle_BlockButtons[blockIndex].GetComponentsInChildren<Transform>(true);

        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "Expand_Button") {
                if (transforms[x].GetComponent<ExpandableButton>().adBlock == true) {
                    transforms[x].gameObject.GetComponent<Image>().sprite = lockedSprite_Ad;
                }
                else {
                    transforms[x].gameObject.GetComponent<Image>().sprite = lockedSprite;
                }
            }
        }
    }

    private void LockSpecificBlockLevels(int blockIndex) {
        Button[] buttons;
        buttons = dailyPuzzle_BlockButtons[blockIndex].GetComponentsInChildren<Button>(true);

        for (int i = 0; i < buttons.Length; i++) {
            if (buttons[i].name.Contains("LevelButton")) {
                buttons[i].interactable = false;
                buttons[i].gameObject.GetComponent<Image>().raycastTarget = false;
            }
        }
    }

    private void GetSeedFromPrefs() {
        seed=PlayerPrefs.GetInt("dailySeedNumber", 0);
    }

    private IEnumerator CheckDate() {
        Debug.Log("Getting The Date For Daily Puzzles.");
        yield return StartCoroutine(TimeManager.sharedInstance.getTime());
        int date = TimeManager.sharedInstance.getCurrentDateNow();

        if (seed != date) {
            Debug.Log("Dailies From NewSeed");
            seed = date;
            PlayerPrefs.SetInt("dailySeedNumber", seed);
            SelectRandomDailyPuzzles(seed);
            SetupDailyBlocksFromNewSeed();
        }
        else {
            Debug.Log("Dailies From GameData");
        }
        checkDateCounter++;
        SetDateDisplay();
    }



    private void SetDateDisplay() {
        string date = TimeManager.sharedInstance._currentDate;
        string month = date.Split('-')[0];
        string day = date.Split('-')[1];
        string year = date.Split('-')[2];
        string year2digits = year.Substring(year.Length - 2);

        dateText.text = month + "/" + day + "/" + year2digits;
    }

    private void CheckLevelResultsAndUpdateDisplay(int blockIndex) {
        Button[] buttons;
        buttons = dailyPuzzle_BlockButtons[blockIndex].GetComponentsInChildren<Button>(true);
        int lvlsWonInBlock = 0;
        int totalLevelButtons = 0;

        for (int x = 0; x < buttons.Length; x++) {
            if (buttons[x].name.Contains("LevelButton")) {
                int levelIndex = totalLevelButtons;
                totalLevelButtons++;
                int hardMultiplier = blockIndex * 5;
                int levelResults = GameDataControl.gdControl.daily_level_results[levelIndex+hardMultiplier];
                if (levelResults == 1) {
                    lvlsWonInBlock++;
                    buttons[x].GetComponent<Image>().color = new Color(179f / 255f, 246f / 255f, 171f / 255f, 1);

                }
            }
        }

        TextMeshProUGUI[] texts;
        texts = dailyPuzzle_BlockButtons[blockIndex].GetComponentsInChildren<TextMeshProUGUI>(true);
        for (int x = 0; x < texts.Length; x++) {
            if (texts[x].name.Contains("progressText")) {
                texts[x].text = lvlsWonInBlock.ToString() + "/" + totalLevelButtons.ToString();
            }
        }
        
        if (lvlsWonInBlock == totalLevelButtons) {
            UnhideCheckMark(blockIndex);
        }
    }

    private void UnhideCheckMark(int blockIndex) {
        Image[] images;
        images = dailyPuzzle_BlockButtons[blockIndex].GetComponentsInChildren<Image>(true);

        for (int x = 0; x < images.Length; x++) {
            if (images[x].name.Contains("CompletedCheckMark")) {
                images[x].gameObject.SetActive(true);
            }
        }
    }

    private void UpdateExpandButton(int blockIndex) {
        Transform[] transforms;
        transforms = dailyPuzzle_BlockButtons[blockIndex].GetComponentsInChildren<Transform>(true);

        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "Expand_Button") {
                transforms[x].gameObject.GetComponent<ExpandableButton>().locked = false;
            }
            if (transforms[x].name == "Footer") {
                transforms[x].gameObject.SetActive(false);
            }
        }
    }

    private void UnHideProgressGRP(int blockIndex) {
        Transform[] transforms;
        transforms = dailyPuzzle_BlockButtons[blockIndex].GetComponentsInChildren<Transform>(true);
        int progressGRPIndex = 0;
        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "levelBlock_Progress") {
                progressGRPIndex = x;
            }
        }
        GameObject progressGRP = transforms[progressGRPIndex].gameObject;
        progressGRP.SetActive(true);
    }

    private void HidePriceGRP(int blockIndex) {
        Transform[] transforms;
        transforms = dailyPuzzle_BlockButtons[blockIndex].GetComponentsInChildren<Transform>();

        int priceGRPIndex = 0;
        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "Price_GRP") {
                priceGRPIndex = x;
            }
        }

        GameObject priceGRP = transforms[priceGRPIndex].gameObject;
        priceGRP.SetActive(false);
    }

    private void ChangeGameObjectIconsColor(int blockIndex) {
        Transform[] transforms;
        transforms = dailyPuzzle_BlockButtons[blockIndex].GetComponentsInChildren<Transform>(true);

        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "RedHouse" || transforms[x].name == "BlueHouse" || transforms[x].name == "Water" || transforms[x].name == "Key" || transforms[x].name == "SecretFlower_QuestionMark") {
                transforms[x].gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        }
    }

    private void ChangeHeaderColor(int blockIndex) {
        Transform[] transforms;
        transforms = dailyPuzzle_BlockButtons[blockIndex].GetComponentsInChildren<Transform>(true);

        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "Expand_Button") {
                transforms[x].gameObject.GetComponent<Image>().sprite = unlockedSprite;
            }
        }
    }

    private void UnlockSpecificBlockLevels(int blockIndex) {
        Button[] buttons;
        buttons = dailyPuzzle_BlockButtons[blockIndex].GetComponentsInChildren<Button>(true);

        for (int i = 0; i < buttons.Length; i++) {
            if (buttons[i].name.Contains("LevelButton")) {
                buttons[i].interactable = true;
                buttons[i].gameObject.GetComponent<Image>().raycastTarget = true;
            }
        }
    }


    private void SelectRandomDailyPuzzles(int seed) {
        int easyLevelsNeeded = 5;
        int hardLevelsNeeded = 5;

        easyLevelsIndex.Clear();
        hardLevelsIndex.Clear();

        //Set the date as the random seed number so all devices get the same daily puzzles.
        UnityEngine.Random.InitState(seed);
        for (int i = 0; i < easyLevelsNeeded; i++) {
            int randomLevel = UnityEngine.Random.Range(0, numberOfEasyLevels);
            if (!easyLevelsIndex.Contains(randomLevel)) {
                easyLevelsIndex.Add(randomLevel);
            }
            else {
                easyLevelsNeeded++;
            }
        }
        
        for (int i = 0; i < hardLevelsNeeded; i++) {
            int randomLevel = UnityEngine.Random.Range(0, numberOfHardLevels);
            if (!hardLevelsIndex.Contains(randomLevel)) {
                hardLevelsIndex.Add(randomLevel);
            }
            else {
                hardLevelsNeeded++;
            }
        }

        UnityEngine.Random.InitState(System.Environment.TickCount);
        ResetGameDataWithNewPuzzles();
    }

    private void ResetGameDataWithNewPuzzles() {
        GameDataControl.gdControl.hardUnlocked = false;
        GameDataControl.gdControl.daily_level_results = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        GameDataControl.gdControl.daily_easyLevel_Indexes = easyLevelsIndex;
        GameDataControl.gdControl.daily_hardLevel_Indexes = hardLevelsIndex;
        GameDataControl.gdControl.SavePlayerData();
    }

    public void UnlockHardBlock() {
        UnlockSpecificBlockLevels(1);
        ChangeHeaderColor(1);
        ChangeGameObjectIconsColor(1);
        HidePriceGRP(1);
        UnHideProgressGRP(1);
        UpdateExpandButton(1);
    }



    private void CheckIsScrolling() {
        isScrolling = scrollClickPrevention.isScrolling;
    }

    private void CheckIsSwiping() {
        isSwiping = scrollClickPrevention.isSwiping;
    }

    public void OnButtonClick(int block, int lvl) {
        CheckIsScrolling();
        CheckIsSwiping();
        if (!isScrolling && !isSwiping) {
            FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
            Debug.Log("Loading Block " + block + " Level " + lvl);
            LevelManager.levelManager.scroll = 1;
            LevelManager.levelManager.block = block;
            LevelManager.levelManager.level = lvl;
            SceneManager.LoadScene("Game");
        }
    }

}
