using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareMechanicsAuto : MonoBehaviour {
    public int gamePositionX;
    public int gamePositionY;
    public int gamePositionIndex;

    public bool start = false;
    public bool end = false;
    public bool activate = false;
    public bool blueSquare = false;
    public bool blackSquare = false;
    public bool greenSquare = false;
    public bool redSquare = false;
    public bool orangeSquare = false;
    public bool coinSquare = false;

    public bool isBad = false;
    public bool unavailable = false;

    AutoGenerate autoBoard;
    //List<string> changeSquares;
    public int changeSquaresIndex = 0;

    public GameObject startMid;
    public GameObject endMid;
    public GameObject coinMid;
    public GameObject orangeMid;

    private void Start() {
        //List<string> changeSquares = new List<string>() { "start", "black", "green", "red", "end" };
        AssignChangeSquareIndex();
    }

    private void AssignChangeSquareIndex() {
        if (start) {
            changeSquaresIndex = 0;
        }
        if (blackSquare && coinSquare == false && orangeSquare == false) {
            changeSquaresIndex = 1;
        }
        if (greenSquare) {
            changeSquaresIndex = 2;
        }
        if (redSquare) {
            changeSquaresIndex = 3;
        }
        if (end) {
            changeSquaresIndex = 4;
        }
        if (blackSquare && coinSquare) {
            changeSquaresIndex = 5;
        }
        if (blackSquare && orangeSquare) {
            changeSquaresIndex = 6;
        }
        if (blueSquare && !end && !start) {
            changeSquaresIndex = 7;
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1)) { 
            Vector2 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(screenPoint, Vector2.zero);
        
            if (hit.collider != null) {
                if (hit.collider.name == gameObject.name) {
                    if (changeSquaresIndex == 0) {
                        ChangeToBlack();
                    }
                    else if (changeSquaresIndex == 1) {
                        ChangeToBlue();
                    }
                    else if (changeSquaresIndex == 2) {
                        ChangeToRed();
                    }
                    else if (changeSquaresIndex == 3) {
                        ChangeToEnd();
                    }
                    else if (changeSquaresIndex == 4) {
                        ChangeToCoin();
                    }
                    else if (changeSquaresIndex == 5) {
                        ChangeToOrange();
                    }
                    else if (changeSquaresIndex == 6) {
                        ChangeToGreen();
                    }
                    else if (changeSquaresIndex == 7) {
                        ChangeToStart();
                    }
                }
            }
        }
    }

    private void ChangeToBlue() {
        changeSquaresIndex = 2;

        blackSquare = false;

        blueSquare = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 238f / 255f, 255f / 255f, 1);
    }

    private void ChangeToOrange() {
        changeSquaresIndex = 6;

        coinSquare = false;
        coinMid.SetActive(false);

        orangeSquare = true;
        orangeMid.SetActive(true);
    }

    private void ChangeToStart() {
        changeSquaresIndex = 0;

        greenSquare = false;

        blueSquare = true;
        start = true;
        startMid.SetActive(true);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 238f / 255f, 255f / 255f, 1);
    }

    private void ChangeToEnd() {
        changeSquaresIndex = 4;

        redSquare = false;

        blueSquare = true;
        end = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 238f / 255f, 255f / 255f, 1);
        endMid.SetActive(true);
    }

    private void ChangeToRed() {
        changeSquaresIndex = 3;

        blueSquare = false;

        redSquare = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 17f / 255f, 0f / 255f, 1);
    }

    private void ChangeToGreen() {
        changeSquaresIndex = 7;

        blackSquare = false;
        orangeSquare = false;
        orangeMid.SetActive(false);

        greenSquare = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 255f / 255f, 33f / 255f, 1);
    }

    private void ChangeToBlack() {
        changeSquaresIndex = 1;

        start = false;
        startMid.SetActive(false);
        blueSquare = false;

        blackSquare = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 0f / 255f, 0f / 255f, 1);
    }

    private void ChangeToCoin() {
        changeSquaresIndex = 5;
        blueSquare = false;
        end = false;
        endMid.SetActive(false);

        blackSquare = true;
        coinSquare = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 0f / 255f, 0f / 255f, 1);
        coinMid.SetActive(true);
    }
}
