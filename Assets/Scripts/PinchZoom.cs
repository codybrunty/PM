using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoom : MonoBehaviour {
    float minZoom;
    float maxZoom;
    public float zoomSpeed = 1f;
    float previousDistance;
    Vector3 zoomCenter;
    Vector3 newClampedCameraPosition;
    //Vector2 startingMidPoint;
    Vector2 pan_startingMidPoint;
    Vector2 pan_oldMidPoint;
    Vector3 camStart;
    private bool isZooming = false;
    [SerializeField] CreateBoard gameBoard = default;
    [SerializeField] float zoomEaseOut = 0.05f;
    [SerializeField] float panXEaseOut = 0.2f;
    [SerializeField] float panYEaseOut = 0.2f;
    //[SerializeField] float pinchBufferNumber = 0.2f;
    [SerializeField] GameObject cameraHolder = default;
    public float cameraHolderOffset = 0;
    Vector2 fingerPosition;

    private float boardWidth;
    private float borderSize;
    private float gamezone;

    private float newOrtho;
    private float oldOrtho;
    private float olderOrtho;
    private float easeZoomDifference;
    private float newPanX;
    private float oldPanX;
    private float olderPanX;
    private float easePanXDifference;
    private float newPanY;
    private float oldPanY;
    private float olderPanY;
    private float easePanYDifference;

    private float deltaMag;
    private float oldDeltaMag;
    private float pinchAmount;
    private float oldPinchAmount;
    //private bool pinchBuffer = false;
    public bool zoomEnabled = true;
    public GameObject canvas = default;
    private float canvasMaxScale;
    private float canvaMinScale;

    public AnimationCurve easeCurve=default;

    private void Start() {
        cameraHolder.transform.position = cameraHolder.transform.position + new Vector3(0f, cameraHolderOffset, 0f);

        minZoom = Camera.main.orthographicSize / 2.5f;
        maxZoom = Camera.main.orthographicSize;
        canvaMinScale = 1 ;
        canvasMaxScale = canvaMinScale * 2.5f;

        camStart = Camera.main.transform.position;
        boardWidth = gameBoard.gameBoardWidth;
        borderSize = gameBoard.borderSize;
        gamezone = boardWidth + borderSize + borderSize;
        newClampedCameraPosition = Camera.main.transform.position;
        SetOrthos();
        CheckZoomEnabled();
    }

    public void AfterWinCheckZoom() {
        if (zoomEnabled) {
            zoomEnabled = false;
            if (Camera.main.orthographicSize != maxZoom) {
                float seconds = 1.5f;
                StartCoroutine(WinZoomOut(seconds));
            }
        }
    }

    IEnumerator WinZoomOut(float seconds) {
        float currentCamOrthographicSize = Camera.main.orthographicSize;
        Vector3 currentCamPosition = Camera.main.transform.localPosition;
        Vector3 finalCamPosition = new Vector3(0f,0f,0f);

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / seconds) {
            Camera.main.orthographicSize = Mathf.Lerp(currentCamOrthographicSize, maxZoom, easeCurve.Evaluate(t));
            Camera.main.transform.localPosition = Vector3.Lerp(currentCamPosition, finalCamPosition, easeCurve.Evaluate(t));
            CanvasScaleToMatch();
            yield return null;
        }
        Camera.main.orthographicSize = maxZoom;
        Camera.main.transform.localPosition = finalCamPosition;
        CanvasScaleToMatch();
    }


    private void CheckZoomEnabled() {
        int width = FindObjectOfType<CreateBoard>().gameBoardWidth;
        if (width < 9) {
            zoomEnabled = false;
        }
    }

    private void SetOrthos() {
        newOrtho = Camera.main.orthographicSize;
        oldOrtho = Camera.main.orthographicSize;
        olderOrtho = Camera.main.orthographicSize;
    }

    void Update() {
        if (zoomEnabled) {
            ZoomProcedure();
            CanvasScaleToMatch();
        }

    }

    private void CanvasScaleToMatch() {
        float currentCamOrtho = Camera.main.orthographicSize;
        float CanvasScaleLerp = Mathf.InverseLerp(minZoom, maxZoom, currentCamOrtho);
        CanvasScaleLerp = 1.0f - CanvasScaleLerp;
        CanvasScaleLerp = Mathf.Lerp(canvaMinScale, canvasMaxScale, CanvasScaleLerp);

        //Vector3 cameraPosition = Camera.main.transform.localPosition;
        //canvas.transform.localPosition = cameraPosition*-1;
        canvas.transform.localScale = new Vector3(CanvasScaleLerp, CanvasScaleLerp, CanvasScaleLerp);
    }

    private void ZoomProcedure() {
        //Debug.Log(easePanXDifference);
        if (Input.touchCount == 0 && isZooming) {
            EnableGameBoard();
            CalculateZoomEaseOut();
            CalculatePanXEaseOut();
            CalculatePanYEaseOut();
        }
        if (Input.touchCount == 2 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)) {
            DisableGameBoard();
            easeZoomDifference = 0f;
            easePanXDifference = 0f;
            easePanYDifference = 0f;

            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            zoomCenter = Camera.main.ScreenToWorldPoint((touch1.position + touch2.position) / 2f);
            zoomCenter.z = Camera.main.transform.position.z;
            //startingMidPoint = (touch1.position + touch2.position) / 2f;

            pan_startingMidPoint = (GetWorldPositionOfFinger(0) + GetWorldPositionOfFinger(1)) / 2f;

            oldDeltaMag = (touch1.position - touch2.position).magnitude;
        }
        else if (Input.touchCount == 2 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)) {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            //Vector2 touchZeroPrevPos = touch1.position - touch1.deltaPosition;
            //Vector2 touchOnePrevPos = touch2.position - touch2.deltaPosition;

            //float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;

            deltaMag = (touch1.position - touch2.position).magnitude;

            //float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            float deltaMagnitudeDiff = oldDeltaMag - deltaMag;
            pinchAmount = (deltaMagnitudeDiff) * zoomSpeed * Time.deltaTime;


            oldDeltaMag = deltaMag;
            
            /*
            if (pinchAmount > 0 && oldPinchAmount > 0 || pinchAmount < 0 && oldPinchAmount < 0) {
            }
            else {
                //Debug.Log("activate buffer");
                pinchBuffer = true;
            }

            if (pinchBuffer) {
                if (pinchAmount < pinchBufferNumber && pinchAmount > -1 * pinchBufferNumber) {
                    pinchAmount = 0;
                }
                else {
                    if (pinchAmount > 0) {
                        pinchAmount -= pinchBufferNumber;
                        pinchBuffer = false;
                    }
                    else {
                        pinchAmount += pinchBufferNumber;
                        pinchBuffer = false;
                    }
                }
                
            }
            */

            //Debug.Log(pinchAmount);
            //zoom the camera
            if (Camera.main.orthographic) {
                olderOrtho = oldOrtho;
                oldOrtho = Camera.main.orthographicSize;

                //StartCoroutine(changeCameraOrtho(Mathf.Clamp(Camera.main.orthographicSize + pinchAmount, minZoom, maxZoom)));

                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + pinchAmount, minZoom, maxZoom);
                newOrtho = Camera.main.orthographicSize;
            }
            else {
                Camera.main.transform.Translate(0, 0, pinchAmount);
            }
            //calculate pan zoom movement
            Vector2 midPoint = (touch1.position + touch2.position) / 2f;
            float multiplier = (1.0f / Camera.main.orthographicSize * pinchAmount);
            Vector3 zoomTowards = new Vector3(midPoint.x, midPoint.y, Camera.main.transform.position.z);
            zoomTowards = Camera.main.ScreenToWorldPoint(zoomTowards);

            //calculating clamps
            float currentOrtho = Camera.main.orthographicSize;
            float diffOrtho = maxZoom - currentOrtho;
            float divOrtho = (maxZoom / currentOrtho);
            float halfGameZone = gamezone / 2f;
            float xDivisor = halfGameZone / divOrtho;
            float minX = (-0.5f - borderSize) + xDivisor;
            float maxX = ((boardWidth - 0.5f) + borderSize) - xDivisor;
            float maxY = camStart.y + diffOrtho;
            float minY = camStart.y - diffOrtho;

            newClampedCameraPosition = Camera.main.transform.position;

            //pan zoom towards pinch
            if (Camera.main.orthographicSize != minZoom) {

                Vector3 newCameraPosition = Camera.main.transform.position - (zoomTowards - transform.position) * multiplier;

                newClampedCameraPosition = new Vector3(
                    Mathf.Clamp(newCameraPosition.x, minX, maxX),
                    Mathf.Clamp(newCameraPosition.y, minY, maxY),
                    Camera.main.transform.position.z);

            }
            //pan movement from touches
            olderPanX = oldPanX;
            oldPanX = newPanX;
            olderPanY = oldPanY;
            oldPanY = newPanY;
            Vector2 pan_currentMidPoint = (GetWorldPositionOfFinger(0) + GetWorldPositionOfFinger(1)) / 2f;
            Vector2 panDifference = pan_currentMidPoint - pan_startingMidPoint;

            newClampedCameraPosition = new Vector3(
                Mathf.Clamp(newClampedCameraPosition.x - panDifference.x, minX, maxX),
                Mathf.Clamp(newClampedCameraPosition.y - panDifference.y, minY, maxY),
                Camera.main.transform.position.z);

            newPanX = newClampedCameraPosition.x;
            newPanY = newClampedCameraPosition.y;
        }

        if (easeZoomDifference != 0f) {
            ZoomEaseOut();
        }
        if (easePanXDifference != 0f || easePanYDifference != 0f) {
            PanEaseOut();
        }

        if (newClampedCameraPosition != Camera.main.transform.position) {
            Camera.main.transform.position = newClampedCameraPosition;
        }

        oldPinchAmount = pinchAmount;
    }

    private IEnumerator changeCameraOrtho(float value) {
        Debug.Log("test1");
        yield return new WaitForEndOfFrame();
        Debug.Log("test2");
        Camera.main.orthographicSize = value;
        newOrtho = Camera.main.orthographicSize;
    }

    private void PanEaseOut() {
        float currentOrtho = Camera.main.orthographicSize;
        float diffOrtho = maxZoom - currentOrtho;
        float divOrtho = (maxZoom / currentOrtho);
        float halfGameZone = gamezone / 2f;
        float xDivisor = halfGameZone / divOrtho;
        float minX = (-0.5f - borderSize) + xDivisor;
        float maxX = ((boardWidth - 0.5f) + borderSize) - xDivisor;
        float maxY = camStart.y + diffOrtho;
        float minY = camStart.y - diffOrtho;

        if (easePanXDifference > 0) {
            easePanXDifference -= easePanXDifference / panXEaseOut * Time.deltaTime;
            if (easePanXDifference < .01f) {
                easePanXDifference = 0f;
            }
        }
        if (easePanXDifference < 0) {
            easePanXDifference -= easePanXDifference / panXEaseOut * Time.deltaTime;
            if (easePanXDifference > -.01f) {
                easePanXDifference = 0f;
            }
        }
        if (easePanYDifference > 0) {
            easePanYDifference -= easePanYDifference / panYEaseOut * Time.deltaTime;
            if (easePanYDifference < .01f) {
                easePanYDifference = 0f;
            }
        }
        if (easePanYDifference < 0) {
            easePanYDifference -= easePanYDifference / panYEaseOut * Time.deltaTime;
            if (easePanYDifference > -.01f) {
                easePanYDifference = 0f;
            }
        }

        newClampedCameraPosition = new Vector3(
            Mathf.Clamp(newClampedCameraPosition.x - easePanXDifference, minX, maxX),
            Mathf.Clamp(newClampedCameraPosition.y - easePanYDifference, minY, maxY),
            Camera.main.transform.position.z);
    }

    private void ZoomEaseOut() {
        if (easeZoomDifference > 0) {
            /*
            if (wheelDifference > 75) {
                wheelDifference = 75;
            }
            */
            easeZoomDifference -= easeZoomDifference / zoomEaseOut * Time.deltaTime;
            if (easeZoomDifference < .01f) {
                easeZoomDifference = 0f;
            }

            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - easeZoomDifference, minZoom, maxZoom);
            //StartCoroutine(changeCameraOrtho(Mathf.Clamp(Camera.main.orthographicSize - easeZoomDifference, minZoom, maxZoom)));
        }

        if (easeZoomDifference < 0) {
            /*
            if (wheelDifference < -75) {
                wheelDifference = -75;
            }
            */
            easeZoomDifference -= easeZoomDifference / zoomEaseOut * Time.deltaTime;
            if (easeZoomDifference > -.01f) {
                easeZoomDifference = 0f;
            }

            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - easeZoomDifference, minZoom, maxZoom);
        }
    }

    private void CalculatePanXEaseOut() {
        easePanXDifference = oldPanX - newPanX;
        if (oldPanX == newPanX) {
            easePanXDifference = olderPanX - newPanX;
        }
    }

    private void CalculatePanYEaseOut() {
        easePanYDifference = oldPanY - newPanY;
        if (oldPanY == newPanY) {
            easePanYDifference = olderPanY - newPanY;
        }
    }

    private void CalculateZoomEaseOut() {
        easeZoomDifference = olderOrtho - newOrtho;
        if (olderOrtho == newOrtho) {
            easeZoomDifference = olderOrtho - newOrtho;
        }
    }

    private void EnableGameBoard() {
        isZooming = false;
        gameBoard.EnableTouch();
    }
    private void DisableGameBoard() {
        isZooming = true;
        gameBoard.DisableTouch();
    }
    Vector2 GetWorldPositionOfFinger(int FingerIndex) {
        return Camera.main.GetComponent<Camera>().ScreenToWorldPoint(Input.GetTouch(FingerIndex).position);
    }
}



