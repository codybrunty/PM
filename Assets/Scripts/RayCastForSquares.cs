using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastForSquares : MonoBehaviour{

    public SquareMechanics square;
    public SquareMechanics oldSquare;

    private void Update() {
        rayCastForSquare();
    }


    private void rayCastForSquare() {
        if (Input.touchCount > 0) {
            if (Input.GetTouch(0).phase.Equals(TouchPhase.Began)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                int layerMaskSquares = LayerMask.NameToLayer(layerName: "Square");
                RaycastHit2D squareClicked = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMaskSquares);
                if (squareClicked.collider != null) {
                    if (squareClicked.collider.tag == "GameBoard_Square") {
                        if (square != squareClicked.collider.gameObject.GetComponent<SquareMechanics>()) {
                            oldSquare = square;
                            square = squareClicked.collider.gameObject.GetComponent<SquareMechanics>();
                            if (square.activate) {
                                square.TouchOnSquare();
                                square.squareEntered = true;
                            }
                            if (!square.down) {
                                square.SquarePressDown();
                            }
                            square.down = true;
                        }
                    }
                }
            }

            if (Input.GetTouch(0).phase.Equals(TouchPhase.Moved)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                int layerMaskSquares = LayerMask.NameToLayer(layerName: "Square");
                RaycastHit2D squareClicked = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMaskSquares);
                if (squareClicked.collider != null) {
                    if (squareClicked.collider.tag == "GameBoard_Square") {
                        if (square != squareClicked.collider.gameObject.GetComponent<SquareMechanics>()) {
                            oldSquare = square;
                            square = squareClicked.collider.gameObject.GetComponent<SquareMechanics>();
                            if (square.activate) {
                                square.TouchEnterSquare();
                                square.squareEntered = true;
                            }

                            if (!square.down) {
                                square.SquarePressDown();
                            }
                            square.down = true;

                            if (square != oldSquare && oldSquare != null ) {
                                if (oldSquare.squareEntered == true && oldSquare.activate) {
                                    oldSquare.TouchExitSquare();
                                    oldSquare.squareEntered = false;
                                }
                                if (oldSquare.down) {
                                    oldSquare.down = false;
                                    oldSquare.SquareRelease();
                                }
                            }
                        }
                        
                    }
                }
            }

            if (Input.GetTouch(0).phase.Equals(TouchPhase.Ended)) {
                if (square != null) {
                    if (square.squareEntered == true && square.activate) {
                        square.TouchUpSquare();
                        square.squareEntered = false;

                    }
                    if (square.down) {
                        square.down = false;
                        square.SquareRelease();
                    }
                }
                square = null;
                oldSquare = null;
            }
        }
    }
}
