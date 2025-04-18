using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollClickPrevention : MonoBehaviour {

    private float startingValue = 1.0f;
    private float newValue;
    [SerializeField] ScrollBarFix swipingRect = default;
    public bool isScrolling=false;
    public bool isSwiping=false;

    private void Start() {
        isScrolling = false;
        isSwiping = false;
    }

    private void Update() {
        if (Input.touchCount > 0) {
            if (!isScrolling) {

                if (Input.GetTouch(0).phase.Equals(TouchPhase.Began)) {
                    startingValue = gameObject.GetComponent<Scrollbar>().value;
                    newValue = startingValue;
                    //Debug.Log("scroll starting value " + startingValue);
                }

                if (Input.GetTouch(0).phase.Equals(TouchPhase.Moved)) {
                    newValue = gameObject.GetComponent<Scrollbar>().value;
                    isSwiping=swipingRect.isSwiping;
                    //Debug.Log("scrolling ended " + newValue);
                }

                float difference = Mathf.Abs(startingValue - newValue);
                //Debug.Log("difference " + difference);

                if (!isSwiping) {
                    if (difference > .01f) {
                        isScrolling = true;
                        Debug.Log("We Scrolling");
                        Debug.Log(gameObject.name);
                    }
                }
            }
            
            if (Input.GetTouch(0).phase.Equals(TouchPhase.Ended)) {
                isScrolling = false;
                isSwiping = false;
                //Debug.Log("We Stopped Scrolling");
            }
        }
    }
}
