using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareMechanics : MonoBehaviour {
    [Header("GameBoard Info")]
    public int gamePositionX;
    public int gamePositionY;
    public int gamePositionIndex;

    [Header("Square Info")]
    public bool start = false;
    public bool end = false;
    public bool currentSquare = false;
    public bool activate = false;
    public bool blueSquare = false;
    public bool orangeSquare = false;
    public bool blackSquare = false;
    public bool greenSquare = false;
    public bool redSquare = false;
    public bool coinSquare = false;
    public bool isBad = false;
    public bool unavailable = false;
    public int isSecret = 0;
    public bool secretFound = false;
    public bool currentOnMouseDown = false;
    public bool treeShown = false;
    public bool xShown = false;
    public bool blankSquare = false;

    public GameObject currentRoad;
    public GameObject previousSquare;
    public Sprite roadSprite;
    public SpriteRenderer road;
    

    [Header("Square Effects")]
    public GameObject hintX;
    public GameObject hintX_fade;
    [SerializeField] Material startMaterial = default;
    [SerializeField] Material endMaterial = default;
    private Material keyMaterial1;
    private Material keyMaterial2;
    [SerializeField] Material keyMaterial1_key1 = default;
    [SerializeField] Material keyMaterial2_key1 = default;
    [SerializeField] Material keyMaterial1_key2 = default;
    [SerializeField] Material keyMaterial2_key2 = default;
    [SerializeField] Material keyMaterial1_key3 = default;
    [SerializeField] Material keyMaterial2_key3 = default;
    [SerializeField] Material keyMaterial1_key4 = default;
    [SerializeField] Material keyMaterial2_key4 = default;
    [SerializeField] GameObject endGraphic = default;

    CreateBoard gameBoard;
    public bool squareEntered = false;
    private bool fadeOutStart = false;
    private bool fadeOutEnd = false;
    private bool fadeOutKey = false;
    private bool endEffectOn = false;
    private bool keyEffectOn = false;
    public bool down = false;
    public int keyNumber = 1;


    private void Start() {
        gameBoard = GetComponentInParent<CreateBoard>();
        SetGrassSortingOrder();
        if (coinSquare) {
            SetKeyMaterials();
            SetKeyParticleEffectMaterial();
        }
    }

    private void SetKeyParticleEffectMaterial() {
        Renderer[] renderers;
        renderers = gameObject.GetComponentsInChildren<Renderer>(true);
        for (int x = 0; x < renderers.Length; x++) {
            if (renderers[x].name == "Sun_Shines_03") {
                renderers[x].material = keyMaterial1;
            }
            if (renderers[x].name == "Dust_01") {
                renderers[x].material = keyMaterial2;
            }
        }
    }

    private void SetKeyMaterials() {
        switch (keyNumber) {
            case 1:
                keyMaterial1 = keyMaterial1_key1;
                keyMaterial2 = keyMaterial2_key1;
                break;
            case 2:
                keyMaterial1 = keyMaterial1_key2;
                keyMaterial2 = keyMaterial2_key2;
                break;
            case 3:
                keyMaterial1 = keyMaterial1_key3;
                keyMaterial2 = keyMaterial2_key3;
                break;
            case 4:
                keyMaterial1 = keyMaterial1_key4;
                keyMaterial2 = keyMaterial2_key4;
                break;
        }
    }

    private void SetGrassSortingOrder() {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>(true);
        for (int x = 0; x < children.Length; x++) {
            if (children[x].name == "GrassTiles") {
                GameObject grassGRP = children[x].gameObject;
                SpriteRenderer[] sprites = grassGRP.GetComponentsInChildren<SpriteRenderer>(true);
                for (int i = 0; i < sprites.Length; i++) {
                    if (sprites[i].name.Contains("GrassTile")) {
                        sprites[i].sortingOrder = -1 * ((gamePositionY * 3) + 1);
                    }
                    if (sprites[i].name.Contains("Bottom")) {
                        sprites[i].sortingOrder = -1 * ((gamePositionY * 3) + 2);
                    }
                    if (sprites[i].name == "Left" || sprites[i].name == "Right" || sprites[i].name == "Top") {
                        sprites[i].sortingOrder = -1 * ((gamePositionY * 3));
                    }
                }
            }
        }
    }

    private void Update() {
        //rayCastForSquare();
        //rayCastForPressDown();
    }



    public void ChangeEndpostToEmpty() {
        gameObject.GetComponent<End_Theme>().SetEmptySign(endGraphic);
        //endGraphic.GetComponent<SpriteRenderer>().sprite = endPostEmpty;
    }
    public void ChangeEndpostToFully() {
        //endGraphic.GetComponent<SpriteRenderer>().sprite = endPost;
        gameObject.GetComponent<End_Theme>().SetFullSign(endGraphic);
    }

    
    private void rayCastForSquare() {
        if (Input.touchCount > 0) {
            if (Input.GetTouch(0).phase.Equals(TouchPhase.Began)) {
                //if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int layerMaskSquares = LayerMask.NameToLayer(layerName: "Square");
                RaycastHit2D squareClicked = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMaskSquares);
                if (squareClicked.collider != null) {
                    if (squareClicked.collider.tag == "GameBoard_Square") {
                        if (squareClicked.collider.name == this.gameObject.name && activate) {
                            TouchOnSquare();
                            squareEntered = true;
                        }
                    }
                }
            }

            if (Input.GetTouch(0).phase.Equals(TouchPhase.Moved)) {
                //if (Input.GetMouseButton(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int layerMaskSquares = LayerMask.NameToLayer(layerName: "Square");
                RaycastHit2D squareClicked = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMaskSquares);
                if (squareClicked.collider != null) {
                    if (squareClicked.collider.tag == "GameBoard_Square") {
                        if (squareClicked.collider.name == this.gameObject.name && activate) {
                            TouchEnterSquare();
                            squareEntered = true;
                        }
                        else if (squareClicked.collider.name != this.gameObject.name && squareEntered == true && activate) {
                            TouchExitSquare();
                            squareEntered = false;
                        }
                    }
                }
            }


            if (Input.GetTouch(0).phase.Equals(TouchPhase.Ended) && squareEntered == true && activate) {
                TouchUpSquare();
                squareEntered = false;
            }
        }
    }
    private void rayCastForPressDown() {
        if (Input.touchCount > 0) {
            if (Input.GetTouch(0).phase.Equals(TouchPhase.Began)) {
                //if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int layerMaskSquares = LayerMask.NameToLayer(layerName: "Square");
                RaycastHit2D squareClicked = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMaskSquares);
                if (squareClicked.collider != null) {
                    if (squareClicked.collider.tag == "GameBoard_Square") {
                        if (squareClicked.collider.name == this.gameObject.name) {
                            if (!down) {
                                SquarePressDown();
                            }
                            down = true;
                        }
                    }
                }
            }

            if (Input.GetTouch(0).phase.Equals(TouchPhase.Moved)) {
                //if (Input.GetMouseButton(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int layerMaskSquares = LayerMask.NameToLayer(layerName: "Square");
                RaycastHit2D squareClicked = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMaskSquares);
                if (squareClicked.collider != null) {
                    if (squareClicked.collider.tag == "GameBoard_Square") {
                        if (squareClicked.collider.name == this.gameObject.name) {
                            if (!down) {
                                SquarePressDown();
                            }
                            down = true;
                        }
                        else if (squareClicked.collider.name != this.gameObject.name) {
                            if (down) {
                                down = false;
                                SquareRelease();
                            }
                        }
                    }
                }
            }

            if (Input.GetTouch(0).phase.Equals(TouchPhase.Ended)) {
                if (down) {
                    down = false;
                    SquareRelease();
                }
            }
        }
    }

    public void TouchOnSquare() {
        //Debug.Log("onMouseDown");
        bool touchEnabled = gameBoard.touchEnabled;
        if (touchEnabled) {
            ChekCurrentOnMouseDown();

            if (blackSquare) {
                gameBoard.AddToHistoryList(gameObject);
                ChangeBlackSquareToBlueSquare();
                CheckSquareForSecrets();
            }
            else if (blueSquare) {
                if (end) {
                    gameBoard.AddToHistoryList(gameObject);
                    SetPreviousSquare();
                    UpdatePreviousDisplayRoad();
                    Debug.Log("End Square Entered OnMouseDown");
                    //gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    activate = false;
                    DisplayEndSquareRoad();
                    gameBoard.CalculateResult();

                    //gameBoard.RemoveFromHistoryList(gameObject);

                }
                else {
                    BlueSquareClicked();
                }
            }
        }
        else {

        }
    }

    public void SquarePressDown() {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - .05f, gameObject.transform.position.z);
        BounceDown();
    }
    public void SquareRelease() {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + .05f, gameObject.transform.position.z);
        BounceUp();
    }

    public void PlayBurstEffect() {
        if (blankSquare == false) {
            GameObject burstEffectGO = null;
            if (orangeSquare == true) {
                burstEffectGO = ThemeManager.TM.GetWaterBurstEffect();
            }
            else {
                burstEffectGO = ThemeManager.TM.GetGrassBurstEffect();
            }


            GameObject burstEffect = Instantiate(burstEffectGO, gameObject.transform.position, Quaternion.identity);
            StartCoroutine(DeleteBurstEffect(burstEffect));
        }
        
    }

    IEnumerator DeleteBurstEffect(GameObject burstEffect) {
        yield return new WaitForSeconds(1f);
        Destroy(burstEffect);
    }

    private void ChekCurrentOnMouseDown() {
        if (currentSquare) {
            currentOnMouseDown = true;
            //Debug.Log("clicked current");
        }
    }

    public void TouchExitSquare() {
        //Debug.Log("onMouseExit");
        currentOnMouseDown = false;
    }

    public void TouchUpSquare() {
        //Debug.Log("onMouseUp");
        if (currentSquare && currentOnMouseDown && start == false) {
            GameObject backTrackSquare = gameBoard.GetPreviousPreviousSquareInHistoryList();
            gameBoard.HistoryAndSquaresResetToThisBlueSquare(backTrackSquare);
            gameBoard.SetActiveSquares(backTrackSquare);
        }
        currentOnMouseDown = false;
    }

    public void TouchEnterSquare() {
        //Debug.Log("onMouseEnter");
        bool touchEnabled = gameBoard.touchEnabled;
        if (touchEnabled) {
            if (Input.GetTouch(0).phase.Equals(TouchPhase.Moved)) {
                if (blackSquare) {
                    gameBoard.AddToHistoryList(gameObject);
                    ChangeBlackSquareToBlueSquare();
                    CheckSquareForSecrets();
                }
                if (blueSquare) {
                    if (end) {
                        gameBoard.AddToHistoryList(gameObject);
                        SetPreviousSquare();
                        UpdatePreviousDisplayRoad();

                        Debug.Log("End Square Entered OnMouseEnter");
                        //gameObject.GetComponent<BoxCollider2D>().enabled = false;
                        activate = false;
                        DisplayEndSquareRoad();
                        gameBoard.CalculateResult();

                        //gameBoard.RemoveFromHistoryList(gameObject);
                    }

                    CheckBackTrackBluePath();
                }
            }
        }
    }

    private void CheckSquareForSecrets() {
        int mistakesMade = gameBoard.CheckForMistakesOnRoad();

        if (mistakesMade == 0) {


            if (isSecret > 0) {
                int blockIndex = LevelManager.levelManager.block;
                if (blockIndex == 1) {
                    blockIndex = 2;
                }
                blockIndex = blockIndex - 2;

                int secretIndex = isSecret - 1;
                int secretStatus = GameDataControl.gdControl.blockSecretsUnlocked[blockIndex][secretIndex];

                if (secretStatus == 0) {
                    PlaySecretFoundFX();
                    Debug.Log("Congrats, secret " + isSecret + "for block " + (LevelManager.levelManager.block - 1) + " has been discovered.");
                    Debug.Log("Finish the level to collect it");
                    secretFound = true;
                    CheckIfItsTheFirstSecretEverFound();
                }
                else {
                    Debug.Log("This secret has already been discovered.");
                }
            }

        }

    }

    public void BounceObect() {
        gameObject.GetComponentInChildren<Animator>().SetTrigger("Bounce");
    }

    public void BounceDown() {
        //Bounce Down Animation OnTouch

        //Start, End, BlueHouse, RedHouse
        if (end == true || start == true || greenSquare == true || redSquare == true) {
            gameObject.GetComponentInChildren<Animator>().enabled = true;
            gameObject.GetComponentInChildren<Animator>().SetTrigger("BounceDown");
        }
        //Trees
        else if (blackSquare == true && isBad==true && orangeSquare == false) {
            if (treeShown == true || xShown == true) {
                gameObject.GetComponentInChildren<Animator>().enabled = true;
                gameObject.GetComponentInChildren<Animator>().SetTrigger("BounceDown");
            }
        }
        else if (blueSquare == true && isBad == true && orangeSquare == false) {
            if ( treeShown == true || xShown == true) {
                gameObject.GetComponentInChildren<Animator>().enabled = true;
                gameObject.GetComponentInChildren<Animator>().SetTrigger("BounceDown");
            }
        }

        //Water, WaterTrees
        else if(orangeSquare == true) {
            //Two Animators, Water and Trees
            if (isBad == true) {
                Animator[] anims = gameObject.GetComponentsInChildren<Animator>(true);
                for (int i = 0; i < anims.Length; i++) {
                    anims[i].enabled = true;
                    anims[i].SetTrigger("BounceDown");
                }
            //One Aniamtor
            }
            else {
                gameObject.GetComponentInChildren<Animator>().enabled = true;
                gameObject.GetComponentInChildren<Animator>().SetTrigger("BounceDown");
            }
        }
    }

    public void BounceUp() {
        //Bounce Up Animation OnRelease

        //Start, End, BlueHouse, RedHouse
        if (end == true || start == true || greenSquare == true || redSquare == true) {
            gameObject.GetComponentInChildren<Animator>().SetTrigger("BounceUp");
        }
        //Trees
        else if (blackSquare == true && isBad == true && orangeSquare == false) {
            if (treeShown == true || xShown == true) {
                gameObject.GetComponentInChildren<Animator>().SetTrigger("BounceUp");
            }
        }
        else if (blueSquare == true && isBad == true && orangeSquare == false ) {
            if (treeShown == true || xShown == true) {
                gameObject.GetComponentInChildren<Animator>().SetTrigger("BounceUp");
            }
        }
        //Water, WaterTrees
        else if (orangeSquare == true) {
            //Two Animators, Water and Trees
            if (isBad == true) {
                Animator[] anims = gameObject.GetComponentsInChildren<Animator>(true);
                for (int i = 0; i < anims.Length; i++) {
                    anims[i].SetTrigger("BounceUp");
                }
            //One Animator
            }
            else {
                gameObject.GetComponentInChildren<Animator>().SetTrigger("BounceUp");
            }
        }

    }

    private void CheckIfItsTheFirstSecretEverFound() {
        int scroll = LevelManager.levelManager.scroll;
        int block = LevelManager.levelManager.block;
        int level = LevelManager.levelManager.level;

        if (scroll == 0 && block == 2 && level == 1) {
            Debug.Log("first secret in the game");
            FindObjectOfType<CreateBoard>().DisableTouch();
            StartCoroutine(ShowHelpMenu());
        }

    }

    IEnumerator ShowHelpMenu() {
        yield return new WaitForSeconds(1f);
        FindObjectOfType<HelpMenuMechanics>().FirstSecret();
    }

    private void PlaySecretFoundFX() {
        GameObject secretPetalDisplay = gameObject.transform.GetChild(3).gameObject;
        GameObject secretPetalEffects = gameObject.transform.GetChild(5).gameObject;
        secretPetalDisplay.SetActive(true);
        secretPetalEffects.SetActive(true);
        FindObjectOfType<SoundManager>().PlaySound("SecretFound");
    }

    public void ShowTreeIfBad() {
        if (isBad == true) {
            if (treeShown == false) {
                treeShown = true;
                StartCoroutine(HoldShowTree());
            }
        }
    }

    public void ShowXIfBad() {
        if (isBad == true) {
            if (xShown == false) {
                xShown = true;
                StartCoroutine(HoldShowX());
            }
        }
    }

    public void ShowXIfBadFade() {
        if (isBad == true) {
            StartCoroutine(HoldShowXFade());
        }
    }

    IEnumerator HoldShowXFade() {
        float seconds = UnityEngine.Random.Range(0.1f, 1.0f);
        yield return new WaitForSeconds(seconds);
        CreateXFade();
    }

    IEnumerator HoldShowX() {
        float seconds = UnityEngine.Random.Range(0.1f, 1.0f);
        yield return new WaitForSeconds(seconds);
        CreateX();
    }

    IEnumerator HoldShowTree() {
        float seconds = UnityEngine.Random.Range(0.1f, 1.0f);
        yield return new WaitForSeconds(seconds);
        //StartCoroutine(ShowTree());
        CreateTree();
    }

    private void CreateX() {
        GameObject treeTileGRP;
        if (orangeSquare == true) {
            treeTileGRP = gameObject.transform.GetChild(3).gameObject;
        }
        else {
            treeTileGRP = gameObject.transform.GetChild(2).gameObject;
        }
        GameObject tree = Instantiate(hintX, gameObject.transform.position, Quaternion.identity, treeTileGRP.transform);

        Transform[] allChildrenTree = tree.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildrenTree) {
            if (!child.name.Contains("Effect")) {
                //added extra -1 to get under trees
                child.GetComponent<ParticleSystemRenderer>().sortingOrder = (-1 * gamePositionY) - 1;
            }
        }

    }

    private void CreateXFade() {
        GameObject treeTileGRP;
        if (orangeSquare == true) {
            treeTileGRP = gameObject.transform.GetChild(3).gameObject;
        }
        else {
            treeTileGRP = gameObject.transform.GetChild(2).gameObject;
        }
        GameObject tree = Instantiate(hintX_fade, gameObject.transform.position, Quaternion.identity, treeTileGRP.transform);

        Transform[] allChildrenTree = tree.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildrenTree) {
            if (!child.name.Contains("Effect")) {
                //added extra -1 to get under trees
                child.GetComponent<ParticleSystemRenderer>().sortingOrder = (-1 * gamePositionY) - 1;
            }
        }

    }

    private void CreateTree() {
        GameObject treeTileGRP;
        if (orangeSquare == true) {
            treeTileGRP = gameObject.transform.GetChild(3).gameObject;
        }
        else {
            treeTileGRP = gameObject.transform.GetChild(2).gameObject;
        }

        GameObject treeGO = ThemeManager.TM.GetTree();
        GameObject tree = Instantiate(treeGO, gameObject.transform.position, Quaternion.identity, treeTileGRP.transform);

        Transform[] allChildrenTree = tree.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildrenTree) {
            if (!child.name.Contains("Effect")) {
                child.GetComponent<ParticleSystemRenderer>().sortingOrder = (-1 * gamePositionY);
            }
        }

    }

    private void CheckBackTrackBluePath() {
        GameObject square = gameBoard.GetPreviousPreviousSquareInHistoryList();
        if (square != null) {
            if (gameObject == square) {
                BackTrackBluePath();
            }
        }
    }

    private void BackTrackBluePath() {
        BlueSquareClicked();

        //SelectTileChangeAudioAndPlay();
    }

    public void SelectTileChangeAudioAndPlay() {
        if (orangeSquare == true|| blankSquare == true) {
            PlayWaterTileChangeAudio();
        }
        else {
            PlayGrassTileChangeAudio();
        }
    }

    private void BlueSquareClicked() {
        //if (!end) {
        //Debug.Log("clicked blue square");
        if (!currentSquare) {
            gameBoard.HistoryAndSquaresResetToThisBlueSquare(gameObject);
            gameBoard.SetActiveSquares(gameObject);
        }
        /*
        }
        else {
            Debug.Log("End Square Clicked");
            Debug.Log("ASDFADSFASDFADSF");
            gameBoard.CalculateResult();
        }
        */
    }



    public void ChangeBlueSquareToBlackSquare() {
        //SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        //renderer.color = new Color(0f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
        HideRoadDisplay();
        CheckIfSecretsDisplayedAndHide();

        BoxCollider2D col = gameObject.GetComponent<BoxCollider2D>();
        col.size = new Vector2(0.9f, 0.9f);
        //col.enabled = false;
        activate = false;

        blueSquare = false;
        blackSquare = true;

        if (coinSquare) {
            BlueToBlackCoinSquare();
        }
        if (orangeSquare) {
            BlueToBlackOrangeSquare();
        }
    }

    private void CheckIfSecretsDisplayedAndHide() {
        if (isSecret > 0) {
            GameObject secretPetalDisplay = gameObject.transform.GetChild(3).gameObject;
            secretPetalDisplay.SetActive(false);

            GameObject secretPetalEffects = gameObject.transform.GetChild(5).gameObject;
            secretPetalEffects.SetActive(false);
        }
    }

    private void ChangeBlackSquareToBlueSquare() {
        SetPreviousSquare();
        UnHideRoadDisplay();
        UpdatePreviousDisplayRoad();
        DisplayRoad();

        PlayBurstEffect();
        SelectTileChangeAudioAndPlay();

        BoxCollider2D col = gameObject.GetComponent<BoxCollider2D>();
        col.size = new Vector2(1f, 1f);
        blueSquare = true;
        blackSquare = false;
        gameBoard.SetActiveSquares(gameObject);

        if (coinSquare) {
            BlackToBlueCoinSquare();
        }
        if (orangeSquare) {
            BlackToBlueOrangeSquare();
        }
    }

    private void PlayGrassTileChangeAudio() {
        int randomGrassSound = UnityEngine.Random.Range(1, 3);
        if (randomGrassSound == 1) {
            FindObjectOfType<SoundManager>().PlayOneShotSound("grassSFX1");
        }
        if (randomGrassSound == 2) {
            FindObjectOfType<SoundManager>().PlayOneShotSound("grassSFX2");
        }
    }
    private void PlayWaterTileChangeAudio() {
        int randomGrassSound = UnityEngine.Random.Range(1, 3);
        if (randomGrassSound == 1) {
            FindObjectOfType<SoundManager>().PlayOneShotSound("WoodSFX1");
        }
        if (randomGrassSound == 2) {
            FindObjectOfType<SoundManager>().PlayOneShotSound("WoodSFX2");
        }
    }




    private void BlackToBlueOrangeSquare() {
        //HideOrangeDisplay();
    }
    private void BlueToBlackOrangeSquare() {
        //DisplayOrange();
    }

    private void BlackToBlueCoinSquare() {
        HideCoinDisplay();
        gameBoard.IncreaseCoinCount();
        gameBoard.EndSquareBounce();
    }

    private void HideOrangeDisplay() {
        GameObject orangeCorners = gameObject.transform.Find("orangeCorners").gameObject;
        orangeCorners.SetActive(false);
    }

    private void HideCoinDisplay() {
        GameObject coin = gameObject.transform.Find("coin").gameObject;
        coin.SetActive(false);
        FindObjectOfType<SoundManager>().PlaySound("Sign");
    }

    private void BlueToBlackCoinSquare() {
        DisplayCoin();
        gameBoard.DecreaseCoinCount();
    }

    private void DisplayCoin() {
        GameObject coin = gameObject.transform.Find("coin").gameObject;
        coin.SetActive(true);
    }

    private void DisplayOrange() {
        GameObject orangeCorners = gameObject.transform.Find("orangeCorners").gameObject;
        orangeCorners.SetActive(true);
    }

    public void TurnOnStartEffect() {

        Debug.Log("Turn on Start Effect");
        if (start == true) {
            Color tmpStartColor = startMaterial.GetColor("_TintColor");
            Color newStartColor = new Color(tmpStartColor.r, tmpStartColor.g, tmpStartColor.b, 0f);
            startMaterial.SetColor("_TintColor", newStartColor);

            GameObject effectGRP = gameObject.transform.GetChild(3).gameObject;
            effectGRP.SetActive(true);

            StartCoroutine(FadeStart(128 / 255f, 0.5f));
        }
    }

    public void TurnOffStartEffect() {
        if (start == true) {
            StartCoroutine(FadeStart(0f, 0.5f));
        }
    }

    IEnumerator FadeStart(float aValue, float aTime) {
        fadeOutStart = false;
        if (aValue == 0f) {
            fadeOutStart = true;
        }
        Color startColor = startMaterial.GetColor("_TintColor");
        float alpha = startColor.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(alpha, aValue, t));
            startMaterial.SetColor("_TintColor", newColor);
            yield return null;
        }
        if (fadeOutStart) {
            GameObject effectGRP = gameObject.transform.GetChild(3).gameObject;
            effectGRP.SetActive(false);
        }
    }

    public void TurnOnEndEffect() {
        if (endEffectOn == false) {
            if (end == true) {
                Debug.Log("Turn on End Effect");
                endEffectOn = true;
                Color tmpEndColor = endMaterial.GetColor("_TintColor");
                Color newEndColor = new Color(tmpEndColor.r, tmpEndColor.g, tmpEndColor.b, 0f);
                endMaterial.SetColor("_TintColor", newEndColor);

                GameObject effectGRP = gameObject.transform.GetChild(3).gameObject;
                effectGRP.SetActive(true);

                StartCoroutine(FadeEnd(128 / 255f, 0.5f));
            }
        }
    }
    public void TurnOffEndEffect() {
        if (endEffectOn) {
            if (end == true) {
                StartCoroutine(FadeEnd(0f, 0.5f));
                Debug.Log("Turn off End Effect");
                endEffectOn = false;
            }
        }
    }

    IEnumerator FadeEnd(float aValue, float aTime) {

        fadeOutEnd = false;
        if (aValue == 0f) {
            fadeOutEnd = true;
        }

        Color endColor = endMaterial.GetColor("_TintColor");
        float alpha = endColor.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
            Color newColor = new Color(endColor.r, endColor.g, endColor.b, Mathf.Lerp(alpha, aValue, t));
            endMaterial.SetColor("_TintColor", newColor);
            yield return null;
        }

        if (fadeOutEnd) {
            GameObject effectGRP = gameObject.transform.GetChild(3).gameObject;
            effectGRP.SetActive(false);
        }
    }



    public void TurnOnKeyEffect() {
        if (keyEffectOn == false) {
            if (coinSquare == true) {
                Debug.Log("Turn on Key Effect");
                keyEffectOn = true;
                Color tmpKeyColor1 = keyMaterial1.GetColor("_TintColor");
                Color tmpKeyColor2 = keyMaterial2.GetColor("_TintColor");

                Color newEndColor1 = new Color(tmpKeyColor1.r, tmpKeyColor1.g, tmpKeyColor1.b, 0f);
                Color newEndColor2 = new Color(tmpKeyColor2.r, tmpKeyColor2.g, tmpKeyColor2.b, 0f);


                keyMaterial1.SetColor("_TintColor", newEndColor1);
                keyMaterial2.SetColor("_TintColor", newEndColor2);

                GameObject effectGRP = gameObject.transform.GetChild(4).gameObject;
                effectGRP.SetActive(true);

                StartCoroutine(FadeKey(128 / 255f, 0.5f));
            }
        }
    }
    public void TurnOffKeyEffect() {
        if (keyEffectOn) {
            if (coinSquare == true) {
                StartCoroutine(FadeKey(0f, 0.5f));
                Debug.Log("Turn off Key Effect");
                keyEffectOn = false;
            }
        }
    }

    IEnumerator FadeKey(float aValue, float aTime) {

        fadeOutKey = false;
        if (aValue == 0f) {
            fadeOutKey = true;
        }

        Color keyColor1 = keyMaterial1.GetColor("_TintColor");
        Color keyColor2 = keyMaterial2.GetColor("_TintColor");

        float alpha = keyColor1.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
            Color newColor1 = new Color(keyColor1.r, keyColor1.g, keyColor1.b, Mathf.Lerp(alpha, aValue, t));
            Color newColor2 = new Color(keyColor2.r, keyColor2.g, keyColor2.b, Mathf.Lerp(alpha, aValue, t));

            keyMaterial1.SetColor("_TintColor", newColor1);
            keyMaterial2.SetColor("_TintColor", newColor2);

            yield return null;
        }

        if (fadeOutKey) {
            GameObject effectGRP = gameObject.transform.GetChild(4).gameObject;
            effectGRP.SetActive(false);
        }
    }














    #region Roads
    private void HideRoadDisplay() {
        //Destroy(currentRoad);
        road.enabled = false;
    }
    private void UnHideRoadDisplay() {
        //Destroy(currentRoad);
        road.enabled = true;
    }

    private void DisplayRoad() {
        SetCorrectRoadPrefab();
        road.sprite = roadSprite;
       // GameObject roadTileGRP = gameObject.transform.GetChild(1).gameObject;
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }

    public void UpdateDisplayRoad(GameObject nextSquare) {
        if (start != true) {
            if (previousSquare == gameBoard.GetAbove(gameObject)) {
                UpdateRoadFromPrevTop(nextSquare);
            }
            if (previousSquare == gameBoard.GetLeft(gameObject)) {
                UpdateRoadFromPrevLeft(nextSquare);
            }
            if (previousSquare == gameBoard.GetRight(gameObject)) {
                UpdateRoadFromPrevRight(nextSquare);
            }
            if (previousSquare == gameBoard.GetBelow(gameObject)) {
                UpdateRoadFromPrevBottom(nextSquare);
            }
        }

        else {
            UpdateStartRoadTile(nextSquare);
        }
    }

    private void UpdateStartRoadTile(GameObject nextSquare) {
        GameObject above = gameBoard.GetAbove(gameObject);
        GameObject below = gameBoard.GetBelow(gameObject);
        GameObject left = gameBoard.GetLeft(gameObject);
        GameObject right = gameBoard.GetRight(gameObject);

        if (above == nextSquare) {
            DisplayStartRoadTileAbove();
        }
        else if (below == nextSquare) {
            DisplayStartRoadTileBelow();
        }
        else if (left == nextSquare) {
            DisplayStartRoadTileLeft();
        }
        else if (right == nextSquare) {
            DisplayStartRoadTileRight();
        }

    }

    private void DisplayStartRoadTileRight() {
        //DeleteStartRoadDisplay();

        //GameObject roadTileGRP = gameObject.transform.GetChild(2).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, RightMiddle.Count + 1);
        //roadSprite = RightMiddle[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        roadSprite = ThemeManager.TM.GetStartRightMiddleSprite();
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }

    private void DisplayStartRoadTileLeft() {
        //DeleteStartRoadDisplay();

        //GameObject roadTileGRP = gameObject.transform.GetChild(2).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, LeftMiddle.Count + 1);
        //roadSprite = LeftMiddle[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        roadSprite = ThemeManager.TM.GetStartLeftMiddleSprite();
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }

    private void DisplayStartRoadTileBelow() {
        //DeleteStartRoadDisplay();

        //GameObject roadTileGRP = gameObject.transform.GetChild(2).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, BottomMiddle.Count + 1);
        //roadSprite = BottomMiddle[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        roadSprite = ThemeManager.TM.GetStartBottomMiddleSprite();
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }

    private void DisplayStartRoadTileAbove() {
        //DeleteStartRoadDisplay();

        //GameObject roadTileGRP = gameObject.transform.GetChild(2).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, TopMiddle.Count + 1);
        //roadSprite = TopMiddle[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);

        roadSprite = ThemeManager.TM.GetStartTopMiddleSprite();
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }


    private void UpdateRoadFromPrevBottom(GameObject nextSquare) {
        if (nextSquare == gameBoard.GetAbove(gameObject)) {
            SetRoadPrefabBottomTop();
        }
        if (nextSquare == gameBoard.GetLeft(gameObject)) {
            SetRoadPrefabBottomLeft();
        }
        if (nextSquare == gameBoard.GetRight(gameObject)) {
            SetRoadPrefabBottomRight();
        }
    }

    private void SetRoadPrefabBottomTop() {
        //DeleteRoadDisplay();

        //GameObject roadTileGRP = gameObject.transform.GetChild(1).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, BottomTop.Count + 1);
        //roadSprite = BottomTop[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        if (orangeSquare == false && blankSquare == false) {
            roadSprite = ThemeManager.TM.GetBottomTopSprite();
        }
        else {
            roadSprite = ThemeManager.TM.GetBottomTopBridgeSprite();
        }
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }

    private void SetRoadPrefabBottomLeft() {
        //DeleteRoadDisplay();

        //GameObject roadTileGRP = gameObject.transform.GetChild(1).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, BottomLeft.Count + 1);
        //roadSprite = BottomLeft[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        if (orangeSquare == false && blankSquare == false) {
            roadSprite = ThemeManager.TM.GetBottomLeftSprite();
        }
        else {
            roadSprite = ThemeManager.TM.GetBottomLeftBridgeSprite();
        }
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }

    private void SetRoadPrefabBottomRight() {
        //DeleteRoadDisplay();

        //GameObject roadTileGRP = gameObject.transform.GetChild(1).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, BottomRight.Count + 1);
        //roadSprite = BottomRight[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        if (orangeSquare == false && blankSquare == false) {
            roadSprite = ThemeManager.TM.GetBottomRightSprite();
        }
        else {
            roadSprite = ThemeManager.TM.GetBottomRightBridgeSprite();
        }
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }

    private void UpdateRoadFromPrevRight(GameObject nextSquare) {
        if (nextSquare == gameBoard.GetAbove(gameObject)) {
            SetRoadPrefabRightTop();
        }
        if (nextSquare == gameBoard.GetLeft(gameObject)) {
            SetRoadPrefabRightLeft();
        }
        if (nextSquare == gameBoard.GetBelow(gameObject)) {
            SetRoadPrefabRightBottom();
        }
    }

    private void SetRoadPrefabRightTop() {
        //DeleteRoadDisplay();

        //GameObject roadTileGRP = gameObject.transform.GetChild(1).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, RightTop.Count + 1);
        //roadSprite = RightTop[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        if (orangeSquare == false && blankSquare == false) {
            roadSprite = ThemeManager.TM.GetRightTopSprite();
        }
        else {
            roadSprite = ThemeManager.TM.GetRightTopBridgeSprite();
        }
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }

    private void SetRoadPrefabRightLeft() {
        //DeleteRoadDisplay();

        //GameObject roadTileGRP = gameObject.transform.GetChild(1).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, RightLeft.Count + 1);
        //roadSprite = RightLeft[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        if (orangeSquare == false && blankSquare == false) {
            roadSprite = ThemeManager.TM.GetRightLeftSprite();
        }
        else {
            roadSprite = ThemeManager.TM.GetRightLeftBridgeSprite();
        }
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }

    private void SetRoadPrefabRightBottom() {
        //DeleteRoadDisplay();

        //GameObject roadTileGRP = gameObject.transform.GetChild(1).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, RightBottom.Count + 1);
        //roadSprite = RightBottom[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        if (orangeSquare == false && blankSquare == false) {
            roadSprite = ThemeManager.TM.GetRightBottomSprite();
        }
        else {
            roadSprite = ThemeManager.TM.GetRightBottomBridgeSprite();
        }
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }

    private void UpdateRoadFromPrevLeft(GameObject nextSquare) {
        if (nextSquare == gameBoard.GetAbove(gameObject)) {
            SetRoadPrefabLeftTop();
        }
        if (nextSquare == gameBoard.GetRight(gameObject)) {
            SetRoadPrefabLeftRight();
        }
        if (nextSquare == gameBoard.GetBelow(gameObject)) {
            SetRoadPrefabLeftBottom();
        }
    }

    private void SetRoadPrefabLeftTop() {
        //DeleteRoadDisplay();

        //GameObject roadTileGRP = gameObject.transform.GetChild(1).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, LeftTop.Count + 1);
        //roadSprite = LeftTop[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        if (orangeSquare == false && blankSquare == false) {
            roadSprite = ThemeManager.TM.GetLeftTopSprite();
        }
        else {
            roadSprite = ThemeManager.TM.GetLeftTopBridgeSprite();
        }
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }

    private void SetRoadPrefabLeftRight() {
        //DeleteRoadDisplay();

        //GameObject roadTileGRP = gameObject.transform.GetChild(1).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, LeftRight.Count + 1);
        //roadSprite = LeftRight[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        if (orangeSquare == false && blankSquare == false) {
            roadSprite = ThemeManager.TM.GetLeftRightSprite();
        }
        else {
            roadSprite = ThemeManager.TM.GetLeftRightBridgeSprite();
        }
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }

    private void SetRoadPrefabLeftBottom() {
        //DeleteRoadDisplay();

        //GameObject roadTileGRP = gameObject.transform.GetChild(1).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, LeftBottom.Count + 1);
        //roadSprite = LeftBottom[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        if (orangeSquare == false && blankSquare == false) {
            roadSprite = ThemeManager.TM.GetLeftBottomSprite();
        }
        else {
            roadSprite = ThemeManager.TM.GetLeftBottomBridgeSprite();
        }
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }

    private void UpdateRoadFromPrevTop(GameObject nextSquare) {
        if (nextSquare == gameBoard.GetLeft(gameObject)) {
            SetRoadPrefabTopLeft();
        }
        if (nextSquare == gameBoard.GetRight(gameObject)) {
            SetRoadPrefabTopRight();
        }
        if (nextSquare == gameBoard.GetBelow(gameObject)) {
            SetRoadPrefabTopBelow();
        }
    }

    private void SetRoadPrefabTopBelow() {
        //DeleteRoadDisplay();

        //GameObject roadTileGRP = gameObject.transform.GetChild(1).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, TopBottom.Count + 1);
        //roadSprite = TopBottom[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        if (orangeSquare == false && blankSquare == false) {
            roadSprite = ThemeManager.TM.GetTopBottomSprite();
        }
        else {
            roadSprite = ThemeManager.TM.GetTopBottomBridgeSprite();
        }
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }

    private void SetRoadPrefabTopRight() {
        //DeleteRoadDisplay();

        //GameObject roadTileGRP = gameObject.transform.GetChild(1).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, TopRight.Count + 1);
        //roadSprite = TopRight[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        if (orangeSquare == false && blankSquare == false) {
            roadSprite = ThemeManager.TM.GetTopRightSprite();
        }
        else {
            roadSprite = ThemeManager.TM.GetTopRightBridgeSprite();
        }

        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }

    private void SetRoadPrefabTopLeft() {
        //DeleteRoadDisplay();

        //GameObject roadTileGRP = gameObject.transform.GetChild(1).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, TopLeft.Count + 1);
        //roadSprite = TopLeft[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        if (orangeSquare == false && blankSquare == false) {
            roadSprite = ThemeManager.TM.GetTopLeftSprite();
        }
        else {
            roadSprite = ThemeManager.TM.GetTopLeftBridgeSprite();
        }
        
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }

    private void SetCorrectRoadPrefab() {
        if (previousSquare == gameBoard.GetAbove(gameObject)) {
            SetRoadPrefabTop();
        }
        if (previousSquare == gameBoard.GetLeft(gameObject)) {
            SetRoadPrefabLeft();
        }
        if (previousSquare == gameBoard.GetRight(gameObject)) {
            SetRoadPrefabRight();
        }
        if (previousSquare == gameBoard.GetBelow(gameObject)) {
            SetRoadPrefabBelow();
        }
    }

    private void SetRoadPrefabTop() {


        //int randomIndex = UnityEngine.Random.Range(1, TopMiddle.Count + 1);
        //roadSprite = TopMiddle[randomIndex - 1];
        if (orangeSquare == false && blankSquare == false) {
            roadSprite = ThemeManager.TM.GetTopMiddleSprite();
        }
        else {
            roadSprite = ThemeManager.TM.GetTopMiddleBridgeSprite();
        }
    }

    private void SetRoadPrefabLeft() {
        //int randomIndex = UnityEngine.Random.Range(1, LeftMiddle.Count + 1);
        //roadSprite = LeftMiddle[randomIndex - 1];
        if (orangeSquare == false && blankSquare == false) {
            roadSprite = ThemeManager.TM.GetLeftMiddleSprite();
        }
        else {
            roadSprite = ThemeManager.TM.GetLeftMiddleBridgeSprite();
        }
    }

    private void SetRoadPrefabRight() {
        //int randomIndex = UnityEngine.Random.Range(1, RightMiddle.Count + 1);
        //roadSprite = RightMiddle[randomIndex - 1];
        if (orangeSquare == false && blankSquare == false) {
            roadSprite = ThemeManager.TM.GetRightMiddleSprite();
        }
        else {
            roadSprite = ThemeManager.TM.GetRightMiddleBridgeSprite();
        }
    }

    private void SetRoadPrefabBelow() {
        //int randomIndex = UnityEngine.Random.Range(1, BottomMiddle.Count + 1);
        //roadSprite = BottomMiddle[randomIndex - 1];

        if (orangeSquare == false && blankSquare == false) {
            roadSprite = ThemeManager.TM.GetBottomMiddleSprite();
        }
        else {
            roadSprite = ThemeManager.TM.GetBottomMiddleBridgeSprite();
        }
    }

    private void UpdatePreviousDisplayRoad() {
        //Debug.Log("test");
        previousSquare.GetComponent<SquareMechanics>().UpdateDisplayRoad(gameObject);
    }

    private void SetPreviousSquare() {
        previousSquare = gameBoard.GetPreviousPreviousSquareInHistoryList();
        //Debug.Log(previousSquare);
    }
    
    private void DisplayEndSquareRoad() {
        DeleteEndRoadDisplay();
        DisplayCorrectTile();
    }



    private void DeleteEndRoadDisplay() {
        //GameObject roadTileGRP = gameObject.transform.GetChild(2).gameObject;
        //foreach (Transform child in roadTileGRP.transform) {
        //    child.transform.parent = null;
        //    Destroy(child.gameObject);
        //}
        road.enabled = false;
    }
    
    private void DisplayCorrectTile() {
        GameObject above = gameBoard.GetAbove(gameObject);
        GameObject below = gameBoard.GetBelow(gameObject);
        GameObject left = gameBoard.GetLeft(gameObject);
        GameObject right = gameBoard.GetRight(gameObject);
        
        road.enabled = true;
        if (above == previousSquare) {
            DisplayEndRoadTileAbove();
        }
        else if (below == previousSquare) {
            DisplayEndRoadTileBelow();
        }
        else if (left == previousSquare) {
            DisplayEndRoadTileLeft();
        }
        else if (right == previousSquare) {
            DisplayEndRoadTileRight();
        }
    }

    private void DisplayEndRoadTileAbove() {
        //GameObject roadTileGRP = gameObject.transform.GetChild(2).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, TopMiddle.Count + 1);
        //roadSprite = TopMiddle[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);


        roadSprite = ThemeManager.TM.GetTopMiddleSprite();
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }
    private void DisplayEndRoadTileBelow() {
        //GameObject roadTileGRP = gameObject.transform.GetChild(2).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, BottomMiddle.Count + 1);
        //roadSprite = BottomMiddle[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        roadSprite = ThemeManager.TM.GetBottomMiddleSprite();
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }
    private void DisplayEndRoadTileRight() {
        //GameObject roadTileGRP = gameObject.transform.GetChild(2).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, RightMiddle.Count + 1);
        //roadSprite = RightMiddle[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        roadSprite = ThemeManager.TM.GetRightMiddleSprite();
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }
    private void DisplayEndRoadTileLeft() {
        //GameObject roadTileGRP = gameObject.transform.GetChild(2).gameObject;
        //int randomIndex = UnityEngine.Random.Range(1, LeftMiddle.Count + 1);
        //roadSprite = LeftMiddle[randomIndex - 1];
        //currentRoad = Instantiate(roadSprite, gameObject.transform.position, Quaternion.identity, roadTileGRP.transform);
        //currentRoad.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
        roadSprite = ThemeManager.TM.GetLeftMiddleSprite();
        road.sprite = roadSprite;
        road.GetComponent<SpriteRenderer>().sortingOrder = ((-1 * gamePositionY) - 3);
    }
    public void FadeEndRoadDisplay() {

        StartCoroutine(FadeOutAndHide(0.5f));
    }

    IEnumerator FadeOutAndHide(float duration) {
        Color currentColor = road.color;
        Color endColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);

        for (float t = 0f; t < duration; t += Time.deltaTime) {
            float normalizedTime = t / duration;
            road.color = Color.Lerp(currentColor, endColor, normalizedTime);
            yield return null;
        }

        road.color = endColor;
        road.enabled = false;
        road.color = currentColor;
    }
    #endregion



}
