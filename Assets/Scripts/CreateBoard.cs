using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CloudOnce;

public class CreateBoard : MonoBehaviour {
    [Header("Important GameObjects")]
    [SerializeField] Button nextButton = default;
    [SerializeField] Sprite nextButton_mainMenu = default;
    [SerializeField] Sprite nextButton_nextLevel = default;
    [SerializeField] Sprite nextButton_withAd = default;
    [SerializeField] Sprite replayButton_replayLevel = default;
    [SerializeField] Sprite replayButton_withAd = default;
    [SerializeField] Button replayButton = default;
    [SerializeField] GameObject cameraHolder = default;
    [SerializeField] Button hintButton = default;
    [SerializeField] Button helpButton = default;
    [SerializeField] Button settingsButton = default;
    [SerializeField] Button levelsButton = default;
    public ScreenShake screenShake;
    [SerializeField] GameObject helpingHand = default;
    [SerializeField] GameObject helpingHand_questionmark = default;
    [SerializeField] HelpMenuMechanics helpmenu = default;

    [Header("GameBoard Settings")]
    public int gameBoardWidth;
    public int gameBoardHeight;
    public float borderSize=1.0f;
    [SerializeField] ColorToPrefab[] colorMappings = default;
    public float hintPercentageShown = 0.5f;

    [Header("Level Settings")]
    public Texture2D currentLevel;
    [SerializeField] List<BlockWrapper> all_levels = new List<BlockWrapper>();
    [SerializeField] List<Texture2D> daily_easy_levels = new List<Texture2D>();
    [SerializeField] List<Texture2D> daily_hard_levels = new List<Texture2D>();
    [SerializeField] List<BlockWrapper> extra_levels = new List<BlockWrapper>();

    [Header("GamePlay Settings")]
    public bool touchEnabled = true;
    public int availableKeys = 0;
    public int keysGrabbed = 0;
    public bool mazeFinished = false;
    [SerializeField] int hintFee = 2;
    [SerializeField] GameObject confetti_small = default;
    [SerializeField] GameObject confetti_big = default;
    [SerializeField] GameObject confettiPosition = default;

    [Header("Results PopUp")]
    [SerializeField] List<Transform> goldenFlowers = new List<Transform>();
    [SerializeField] GameObject goldenFlowerEffect = default;
    [SerializeField] TextMeshProUGUI resultsTitleText = default;
    [SerializeField] TextMeshProUGUI resultsLevelsCompletedText = default;
    [SerializeField] TextMeshProUGUI currentLevelText = default;
    [SerializeField] Image progressGreenArea=default;
    [SerializeField] GameObject resultsPopUp = default;
    [SerializeField] GameObject coinsPopUp = default;
    [SerializeField] GameObject button_playAd = default;
    [SerializeField] TextMeshProUGUI adText = default;
    [SerializeField] List<Sprite> secretFlowerImages = new List<Sprite>();
    [SerializeField] List<Sprite> secretFlowerWhiteImages = new List<Sprite>();
    [SerializeField] GameObject secretFlowerGroup = default;
    [SerializeField] GameObject prizeWheel = default;
    [SerializeField] GameObject bankEffectPosition = default;
    [SerializeField] List<GameObject> completedFlowerEffects = new List<GameObject>();
    [SerializeField] List<GameObject> completedFlowerEffectsResultScreen = new List<GameObject>();
    [SerializeField] GameObject whiteEffects = default;
    [SerializeField] GameObject whiteEffects2 = default;
    [SerializeField] GameObject whiteEffectsLight = default;
    [SerializeField] GameObject whiteEffectPetal = default;
    [SerializeField] Image secretCompletionBG = default;
    [SerializeField] TextMeshProUGUI tapToContinueText = default;
    [SerializeField] TextMeshProUGUI plus20flowersText = default;
    [SerializeField] Sprite prizeWheel0 = default;
    [SerializeField] Sprite prizeWheel1 = default;
    [SerializeField] Sprite prizeWheel2 = default;
    private int prizeWheelIndex = 0;


    int timesCoinsGiven = 0;

    //private variables
    private List<string> funResultTitles = new List<string> { "resultMenu_Title_0", "resultMenu_Title_1", "resultMenu_Title_2", "resultMenu_Title_3", "resultMenu_Title_4", "resultMenu_Title_5" };
    private int randomCoinReward=0;
    //private int rewardCoinsAddedThisLevel=0;
    private bool showTrees = false;
    private bool showXs = false;
    private int newSecret = 0;
    private GameObject startingSquare;
    private GameObject endingSquare;
    private List<GameObject> keySquares = new List<GameObject>();
    private GameObject currentPlayerSquare;
    private List<GameObject> activeGameBoard = new List<GameObject>();
    private List<GameObject> historyListOfSquares = new List<GameObject>();
    private Image questionMark = default;
    private Image outline = default;
    private Image petalImageColor1 = default;
    private Image petalImageColor2 = default;
    private Image petalImageColor3 = default;
    private Image petalImageColor4 = default;
    private Image completeFlower = default;
    private Image petalImageWhite1 = default;
    private Image petalImageWhite2 = default;
    private Image petalImageWhite3 = default;
    private Image petalImageWhite4 = default;
    private Image petalImageWhite21 = default;
    private Image petalImageWhite22 = default;
    private Image petalImageWhite23 = default;
    private Image petalImageWhite24 = default;
    private bool coinSound = false;
    private bool endSignCompleted = true;
    private bool endSignEmptyCompleted = false;
    private bool allSecretsFound = false;
    private int failedAttempts = 0;

    [System.Serializable]
    public class BlockWrapper {
        public List<Texture2D> levels;
    }

    private void GetLevelFromLevelManager() {
        int scroll = LevelManager.levelManager.scroll;
        int block = LevelManager.levelManager.block;
        int lvl = LevelManager.levelManager.level;

        if (scroll == 0) {
            currentLevel = all_levels[block - 1].levels[lvl - 1];
        }
        if (scroll == 1) {
            Debug.Log("Daily Puzzle");
            if (block == 1) {
                Debug.Log("Easy Block");
                Debug.Log("Button " + lvl + " Clicked");
                lvl = GameDataControl.gdControl.daily_easyLevel_Indexes[lvl - 1];
                currentLevel = daily_easy_levels[lvl];
            }
            if (block == 2) {
                Debug.Log("Hard Block");
                Debug.Log("Button " + lvl + " Clicked");
                lvl = GameDataControl.gdControl.daily_hardLevel_Indexes[lvl - 1];
                currentLevel = daily_hard_levels[lvl];
            }
            if (block == 3) {
                Debug.Log("Extra Block 1");
                Debug.Log("Button " + lvl + " Clicked");
                currentLevel = extra_levels[0].levels[lvl - 1];
            }

            Debug.Log("Level "+lvl+ " Loaded");
        }
    }

    void Start () {
        GetLevelFromLevelManager();
        AssignFunctionsToButtons();
        AssignImagesToButtons();
        GetBoardSize();
        SetUpCamera();
        CreateSquares();
        SetBadSquaresAfterCreation();
        GetAvailableCoinsAndSetKeyNumbers();
        ChangeEndSquareIfCoinGame();
        DisplayGrassOnTiles();
        DisplayWaterOnTiles();
        InstantiateTileDebri();
        RevealGameBoard();
        AssignSecretFlowerVariables();
        StartCoroutine(HoldStartTillAfterBoardCreation());
    }

    private void AssignSecretFlowerVariables() {
        Image[] imgs;
        imgs = resultsPopUp.transform.parent.GetComponentsInChildren<Image>(true);


        for (int x = 0; x < imgs.Length; x++) {
            if (imgs[x].name.Contains("QuestionMark")) {
                questionMark = imgs[x];
            }
            if (imgs[x].name.Contains("Outline")) {
                outline = imgs[x];
            }
            if (imgs[x].name.Contains("Petal1")) {
                petalImageColor1 = imgs[x];
            }
            if (imgs[x].name.Contains("Petal2")) {
                petalImageColor2 = imgs[x];
            }
            if (imgs[x].name.Contains("Petal3")) {
                petalImageColor3 = imgs[x];
            }
            if (imgs[x].name.Contains("Petal4")) {
                petalImageColor4 = imgs[x];
            }
            if (imgs[x].name.Contains("CompleteFlower")) {
                completeFlower = imgs[x];
            }
            if (imgs[x].name.Contains("whiteP1")) {
                petalImageWhite1 = imgs[x];
            }
            if (imgs[x].name.Contains("whiteP2")) {
                petalImageWhite2 = imgs[x];
            }
            if (imgs[x].name.Contains("whiteP3")) {
                petalImageWhite3 = imgs[x];
            }
            if (imgs[x].name.Contains("whiteP4")) {
                petalImageWhite4 = imgs[x];
            }
            if (imgs[x].name.Contains("white2P1")) {
                petalImageWhite21 = imgs[x];
            }
            if (imgs[x].name.Contains("white2P2")) {
                petalImageWhite22 = imgs[x];
            }
            if (imgs[x].name.Contains("white2P3")) {
                petalImageWhite23 = imgs[x];
            }
            if (imgs[x].name.Contains("white2P4")) {
                petalImageWhite24 = imgs[x];
            }
        }
    }

    private void ChangeEndSquareIfCoinGame() {
        GetEndingSquare();
        if (availableKeys > 0) {
            endingSquare.GetComponent<SquareMechanics>().ChangeEndpostToEmpty();
        }
        else {
            endingSquare.GetComponent<SquareMechanics>().ChangeEndpostToFully();
        }
    }

    private void ChangeEndSquareEndPostFull() {
        endSignCompleted = true;
        endSignEmptyCompleted = false;
        GetEndingSquare();
        Debug.Log("Change end square");
        endingSquare.GetComponent<SquareMechanics>().ChangeEndpostToFully();
    }

    private void ChangeEndSquareEndPostEmpty() {
        endSignEmptyCompleted = true;
        endSignCompleted = false;
        GetEndingSquare();
        endingSquare.GetComponent<SquareMechanics>().ChangeEndpostToEmpty();
    }

    private void DisplayWaterOnTiles() {
        for (int i = 0; i < activeGameBoard.Count; i++) {
            GameObject square = activeGameBoard[i];
            SquareMechanics squareDetails = square.GetComponent<SquareMechanics>();

            if (squareDetails.orangeSquare == true) {
                AddWaterDebriLayout(square);
            }
        }
    }

    public int CheckForMistakesOnRoad() {
        int mistakesMade = 0;

        for (int i =0; i < historyListOfSquares.Count; i++) {
            if (historyListOfSquares[i].GetComponent<SquareMechanics>().isBad == true){
                mistakesMade++;
            }
        }


        return mistakesMade;
    }

    private void AddWaterDebriLayout(GameObject square) {
        GameObject WaterDebriGO = ThemeManager.TM.GetWaterDebriLayout();

        SquareMechanics squareDetails = square.GetComponent<SquareMechanics>();
        foreach (Transform child in square.transform) {
            if (child.name == "DebriLayoutGRP") {
                GameObject DebriLayoutGRP = child.gameObject;
                GameObject newDebri = Instantiate(WaterDebriGO, square.transform.position, Quaternion.identity, DebriLayoutGRP.transform);
                GameObject newDebri_ForeGround = newDebri.transform.GetChild(0).gameObject;
                GameObject newDebri_MidGround = newDebri.transform.GetChild(1).gameObject;
                GameObject newDebri_BackGround = newDebri.transform.GetChild(2).gameObject;

                foreach (Transform debri in newDebri_ForeGround.transform) {
                    debri.GetComponent<SpriteRenderer>().sortingOrder = (squareDetails.gamePositionY * -1);
                }
                foreach (Transform debri in newDebri_MidGround.transform) {
                    debri.GetComponent<SpriteRenderer>().sortingOrder = ((squareDetails.gamePositionY * -1) - 1);
                }
                foreach (Transform debri in newDebri_BackGround.transform) {
                    debri.GetComponent<SpriteRenderer>().sortingOrder = ((squareDetails.gamePositionY * -1) - 2);
                }
            }
        }
    }

    private void InstantiateTileDebri() {
        for (int i = 0; i < activeGameBoard.Count; i++) {
            GameObject square = activeGameBoard[i];
            SetRandomDebriLayout(square);
        }
    }

    private void SetRandomDebriLayout(GameObject square) {
        GameObject GrassDebriGO = ThemeManager.TM.GetGrassDebriLayout();
        SquareMechanics squareDetails = square.GetComponent<SquareMechanics>();
        if (squareDetails.blackSquare == true && squareDetails.orangeSquare == false && squareDetails.coinSquare == false && squareDetails.blankSquare == false) {

            foreach (Transform child in square.transform) {
                if (child.name == "DebriLayoutGRP") {
                    int randomCleanTile = UnityEngine.Random.Range(0, 4);
                    if (randomCleanTile != 0) {
                        GameObject DebriLayoutGRP = child.gameObject;
                        GameObject newDebri = Instantiate(GrassDebriGO, square.transform.position, Quaternion.identity, DebriLayoutGRP.transform);
                        GameObject newDebri_ForeGround = newDebri.transform.GetChild(0).gameObject;
                        GameObject newDebri_MidGround = newDebri.transform.GetChild(1).gameObject;
                        GameObject newDebri_BackGround = newDebri.transform.GetChild(2).gameObject;

                        foreach (Transform debri in newDebri_ForeGround.transform) {
                            debri.GetComponent<SpriteRenderer>().sortingOrder = (squareDetails.gamePositionY * -1) ;
                        }
                        foreach (Transform debri in newDebri_MidGround.transform) {
                            debri.GetComponent<SpriteRenderer>().sortingOrder = ((squareDetails.gamePositionY * -1) -1);
                        }
                        foreach (Transform debri in newDebri_BackGround.transform) {
                            debri.GetComponent<SpriteRenderer>().sortingOrder = ((squareDetails.gamePositionY * -1)-2);
                        }
                    }
                }
            }

        }
    }
    private void DisplayGrassOnTiles() {
        for (int i = 0; i < activeGameBoard.Count; i++) {
            GameObject square = activeGameBoard[i];
            SetRandomGrassTile(square);
        }
    }

    private void SetRandomGrassTile(GameObject square) {
        square.GetComponent<SpriteRenderer>().enabled = false;
        GameObject squareGrassGRP = square.transform.GetChild(0).gameObject;
        int randomGrassTileNum = UnityEngine.Random.Range(1, 4);
        GameObject randomGrassTile = squareGrassGRP.transform.GetChild(randomGrassTileNum-1).gameObject;

        //added in for new blank squares
        if (square.GetComponent<SquareMechanics>().blankSquare != true) {
            randomGrassTile.GetComponent<SpriteRenderer>().enabled = true;

            GameObject above = GetAbove(square);
            GameObject left = GetLeft(square);
            GameObject right = GetRight(square);

            if (above == null || above.GetComponent<SquareMechanics>().blankSquare==true) {
                GameObject aboveExtension = randomGrassTile.transform.GetChild(3).gameObject;
                aboveExtension.GetComponent<SpriteRenderer>().enabled = true;
            }
            if (left == null || left.GetComponent<SquareMechanics>().blankSquare == true) {
                GameObject leftExtension = randomGrassTile.transform.GetChild(1).gameObject;
                leftExtension.GetComponent<SpriteRenderer>().enabled = true;
            }
            if (right == null || right.GetComponent<SquareMechanics>().blankSquare == true) {
                GameObject rightExtension = randomGrassTile.transform.GetChild(2).gameObject;
                rightExtension.GetComponent<SpriteRenderer>().enabled = true;
            }

            GameObject belowExtension = randomGrassTile.transform.GetChild(0).gameObject;
            belowExtension.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    
    public void EnableTouch() {
        touchEnabled = true;
    }

    public void DisableTouch() {
        touchEnabled = false;
    }
    

    private void GetAvailableCoinsAndSetKeyNumbers() {
        int keyNumber = 1;
        for (int i = 0; i < activeGameBoard.Count; i++) {
            if (activeGameBoard[i].GetComponent<SquareMechanics>().coinSquare) {
                availableKeys++;

                //recently added for material fading on multi key games.
                activeGameBoard[i].GetComponent<SquareMechanics>().keyNumber = keyNumber;
                keyNumber++;
            }
        }
    }

    IEnumerator HoldStartTillAfterBoardCreation() {
        yield return new WaitForSeconds(1.0f);
        SetupBoardStartingSquare();
    }
    private void SetupBoardStartingSquare() {
        GetStartingSquare();
        ToggleStartCollider();
        SetActiveSquares(startingSquare);
        ResetHistoryListTo(startingSquare);
        ActivateAllSquares();
    }

    private void ToggleStartCollider() {
        //startingSquare.GetComponent<BoxCollider2D>().enabled = false;
        //startingSquare.GetComponent<BoxCollider2D>().enabled = true;

        startingSquare.GetComponent<SquareMechanics>().activate = false;
        startingSquare.GetComponent<SquareMechanics>().activate = true;
    }

    public void IncreaseCoinCount() {
        keysGrabbed++;
        UpdateGameBoardEffectsAndSprites();
    }

    public void DecreaseCoinCount() {
        keysGrabbed--;
        UpdateGameBoardEffectsAndSprites();
    }

    private void SetUpCamera() {
        cameraHolder.transform.position = new Vector3((float)(gameBoardWidth - 1) / 2f, (float)(gameBoardHeight - 1) / 2f, -10f);

        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float verticalSize = (float)gameBoardHeight / 2f + (borderSize);
        float horizontalSize = ((float)gameBoardWidth / 2f + (borderSize)) / aspectRatio;

        if (verticalSize > horizontalSize) {
            Camera.main.orthographicSize = verticalSize;
        }
        else {
            Camera.main.orthographicSize = horizontalSize;
        }
    }

    private void GetBoardSize() {
        gameBoardWidth = currentLevel.width;
        gameBoardHeight = currentLevel.height;
    }

    public void AssignImagesToButtons() {
        Debug.Log("Result Screen Button Images Assigned");
        int scroll = LevelManager.levelManager.scroll;
        int block = LevelManager.levelManager.block;
        int currentLevel = LevelManager.levelManager.level;

        replayButton.GetComponent<Image>().sprite = replayButton_replayLevel;

        if (scroll == 0) {
            if (block == 1 || block == 2 || block == 3 || block == 4) {
                if (currentLevel == 10) {
                    nextButton.GetComponent<Image>().sprite = nextButton_mainMenu;
                }
                else {
                    nextButton.GetComponent<Image>().sprite = nextButton_nextLevel;
                }
            }
            else {
                if (currentLevel == 20) {
                    nextButton.GetComponent<Image>().sprite = nextButton_mainMenu;
                }
                else {
                    nextButton.GetComponent<Image>().sprite = nextButton_nextLevel;
                }
            }
        }
        if (scroll == 1) {
            if (block == 1 || block == 2) {
                if (currentLevel == 5) {
                    nextButton.GetComponent<Image>().sprite = nextButton_mainMenu;
                }
                else {
                    nextButton.GetComponent<Image>().sprite = nextButton_nextLevel;
                }
            }
            else {
                if (currentLevel == 20) {
                    nextButton.GetComponent<Image>().sprite = nextButton_mainMenu;
                }
                else {
                    nextButton.GetComponent<Image>().sprite = nextButton_nextLevel;
                }
            }
        }

    }

    private void AssignFunctionsToButtons() {
        nextButton.onClick.AddListener(LoadNextScene);
        secretCompletionBG.GetComponent<Button>().onClick.AddListener(LoadNextScene);
    }

    public void LoadNextScene() {
        FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
        int scroll = LevelManager.levelManager.scroll;
        int block = LevelManager.levelManager.block;
        int currentLevel = LevelManager.levelManager.level;

        if (scroll == 0) {
            if (block == 1 || block == 2 || block == 3 || block == 4) {
                if (currentLevel == 10) {
                    SceneManager.LoadScene("Menu_Level");
                }
                else {
                    LevelManager.levelManager.level++;
                    SceneManager.LoadScene("Game");
                }
            }
            else {
                if (currentLevel == 20) {
                    SceneManager.LoadScene("Menu_Level");
                }
                else {
                    LevelManager.levelManager.level++;
                    SceneManager.LoadScene("Game");

                }
            }
        }
        if (scroll == 1) {
            if (block == 1 || block == 2) {
                if (currentLevel == 5) {
                    SceneManager.LoadScene("Menu_Level");
                }
                else {
                    LevelManager.levelManager.level++;
                    SceneManager.LoadScene("Game");
                }
            }
            else {
                if (currentLevel == 20) {
                    SceneManager.LoadScene("Menu_Level");
                }
                else {
                    LevelManager.levelManager.level++;
                    SceneManager.LoadScene("Game");
                }
            }
        }

    }

    public void LoadLevelsMenu() {
        SceneManager.LoadScene(0);
    }

    private bool PayHintFee() {
        if (GameDataControl.gdControl.coinsTotal >= hintFee) {
            Debug.Log("Player has " + GameDataControl.gdControl.coinsTotal + " total coins. Player pays " + hintFee + " coins for level hint.");
            GameDataControl.gdControl.coinsSpent += hintFee;
            GameDataControl.gdControl.coinsTotal -= hintFee;
            GameDataControl.gdControl.SavePlayerData();
            Debug.Log("Players new coin total is " + GameDataControl.gdControl.coinsTotal + ".");
            return true;
        }
        else {
            Debug.Log("Player has "+ GameDataControl.gdControl.coinsTotal + " coins. That is not enough to pay the Hint Fee of " + hintFee);
            return false;
        }

    }

    private void SetBadSquaresAfterCreation() {
        for (int i = 0; i < activeGameBoard.Count; i++) {
            SquareMechanics squareMechanics = activeGameBoard[i].GetComponent<SquareMechanics>();
            bool isRed = squareMechanics.redSquare;
            bool isGreen = squareMechanics.greenSquare;
            bool isOrange = squareMechanics.orangeSquare;
            if (isRed) {
                ActivateAboveAndBelowAsBad(activeGameBoard[i]);
            }
            if (isGreen) {
                ActivateLeftAndRightAsBad(activeGameBoard[i]);
            }
            if (isOrange) {
                ActivateDiagonalsAsBad(activeGameBoard[i]);
            }
        }
    }

    private void ActivateDiagonalsAsBad(GameObject orangeSquare) {
        List<GameObject> diagonals = new List<GameObject>();
        diagonals.Add(GetAbove(GetLeft(orangeSquare)));
        diagonals.Add(GetAbove(GetRight(orangeSquare)));
        diagonals.Add(GetBelow(GetLeft(orangeSquare)));
        diagonals.Add(GetBelow(GetRight(orangeSquare)));

        for (int i = 0; i < diagonals.Count; i++) {
            if (diagonals[i] != null) {
                SquareMechanics squareMechanics = diagonals[i].GetComponent<SquareMechanics>();
                if (squareMechanics.blackSquare) {
                    squareMechanics.isBad = true;
                    //Debug.Log(diagonals[i].name);
                }
            }
        }
    }

    private void ActivateLeftAndRightAsBad(GameObject greenSquare) {
        List<GameObject> LeftAndRight = new List<GameObject>();
        LeftAndRight.Add(GetLeft(greenSquare));
        LeftAndRight.Add(GetRight(greenSquare));

        for (int i = 0; i < LeftAndRight.Count; i++) {
            if (LeftAndRight[i] != null) {
                SquareMechanics squareMechanics = LeftAndRight[i].GetComponent<SquareMechanics>();
                if (squareMechanics.blackSquare) {
                    squareMechanics.isBad = true;
                    //Debug.Log(AboveAndBelow[i].name);
                }
            }
        }
    }

    private void ActivateAboveAndBelowAsBad(GameObject redSquare) {
        List<GameObject> AboveAndBelow = new List<GameObject>();
        AboveAndBelow.Add(GetAbove(redSquare));
        AboveAndBelow.Add(GetBelow(redSquare));

        for (int i = 0; i < AboveAndBelow.Count; i++) {
            if (AboveAndBelow[i] != null) {
                SquareMechanics squareMechanics = AboveAndBelow[i].GetComponent<SquareMechanics>();
                if (squareMechanics.blackSquare) {
                    squareMechanics.isBad = true;
                    //Debug.Log(AboveAndBelow[i].name);
                }
            }
        }
    }

    public void HistoryAndSquaresResetToThisBlueSquare(GameObject clickedBlueSquare) {
        Debug.Log("Resetting History List to " + clickedBlueSquare.name);
        ResetHistoryListTo(clickedBlueSquare);
    }

    public void CalculateResult() {
        int mistakeCount = CountMistakes();
        if (mistakeCount > 0) {
            MistakeMade();
        }
        else {
            NoMistakesCheckMazeStatus();
        }
    }

    private void NoMistakesCheckMazeStatus() {
        CheckMazeFinished();
        if (mazeFinished) {
            CalculateTypeOfWin();
        }
    }

    private void CalculateTypeOfWin() {
        if (availableKeys > 0) {
            CheckCoinWin();
        }
        else {
            NormalWin();
        }
    }

    private void CheckCoinWin() {
        if (keysGrabbed == availableKeys) {
            CoinWin();
        }
        else {
            ForgotCoin();
        }
    }

    private int CountMistakes() {
        int mistakeCount = 0;
        for (int i = 0; i < activeGameBoard.Count; i++) {
            SquareMechanics squareMechanics = activeGameBoard[i].GetComponent<SquareMechanics>();
            if (squareMechanics.blueSquare && squareMechanics.isBad) {
                mistakeCount++;
            }
        }
        return mistakeCount;
    }

    private void MistakeMade() {
        Debug.Log("mistake made removing last from historylist");
        RemoveEndSquareRoadTile();
        RemoveLastFromHistoryList();
        StartCoroutine(screenShake.Shake(.2f, .05f));
        CheckIfTutorialLevelAndShowFadeXs();
    }

    private void CheckIfTutorialLevelAndShowFadeXs() {
        int scroll = LevelManager.levelManager.scroll;
        int block = LevelManager.levelManager.block;
        int level = LevelManager.levelManager.level;

        if (scroll == 0) {
            if (block == 1) {
                ShowXsOnGameBoard(true);
            }
            if (block == 2) {
                if (level == 1) {
                    ShowXsOnGameBoard(true);
                }
            }
            if (block == 4) {
                if (level == 1 || level == 2) {
                    ShowXsOnGameBoard(true);
                }
            }
        }
    }

    private void RemoveEndSquareRoadTile() {
        GetEndingSquare();
        FindObjectOfType<SoundManager>().PlaySound("neg1");
        endingSquare.GetComponent<SquareMechanics>().FadeEndRoadDisplay();
        failedAttempts++;
        CheckNumberOfFailedAttempts();
    }

    private void CheckNumberOfFailedAttempts() {
        bool tutorialLevel = CheckTutorialLevel();
        int block = LevelManager.levelManager.block;

        if (tutorialLevel) {
            if (failedAttempts % 2 == 0) {
                helpmenu.HelpButtonCommand();
            }
        }
        else {
            if (failedAttempts % 2 == 0) {
                if (block == 1) {
                    helpingHand_questionmark.GetComponent<HandBobble3>().TurnOnHelpingHand();
                }
                else {
                    helpingHand.GetComponent<HandBobble3>().TurnOnHelpingHand();
                }
            }
        }
    }

    private bool CheckTutorialLevel() {
        bool isTutorialLevel = false;
        LevelManager lm = LevelManager.levelManager;

        if (lm.block == 1) {
            if (lm.level == 2 || lm.level == 7) {
                isTutorialLevel = true;
            }
        }
        if (lm.block == 3) {
            if (lm.level == 1) {
                isTutorialLevel = true;
            }
        }
        if (lm.block == 4) {
            if (lm.level == 1) {
                isTutorialLevel = true;
            }
        }

        return isTutorialLevel;
    }

    private void CoinWin() {
        Debug.Log("Excellent, you retrieved the coin!");
        GameWon();
    }

    public void NormalWin() {
        Debug.Log("Excellent, you made it to the end!");
        GameWon();
    }

    public void GameWon() {
        CheckFirstTutorialStatus();
        CheckForSecretsWon();
        PlayWinSFX();
        DisableTouch();
        Camera.main.GetComponent<PinchZoom>().AfterWinCheckZoom();
        AddPuzzleCompletedPerSession();
        DisableEndSquareEffect();
        StartCoroutine(WinResultsDisplay());
    }

    private void AddPuzzleCompletedPerSession() {
        int puzzlesCompletedThisSession = PlayerPrefs.GetInt("puzzlesCompletedThisSession", 0);
        puzzlesCompletedThisSession++;

        //if no_ads is true then puzzlesCompletedThisSession is always 0
        int no_ads = PlayerPrefs.GetInt("no_ads",0);
        if (no_ads == 1) {
            puzzlesCompletedThisSession = 0;
        }
        //Debug.LogWarning("add puzzles completed this sesh: " + puzzlesCompletedThisSession);
        PlayerPrefs.SetInt("puzzlesCompletedThisSession", puzzlesCompletedThisSession);
    }

    private void RemovePuzzleCompletedPerSession() {
        int puzzlesCompletedThisSession = PlayerPrefs.GetInt("puzzlesCompletedThisSession", 0);
        if (puzzlesCompletedThisSession > 0) {
            puzzlesCompletedThisSession--;
        }

        //if no_ads is true then puzzlesCompletedThisSession is always 0
        int no_ads = PlayerPrefs.GetInt("no_ads", 0);
        if (no_ads == 1) {
            puzzlesCompletedThisSession = 0;
        }
        //Debug.LogWarning("removed puzzles completed this sesh: " + puzzlesCompletedThisSession);
        PlayerPrefs.SetInt("puzzlesCompletedThisSession", puzzlesCompletedThisSession);
    }

    private void CheckFirstTutorialStatus() {
        int tutorial = PlayerPrefs.GetInt("FirstTutorialComplete",0);
        if (tutorial == 0) {
            CheckTutorialProgress();
        }
    }

    private void CheckTutorialProgress() {
        int tutorial_levels_won = 1;
        for (int i = 0; i < GameDataControl.gdControl.all_level_results[0].Count; i++) {
            if (GameDataControl.gdControl.all_level_results[0][i] == 1) {
                tutorial_levels_won++;
            }
        }
        if (tutorial_levels_won > 9) {
            Debug.Log("Tutorial has been complete");
            PlayerPrefs.SetInt("FirstTutorialComplete", 1);
        }
    }

    public void EndSquareBounce() {
        endingSquare.GetComponent<SquareMechanics>().BounceObect();
    }

    private void DisableEndSquareEffect() {
        GetEndingSquare();
        endingSquare.GetComponent<SquareMechanics>().TurnOffEndEffect();
    }

    IEnumerator WinResultsDisplay() {
        HideButtonsAndMenus();
        ShowTreesOnGameBoard();
        yield return new WaitForSeconds(2.5f);
        HideGameBoard();
        StartCoroutine(HoldThenFinishResults());
    }

    public void HintShowSomeXs() {

        Debug.Log("New Hint Initiated");

        List<GameObject> badSquares = new List<GameObject>();
        List<GameObject> showSquares = new List<GameObject>();

        for (int i = 0; i < activeGameBoard.Count; i++) {
            if (activeGameBoard[i].GetComponent<SquareMechanics>().isBad == true) {
                badSquares.Add(activeGameBoard[i]);
                if (activeGameBoard[i].GetComponent<SquareMechanics>().blueSquare == true) {
                    showSquares.Add(activeGameBoard[i]);
                }
            }
        }

        Debug.Log("Bad Squares: " + badSquares.Count);
        Debug.Log("Show Squares: " + showSquares.Count);

        while ((float)(showSquares.Count) / (float)(badSquares.Count) < hintPercentageShown) {
            Debug.Log("Current Ammoutn of Shown Squares Lower then Hint Percentage");
            Debug.Log("Add Another Square");
            GameObject randomSquare= RandomBadSquareNotInShown(badSquares,showSquares);
            showSquares.Add(randomSquare);
        }

        Debug.Log("Bad Squares: " + badSquares.Count);
        Debug.Log("Show Squares: " + showSquares.Count);

        ShowXsInShowList(showSquares);
        
    }

    private GameObject RandomBadSquareNotInShown(List<GameObject> badSquares, List<GameObject> showSquares) {
        badSquares = RandomizeList(badSquares);
        int i = 0;
        GameObject randomSquare = badSquares[i];
        while (showSquares.Contains(randomSquare)) {
            i++;
            randomSquare = badSquares[i];
        }
        Debug.Log("Adding Random Square " + randomSquare.name);
        return randomSquare;
    }

    private List<GameObject> RandomizeList(List<GameObject> badSquares) {
        Debug.Log("Randomizing BadSquares");
        for (int i = 0; i < badSquares.Count; i++) {
            int randomIndex = UnityEngine.Random.Range(0, badSquares.Count);
            GameObject temp = badSquares[i];
            badSquares[i] = badSquares[randomIndex];
            badSquares[randomIndex] = temp;
        }
        return badSquares;
    }

    private void ShowXsInShowList(List<GameObject> showSquares) {

        Debug.Log("Hint Pressed Showing Some Xs");
        for (int i = 0; i < showSquares.Count; i++) {
            showSquares[i].GetComponent<SquareMechanics>().ShowXIfBad();
        }
    }

    public void ShowXsOnGameBoard(bool tutorial) {
        if (showXs == false) {
            showXs = true;
            for (int i = 0; i < activeGameBoard.Count; i++) {
                if (tutorial) {
                    showXs = false;
                    activeGameBoard[i].GetComponent<SquareMechanics>().ShowXIfBadFade();
                }
                else {
                    activeGameBoard[i].GetComponent<SquareMechanics>().ShowXIfBad();
                }
            }
        }
    }

    public void ShowTreesOnGameBoard() {
        if (showTrees == false) {
            showTrees = true;
            for (int i = 0; i < activeGameBoard.Count; i++) {
                activeGameBoard[i].GetComponent<SquareMechanics>().ShowTreeIfBad();
            }
        }
    }

    IEnumerator HoldThenFinishResults() {
        yield return new WaitForSeconds(1f);
        ShowResultsPopUp();
        UpdatePlayerData();
    }

    private void HideButtonsAndMenus() {
        StartCoroutine(HoldHideLevelsButton());
        StartCoroutine(HoldHideSettingsButton());
        StartCoroutine(HoldHideHintButton());
        StartCoroutine(HoldHideHelpButton());
    }
    IEnumerator HoldHideSettingsButton() {
        yield return new WaitForSeconds(.1f);
        StartCoroutine(HideSettingsButton());
    }
    IEnumerator HoldHideLevelsButton() {
        yield return new WaitForSeconds(.1f);
        StartCoroutine(HideLevelsButton());
    }
    IEnumerator HoldHideHintButton() {
        yield return new WaitForSeconds(.15f);
        StartCoroutine(HideHintButton());
    }
    IEnumerator HoldHideHelpButton() {
        yield return new WaitForSeconds(.15f);
        StartCoroutine(HideHelpButton());
    }

    IEnumerator HideSettingsButton() {
        Vector3 originalPos = settingsButton.transform.localPosition;
        Vector3 destinationPos = new Vector3(-440f, 1800f, 0f);
        float time = 0.5f;
        float currentTime = 0.0f;
        do {
            currentTime += Time.deltaTime;
            settingsButton.transform.localPosition = Vector3.Lerp(originalPos, destinationPos, currentTime / time);
            yield return null;
        }
        while (currentTime <= time);
        settingsButton.gameObject.SetActive(false);
    }

    IEnumerator HideLevelsButton() {
        Vector3 originalPos = levelsButton.transform.localPosition;
        Vector3 destinationPos = new Vector3(-440f, 1800f, 0f);
        float time = 0.5f;
        float currentTime = 0.0f;
        do {
            currentTime += Time.deltaTime;
            levelsButton.transform.localPosition = Vector3.Lerp(originalPos, destinationPos, currentTime / time);
            yield return null;
        }
        while (currentTime <= time);
        levelsButton.gameObject.SetActive(false);
    }

    IEnumerator HideHintButton() {
        Vector3 originalPos = hintButton.transform.localPosition;
        Vector3 destinationPos = new Vector3(-215.0f, 1800.0f, 0f);
        float time = 0.5f;
        float currentTime = 0.0f;
        do {
            currentTime += Time.deltaTime;
            hintButton.transform.localPosition = Vector3.Lerp(originalPos, destinationPos, currentTime / time);
            yield return null;
        }
        while (currentTime <= time);
        hintButton.gameObject.SetActive(false);
    }

    IEnumerator HideHelpButton() {
        Vector3 originalPos = helpButton.transform.localPosition;
        Vector3 destinationPos = new Vector3(-215.0f, 1800.0f, 0f);
        float time = 0.5f;
        float currentTime = 0.0f;
        do {
            currentTime += Time.deltaTime;
            helpButton.transform.localPosition = Vector3.Lerp(originalPos, destinationPos, currentTime / time);
            yield return null;
        }
        while (currentTime <= time);
        helpButton.gameObject.SetActive(false);
    }

    IEnumerator ShowCoinsTotalPopUp() {
        coinsPopUp.SetActive(true);
        Vector3 originalPos = coinsPopUp.transform.localPosition;
        Vector3 destinationPos = new Vector3(490f, 910.0f, 90.0f);
        float time = 0.25f;
        float currentTime = 0.0f;
        do {
            currentTime += Time.deltaTime;
            coinsPopUp.transform.localPosition = Vector3.Lerp(originalPos, destinationPos, currentTime / time);
            yield return null;
        }
        while (currentTime <= time);
    }
    
    private void SetPrizeWheel() {
        //8.3% chance its the 500 wheel
        int randomNumber = UnityEngine.Random.Range(1,13);

        if (randomNumber == 1) {
            prizeWheelIndex = 2;
        }
        else {
            prizeWheelIndex = UnityEngine.Random.Range(0, 2);
        }


        if (prizeWheelIndex == 0) {
            prizeWheel.GetComponent<Image>().sprite = prizeWheel0;
        }
        else if (prizeWheelIndex == 1) {
            prizeWheel.GetComponent<Image>().sprite = prizeWheel1;
        }
        else {
            prizeWheel.GetComponent<Image>().sprite = prizeWheel2;
        }

    }

    private void ShowResultsPopUp() {
        Debug.Log("Results Menu Drops Down!");
        resultsPopUp.SetActive(true);
        SetPrizeWheel();
        ReducePuzzlesCompletedOnLastLevelInBlock();
        UpdateNextImageIfPuzzlesCompletedHitsThreshold();

        int scroll = LevelManager.levelManager.scroll;

        if (scroll == 0) {
            LoadCorectBlockSecretFlower();
            ShowSecretsDiscoveredOnResultsPopUpForBlock();
        }
        else {
            HideSecretFlowerGroup();
        }
        UpdateResultsTitle();
    }

    private void UpdateNextImageIfPuzzlesCompletedHitsThreshold() {

        int puzzlesCompletedThisSession = PlayerPrefs.GetInt("puzzlesCompletedThisSession", 0);
        int completedThreshold = hintButton.GetComponent<Hint_AdManager>().puzzleCompleteThreshold;


        if (Application.internetReachability != NetworkReachability.NotReachable) {
            if (puzzlesCompletedThisSession % completedThreshold == 0 && puzzlesCompletedThisSession > 0) {
                nextButton.GetComponent<Image>().sprite = nextButton_withAd;
                replayButton.GetComponent<Image>().sprite = replayButton_withAd;
            }
        }


    }

    private void ReducePuzzlesCompletedOnLastLevelInBlock() {
        int scroll = LevelManager.levelManager.scroll;
        int block = LevelManager.levelManager.block;
        int currentLevel = LevelManager.levelManager.level;

        if (scroll == 0) {
            if (block == 1 || block == 2 || block == 3 || block == 4) {
                if (currentLevel == 10) {
                    RemovePuzzleCompletedPerSession();
                }
            }
            else {
                if (currentLevel == 20) {
                    RemovePuzzleCompletedPerSession();
                }
            }
        }
        if (scroll == 1) {
            if (block == 1 || block == 2) {
                if (currentLevel == 5) {
                    RemovePuzzleCompletedPerSession();
                }
            }
            else {
                if (currentLevel == 20) {
                    RemovePuzzleCompletedPerSession();
                }
            }
        } 
    }

    private void HideSecretFlowerGroup() {
        secretFlowerGroup.SetActive(false);
    }

    private void UpdateLevelsCompleted(bool newWin) {
        int scrollIndex = LevelManager.levelManager.scroll;
        int blockIndex = (LevelManager.levelManager.block - 1);
        int levelIndex = (LevelManager.levelManager.level - 1);
        int totalCompleted = 0;
        int levelAmount = 0;
        if (scrollIndex == 0) {
            levelAmount = GameDataControl.gdControl.all_level_results[blockIndex].Count;
            for (int i = 0; i < levelAmount; i++) {
                int result = GameDataControl.gdControl.all_level_results[blockIndex][i];
                if (result == 1) {
                    totalCompleted++;
                }
            }
        }
        if (scrollIndex == 1) {
            if (blockIndex == 0) {
                levelAmount = GameDataControl.gdControl.daily_level_results.Count/2;
                for (int i = 0; i < levelAmount; i++) {
                    int result = GameDataControl.gdControl.daily_level_results[i];
                    if (result == 1) {
                        totalCompleted++;
                    }
                }
            }
            if (blockIndex == 1) {
                levelAmount = GameDataControl.gdControl.daily_level_results.Count / 2;
                for (int i = 0; i < levelAmount; i++) {
                    int result = GameDataControl.gdControl.daily_level_results[i + levelAmount];
                    if (result == 1) {
                        totalCompleted++;
                    }
                }
            }

            if (blockIndex == 2) {
                levelAmount = GameDataControl.gdControl.extra_level_block_1_results.Count;
                for (int i = 0; i < levelAmount; i++) {
                    int result = GameDataControl.gdControl.extra_level_block_1_results[i];
                    if (result == 1) {
                        totalCompleted++;
                    }
                }
            }
        }

        currentLevelText.text = LocalisationSystem.GetLocalisedValue("resultMenu_level") + " " + LevelManager.levelManager.level;
        resultsLevelsCompletedText.text = (totalCompleted) + "/" + levelAmount + " " + LocalisationSystem.GetLocalisedValue("resultMenu");
        float value = ((float)totalCompleted) / ((float)levelAmount);
        progressGreenArea.fillAmount = value;

    }
    
    private void UpdateResultsTitle() {
        int randomRange = funResultTitles.Count;
        int titleIndex = UnityEngine.Random.Range(0, randomRange);
        resultsTitleText.text = LocalisationSystem.GetLocalisedValue(funResultTitles[titleIndex]);

    }

    private void HideGameBoard() {
        foreach (Transform square in gameObject.transform){
            StartCoroutine(HoldHide(square.gameObject));
        }
    }


    private void RevealGameBoard() {
        foreach (Transform square in gameObject.transform) {
            if (!square.name.Contains("Starting")) {
                StartCoroutine(HoldReveal(square.gameObject));
            }
        }
    }


    IEnumerator HoldHouseReveal(GameObject house) {
        float secondsToWait = UnityEngine.Random.Range(0.05f, .5f);
        yield return new WaitForSeconds(secondsToWait);
        house.SetActive(true);
    }

    IEnumerator HoldReveal(GameObject square) {
        float secondsToWait = UnityEngine.Random.Range(0.05f, .5f);
        yield return new WaitForSeconds(secondsToWait);
        //square.transform.localScale = new Vector3(1f, 1f, 1f);
        StartCoroutine(RevealSquare(square));
    }

    IEnumerator HoldHide(GameObject square) {
        float secondsToWait = UnityEngine.Random.Range(0.05f, .5f);
        yield return new WaitForSeconds(secondsToWait);
        StartCoroutine(HideSquare(square));
    }

    IEnumerator RevealSquare(GameObject square) {
        Vector3 originalScale = square.transform.localScale;
        Vector3 destinationScale = new Vector3(1.0f, 1.0f, 1.0f);
        float time = .5f;
        float currentTime = 0.0f;
        do {
            currentTime += Time.deltaTime;
            square.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
            yield return null;
        }
        while (currentTime <= time);
        
        SquareMechanics squareMech = square.GetComponent<SquareMechanics>();
        bool red = squareMech.redSquare;
        bool blue = squareMech.greenSquare;

        //Debug.Log(square.name);
        
        if (red || blue) {
            StartCoroutine(HoldHouseReveal(square.transform.GetChild(1).gameObject));
        }
    }

    IEnumerator HideSquare(GameObject square) {
        Vector3 originalScale = square.transform.localScale;
        Vector3 destinationScale = new Vector3(0.0f, 0.0f, 0.0f);
        float time = .5f;
        float currentTime = 0.0f;
        do {
            currentTime += Time.deltaTime;
            square.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
            yield return null;
        }
        while (currentTime <= time);
        Destroy(square);
    }

    private void PlayWinSFX() {
        //SoundManager.PlaySound("winSFX");
        FindObjectOfType<SoundManager>().PlaySound("winSFX");
        Instantiate(confetti_small, endingSquare.transform.position, Quaternion.identity);

        Instantiate(confetti_big, confettiPosition.transform.position, Quaternion.Euler(new Vector3(90f, 0f, 0f)),confettiPosition.transform);
    }


    private void UpdatePlayerData() {
        int lvlWon = CheckPlayerDataLevelResults();
        bool newWin = false;

        if (lvlWon == 0) {
            UpdateResultsInPlayerData();
            AddRewardCoinsToPlayerDataAndUpdateFlowerDisplay();
            UpdatePlayerProfile();
            SavePlayerData();
            newWin = true;
        }
        else {
            Debug.Log("You have already beat this level.");
        }

        NewSecretsFoundAnimation();

        CheckIfAllSecretsAreFound();

        if (allSecretsFound && newSecret>0) {
            Debug.Log("flower show dont show ad");
        }
        else {
            StartCoroutine(ShowAdButton(newWin));
        }




        UpdateLevelsCompleted(newWin);

        StartCoroutine(EnableResultButtons(newWin));
    }

    IEnumerator EnableResultButtons(bool newWin) {
        CheckIfAllSecretsAreFound();
        if (!allSecretsFound || newSecret==0) { 
            if (newWin) {
                float seconds = 1.5f;
                if (randomCoinReward > 0) {
                    seconds += 0.5f;
                    //seconds += FindObjectOfType<PrizeWheelMechanics>().SpinDuration;
                    if (randomCoinReward > 40) {
                        if (randomCoinReward > 101) {
                            seconds += ((185) * 0.015f);
                        }
                        else {
                            seconds += ((randomCoinReward) * 0.015f);
                        }
                    }
                    else {
                        seconds += ((randomCoinReward) * 0.01f);
                    }
                }
                yield return new WaitForSeconds(seconds);
                nextButton.interactable = true;
                nextButton.GetComponent<Image>().raycastTarget = true;
                replayButton.interactable = true;
                replayButton.GetComponent<Image>().raycastTarget = true;
            }
            else {
                float seconds = 0f;
                if (newSecret > 0) {
                    seconds = 1.5f;
                }
                yield return new WaitForSeconds(seconds);
                nextButton.interactable = true;
                nextButton.GetComponent<Image>().raycastTarget = true;
                replayButton.interactable = true;
                replayButton.GetComponent<Image>().raycastTarget = true;
            }
        }
    }

    private static void UpdatePlayerProfile() {
        GameDataControl.gdControl.AddPuzzleSolved();
    }

    private void NewSecretsFoundAnimation() {

        if (newSecret == 1) {
            StartCoroutine(Secret1Animation());
        }
        if (newSecret == 2) {
            StartCoroutine(Secret2Animation());
        }
        if (newSecret == 3) {
            StartCoroutine(Secret3Animation());
        }
        if (newSecret == 4) {
            StartCoroutine(Secret4Animation());
        }

    }

    private void CheckIfAllSecretsAreFound() {
        int blockIndex = 0;
        if (LevelManager.levelManager.block>1) {
            blockIndex = LevelManager.levelManager.block - 2;
        }

        int secretOne = GameDataControl.gdControl.blockSecretsUnlocked[blockIndex][0];
        int secretTwo = GameDataControl.gdControl.blockSecretsUnlocked[blockIndex][1];
        int secretThree = GameDataControl.gdControl.blockSecretsUnlocked[blockIndex][2];
        int secretFour = GameDataControl.gdControl.blockSecretsUnlocked[blockIndex][3];

        if (secretOne==1 && secretTwo==1 && secretThree == 1 && secretFour == 1) {
            allSecretsFound = true;
        }
    }

    private void ShowGreenPetal1() {
        petalImageWhite21.gameObject.SetActive(true);
    }

    private void ShowGreenPetal2() {
        petalImageWhite22.gameObject.SetActive(true);
    }
    private void ShowGreenPetal3() {
        petalImageWhite23.gameObject.SetActive(true);
    }
    private void ShowGreenPetal4() {
        petalImageWhite24.gameObject.SetActive(true);
    }

    IEnumerator Secret1Animation() {
        float seconds = 1.5f;
        if (randomCoinReward > 0) {
            seconds += 0.5f;
            //seconds += FindObjectOfType<PrizeWheelMechanics>().SpinDuration;
            if (randomCoinReward > 40) {
                if (randomCoinReward > 101) {
                    seconds += ((185) * 0.015f);
                }
                else {
                    seconds += ((randomCoinReward) * 0.015f);
                }
            }
            else {
                seconds += ((randomCoinReward) * 0.01f);
            }
        }
        else {
            seconds = seconds - 0.5f;
        }
        ShowGreenPetal1();
        yield return new WaitForSeconds(seconds);
        StartCoroutine(PokemonRevealSecretPetalOne());
    }

    IEnumerator Secret2Animation() {
        float seconds = 1.5f;
        if (randomCoinReward > 0) {
            seconds += 0.5f;
            //seconds += FindObjectOfType<PrizeWheelMechanics>().SpinDuration;
            if (randomCoinReward > 40) {
                if (randomCoinReward > 101) {
                    seconds += ((185) * 0.015f);
                }
                else {
                    seconds += ((randomCoinReward) * 0.015f);
                }
            }
            else {
                seconds += ((randomCoinReward) * 0.01f);
            }
        }
        else {
            seconds = seconds - 0.5f;
        }
        ShowGreenPetal2();
        yield return new WaitForSeconds(seconds);
        StartCoroutine(PokemonRevealSecretPetalTwo());
    }
    IEnumerator Secret3Animation() {
        float seconds = 1.5f;
        if (randomCoinReward > 0) {
            seconds += 0.5f;
            //seconds += FindObjectOfType<PrizeWheelMechanics>().SpinDuration;
            if (randomCoinReward > 40) {
                if (randomCoinReward > 101) {
                    seconds += ((185) * 0.015f);
                }
                else {
                    seconds += ((randomCoinReward) * 0.015f);
                }
            }
            else {
                seconds += ((randomCoinReward) * 0.01f);
            }
        }
        else {
            seconds = seconds - 0.5f;
        }
        ShowGreenPetal3();
        yield return new WaitForSeconds(seconds);
        StartCoroutine(PokemonRevealSecretPetalThree());
    }
    IEnumerator Secret4Animation() {
        float seconds = 1.5f;
        if (randomCoinReward > 0) {
            seconds += 0.5f;
            //seconds += FindObjectOfType<PrizeWheelMechanics>().SpinDuration;
            if (randomCoinReward > 40) {
                if (randomCoinReward > 101) {
                    seconds += ((185) * 0.015f);
                }
                else {
                    seconds += ((randomCoinReward) * 0.015f);
                }
            }
            else {
                seconds += ((randomCoinReward) * 0.01f);
            }
        }
        else {
            seconds = seconds - 0.5f;
        }
        ShowGreenPetal4();
        yield return new WaitForSeconds(seconds);
        StartCoroutine(PokemonRevealSecretPetalFour());
    }

    private void LoadCorectBlockSecretFlower(){
        int blockIndex = LevelManager.levelManager.block - 1;
        int flowerIndex = blockIndex * 6;
        int whiteFlowerIndex = blockIndex * 5;

        outline.sprite = secretFlowerImages[flowerIndex + 0];
        petalImageWhite1.sprite = secretFlowerWhiteImages[whiteFlowerIndex + 0];
        petalImageWhite2.sprite = secretFlowerWhiteImages[whiteFlowerIndex + 1];
        petalImageWhite3.sprite = secretFlowerWhiteImages[whiteFlowerIndex + 2];
        petalImageWhite4.sprite = secretFlowerWhiteImages[whiteFlowerIndex + 3];
        petalImageWhite21.sprite = secretFlowerWhiteImages[whiteFlowerIndex + 0];
        petalImageWhite22.sprite = secretFlowerWhiteImages[whiteFlowerIndex + 1];
        petalImageWhite23.sprite = secretFlowerWhiteImages[whiteFlowerIndex + 2];
        petalImageWhite24.sprite = secretFlowerWhiteImages[whiteFlowerIndex + 3];
        petalImageColor1.sprite = secretFlowerImages[flowerIndex + 1];
        petalImageColor2.sprite = secretFlowerImages[flowerIndex + 2];
        petalImageColor3.sprite = secretFlowerImages[flowerIndex + 3];
        petalImageColor4.sprite = secretFlowerImages[flowerIndex + 4];
        completeFlower.sprite = secretFlowerImages[flowerIndex + 5];
    }

    private void ShowSecretsDiscoveredOnResultsPopUpForBlock() {
        //First block secret flower is same as block 2
        int blockIndex = LevelManager.levelManager.block;
        if (blockIndex == 1) {
            blockIndex = 2;
        }
        blockIndex = blockIndex - 2;

        int secretOne = GameDataControl.gdControl.blockSecretsUnlocked[blockIndex][0];
        int secretTwo = GameDataControl.gdControl.blockSecretsUnlocked[blockIndex][1];
        int secretThree = GameDataControl.gdControl.blockSecretsUnlocked[blockIndex][2];
        int secretFour = GameDataControl.gdControl.blockSecretsUnlocked[blockIndex][3];
        
        if (secretOne == 1 && newSecret !=1) {
            RevealSecretPetalOne();
        }
        if (secretTwo == 1 && newSecret != 2) {
            RevealSecretPetalTwo();
        }
        if (secretThree == 1 && newSecret != 3) {
            RevealSecretPetalThree();
        }
        if (secretFour == 1 && newSecret != 4) {
            RevealSecretPetalFour();
        }
        if (secretOne == 1&& secretTwo == 1&& secretThree == 1&& secretFour == 1 && newSecret == 0) {
            RevealColoredFlower();
        }
        
    }

    private void RevealColoredFlower() {
        completeFlower.gameObject.SetActive(true);
        completeFlower.gameObject.GetComponent<Image>().enabled = false;

        int flowerIndex = LevelManager.levelManager.block - 2;
        if (flowerIndex < 0) {
            flowerIndex = 0;
        }

        GameObject newFlower = Instantiate(completedFlowerEffectsResultScreen[flowerIndex], new Vector3(0, 0, 0), Quaternion.identity, completeFlower.transform);
        GameObject whiteEffectsGO = Instantiate(whiteEffects2, new Vector3(0, 0, 0), Quaternion.identity, secretFlowerGroup.transform);
        newFlower.transform.localPosition = new Vector3(0f, 0f, 0f);
        whiteEffectsGO.transform.localPosition = new Vector3(0f, 0f, 0f);

    }


    private void RevealSecretPetalFour() {
        petalImageColor4.gameObject.SetActive(true);
    }

    private void RevealSecretPetalThree() {
        petalImageColor3.gameObject.SetActive(true);
    }

    private void RevealSecretPetalTwo() {
        petalImageColor2.gameObject.SetActive(true);
    }
    private void RevealSecretPetalOne() {
        petalImageColor1.gameObject.SetActive(true);
    }

    IEnumerator FadeOutOverTime(Image img, float alphaEnd, float duration){

        Color start = img.color;
        Color end = new Color(img.color.r, img.color.g, img.color.b, alphaEnd);

        for (float t = 0f; t < duration; t += Time.deltaTime){
            float normalizedTime = t / duration;
            img.color = Color.Lerp(start, end, normalizedTime);
            yield return null;
        }

        img.color = end;
    }
    
    private void SecretFlowerCompletionAnimation(int startingAnimationPetal) {
        Debug.Log("Secret Flower Completed");
        
        StartCoroutine(FadeOutOverTime(questionMark, 0f, 0.5f));
        petalImageWhite1.color = new Color(petalImageWhite1.color.r, petalImageWhite1.color.g, petalImageWhite1.color.b, 1f);
        petalImageWhite1.gameObject.SetActive(true);
        petalImageWhite2.color = new Color(petalImageWhite2.color.r, petalImageWhite2.color.g, petalImageWhite2.color.b, 1f);
        petalImageWhite2.gameObject.SetActive(true);
        petalImageWhite3.color = new Color(petalImageWhite3.color.r, petalImageWhite3.color.g, petalImageWhite3.color.b, 1f);
        petalImageWhite3.gameObject.SetActive(true);
        petalImageWhite4.color = new Color(petalImageWhite4.color.r, petalImageWhite4.color.g, petalImageWhite4.color.b, 1f);
        petalImageWhite4.gameObject.SetActive(true);
        
        if (startingAnimationPetal == 1) {
            TurnOffGreenPetal1Image();
            StartCoroutine(PetalsAllWhite(petalImageColor1, petalImageColor2, petalImageColor3, petalImageColor4));
        }
        if (startingAnimationPetal == 2) {
            TurnOffGreenPetal2Image();
            StartCoroutine(PetalsAllWhite(petalImageColor2, petalImageColor3, petalImageColor4, petalImageColor1));
        }
        if (startingAnimationPetal == 3) {
            TurnOffGreenPetal3Image();
            StartCoroutine(PetalsAllWhite(petalImageColor3, petalImageColor4, petalImageColor1, petalImageColor2));
        }
        if (startingAnimationPetal == 4) {
            TurnOffGreenPetal4Image();
            StartCoroutine(PetalsAllWhite(petalImageColor4, petalImageColor1, petalImageColor2, petalImageColor3));
        }
    }

    private void TurnOffGreenPetal1Image() {
        petalImageWhite21.gameObject.SetActive(false);
    }

    private void TurnOffGreenPetal2Image() {
        petalImageWhite22.gameObject.SetActive(false);
    }

    private void TurnOffGreenPetal3Image() {
        petalImageWhite23.gameObject.SetActive(false);
    }

    private void TurnOffGreenPetal4Image() {
        petalImageWhite24.gameObject.SetActive(false);
    }



    IEnumerator PetalsAllWhite(Image firstPetal, Image secondPetal, Image thirdPetal, Image fourthPetal) {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeOutOverTime(firstPetal, 0f, 0.5f));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeOutOverTime(secondPetal, 0f, 0.5f));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeOutOverTime(thirdPetal, 0f, 0.5f));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeOutOverTime(fourthPetal, 0f, 0.5f));
        StartWhiteEffectsLight();
        yield return new WaitForSeconds(1.5f);
        FlowerAchievementUnlock();
        CompletedFlowerEffect();
        
    }

    private void FlowerAchievementUnlock() {
        int blockNumber = LevelManager.levelManager.block;
        switch (blockNumber) {
            case 2:
                Achievements.Flower1Completed.Unlock();
                break;
            case 3:
                Achievements.Flower2Completed.Unlock();
                break;
            case 4:
                Achievements.Flower3Completed.Unlock();
                break;
            case 5:
                Achievements.Flower4Completed.Unlock();
                break;
            case 6:
                Achievements.Flower5Completed.Unlock();
                break;
            case 7:
                Achievements.Flower6Completed.Unlock();
                break;
            case 8:
                Achievements.Flower7Completed.Unlock();
                break;
            case 9:
                Achievements.Flower8Completed.Unlock();
                break;
            case 10:
                Achievements.Flower9Completed.Unlock();
                break;
            case 11:
                Achievements.Flower10Completed.Unlock();
                break;
            case 12:
                Achievements.Flower11Completed.Unlock();
                break;
            case 13:
                Achievements.Flower12Completed.Unlock();
                break;
            case 14:
                Achievements.Flower13Completed.Unlock();
                break;
            case 15:
                Achievements.Flower14Completed.Unlock();
                break;
            case 16:
                Achievements.Flower15Completed.Unlock();
                break;
            case 17:
                Achievements.Flower16Completed.Unlock();
                break;
            case 18:
                Achievements.Flower17Completed.Unlock();
                break;
            case 19:
                Achievements.Flower18Completed.Unlock();
                break;
            case 20:
                Achievements.Flower19Completed.Unlock();
                break;
            case 21:
                Achievements.Flower20Completed.Unlock();
                break;
        }
    }

    private void StartWhiteEffectsLight() {
        GameObject whiteEffectsLightGO = Instantiate(whiteEffectsLight, new Vector3(0, 0, 0), Quaternion.identity, secretFlowerGroup.transform);
        whiteEffectsLightGO.transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    IEnumerator MoveSecretFlowerGroupToOrigin(float duration) {

            Vector3 startPosition = secretFlowerGroup.transform.localPosition;
            Vector3 endPosition = new Vector3(0f, 0f, 0f);

            for (float t = 0f; t < duration; t += Time.deltaTime) {
                float normalizedTime = t / duration;
                secretFlowerGroup.transform.localPosition = Vector3.Lerp(startPosition, endPosition, normalizedTime);
                yield return null;
            }

            secretFlowerGroup.transform.localPosition = endPosition;
        }

    private void FadeInSecretCompletionBG() {
        secretCompletionBG.gameObject.SetActive(true);
        secretFlowerGroup.transform.SetParent(resultsPopUp.transform.parent);
        StartCoroutine(FadeOutOverTime(secretCompletionBG, 1f, 1f));

        SoundManager.instance.PlayOneShotSound("FlowerCompletedBeg");
        StartCoroutine(HoldTapToContinueTextReveal());
        StartCoroutine(HoldPlus20FlowersTextReveal());
    }

    IEnumerator HoldPlus20FlowersTextReveal() {
        yield return new WaitForSeconds(1.5f);
        SoundManager.instance.PlayOneShotSound("FlowerCompletedBeg");
        yield return new WaitForSeconds(2.3f);
        SoundManager.instance.PlayOneShotSound("FlowerCompleted");
        yield return new WaitForSeconds(0.2f);
        plus20flowersText.gameObject.SetActive(true);
        secretCompletionBG.GetComponent<Button>().interactable = true;
        secretCompletionBG.GetComponent<Image>().raycastTarget = true;
    }

    IEnumerator HoldTapToContinueTextReveal() {
        yield return new WaitForSeconds(2.5f);
        tapToContinueText.gameObject.SetActive(true);
        tapToContinueText.gameObject.GetComponent<BlinkingText>().blink = true;
    }

    private void CompletedFlowerEffect() {
        int flowerIndex = LevelManager.levelManager.block - 2;
        GameObject newFlower = Instantiate(completedFlowerEffects[flowerIndex], new Vector3(0, 0, 0), Quaternion.identity, secretFlowerGroup.transform);
        GameObject whiteEffectsGO = Instantiate(whiteEffects, new Vector3(0, 0, 0), Quaternion.identity, secretFlowerGroup.transform);
        newFlower.transform.localPosition = new Vector3(0f, 0f, 0f);
        whiteEffectsGO.transform.localPosition = new Vector3(0f,0f,0f);
    }

    IEnumerator PokemonRevealSecretPetalOne() {
        CheckIfAllSecretsAreFound();

        petalImageColor1.color = new Color(petalImageColor1.color.r, petalImageColor1.color.g, petalImageColor1.color.b, 0f);
        petalImageColor1.gameObject.SetActive(true);

        GameObject explosionEffects = Instantiate(whiteEffectPetal, new Vector3(0, 0, 0), Quaternion.identity, petalImageWhite21.transform) as GameObject;
        explosionEffects.transform.localPosition = new Vector3(-50f, 50f, 0f);

        yield return new WaitForSeconds(0.3f);
        StartCoroutine(FadeOutOverTime(petalImageColor1, 1f, 0.1f));
        SoundManager.instance.PlayOneShotSound("PetalFound");
        yield return new WaitForSeconds(1f);

        if (allSecretsFound) {
            FadeInSecretCompletionBG();
            StartCoroutine(MoveSecretFlowerGroupToOrigin(1.5f));
            SecretFlowerCompletionAnimation(1);
        }
    }

    IEnumerator PokemonRevealSecretPetalTwo() {
        CheckIfAllSecretsAreFound();

        petalImageColor2.color = new Color(petalImageColor2.color.r, petalImageColor2.color.g, petalImageColor2.color.b, 0f);
        petalImageColor2.gameObject.SetActive(true);

        GameObject explosionEffects = Instantiate(whiteEffectPetal, new Vector3(0, 0, 0), Quaternion.identity, petalImageWhite22.transform) as GameObject;
        explosionEffects.transform.localPosition = new Vector3(50f, 50f, 0f);

        yield return new WaitForSeconds(0.3f);
        StartCoroutine(FadeOutOverTime(petalImageColor2, 1f, 0.1f));
        SoundManager.instance.PlayOneShotSound("PetalFound");
        yield return new WaitForSeconds(1f);

        if (allSecretsFound) {
            FadeInSecretCompletionBG();
            StartCoroutine(MoveSecretFlowerGroupToOrigin(1.5f));
            SecretFlowerCompletionAnimation(2);
        }
    }

    IEnumerator PokemonRevealSecretPetalThree() {
        CheckIfAllSecretsAreFound();

        petalImageColor3.color = new Color(petalImageColor3.color.r, petalImageColor3.color.g, petalImageColor3.color.b, 0f);
        petalImageColor3.gameObject.SetActive(true);

        GameObject explosionEffects = Instantiate(whiteEffectPetal, new Vector3(0, 0, 0), Quaternion.identity, petalImageWhite23.transform) as GameObject;
        explosionEffects.transform.localPosition = new Vector3(-50f, -50f, 0f);



        yield return new WaitForSeconds(0.1f);
        SoundManager.instance.PlayOneShotSound("PetalFound");
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(FadeOutOverTime(petalImageColor3, 1f, 0.1f));
        yield return new WaitForSeconds(1f);

        if (allSecretsFound) {
            FadeInSecretCompletionBG();
            StartCoroutine(MoveSecretFlowerGroupToOrigin(1.5f));
            SecretFlowerCompletionAnimation(3);
        }
    }

    IEnumerator PokemonRevealSecretPetalFour() {
        CheckIfAllSecretsAreFound();

        petalImageColor4.color = new Color(petalImageColor4.color.r, petalImageColor4.color.g, petalImageColor4.color.b, 0f);
        petalImageColor4.gameObject.SetActive(true);
        
        GameObject explosionEffects = Instantiate(whiteEffectPetal, new Vector3(0, 0, 0), Quaternion.identity, petalImageWhite24.transform) as GameObject;
        explosionEffects.transform.localPosition = new Vector3(50f, -50f, 0f);

        yield return new WaitForSeconds(0.3f);
        StartCoroutine(FadeOutOverTime(petalImageColor4, 1f, 0.1f));
        SoundManager.instance.PlayOneShotSound("PetalFound");
        yield return new WaitForSeconds(1f);

        if (allSecretsFound) {
            FadeInSecretCompletionBG();
            StartCoroutine(MoveSecretFlowerGroupToOrigin(1.5f));
            SecretFlowerCompletionAnimation(4);
        }
    }



    IEnumerator ShowAdButton(bool newWin) {
        if (newWin) {
            float seconds = 1.5f;
            if (randomCoinReward > 0) {
                seconds += 0.5f;
                //seconds += FindObjectOfType<PrizeWheelMechanics>().SpinDuration;
                if (randomCoinReward > 40) {
                    if (randomCoinReward > 101) {
                        seconds += ((185) * 0.015f);
                    }
                    else {
                        seconds += ((randomCoinReward) * 0.015f);
                    }
                }
                else {
                    seconds += ((randomCoinReward) * 0.01f);
                }
            }
            //if a secret petal is being revveals hold wheel spin for 1second
            if (newSecret > 0) {
                seconds += 1f;
            }
            yield return new WaitForSeconds(seconds);
            button_playAd.gameObject.SetActive(true);
        }
        else {
            adText.gameObject.SetActive(true);
            adText.text = LocalisationSystem.GetLocalisedValue("resultsMenu_alreadyCompleted");
        }
    }

    private void SavePlayerData() {
        GameDataControl.gdControl.SavePlayerData();
    }

    public void AddRewardCoinsToPlayerDataAndUpdateFlowerDisplay() {
        timesCoinsGiven++;

        randomCoinReward = 25;

        if (timesCoinsGiven>1) {
            randomCoinReward = GetRandomCoinReward();

            //This bool is because the big prize is slow.
            bool bigPrize = false;
            if (randomCoinReward == 500) {
                bigPrize = true;
             }

        StartCoroutine(HoldWheelSpin(randomCoinReward, bigPrize));

        }
        else {
            StartCoroutine(FirstResultsPrize());
        }


        GameDataControl.gdControl.coinsEarned += randomCoinReward;
        GameDataControl.gdControl.coinsTotal += randomCoinReward;
        Debug.Log("Adding "+ randomCoinReward + " coins to playerData.");
        Debug.Log("New coinsTotal = " + GameDataControl.gdControl.coinsTotal);
    }

    private int GetRandomCoinReward() {
        int reward = 0;

        if (prizeWheelIndex == 0) { //all 20% except 500 at 1%
            int randomNumber = UnityEngine.Random.Range(1, 101);
            if (randomNumber == 1) {
                reward = 500;
            }
            else if (randomNumber >= 2 && randomNumber <= 21) {
                reward = 100;
            }
            else if (randomNumber >= 22 && randomNumber <= 41) {
                reward = 55;
            }
            else if (randomNumber >= 42 && randomNumber <= 61) {
                reward = 40;
            }
            else if (randomNumber >= 62 && randomNumber <= 80) {
                reward = 75;
            }
            else {
                reward = 25;
            }
        }
        else if (prizeWheelIndex == 1) { //all 16.5%
            int randomNumber = UnityEngine.Random.Range(1, 7);
            if (randomNumber == 1) {
                reward = 100;
            }
            else if (randomNumber == 2) {
                reward = 25;
            }
            else if (randomNumber == 3) {
                reward = 25;
            }
            else if (randomNumber == 4) {
                reward = 100;
            }
            else if (randomNumber == 5) {
                reward = 25;
            }
            else {
                reward = 25;
            }
        }
        else { //all 16.5%
            int randomNumber = UnityEngine.Random.Range(1, 7);
            Debug.Log(randomNumber);
            if (randomNumber == 1) {
                reward = 500;
            }
            else if (randomNumber == 2) {
                reward = 10;
            }
            else if (randomNumber == 3) {
                reward = 500;
            }
            else if (randomNumber == 4) {
                reward = 10;
            }
            else if (randomNumber == 5) {
                reward = 500;
            }
            else {
                reward = 10;
            }
        }

        return reward;
    }

    IEnumerator NewWheelSpin(int randomCoinReward,bool bigPrize) {
        int target = GetPrizeWheelTargetNumber();

        prizeWheel.GetComponent<PrizeWheelMechanics>().targetNumber = target;
        prizeWheel.GetComponent<PrizeWheelMechanics>().SpinWheel();
        yield return new WaitForSeconds(FindObjectOfType<PrizeWheelMechanics>().SpinDuration);
        StartCoroutine(HoldShowFlower(randomCoinReward, bigPrize));
    }

    private int GetPrizeWheelTargetNumber() {
        int target = 0;

        if (prizeWheelIndex == 0) {
            if (randomCoinReward == 500) {
                target = 0;
            }
            else if (randomCoinReward == 25) {
                target = 1;
            }
            else if (randomCoinReward == 100) {
                target = 2;
            }
            else if (randomCoinReward == 40) {
                target = 3;
            }
            else if (randomCoinReward == 55) {
                target = 4;
            }
            else {//75
                target = 5;
            }
        } 
        else if (prizeWheelIndex == 1) {
            if (randomCoinReward == 100) {
                int randomIndex = UnityEngine.Random.Range(0,2);
                if (randomIndex == 0) {
                    target = 0;
                }
                else {
                    target = 3;
                }
            }
            else {//25
                int randomIndex = UnityEngine.Random.Range(0, 4);

                if (randomIndex == 0) {
                    target = 1;
                }
                else if (randomIndex == 1) {
                    target = 2;
                }
                else if (randomIndex == 2) {
                    target = 4;
                }
                else {
                    target = 5;
                }
            }

        }
        else {
            if(randomCoinReward == 500) {
                int randomIndex = UnityEngine.Random.Range(0, 3);
                if (randomIndex == 0) {
                    target = 0;
                }
                else if (randomIndex == 1) {
                    target = 2;
                }
                else {
                    target = 4;
                }
            }
            else {//10
                int randomIndex = UnityEngine.Random.Range(0, 3);
                if (randomIndex == 0) {
                    target = 1;
                }
                else if (randomIndex == 1) {
                    target = 3;
                }
                else {
                    target = 5;
                }
            }

        }

        return target;
    }

    IEnumerator FirstResultsPrize() {
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(HoldShowFlower(randomCoinReward, false));
    }

    IEnumerator HoldWheelSpin(int randomCoinReward, bool bigPrize) {
        yield return new WaitForSeconds(.05f);
        StartCoroutine(NewWheelSpin(randomCoinReward, bigPrize));
    }

    IEnumerator HoldShowFlower(int randomCoinReward, bool bigPrize) {
        int showFlowersTotal = randomCoinReward;
        RadomizeGoldenFlowerList();

        for (int i = 0; i < showFlowersTotal; i++) {
            yield return new WaitForSeconds(0f);

            GameObject goldenFlowerSpawn = Instantiate(goldenFlowerEffect, goldenFlowers[(i%goldenFlowers.Count)].position ,Quaternion.identity, goldenFlowers[(i % goldenFlowers.Count)]) as GameObject;
            goldenFlowerSpawn.transform.localPosition = new Vector3(0f,0f,0f);
            StartCoroutine(HoldGoldenFlowerThenFlyToBank(goldenFlowerSpawn));

            //added sound delay because to many sfx were playing over top eachother
            if (!coinSound) {
                coinSound = true;
                StartCoroutine(PlayCoinSFXDelayed());
            }


            if (bigPrize) {
                GameObject goldenFlowerSpawnSecond = Instantiate(goldenFlowerEffect, goldenFlowers[((i + 1) % goldenFlowers.Count)].position, Quaternion.identity, goldenFlowers[(i % goldenFlowers.Count)]) as GameObject;
                goldenFlowerSpawnSecond.transform.localPosition = new Vector3(0f, 0f, 0f);
                StartCoroutine(HoldGoldenFlowerThenFlyToBank(goldenFlowerSpawnSecond));

                GameObject goldenFlowerSpawnThird = Instantiate(goldenFlowerEffect, goldenFlowers[((i + 2) % goldenFlowers.Count)].position, Quaternion.identity, goldenFlowers[(i % goldenFlowers.Count)]) as GameObject;
                goldenFlowerSpawnThird.transform.localPosition = new Vector3(0f, 0f, 0f);
                StartCoroutine(HoldGoldenFlowerThenFlyToBank(goldenFlowerSpawnThird));

                GameObject goldenFlowerSpawnFourth = Instantiate(goldenFlowerEffect, goldenFlowers[((i + 3) % goldenFlowers.Count)].position, Quaternion.identity, goldenFlowers[(i % goldenFlowers.Count)]) as GameObject;
                goldenFlowerSpawnFourth.transform.localPosition = new Vector3(0f, 0f, 0f);
                StartCoroutine(HoldGoldenFlowerThenFlyToBank(goldenFlowerSpawnFourth));

                showFlowersTotal -= 3;
            }
        }
    }
    IEnumerator HoldGoldenFlowerThenFlyToBank(GameObject goldenFlower) {
        yield return new WaitForSeconds(1f);
        StartCoroutine(GoldenFlowerFlyToTheBank(goldenFlower));
    }

    IEnumerator GoldenFlowerFlyToTheBank(GameObject goldenFlower) {
        Vector3 a = goldenFlower.transform.position;
        Vector3 b = bankEffectPosition.transform.position;
        float duration = 0.5f;

        for (float t = 0f; t < duration; t += Time.fixedDeltaTime) {
            float normalizedTime = t / duration;
            goldenFlower.transform.position = Vector3.Lerp(a, b, normalizedTime);
            yield return new WaitForFixedUpdate();
        }
        
        goldenFlower.transform.position = b;
        goldenFlower.SetActive(false);
        FindObjectOfType<CoinTotal>().UpdateCoinsTotalTextByOne(randomCoinReward);
        Destroy(goldenFlower,.5f);

    }

    IEnumerator PlayCoinSFXDelayed() {
        //add alittle delay before next sound can play
        yield return new WaitForSeconds(.06f);
        coinSound = false;
        yield return new WaitForSeconds(.14f);
        FindObjectOfType<SoundManager>().PlayOneShotSound("Bell");
    }
    private void RadomizeGoldenFlowerList() {
        
        for (int i = 0; i < goldenFlowers.Count; i++) {
            Transform temp = goldenFlowers[i];
            int randomIndex = UnityEngine.Random.Range(i, goldenFlowers.Count);
            goldenFlowers[i] = goldenFlowers[randomIndex];
            goldenFlowers[randomIndex] = temp;
        }
        
    }

    private void UpdateResultsInPlayerData() {
        int scrollIndex = (LevelManager.levelManager.scroll);
        int blockIndex = (LevelManager.levelManager.block - 1);
        int levelIndex = (LevelManager.levelManager.level - 1);

        if (scrollIndex == 0) {
            GameDataControl.gdControl.all_level_results[blockIndex][levelIndex] = 1;
            Debug.Log("BlockIndex " + blockIndex + " LevelIndex " + levelIndex + " Results updated to 1.");
        }

        if (scrollIndex == 1) {
            if (blockIndex == 0) {
                GameDataControl.gdControl.daily_level_results[levelIndex]=1;
                Debug.Log("Daily Puzzle Easy Block Level " + levelIndex + " Results updated to 1.");
            }
            if (blockIndex == 1) {
                GameDataControl.gdControl.daily_level_results[levelIndex + 5] = 1;
                Debug.Log("Daily Puzzle Hard Block Level " + levelIndex + " Results updated to 1.");
            }
            if (blockIndex == 2) {
                GameDataControl.gdControl.extra_level_block_1_results[levelIndex] = 1;
                Debug.Log("Extra Puzzle Block 1 Level " + levelIndex + " Results updated to 1.");
            }
        }
    }

    private int CheckPlayerDataLevelResults() {
        int scrollIndex = (LevelManager.levelManager.scroll);
        int blockIndex = (LevelManager.levelManager.block - 1);
        int levelIndex = (LevelManager.levelManager.level-1);
        int result = 0;

        if (scrollIndex == 0) {
            result = GameDataControl.gdControl.all_level_results[blockIndex][levelIndex];
        }

        if (scrollIndex == 1) {
            if (blockIndex == 0) {
                result = GameDataControl.gdControl.daily_level_results[levelIndex];
            }
            if (blockIndex == 1) {
                result = GameDataControl.gdControl.daily_level_results[levelIndex + 5];
            }
            if (blockIndex == 2) {
                result = GameDataControl.gdControl.extra_level_block_1_results[levelIndex];
            }
        }

        return result;
    }

    private void ForgotCoin() {
        Debug.Log("Awww, you forgot the coin.");
        MistakeMade();
    }


    private void CheckForSecretsWon() {
        for (int i = 0; i < activeGameBoard.Count; i++) {
            if (activeGameBoard[i].GetComponent<SquareMechanics>().secretFound == true && activeGameBoard[i].GetComponent<SquareMechanics>().blueSquare == true) {
                Debug.Log("recording secret data");
                int secretNumber = activeGameBoard[i].GetComponent<SquareMechanics>().isSecret;
                UpdateGameDataWithSecretFound(secretNumber);
            }
        }
    }

    private void UpdateGameDataWithSecretFound(int secretNumber) {
        newSecret = secretNumber;

        //First block has no secret flower. So 1 will display the same as 2.
        int lvlBlock = LevelManager.levelManager.block;
        if (lvlBlock == 1) {
            lvlBlock = 2;
        }
        lvlBlock = lvlBlock - 2;

        int currentLevel = LevelManager.levelManager.level;
        GameDataControl.gdControl.blockSecretsUnlocked[lvlBlock][secretNumber - 1] = 1;

        GameDataControl.gdControl.blockSecretsUnlocked[lvlBlock][(secretNumber - 1)+4] = currentLevel;
        GameDataControl.gdControl.SavePlayerData();
        //GameDataControl.gdControl.PrintSecretResults();
    }

    private void ActivateAllSquares() {
        for (int i = 0; i < activeGameBoard.Count; i++) {
            BoxCollider2D col = activeGameBoard[i].GetComponent<BoxCollider2D>();
            col.enabled = true;
        }
    }

    private void CheckMazeFinished() {
        GetEndingSquare();
        List<GameObject> adjescentSquares = new List<GameObject>();
        adjescentSquares = GetAdjescentSquares(endingSquare);

        bool anyCurrent = CheckIfAnySquaresAreCurrent(adjescentSquares);
        if (anyCurrent) {
            mazeFinished = true;
        }
        else {
            mazeFinished = false;
        }
    }



    private bool CheckIfAnySquaresAreCurrent(List<GameObject> adjescentSquares) {
        bool anyCurrent = false;
        for (int i = 0; i < adjescentSquares.Count; i++) {
            if (adjescentSquares[i] != null) {
                if (adjescentSquares[i].GetComponent<SquareMechanics>().currentSquare == true) {
                    anyCurrent = true;
                }
            }
        }
        return anyCurrent;
    }

    private void GetEndingSquare() {
        for (int i = 0; i < activeGameBoard.Count; i++) {
            if (activeGameBoard[i].GetComponent<SquareMechanics>().end) {
                endingSquare = activeGameBoard[i];
            }
        }
    }

    private void GetKeySquares() {
        keySquares.Clear();
        for (int i = 0; i < activeGameBoard.Count; i++) {
            if (activeGameBoard[i].GetComponent<SquareMechanics>().coinSquare) {
                keySquares.Add(activeGameBoard[i]);
            }
        }
    }

    public GameObject GetPreviousPreviousSquareInHistoryList() {
        if (historyListOfSquares.Count > 1) {
            return historyListOfSquares[historyListOfSquares.Count - 2];
        }
        else {
            return null;
        }
    }

    public GameObject GetFirstSquareInHistoryList() {
            return historyListOfSquares[0];
    }

    public GameObject GetPreviousSquareInHistoryList() {
        if (historyListOfSquares.Count > 1) {
            return historyListOfSquares[historyListOfSquares.Count - 1];
        }
        else {
            return null;
        }
    }

    public void ResetHistoryListTo(GameObject currentTarget) {
        if (historyListOfSquares.Count == 0) {
            AddToHistoryList(currentTarget);
        }

        else {
            List<GameObject> newHistoryList = new List<GameObject>();
            List<GameObject> deletedFromHistory = new List<GameObject>();
            bool reachedCurrentTarget = false;
            for (int i = 0; i < historyListOfSquares.Count; i++) {
                if (!reachedCurrentTarget) { 
                    if (historyListOfSquares[i] != currentTarget) {
                        newHistoryList.Add(historyListOfSquares[i]);
                    }
                    else {
                        newHistoryList.Add(historyListOfSquares[i]);
                        reachedCurrentTarget = true;
                    }
                }
                else {
                    deletedFromHistory.Add(historyListOfSquares[i]);
                }
            }
            historyListOfSquares = newHistoryList;
            PrintHistoryList();

            currentTarget.GetComponent<SquareMechanics>().SelectTileChangeAudioAndPlay();
            currentTarget.GetComponent<SquareMechanics>().PlayBurstEffect();

            ResetSquaresInDeletedHistory(deletedFromHistory);
        }
        
    }


    private void ResetSquaresInDeletedHistory(List<GameObject> deletedFromHistory) {
        if (deletedFromHistory.Count > 0) {
            for (int i = deletedFromHistory.Count-1; i > -1; i--) {
                //Debug.Log("Reset "+deletedFromHistory[i].name);
                deletedFromHistory[i].GetComponent<SquareMechanics>().ChangeBlueSquareToBlackSquare();
            }
        }
    }

    public void AddToHistoryList(GameObject currentSquare) {
        historyListOfSquares.Add(currentSquare);
        PrintHistoryList();
    }

    public void RemoveFromHistoryList(GameObject currentSquare) {
        historyListOfSquares.Remove(currentSquare);
        PrintHistoryList();
    }
    public void RemoveLastFromHistoryList() {
        int listCount = historyListOfSquares.Count-1;
        historyListOfSquares.RemoveAt(listCount);
        PrintHistoryList();
    }

    private void PrintHistoryList() {
        string historyList = "";
        for (int i = 0; i < historyListOfSquares.Count; i++) {
            historyList += historyListOfSquares[i].name + "  ";
        }
        Debug.Log("History List: " + historyList);

        UpdateGameBoardEffectsAndSprites();
    }

    private void UpdateGameBoardEffectsAndSprites() {
        GetEndingSquare();
        GetStartingSquare();

        bool keyGame = false;
        if (availableKeys > 0) {
            keyGame = true;
            GetKeySquares();
        }


        if (keyGame) {
            if (historyListOfSquares.Count > 1) {
                startingSquare.GetComponent<SquareMechanics>().TurnOffStartEffect();

                foreach (GameObject keySquare in keySquares) {

                    if (historyListOfSquares.Contains(keySquare)) {
                        keySquare.GetComponent<SquareMechanics>().TurnOffKeyEffect();
                    }
                    else {
                        keySquare.GetComponent<SquareMechanics>().TurnOnKeyEffect();
                    }

                }
                if (availableKeys == keysGrabbed) {
                    if (!endSignCompleted) {
                        endingSquare.GetComponent<SquareMechanics>().TurnOnEndEffect();
                        ChangeEndSquareEndPostFull();
                    }
                }
                else {
                    if (!endSignEmptyCompleted) {
                        endingSquare.GetComponent<SquareMechanics>().TurnOffEndEffect();
                        ChangeEndSquareEndPostEmpty();
                    }
                }
            }

            if (historyListOfSquares.Count == 1) {
                startingSquare.GetComponent<SquareMechanics>().TurnOnStartEffect();

                foreach (GameObject keySquare in keySquares) {
                    keySquare.GetComponent<SquareMechanics>().TurnOffKeyEffect();

                }

                endingSquare.GetComponent<SquareMechanics>().TurnOffEndEffect();
                ChangeEndSquareEndPostEmpty();
            }
        }


        else {
            if (historyListOfSquares.Count > 1) {
                startingSquare.GetComponent<SquareMechanics>().TurnOffStartEffect();
                endingSquare.GetComponent<SquareMechanics>().TurnOnEndEffect();
            }
            if (historyListOfSquares.Count == 1) {
                startingSquare.GetComponent<SquareMechanics>().TurnOnStartEffect();
                endingSquare.GetComponent<SquareMechanics>().TurnOffEndEffect();
            }
        }
    }

    private void SetCurrentPlayerSquare(GameObject currentSquare) {
        DeactivateOldCurrentPlayerSquare();
        currentSquare.GetComponent<SquareMechanics>().currentSquare = true;
        currentPlayerSquare = currentSquare;
    }

    private void DeactivateOldCurrentPlayerSquare() {
        if (currentPlayerSquare != null) {
            currentPlayerSquare.GetComponent<SquareMechanics>().currentSquare = false;
        }
    }

    public void SetActiveSquares(GameObject currentSquare) {
        DeactiveNonBlueSquares();
        SetCurrentPlayerSquare(currentSquare);
        List<GameObject> adjescentSquares = new List<GameObject>();
        adjescentSquares = GetAdjescentSquares(currentSquare);
        SetAdjescentSquaresActive(adjescentSquares);
    }

    private void DeactiveNonBlueSquares() {
        for (int i = 0; i < activeGameBoard.Count; i++) {
            bool blueSquare = activeGameBoard[i].GetComponent<SquareMechanics>().blueSquare;
            bool isEnd = activeGameBoard[i].GetComponent<SquareMechanics>().end;

            if (!blueSquare) {
                activeGameBoard[i].GetComponent<SquareMechanics>().activate = false;
                //BoxCollider2D col = activeGameBoard[i].GetComponent<BoxCollider2D>();
                //col.enabled = false;
            }
            else if (blueSquare && isEnd) {
                activeGameBoard[i].GetComponent<SquareMechanics>().activate = false;
                //BoxCollider2D col = activeGameBoard[i].GetComponent<BoxCollider2D>();
                //col.enabled = false;
            }
        }
    }

    private void SetAdjescentSquaresActive(List<GameObject> adjescentSquares) {

        for (int i = 0; i< adjescentSquares.Count; i++) {
            if (adjescentSquares[i] != null) {
                bool blackSquare = adjescentSquares[i].GetComponent<SquareMechanics>().blackSquare;
                bool blueSquare = adjescentSquares[i].GetComponent<SquareMechanics>().blueSquare;
                bool isEnd = adjescentSquares[i].GetComponent<SquareMechanics>().end;
                if (blackSquare) {
                    adjescentSquares[i].GetComponent<SquareMechanics>().activate = true;
                }
                else if (blueSquare && isEnd) {
                    adjescentSquares[i].GetComponent<SquareMechanics>().activate = true;
                }
            }
        }   
    }

    private List<GameObject> GetAdjescentSquares(GameObject targetSquare) {
        List<GameObject> adjescentSquares = new List<GameObject>();
        adjescentSquares.Add(GetAbove(targetSquare));
        adjescentSquares.Add(GetBelow(targetSquare));
        adjescentSquares.Add(GetLeft(targetSquare));
        adjescentSquares.Add(GetRight(targetSquare));
        return adjescentSquares;
    }

    public GameObject GetRight(GameObject targetSquare) {
        int targetX = targetSquare.GetComponent<SquareMechanics>().gamePositionX;
        int rightX = targetX + 1;

        if (rightX < gameBoardWidth && rightX > -1) {
            int targetIndex = targetSquare.GetComponent<SquareMechanics>().gamePositionIndex;
            int rightIndex = targetIndex + gameBoardHeight;
            //Debug.Log("Right: " + activeGameBoard[rightIndex].name);
            return activeGameBoard[rightIndex];
        }
        else {
            //Debug.Log("Right: NA");
            return null;
        }
    }

    public GameObject GetLeft(GameObject targetSquare) {
        //Debug.Log("test");
        int targetX = targetSquare.GetComponent<SquareMechanics>().gamePositionX;
        int leftX = targetX - 1;

        if (leftX < gameBoardWidth && leftX > -1) {
            int targetIndex = targetSquare.GetComponent<SquareMechanics>().gamePositionIndex;
            int leftIndex = targetIndex - gameBoardHeight;
            //Debug.Log("Left: " + activeGameBoard[leftIndex].name);
            return activeGameBoard[leftIndex];
        }
        else {
            //Debug.Log("Left: NA");
            return null;
        }
    }

    public GameObject GetBelow(GameObject targetSquare) {
        if (targetSquare != null) {
            int targetY = targetSquare.GetComponent<SquareMechanics>().gamePositionY;
            int belowY = targetY - 1;

            if (belowY < gameBoardHeight && belowY > -1) {
                int targetIndex = targetSquare.GetComponent<SquareMechanics>().gamePositionIndex;
                int belowIndex = targetIndex - 1;
                //Debug.Log("Below: " + activeGameBoard[belowIndex].name);
                return activeGameBoard[belowIndex];
            }
            else {
                //Debug.Log("Below: NA");
                return null;
            }
        }
        else {
            return null;
        }
    }

    public GameObject GetAbove(GameObject targetSquare) {
        if (targetSquare != null) {
            int targetY = targetSquare.GetComponent<SquareMechanics>().gamePositionY;
            int aboveY = targetY + 1;

            if (aboveY < gameBoardHeight && aboveY > -1) {
                int targetIndex = targetSquare.GetComponent<SquareMechanics>().gamePositionIndex;
                int aboveIndex = targetIndex + 1;
                //Debug.Log("Above: " + activeGameBoard[aboveIndex].name);
                return activeGameBoard[aboveIndex];
            }
            else {
                //Debug.Log("Above: NA");
                return null;
            }
        }
        else {
            return null;
        }
    }

    private void GetStartingSquare() {
        for (int i = 0; i < activeGameBoard.Count; i++) {
            if (activeGameBoard[i].GetComponent<SquareMechanics>().start) {
                startingSquare = activeGameBoard[i];
            }
        }
    }

    private void CreateSquares() {
        for (int x = 0; x < gameBoardWidth; x++) {
            for (int y = 0; y < gameBoardHeight; y++) {
                GenerateSquare(x, y);
            }

        }
    }

    private void GenerateSquare(int x, int y) {
        Vector3 squareSpawnPoint = new Vector3(x, y, 0);
        GetColorAndInstantiate(x, y, squareSpawnPoint);
    }

    private void GetColorAndInstantiate(int x, int y, Vector3 squareSpawnPoint) {
        Color pixelColor = currentLevel.GetPixel(x, y);

        foreach (ColorToPrefab colorMapping in colorMappings) {
            if (colorMapping.color == pixelColor) {
                GameObject newSquare = Instantiate(colorMapping.prefab, squareSpawnPoint, Quaternion.identity, gameObject.transform);
                newSquare.name = x + "," + y;
                SquareMechanics squareData = newSquare.GetComponent<SquareMechanics>();
                squareData.gamePositionX = x;
                squareData.gamePositionY = y;
                squareData.gamePositionIndex = activeGameBoard.Count;
                activeGameBoard.Add(newSquare);
                newSquare.transform.localScale = new Vector3(0.0f,0.0f,0.0f);
                SetGameBoard_Level1_Art_OrderInlayer(newSquare);
            }
        }

    }

    private void SetGameBoard_Level1_Art_OrderInlayer(GameObject square) {
        SquareMechanics squareData = square.GetComponent<SquareMechanics>();
        //moved to cosmetics_redhouse script
        //if (squareData.redSquare == true) {
        //    SetRedHouse_OrderInLayer(square);
        //}
        //moved to cosmetics_bluehouse script
        //if (squareData.greenSquare == true) {
        //    SetGreenHouse_OrderInLayer(square);
        //}
        if (squareData.orangeSquare == true) {
            SetOrangeWater_OrderInLayer(square);
        }
        else if (squareData.start == true) {
            SetStartDisplay_OrderInLayer(square);
        }
        else if (squareData.end == true) {
            SetEndDisplay_OrderInLayer(square);
        }
        else if (squareData.coinSquare == true) {
            SetCoin_OrderInLayer(square);
        }
    }

    private void SetCoin_OrderInLayer(GameObject square) {
        SquareMechanics squareData = square.GetComponent<SquareMechanics>();
        GameObject coinDisplay = square.transform.GetChild(2).gameObject;
        coinDisplay.GetComponent<SpriteRenderer>().sortingOrder = (-1 * squareData.gamePositionY);
    }

    private void SetEndDisplay_OrderInLayer(GameObject square) {
        SquareMechanics squareData = square.GetComponent<SquareMechanics>();
        Transform endDisplayGRP = square.transform.GetChild(1);
        foreach (Transform endPiece in endDisplayGRP) {
            endPiece.gameObject.GetComponent<SpriteRenderer>().sortingOrder = (-1 * squareData.gamePositionY);
        }
    }

    private void SetStartDisplay_OrderInLayer(GameObject square) {
        SquareMechanics squareData = square.GetComponent<SquareMechanics>();
        Transform startDisplayGRP = square.transform.GetChild(1);
        foreach (Transform startPiece in startDisplayGRP) {
            startPiece.gameObject.GetComponent<SpriteRenderer>().sortingOrder = (-1 * squareData.gamePositionY);
        }
    }

    private void SetOrangeWater_OrderInLayer(GameObject square) {
        SquareMechanics squareData = square.GetComponent<SquareMechanics>();
        GameObject waterGRP = square.transform.GetChild(2).gameObject;
        foreach(Transform child in waterGRP.transform) {
            child.GetComponent<SpriteRenderer>().sortingOrder = (-1 * squareData.gamePositionY);
        }
    }

}
