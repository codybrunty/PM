using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.IO;


public class AutoGenerate : MonoBehaviour {
    [Header("Static Parameters")]
    [SerializeField] GameObject startSquare = default;
    [SerializeField] GameObject blueOrangeSquare = default;
    [SerializeField] GameObject blueSquare = default;
    [SerializeField] GameObject endSquare = default;
    [SerializeField] GameObject redSquare = default;
    [SerializeField] GameObject greenSquare = default;
    [SerializeField] GameObject blackSquare = default;
    [SerializeField] GameObject coinSquare = default;
    [SerializeField] GameObject orangeSquare = default;
    [SerializeField] Button newButton = default;
    [SerializeField] Button testButton = default;
    [SerializeField] Button saveButton = default;
    [SerializeField] Button fillButton = default;
    private List<GameObject> randomGeneration = new List<GameObject>();
    private List<GameObject> activeGameBoard = new List<GameObject>();
    private List<GameObject> badPath = new List<GameObject>();
    private GameObject startingSquare;
    private GameObject currentSquare;
    [SerializeField] ColorToPrefab[] colorMappings = default;

    [Header("GameBoard Settings")]
    public int gameBoardWidth;
    public int gameBoardHeight;
    [SerializeField] float borderSize = 1.0f;
    [SerializeField] bool allBlackBoard = default;
    public int randomFromBlackSquares = 8;
    public int randomFromGreenSquares = 1;
    public int randomFromRedSquares = 1;
    public int randomFromOrangeSquares = 1;
    public int numOfCoins = 1;

    [Header("AI Settings")]
    [SerializeField] int attempts = default;
    public int winCount;
    public int lossCount;
    public float winPercentage;
    [SerializeField] Text resultsText = default;
    private List<GameObject> availableMoves = new List<GameObject>();
    private int coinVictory = 0;

    [Header("MultiMap Settings")]
    [SerializeField] bool multiMaps = false;
    [SerializeField] int mapsCreated = 10;
    [SerializeField] float targetWinPercentage = 10.0f;
    public float lowestWinPercentage;

    private void Start() {
        AssignFunctionsToButtons();
        if (multiMaps) { 
            StartCoroutine(MultiMapGeneration());
        }
        else {
            SingleMapGeneration();
        }
    }

    IEnumerator MultiMapGeneration() {
        for (int i = 0; i < mapsCreated; i++) {
            yield return null;
            DisplayPercentageCompleted(i);
            NewButtonOnClick();
        }
    }

    private void DisplayPercentageCompleted(int i) {
        int currentMapGenerated = i + 1;
        float PercentageCompleted=((float)currentMapGenerated / (float)mapsCreated)*100;
        if (PercentageCompleted % 10 == 0) {
            Debug.Log("Percentage Complete: "+PercentageCompleted+"%");
        }
    }

    private void SingleMapGeneration() {
        GameBoardRandomGeneration();
        CreateBoardFromRandomGeneration();
        SetUpCamera();
        SetIsBadOnSquares();

        StartAITesting();
    }

    private void DisableAllButtons() {
        DisableSaveButton();
        DisableTestButton();
        DisableNewButton();
        DisableFillButton();
    }

    private void DisableFillButton() {
        fillButton.interactable = false;
    }

    private void StartAITesting() {
        bool playable = CheckPlayable();
        if (playable) {
            ResetResults();
            CalculateResults();
            PostResults();
        }

    }

    private bool CheckPlayable() {
        bool playable = false;
        bool randomEnd = false;
        bool randomStart = false;
        for (int i = 0; i < randomGeneration.Count; i++) {
            if (randomGeneration[i] == endSquare) {
                randomEnd = true;
            }
            else if (randomGeneration[i] == startSquare) {
                randomStart = true;
            }
        }
        if (randomEnd && randomStart) {
            playable = true;
        }
        return playable;
    }

    private void CalculateResults() {
        for (int i = 0; i < attempts; i++) {
            ResetUnavailableSquares();
            ResetIsBadOnSquares();
            ResetCoinVictory();
            SetIsBadOnSquares();
            FindStart();
            FindAvailableMoves(startingSquare);
            MoveAI();
        }
    }

    private void ResetCoinVictory() {
        coinVictory = 0;
    }

    private void ResetResults() {
        winCount = 0;
        lossCount = 0;
    }

    private void PostResults() {
        CalculateWinPercentage();
        RecordLowestWinPercentage();
        if (multiMaps) {
            CheckWinPercentageAndSave();
        }
        else {
            resultsText.text = "Attempts: " + attempts + "\nWins: " + winCount + "\nLosses: " + lossCount + "\nWin (%): " + winPercentage + "%";
        }
    }

    private void RecordLowestWinPercentage() {
        if (lowestWinPercentage == 0f ) {
            lowestWinPercentage = winPercentage;
        }
        else if (winPercentage< lowestWinPercentage && winPercentage !=0f) {
            lowestWinPercentage = winPercentage;
        }
    }

    private void CheckWinPercentageAndSave() {
        if (winPercentage <= targetWinPercentage && winPercentage > 0.0f) {
            Debug.Log("Target Match: " + winPercentage + "%");
            SaveButtonOnClick();
        }
    }


    private void CalculateWinPercentage() {
        winPercentage = ((float)winCount/ (float)attempts)*100;
        //Debug.Log(winPercentage);
    }

    private void ResetUnavailableSquares() {
        for (int i = 0; i < activeGameBoard.Count; i++) {
            activeGameBoard[i].GetComponent<SquareMechanicsAuto>().unavailable = false;
        }
    }

    private void MoveAI() {
        if (availableMoves.Count > 0) {
            bool winningMove = CheckIfEndSquareIsAvailable();
            if (winningMove) {
                if (coinVictory != numOfCoins && numOfCoins > 0) {
                    ModifyAvailableMoves();
                    if (availableMoves.Count > 0) {
                        ContinueMoving();
                    }
                    else {
                        TallyLoss();
                    }
                }
                else if (coinVictory==numOfCoins || numOfCoins==0) {
                    TallyWin();
                }
            }
            else {
                ContinueMoving();
            }
        }
        else {
            TallyLoss();
        }
    }

    private void ContinueMoving() {
        int randomSquareIndex = UnityEngine.Random.Range(0, availableMoves.Count);
        MoveToSquare(availableMoves[randomSquareIndex]);
    }

    private void TallyWin() {
        winCount++;
    }
    private void TallyLoss() {
        lossCount++;
    }

    private bool CheckIfEndSquareIsAvailable() {
        bool winningMove = false;

        for (int i = 0; i < availableMoves.Count; i++) {
            bool isEnd = availableMoves[i].GetComponent<SquareMechanicsAuto>().end;
            if (isEnd) {
                winningMove = true;
            }
        }
        return winningMove;
    }

    private void ModifyAvailableMoves() {
        List<GameObject> availableMovesTMP = new List<GameObject>();
        for (int i=0; i < availableMoves.Count; i++) {
            if(availableMoves[i].GetComponent<SquareMechanicsAuto>().end ==false ) {
                availableMovesTMP.Add(availableMoves[i]);
            }
        }
        availableMoves.Clear();
        for(int i = 0; i < availableMovesTMP.Count; i++) {
            availableMoves.Add(availableMovesTMP[i]);
        }
    }

    private void MoveToSquare(GameObject targetSquare) {
        //Debug.Log(currentSquare.name +" moving to "+targetSquare.name);
        currentSquare.GetComponent<SquareMechanicsAuto>().unavailable = true;

        bool coinSquare = targetSquare.GetComponent<SquareMechanicsAuto>().coinSquare;
        if (coinSquare) {
            coinVictory += 1;
        }
        currentSquare = targetSquare;
        FindAvailableMoves(currentSquare);
        MoveAI();
    }

    private void FindAvailableMoves(GameObject targetSquare) {
        availableMoves.Clear();
        List<GameObject> adjescentSquares = new List<GameObject>();
        adjescentSquares.Add(GetAbove(targetSquare));
        adjescentSquares.Add(GetBelow(targetSquare));
        adjescentSquares.Add(GetLeft(targetSquare));
        adjescentSquares.Add(GetRight(targetSquare));

        DetermineIfAdjescentSquaresAreAvailableToMove(adjescentSquares);
    }

    private void DetermineIfAdjescentSquaresAreAvailableToMove(List<GameObject> adjescentSquares) {
        for (int i = 0; i < adjescentSquares.Count; i++) {
            if (adjescentSquares[i] != null) {
                bool isBlack = adjescentSquares[i].GetComponent<SquareMechanicsAuto>().blackSquare;
                bool isBlue = adjescentSquares[i].GetComponent<SquareMechanicsAuto>().blueSquare;
                bool isEnd = adjescentSquares[i].GetComponent<SquareMechanicsAuto>().end;
                if (isBlack || isEnd || isBlue) {
                    bool isBad = adjescentSquares[i].GetComponent<SquareMechanicsAuto>().isBad;
                    bool isUnavailable = adjescentSquares[i].GetComponent<SquareMechanicsAuto>().unavailable;
                    if (!isBad && !isUnavailable) {
                        availableMoves.Add(adjescentSquares[i]);
                    }
                }
            }
        }
    }

    private void FindStart() {
        for (int i = 0; i < activeGameBoard.Count; i++) {
            bool isStart = activeGameBoard[i].GetComponent<SquareMechanicsAuto>().start;
            if (isStart) {
                startingSquare = activeGameBoard[i];
                currentSquare = startingSquare;
            }
        }
    }

    private void AssignFunctionsToButtons() {
        newButton.onClick.AddListener(NewButtonOnClick);
        testButton.onClick.AddListener(TestButtonOnClick);
        saveButton.onClick.AddListener(SaveButtonOnClick);
        fillButton.onClick.AddListener(FillButtonOnClick);
    }

    private void TestButtonOnClick() {
        UpdateRandomGenerationList();
        StartAITesting();
    }



    private void SaveButtonOnClick() {
        UpdateRandomGenerationList();
        SaveMapAsPNG();
        RemoveActiveGameBoard();
        DisableButtons();
    }

    private void UpdateRandomGenerationList() {
        ClearRandomGeneration();
        for (int i = 0; i < activeGameBoard.Count; i++) {
            SquareMechanicsAuto square = activeGameBoard[i].GetComponent<SquareMechanicsAuto>();
            if (square.blackSquare && square.coinSquare==false && square.orangeSquare==false) {
                randomGeneration.Add(blackSquare);
            }
            else if (square.blackSquare && square.coinSquare) {
                randomGeneration.Add(coinSquare);
            }
            else if (square.blackSquare && square.orangeSquare) {
                randomGeneration.Add(orangeSquare);
            }
            else if (square.greenSquare) {
                randomGeneration.Add(greenSquare);
            }
            else if (square.redSquare) {
                randomGeneration.Add(redSquare);
            }
            else if (square.start) {
                randomGeneration.Add(startSquare);
            }
            else if (square.end) {
                randomGeneration.Add(endSquare);
            }
            else if (square.blueSquare && square.end == false && square.start == false && square.orangeSquare == false) {
                randomGeneration.Add(blueSquare);
            }
            else if (square.blueSquare && square.end == false && square.start == false && square.orangeSquare == true) {
                randomGeneration.Add(blueOrangeSquare);
            }
        }
    }

    private void DisableButtons() {
        DisableSaveButton();
        DisableTestButton();
    }

    private void EnableButtons() {
        EnableSaveButton();
        EnableTestButton();
        EnableFillButton();
    }

    private void DisableTestButton() {
        testButton.interactable = false;
    }

    private void DisableNewButton() {
        newButton.interactable = false;
    }

    private void DisableSaveButton() {
        saveButton.interactable = false;
    }

    private void EnableTestButton() {
        testButton.interactable = true;
    }
    private void EnableFillButton() {
        fillButton.interactable = true;
    }

    private void EnableSaveButton() {
        saveButton.interactable = true;
    }

    private void SaveMapAsPNG() {
        Texture2D mapTexture = new Texture2D(gameBoardWidth, gameBoardHeight);
        int boardIndex = 0;
        for (int x = 0; x < mapTexture.width; x++) {
            for (int y = 0; y < mapTexture.height; y++) {
                GameObject activeSquare = randomGeneration[boardIndex];

                //Debug.Log(activeSquare);
                Color pixelColor = new Color(0, 0, 0, 0);
                foreach (ColorToPrefab colorMapping in colorMappings) {
                    if (colorMapping.prefab == activeSquare) {
                        pixelColor = colorMapping.color;
                        //Debug.Log(pixelColor);
                    }
                }

                mapTexture.SetPixel(x, y, pixelColor);
                boardIndex++;
            }
        }
        mapTexture.Apply();


        // Encode texture into PNG
        byte[] bytes = mapTexture.EncodeToPNG();
        Destroy(mapTexture);

        string mapName = MapName();

        Debug.Log("saving " + mapName);

        File.WriteAllBytes(Application.dataPath + "/Levels/AutoGenerate/" + mapName + ".png", bytes);

    }

    private string MapName() {
        string date = System.DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        string isCoinMap = "";
        if (numOfCoins>0) {
            isCoinMap = "C";
        }
        string winPercentageInt = ((int)winPercentage).ToString();

        return "map"+ isCoinMap + "_" + gameBoardWidth + "x" + gameBoardHeight + "_" + winPercentageInt +"_"+ date;
    }

    private void NewButtonOnClick() {
        RemoveActiveGameBoard();
        CreateNewActiveGameBoard();
        EnableButtons();
        StartAITesting();
    }

    private void RemoveActiveGameBoard() {
        ClearRandomGeneration();
        DeleteAndClearActiveGameBoard();
    }

    private void CreateNewActiveGameBoard() {
        GameBoardRandomGeneration();
        CreateBoardFromRandomGeneration();
        SetUpCamera();
        ResetIsBadOnSquares();
        SetIsBadOnSquares();
    }

    private void DeleteAndClearActiveGameBoard() {
        for (int i = 0; i < activeGameBoard.Count; i++) {
            Destroy(activeGameBoard[i]);
        }
        activeGameBoard.Clear();
    }

    private void ClearRandomGeneration() {
        randomGeneration.Clear();
    }

    private void SetIsBadOnSquares() {
        FindGreenTurnSquaresBad();
        FindRedTurnSquaresBad();
        FindOrangeTurnSquaresBad();
    }

    private void FindOrangeTurnSquaresBad() {
        for (int i = 0; i < activeGameBoard.Count; i++) {
            bool isOrange = activeGameBoard[i].GetComponent<SquareMechanicsAuto>().orangeSquare;
            if (isOrange) {
                SetIsBadOnSquaresDiagonal(activeGameBoard[i]);
            }
        }
    }



    private void ResetIsBadOnSquares() {
        for (int i = 0; i < activeGameBoard.Count; i++) {
            activeGameBoard[i].GetComponent<SquareMechanicsAuto>().isBad = false;
        }
    }

    private void FindGreenTurnSquaresBad() {
        for (int i = 0; i < activeGameBoard.Count; i++) {
            bool isGreen = activeGameBoard[i].GetComponent<SquareMechanicsAuto>().greenSquare;
            //Debug.Log(isGreen);
            if (isGreen) {
                SetIsBadOnSquaresLeftAndRight(activeGameBoard[i]);
            }
        }
    }

    private void FindRedTurnSquaresBad() {
        for (int i = 0; i < activeGameBoard.Count; i++) {
            bool isRed = activeGameBoard[i].GetComponent<SquareMechanicsAuto>().redSquare;
            //Debug.Log(isGreen);
            if (isRed) {
                SetIsBadOnSquaresAboveAndBelow(activeGameBoard[i]);
            }
        }
    }

    private void SetIsBadOnSquaresDiagonal(GameObject orangeSquare) {
        List<GameObject> diagonals = new List<GameObject>();
        
        diagonals.Add(GetAbove(GetLeft(orangeSquare)));
        diagonals.Add(GetAbove(GetRight(orangeSquare)));
        diagonals.Add(GetBelow(GetLeft(orangeSquare)));
        diagonals.Add(GetBelow(GetRight(orangeSquare)));

        for (int i = 0; i < diagonals.Count; i++) {
            if (diagonals[i] != null) {
                SquareMechanicsAuto SquareMechanicsAuto = diagonals[i].GetComponent<SquareMechanicsAuto>();
                if (SquareMechanicsAuto.blackSquare) {
                    SquareMechanicsAuto.isBad = true;
                }
                else if (SquareMechanicsAuto.blueSquare && SquareMechanicsAuto.end==false && SquareMechanicsAuto.start==false) {
                    SquareMechanicsAuto.isBad = true;
                }
            }
        }
    }

    private void SetIsBadOnSquaresLeftAndRight(GameObject greenSquare) {
        List<GameObject> LeftAndRight = new List<GameObject>();
        LeftAndRight.Add(GetLeft(greenSquare));
        LeftAndRight.Add(GetRight(greenSquare));

        for (int i = 0; i < LeftAndRight.Count; i++) {
            if (LeftAndRight[i] != null) {
                SquareMechanicsAuto SquareMechanicsAuto = LeftAndRight[i].GetComponent<SquareMechanicsAuto>();
                if (SquareMechanicsAuto.blackSquare) {
                    SquareMechanicsAuto.isBad = true;
                }
                else if (SquareMechanicsAuto.blueSquare && SquareMechanicsAuto.end == false && SquareMechanicsAuto.start == false) {
                    SquareMechanicsAuto.isBad = true;
                }
            }
        }
    }

    private void SetIsBadOnSquaresAboveAndBelow(GameObject redSquare) {
        List<GameObject> AboveAndBelow = new List<GameObject>();
        AboveAndBelow.Add(GetAbove(redSquare));
        AboveAndBelow.Add(GetBelow(redSquare));

        for (int i = 0; i < AboveAndBelow.Count; i++) {
            if (AboveAndBelow[i] != null) {
                SquareMechanicsAuto SquareMechanicsAuto = AboveAndBelow[i].GetComponent<SquareMechanicsAuto>();
                if (SquareMechanicsAuto.blackSquare) {
                    SquareMechanicsAuto.isBad = true;
                }
                else if (SquareMechanicsAuto.blueSquare && SquareMechanicsAuto.end == false && SquareMechanicsAuto.start == false) {
                    SquareMechanicsAuto.isBad = true;
                }
            }
        }
    }
    private GameObject GetRight(GameObject targetSquare) {
        int targetX = targetSquare.GetComponent<SquareMechanicsAuto>().gamePositionX;
        int rightX = targetX + 1;

        if (rightX < gameBoardWidth && rightX > -1) {
            int targetIndex = targetSquare.GetComponent<SquareMechanicsAuto>().gamePositionIndex;
            int rightIndex = targetIndex + gameBoardHeight;
            //Debug.Log("Right: " + activeGameBoard[rightIndex].name);
            return activeGameBoard[rightIndex];
        }
        else {
            //Debug.Log("Right: NA");
            return null;
        }
    }

    private GameObject GetLeft(GameObject targetSquare) {
        int targetX = targetSquare.GetComponent<SquareMechanicsAuto>().gamePositionX;
        int leftX = targetX - 1;

        if (leftX < gameBoardWidth && leftX > -1) {
            int targetIndex = targetSquare.GetComponent<SquareMechanicsAuto>().gamePositionIndex;
            int leftIndex = targetIndex - gameBoardHeight;
            //Debug.Log("Left: " + activeGameBoard[leftIndex].name);
            return activeGameBoard[leftIndex];
        }
        else {
            //Debug.Log("Left: NA");
            return null;
        }
    }

    private GameObject GetBelow(GameObject targetSquare) {
        if (targetSquare != null) {
            int targetY = targetSquare.GetComponent<SquareMechanicsAuto>().gamePositionY;
            int belowY = targetY - 1;

            if (belowY < gameBoardHeight && belowY > -1) {
                int targetIndex = targetSquare.GetComponent<SquareMechanicsAuto>().gamePositionIndex;
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

    private GameObject GetAbove(GameObject targetSquare) {
        if (targetSquare != null) {
            int targetY = targetSquare.GetComponent<SquareMechanicsAuto>().gamePositionY;
            int aboveY = targetY + 1;

            if (aboveY < gameBoardHeight && aboveY > -1) {
                int targetIndex = targetSquare.GetComponent<SquareMechanicsAuto>().gamePositionIndex;
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

    private void FillButtonOnClick() {
        UpdateRandomGenerationList();
        DeleteAndClearActiveGameBoard();
        ChangeRandomGeneration();
        CreateBoardFromRandomGeneration();
        SetUpCamera();
        ResetIsBadOnSquares();
        SetIsBadOnSquares();
        StartCoroutine(ChangeGameBoardAroundBluePath());
        EnableButtons();
    }

    IEnumerator ChangeGameBoardAroundBluePath() {
        CheckBlueSquaresAndCoinSquaresForBad();
        while (badPath.Count > 0) {
            //Debug.Log("while loop count");
            UpdateActiveGameBoardAroundBadPath();
            UpdateRandomGenerationList();
            ResetIsBadOnSquares();
            SetIsBadOnSquares();
            CheckBlueSquaresAndCoinSquaresForBad();
            yield return null;
        }
        TestButtonOnClick();
    }

    private void UpdateActiveGameBoardAroundBadPath() {
        for (int i = 0; i < badPath.Count; i++) {
            FindSuroundingSquaresAndChangeToGood(i);
        }
    }

    private void FindSuroundingSquaresAndChangeToGood(int i) {

        GameObject aboveSquare = GetAbove(badPath[i]);
        if (aboveSquare != null) {
            if (aboveSquare.GetComponent<SquareMechanicsAuto>().redSquare == true) {
                OnRedChange(aboveSquare);
            }
        }

        GameObject belowSquare = GetBelow(badPath[i]);
        if (belowSquare != null) {
            if (belowSquare.GetComponent<SquareMechanicsAuto>().redSquare == true) {
                OnRedChange(belowSquare);
            }
        }

        GameObject leftSquare = GetLeft(badPath[i]);
        if (leftSquare != null) {
            if (leftSquare.GetComponent<SquareMechanicsAuto>().greenSquare == true) {
                OnGreenChange(leftSquare);
            }
        }

        GameObject rightSquare = GetRight(badPath[i]);
        if (rightSquare != null) {
            if (rightSquare.GetComponent<SquareMechanicsAuto>().greenSquare == true) {
                OnGreenChange(rightSquare);
            }
        }


        GameObject topLeftSquare = GetAbove(GetLeft(badPath[i]));
        if (topLeftSquare != null) {
            if (topLeftSquare.GetComponent<SquareMechanicsAuto>().orangeSquare == true) {
                if (topLeftSquare.GetComponent<SquareMechanicsAuto>().blueSquare == true){
                    OnOrangeBlueChange(topLeftSquare);
                }
                else {
                    OnOrangeChange(topLeftSquare);
                }
            }
        }


        GameObject topRightSquare = GetAbove(GetRight(badPath[i]));
        if (topRightSquare != null) {
            if (topRightSquare.GetComponent<SquareMechanicsAuto>().orangeSquare == true) {
                if (topRightSquare.GetComponent<SquareMechanicsAuto>().blueSquare == true) {
                    OnOrangeBlueChange(topRightSquare);
                }
                else {
                    OnOrangeChange(topRightSquare);
                }
            }
        }

        GameObject bottomLeftSquare = GetBelow(GetLeft(badPath[i]));
        if (bottomLeftSquare != null) {
            if (bottomLeftSquare.GetComponent<SquareMechanicsAuto>().orangeSquare == true) {
                if (bottomLeftSquare.GetComponent<SquareMechanicsAuto>().blueSquare == true) {
                    OnOrangeBlueChange(bottomLeftSquare);
                }
                else {
                    OnOrangeChange(bottomLeftSquare);
                }
            }
        }

        GameObject bottomRightSquare = GetBelow(GetRight(badPath[i]));
        if (bottomRightSquare != null) {
            if (bottomRightSquare.GetComponent<SquareMechanicsAuto>().orangeSquare == true) {
                if (bottomRightSquare.GetComponent<SquareMechanicsAuto>().blueSquare == true) {
                    OnOrangeBlueChange(bottomRightSquare);
                }
                else {
                    OnOrangeChange(bottomRightSquare);
                }
            }
        }

    }

    private void OnOrangeBlueChange(GameObject square) {
        //Debug.Log("orangeblue caught");
        ChangeActiveGameBoardSquareToBlue(square);
    }



    private void OnOrangeChange(GameObject square) {
        List<String> randomFillerSquares = new List<String>();
        randomFillerSquares.Add("black");
        if (randomFromRedSquares > 0) {
            randomFillerSquares.Add("red");
        }
        if (randomFromGreenSquares > 0) {
            randomFillerSquares.Add("green");
        }

        int randomSquareIndex = UnityEngine.Random.Range(0, randomFillerSquares.Count);
        string changeSquare = randomFillerSquares[randomSquareIndex];

        if (changeSquare == "black") {
            ChangeActiveGameBoardSquareToBlack(square);
        }
        if (changeSquare == "red") {
            ChangeActiveGameBoardSquareToRed(square);
        }
        if (changeSquare == "green") {
            ChangeActiveGameBoardSquareToGreen(square);
        }
    }

    private void OnGreenChange(GameObject square) {
        List<String> randomFillerSquares = new List<String>();
        randomFillerSquares.Add("black");
        randomFillerSquares.Add("red");
        randomFillerSquares.Add("orange");
        randomFillerSquares.Add("black");

        int randomSquareIndex = UnityEngine.Random.Range(0, randomFillerSquares.Count);
        string changeSquare = randomFillerSquares[randomSquareIndex];

        if (changeSquare == "black") {
            ChangeActiveGameBoardSquareToBlack(square);
        }
        if (changeSquare == "red") {
            ChangeActiveGameBoardSquareToRed(square);
        }
        if (changeSquare == "orange") {
            ChangeActiveGameBoardSquareToOrange(square);
        }
    }


    private void OnRedChange(GameObject square) {
        List<String> randomFillerSquares = new List<String>();
        randomFillerSquares.Add("black");
        randomFillerSquares.Add("green");
        randomFillerSquares.Add("orange");
        randomFillerSquares.Add("black");

        int randomSquareIndex = UnityEngine.Random.Range(0, randomFillerSquares.Count);
        string changeSquare = randomFillerSquares[randomSquareIndex];

        if (changeSquare == "black") {
            ChangeActiveGameBoardSquareToBlack(square);
        }
        if (changeSquare == "green") {
            ChangeActiveGameBoardSquareToGreen(square);
        }
        if (changeSquare == "orange") {
            ChangeActiveGameBoardSquareToOrange(square);
        }
    }

    private void ChangeActiveGameBoardSquareToBlue(GameObject square) {
        SquareMechanicsAuto squareMech = square.GetComponent<SquareMechanicsAuto>();
        square.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 238f / 255f, 255f / 255f, 1);
        squareMech.orangeMid.SetActive(false);
        squareMech.start = false;
        squareMech.end = false;
        squareMech.blueSquare = true;
        squareMech.blackSquare = false;
        squareMech.greenSquare = false;
        squareMech.redSquare = false;
        squareMech.orangeSquare = false;
        squareMech.coinSquare = false;
    }

    private void ChangeActiveGameBoardSquareToRed(GameObject square) {
        SquareMechanicsAuto squareMech = square.GetComponent<SquareMechanicsAuto>();
        square.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 17f / 255f, 0f / 255f, 1);
        squareMech.orangeMid.SetActive(false);
        squareMech.start = false;
        squareMech.end = false;
        squareMech.blueSquare = false;
        squareMech.blackSquare = false;
        squareMech.greenSquare = false;
        squareMech.redSquare = true;
        squareMech.orangeSquare = false;
        squareMech.coinSquare = false;
    }

    private void ChangeActiveGameBoardSquareToOrange(GameObject square) {
        SquareMechanicsAuto squareMech = square.GetComponent<SquareMechanicsAuto>();
        square.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 0f / 255f, 0f / 255f, 1);
        squareMech.orangeMid.SetActive(true);
        squareMech.start = false;
        squareMech.end = false;
        squareMech.blueSquare = false;
        squareMech.blackSquare = true;
        squareMech.greenSquare = false;
        squareMech.redSquare = false;
        squareMech.orangeSquare = true;
        squareMech.coinSquare = false;
    }

    private void ChangeActiveGameBoardSquareToGreen(GameObject square) {
        SquareMechanicsAuto squareMech = square.GetComponent<SquareMechanicsAuto>();
        square.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 255f / 255f, 33f / 255f, 1);
        squareMech.orangeMid.SetActive(false);
        squareMech.start = false;
        squareMech.end = false;
        squareMech.blueSquare = false;
        squareMech.blackSquare = false;
        squareMech.greenSquare = true;
        squareMech.redSquare = false;
        squareMech.orangeSquare = false;
        squareMech.coinSquare = false;
    }

    private void ChangeActiveGameBoardSquareToBlack(GameObject square) {
        SquareMechanicsAuto squareMech = square.GetComponent<SquareMechanicsAuto>();
        square.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 0f / 255f, 0f / 255f, 1);
        squareMech.orangeMid.SetActive(false);
        squareMech.start = false;
        squareMech.end = false;
        squareMech.blueSquare = false;
        squareMech.blackSquare = true;
        squareMech.greenSquare = false;
        squareMech.redSquare = false;
        squareMech.orangeSquare = false;
        squareMech.coinSquare = false;
    }

    private void CheckBlueSquaresAndCoinSquaresForBad() {
        badPath.Clear();
        for (int i = 0; i < activeGameBoard.Count; i++) {
            SquareMechanicsAuto square = activeGameBoard[i].GetComponent<SquareMechanicsAuto>();
            if (square.blueSquare && square.end == false && square.start == false) {
                if (square.isBad) {
                    badPath.Add(activeGameBoard[i]);
                }
            }
            else if (square.blackSquare && square.coinSquare) {
                if (square.isBad) {
                    badPath.Add(activeGameBoard[i]);
                }
            }
        }
    }

    private void ChangeRandomGeneration() {
        List<GameObject> randomFillerSquares = new List<GameObject>();

        for (int i = 0; i < randomFromBlackSquares; i++) {
            randomFillerSquares.Add(blackSquare);
        }
        for (int i = 0; i < randomFromGreenSquares; i++) {
            randomFillerSquares.Add(greenSquare);
        }
        for (int i = 0; i < randomFromRedSquares; i++) {
            randomFillerSquares.Add(redSquare);
        }
        for (int i = 0; i < randomFromOrangeSquares; i++) {
            randomFillerSquares.Add(orangeSquare);
        }
        //Keep filling
        for (int i = 0; i < randomGeneration.Count; i++) {
            //if (randomGeneration[i] == blackSquare || randomGeneration[i] == greenSquare || randomGeneration[i] == redSquare || randomGeneration[i] == orangeSquare) {
            if (randomGeneration[i] == blackSquare) {
                int randomSquareIndex = UnityEngine.Random.Range(0, randomFillerSquares.Count);
                randomGeneration[i]=randomFillerSquares[randomSquareIndex];
            }
            //if(randomGeneration[i] == blueSquare || randomGeneration[i] == blueOrangeSquare) {
            if(randomGeneration[i] == blueSquare) {
                int randomSquareIndex = UnityEngine.Random.Range(0, randomFillerSquares.Count);
                if (randomFillerSquares[randomSquareIndex] == orangeSquare) {
                    randomGeneration[i] = blueOrangeSquare;
                }
            }

        }
    }

    private void GameBoardRandomGeneration() {
        int totalSquares = (gameBoardWidth * gameBoardHeight);

        if (!allBlackBoard) {
            totalSquares = totalSquares - 2;
            List<GameObject> randomFillerSquares = new List<GameObject>();

            for (int i = 0; i < randomFromBlackSquares; i++) {
                randomFillerSquares.Add(blackSquare);
            }
            for (int i = 0; i < randomFromGreenSquares; i++) {
                randomFillerSquares.Add(greenSquare);
            }
            for (int i = 0; i < randomFromRedSquares; i++) {
                randomFillerSquares.Add(redSquare);
            }
            for (int i = 0; i < randomFromOrangeSquares; i++) {
                randomFillerSquares.Add(orangeSquare);
            }

            for (int i = 0; i < numOfCoins; i++) {
                randomGeneration.Add(coinSquare);
                totalSquares--;
            }

            for (int i = 0; i < totalSquares; i++) {
                int randomSquareIndex = UnityEngine.Random.Range(0, randomFillerSquares.Count);
                //Debug.Log(randomSquareIndex);
                randomGeneration.Add(randomFillerSquares[randomSquareIndex]);
            }

            randomGeneration.Add(startSquare);
            randomGeneration.Add(endSquare);
            RandomizeGeneration();
        }
        else {
            for (int i = 0; i < totalSquares; i++) {
                randomGeneration.Add(blackSquare);
            }
        }

    }

    private void RandomizeGeneration() {
        for (int i = 0; i < randomGeneration.Count; i++) {
            GameObject tempGameObject = randomGeneration[i];
            int randomIndex = UnityEngine.Random.Range(i, randomGeneration.Count);
            randomGeneration[i] = randomGeneration[randomIndex];
            randomGeneration[randomIndex] = tempGameObject;
        }
    }

    private void CreateBoardFromRandomGeneration() {
        int randomGenerationIndex = 0;
        for (int x = 0; x < gameBoardWidth; x++) {
            for (int y = 0; y < gameBoardHeight; y++) {
                GenerateSquare(x, y, randomGenerationIndex);
                randomGenerationIndex++;
            }

        }
    }

    private void GenerateSquare(int x, int y, int randomGenerationIndex) {
        Vector3 squareSpawnPoint = new Vector3(x, y, 0);
        GameObject square = randomGeneration[randomGenerationIndex];
        GameObject newSquare = Instantiate(square, squareSpawnPoint, Quaternion.identity, gameObject.transform);
        newSquare.name = x + "," + y;
        SquareMechanicsAuto squareData = newSquare.GetComponent<SquareMechanicsAuto>();
        squareData.gamePositionX = x;
        squareData.gamePositionY = y;
        squareData.gamePositionIndex = randomGenerationIndex;

        activeGameBoard.Add(newSquare);
    }

    private void SetUpCamera() {
        Camera.main.transform.position = new Vector3((float)(gameBoardWidth - 1) / 2f, (float)(gameBoardHeight - 1) / 2f, -10f);
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float verticalSize = (float)gameBoardHeight / 2f + borderSize;
        float horizontalSize = ((float)gameBoardWidth / 2f + borderSize) / aspectRatio;

        if (verticalSize > horizontalSize) {
            Camera.main.orthographicSize = verticalSize;
        }
        else {
            Camera.main.orthographicSize = horizontalSize;
        }
    }
}
