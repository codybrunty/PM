using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class UnlockBlocks : MonoBehaviour {

    public List<int> blockUnlockData;
    [SerializeField] List<GameObject> blockButtons = new List<GameObject>();
    private List<TextMeshProUGUI> blockTextResults = new List<TextMeshProUGUI>();
    [SerializeField] Sprite unlockedSprite = default;
    private bool isScrolling = false;
    private bool isSwiping = false;
    [SerializeField] ScrollClickPrevention scrollClickPrevention = default;
    [SerializeField] Sprite red = default;
    [SerializeField] Sprite green = default;
    [SerializeField] Sprite orange = default;
    [SerializeField] Sprite key = default;
    private List<string> blockSizes = new List<string> { "5x7", "5x7", "6x8", "6x8", "7x10", "7x10", "8x11", "8x11", "8x11", "9x12", "9x12", "9x12", "10x14", "10x14", "10x14", "11x15", "11x15", "11x15" , "12x17", "12x17", "14x20" };
    private List<List<string>> blockObjects;
    [SerializeField] List<Sprite> allFlowers = new List<Sprite>();
    private List<Transform> blockObjectTransforms = new List<Transform>();
    private List<Transform> priceGRPs = new List<Transform>();
    private List<Transform> footerTransforms = new List<Transform>();
    private List<Transform> expandTransforms = new List<Transform>();
    private List<Transform> progressTransforms = new List<Transform>();
    private List<Image> secretFlower_BG = new List<Image>();
    private List<Image> SecretFlower_1 = new List<Image>();
    private List<Image> SecretFlower_2 = new List<Image>();
    private List<Image> SecretFlower_3 = new List<Image>();
    private List<Image> SecretFlower_4 = new List<Image>();
    private List<Image> SecretFlower_FullColor = new List<Image>();
    private List<Image> SecretFlower_Outline = new List<Image>();
    private List<Image> checkMarks = new List<Image>();
    private List<TextMeshProUGUI> blockSizeTexts = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> unlockFooterTexts = new List<TextMeshProUGUI>();
    private List<List<Button>> LevelButtons = new List<List<Button>>();
    private List<List<Image>> LevelButtonSecrets = new List<List<Image>>();

    void Start () {
        AssignVariables();
        FillBlockObjects();
        GetBlockUnlockDataFromGameData();
        SetBlockNumbersForPurchasing();
        SetBlockPurchasePrices();
        SetFooterTextWithPrice();
        SetBlockSizeText();
        SetBlockObjects();
        SetSpecificFlowerOutlinesAndPetals();
        UnlockBlockButtons();
        ResetLevelManager();
        SetAllLevelOnClickNumbers();
        SetDisplayOfLevelsWon();
        SetDisplayOfSecretsFound();
    }

    private void AssignVariables() {
        for (int i = 0; i < blockButtons.Count; i++) {
            Transform[] transforms;
            transforms = blockButtons[i].GetComponentsInChildren<Transform>(true);
            for (int x = 0; x < transforms.Length; x++) {
                if (transforms[x].name == "levelBlock_Objectives") {
                    blockObjectTransforms.Add(transforms[x]);
                }
                if (transforms[x].name == "Price_GRP") {
                    priceGRPs.Add(transforms[x]);
                }
                if (transforms[x].name == "Footer") {
                    footerTransforms.Add(transforms[x]);
                }
                if (transforms[x].name == "Expand_Button") {
                    expandTransforms.Add(transforms[x]);
                }
                if (transforms[x].name == "levelBlock_Progress") {
                    progressTransforms.Add(transforms[x]);
                }
            }
            Image[] images;
            images = blockButtons[i].GetComponentsInChildren<Image>(true);
            for (int x = 0; x < images.Length; x++) {
                if (images[x].name == "SecretFlower_BG") {
                    secretFlower_BG.Add(images[x]);
                }
                if (images[x].name == "SecretFlower_1") {
                    SecretFlower_1.Add(images[x]);
                }
                if (images[x].name == "SecretFlower_2") {
                    SecretFlower_2.Add(images[x]);
                }
                if (images[x].name == "SecretFlower_3") {
                    SecretFlower_3.Add(images[x]);
                }
                if (images[x].name == "SecretFlower_4") {
                    SecretFlower_4.Add(images[x]);
                }
                if (images[x].name == "SecretFlower_FullColor") {
                    SecretFlower_FullColor.Add(images[x]);
                }
                if (images[x].name == "SecretFlower_Outline") {
                    SecretFlower_Outline.Add(images[x]);
                }
                if (images[x].name.Contains("CompletedCheckMark")) {
                    checkMarks.Add(images[x]);
                }
            }
            TextMeshProUGUI[] texts;
            texts = blockButtons[i].GetComponentsInChildren<TextMeshProUGUI>(true);
            for (int x = 0; x < texts.Length; x++) {
                if (texts[x].name == "BlockSizeText") {
                    blockSizeTexts.Add(texts[x]);
                }
                if (texts[x].name == "UnlockText") {
                    unlockFooterTexts.Add(texts[x]);
                }
                if (texts[x].name == "progressText") {
                    blockTextResults.Add(texts[x]);
                }
            }
            Button[] buttons;
            buttons = blockButtons[i].GetComponentsInChildren<Button>(true);
            List<Button> currentBlockLevels = new List<Button>();
            List<Image> currentBlockLevelSecrets = new List<Image>();

            for (int x = 0; x < buttons.Length; x++) {
                if (buttons[x].name.Contains("LevelButton")) {
                    currentBlockLevels.Add(buttons[x]);
                    Image[] secretImages;
                    secretImages = buttons[x].GetComponentsInChildren<Image>(true);
                    for (int j = 0; j < secretImages.Length; j++) {
                        if (secretImages[j].name.Contains("secret")) {
                            currentBlockLevelSecrets.Add(secretImages[j]);
                        }
                    }
                }
            }
            LevelButtons.Add(currentBlockLevels);
            LevelButtonSecrets.Add(currentBlockLevelSecrets);
        }
    }


    private void FillBlockObjects() {
        blockObjects = new List<List<string>> {
            new List<string> { "red","green","","" },
            new List<string> { "red","green","","" },
            new List<string> { "red","green","key","" },
            new List<string> { "red","orange","","" },
            new List<string> { "red","green","orange","" }, 
            new List<string> { "red","green","orange","key" },
            new List<string> { "green","orange","","" },
            new List<string> { "red","green","key","" },
            new List<string> { "red","green","orange","key" },
            new List<string> { "red","green","","" },
            new List<string> { "red", "green", "orange", "" },
            new List<string> { "red","green","orange","key" },
            new List<string> { "red","green","orange","" },
            new List<string> { "red","green","key","" },
            new List<string> { "red","green","orange","key" },
            new List<string> { "orange", "key", "", "" },
            new List<string> { "red","green","orange","" },
            new List<string> { "red","green","orange","key" },
            new List<string> { "red","green","key","" },
            new List<string> { "red","green","orange","key" },
            new List<string> { "red","green","orange","key" }};
}

    private void SetSpecificFlowerOutlinesAndPetals() {
        for (int i = 0; i < blockButtons.Count; i++) {
            int indexStart = i * 8;
            secretFlower_BG[i].sprite = allFlowers[indexStart];
            SecretFlower_1[i].sprite = allFlowers[indexStart + 1];
            SecretFlower_2[i].sprite = allFlowers[indexStart + 2];
            SecretFlower_3[i].sprite = allFlowers[indexStart + 3];
            SecretFlower_4[i].sprite = allFlowers[indexStart + 4];
            SecretFlower_FullColor[i].sprite = allFlowers[indexStart + 5];
            SecretFlower_Outline[i].sprite = allFlowers[indexStart + 7];
        }
    }

    private void SetBlockObjects() {
        for (int i = 0; i < blockObjectTransforms.Count; i++) {
            for (int j = 0; j < blockObjects[i].Count; j++) {
                Transform child = blockObjectTransforms[i].GetChild(j);
                switch (blockObjects[i][j]) {
                    case "red":
                        child.gameObject.GetComponent<Image>().overrideSprite = red;
                        break;
                    case "green":
                        child.gameObject.GetComponent<Image>().overrideSprite = green;
                        break;
                    case "orange":
                        child.gameObject.GetComponent<Image>().overrideSprite = orange;
                        break;
                    case "key":
                        child.gameObject.GetComponent<Image>().overrideSprite = key;
                        break;
                    case "":
                        child.gameObject.SetActive(false);
                        break;
                }
            }
        }
    }
    

    private void SetBlockSizeText() {
        for (int i = 0; i < blockButtons.Count; i++) {
            blockSizeTexts[i].text = blockSizes[i];
        }
    }

    private void SetDisplayOfSecretsFound() {
        //block 0 has no secret flower
        for (int i = 1; i < blockButtons.Count; i++) {
            if (blockUnlockData[i] == 1) {
                CheckSecretsFoundForBlockAndUpdateDisplay(i);
                SwitchSecretFlowerDisplayToUnlocked(i);
            }
        }
    }

    private void SwitchSecretFlowerDisplayToUnlocked(int blockIndex) {
        int indexStart = blockIndex * 8;
        SecretFlower_Outline[blockIndex].sprite = allFlowers[indexStart + 6];
    }


    private void CheckSecretsFoundForBlockAndUpdateDisplay(int blockIndex) {
        int flowerIndex = blockIndex-1;
        int firstSecretLevel = GameDataControl.gdControl.blockSecretsUnlocked[flowerIndex][4];
        int secondSecretLevel = GameDataControl.gdControl.blockSecretsUnlocked[flowerIndex][5];
        int thirdSecretLevel = GameDataControl.gdControl.blockSecretsUnlocked[flowerIndex][6];
        int fourthSecretLevel = GameDataControl.gdControl.blockSecretsUnlocked[flowerIndex][7];

        if (firstSecretLevel != 0) {
            RevealSecretDisplay(blockIndex, firstSecretLevel);
            RevealSecretBlockDisplay(blockIndex, 1);
        }
        if (secondSecretLevel != 0) {
            RevealSecretDisplay(blockIndex, secondSecretLevel);
            RevealSecretBlockDisplay(blockIndex, 2);
        }
        if (thirdSecretLevel != 0) {
            RevealSecretDisplay(blockIndex, thirdSecretLevel);
            RevealSecretBlockDisplay(blockIndex, 3);
        }
        if (fourthSecretLevel != 0) {
            RevealSecretDisplay(blockIndex, fourthSecretLevel);
            RevealSecretBlockDisplay(blockIndex, 4);
        }

        if (firstSecretLevel != 0 && secondSecretLevel != 0 && thirdSecretLevel != 0 && fourthSecretLevel != 0) {
            RevealSecretBlockDisplay(blockIndex, 5);
        }

    }

    private void RevealSecretBlockDisplay(int blockIndex, int secretNumber) {
        if (secretNumber == 1) {
            SecretFlower_1[blockIndex].gameObject.SetActive(true);
        }
        if (secretNumber == 2) {
            SecretFlower_2[blockIndex].gameObject.SetActive(true);
        }
        if (secretNumber == 3) {
            SecretFlower_3[blockIndex].gameObject.SetActive(true);
        }
        if (secretNumber == 4) {
            SecretFlower_4[blockIndex].gameObject.SetActive(true);
        }
        if (secretNumber == 5) {
            SecretFlower_FullColor[blockIndex].gameObject.SetActive(true);
        }
    }

    private void RevealSecretDisplay(int blockIndex, int level) {
        int levelIndex = level - 1;
        LevelButtonSecrets[blockIndex][levelIndex].gameObject.SetActive(true);
    }

    private void SetFooterTextWithPrice() {
        for (int i = 0; i < blockButtons.Count; i++) {
            int price = priceGRPs[i].GetComponent<PurchaseBlock>().purchasePrice;
            unlockFooterTexts[i].text = LocalisationSystem.GetLocalisedValue("campaignMenu_footer") + " " + price;
        }
    }

    private void SetBlockPurchasePrices() {
        for (int i = 0; i < blockButtons.Count; i++) {
            priceGRPs[i].GetComponent<PurchaseBlock>().SetPriceForBlock();
        }
    }

    private void GetBlockUnlockDataFromGameData() {
        blockUnlockData = new List<int>();
        blockUnlockData = GameDataControl.gdControl.blocksUnlocked;
    }

    private void SetDisplayOfLevelsWon() {
        for (int i = 0; i < blockButtons.Count; i++) {
            if (blockUnlockData[i] == 1) {
                CheckLevelResultsAndUpdateDisplay(i);
            }
        }
    }

    private void CheckLevelResultsAndUpdateDisplay(int blockIndex) {
        int lvlsWon = 0;
        for (int x = 0; x < LevelButtons[blockIndex].Count; x++) {
            if (GameDataControl.gdControl.all_level_results[blockIndex][x] == 1) {
                lvlsWon++;
                LevelButtons[blockIndex][x].GetComponent<Image>().color = new Color(179f / 255f, 246f / 255f, 171f / 255f, 1);
            }
        }
        blockTextResults[blockIndex].text = lvlsWon.ToString() +"/"+ LevelButtons[blockIndex].Count.ToString();
        if (lvlsWon == LevelButtons[blockIndex].Count) {
            UnhideCheckMark(blockIndex);
        }
    }

    private void UnhideCheckMark(int blockIndex) {
        checkMarks[blockIndex].gameObject.SetActive(true);
    }

    private void SetAllLevelOnClickNumbers() {
        for (int i = 0; i < blockButtons.Count; i++) {
            for (int x = 0; x < LevelButtons[i].Count; x++) {
                EventTrigger trigger = LevelButtons[i][x].GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                int block = i + 1;
                int level = x + 1;
                entry.callback.AddListener((eventData) => { OnButtonClick(block, level); });
                trigger.triggers.Add(entry);
            }
        }
    }

    public void OnButtonClick(int block, int lvl) {
        CheckIsScrolling();
        CheckIsSwiping();
        if (!isScrolling && !isSwiping) {
            FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
            Debug.Log("Loading Block " + block + " Level " + lvl);
            LevelManager.levelManager.scroll = 0;
            LevelManager.levelManager.block = block;
            LevelManager.levelManager.level = lvl;
            SceneManager.LoadScene("Game");
        }
    }

    private void CheckIsSwiping() {
        isSwiping = scrollClickPrevention.isSwiping;
    }

    private void CheckIsScrolling() {
        isScrolling = scrollClickPrevention.isScrolling;
    }

    private void ResetLevelManager() {
        LevelManager.levelManager.level = 0;
    }

    private void SetBlockNumbersForPurchasing() {
        for (int i = 0; i < blockButtons.Count; i++) {
            int block = i + 1;
            priceGRPs[i].GetComponent<PurchaseBlock>().blockNumber = block;
        }
    }

    private void UnlockBlockButtons() {
        for (int i = 0; i<blockButtons.Count; i++) {
            if (blockUnlockData[i]==1) {
                UnlockSpecificBlockLevels(i);
                ChangeHeaderColor(i);
                SwitchSecretFlowerDisplayToUnlocked(i);
                ChangeGameObjectIconsColor(i);
                HidePriceGRP(i);
                UnHideProgressGRP(i);
                UpdateExpandButton(i);
            }
        }
    }

    private void UpdateExpandButton(int blockIndex) {
        footerTransforms[blockIndex].gameObject.SetActive(false);
        expandTransforms[blockIndex].gameObject.GetComponent<ExpandableButton>().locked = false;
    }

    private void ChangeGameObjectIconsColor(int blockIndex) {
        Transform[] transforms;
        transforms = blockObjectTransforms[blockIndex].GetComponentsInChildren<Transform>(true);
        for (int x = 0; x < transforms.Length; x++) {
            if (transforms[x].name == "RedHouse" || transforms[x].name == "BlueHouse" || transforms[x].name == "Water" || transforms[x].name == "Key" || transforms[x].name == "SecretFlower_QuestionMark") {
                transforms[x].gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        }
    }

    private void ChangeHeaderColor(int blockIndex) {
        expandTransforms[blockIndex].gameObject.GetComponent<Image>().sprite = unlockedSprite;
    }

    private void UnHideProgressGRP(int blockIndex) {
        progressTransforms[blockIndex].gameObject.SetActive(true);
    }

    private void HidePriceGRP(int blockIndex) {
        priceGRPs[blockIndex].gameObject.SetActive(false);
    }

    private void UnlockSpecificBlockLevels(int blockIndex) {
        for (int i = 0; i < LevelButtons[blockIndex].Count; i++) {
            LevelButtons[blockIndex][i].interactable = true;
            LevelButtons[blockIndex][i].gameObject.GetComponent<Image>().raycastTarget = true;
        }
    }

    public void UnlockSpecificBlock(int blockNumber) {
        int blockIndex = blockNumber - 1;
        UnlockSpecificBlockLevels(blockIndex);
        ChangeHeaderColor(blockIndex);
        SwitchSecretFlowerDisplayToUnlocked(blockIndex);
        ChangeGameObjectIconsColor(blockIndex);
        HidePriceGRP(blockIndex);
        UnHideProgressGRP(blockIndex);
        UpdateExpandButton(blockIndex);
        CheckLevelResultsAndUpdateDisplay(blockIndex);
        CheckSecretsFoundForBlockAndUpdateDisplay(blockIndex);
    }

}
