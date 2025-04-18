using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class ScrollBarFix : ScrollRect {

    private bool routeToParent = false;
    public Vector3 destination;
    public float percentThreshold = 0.2f;
    public float easing = 0.05f;
    private int positionIndex = 2;
    private bool first = false;
    private int scrolling=0;
    public bool isSwiping = false;
    private bool isScrolling = false;
    private float lastTouchDeltaNormalized = 0.0f;
    public AnimationCurve easeCurve = default;

    protected override void Start() {
        positionIndex = 2;
        scrolling = 0;
        isScrolling = false;
        first = false;

        easeCurve = FindObjectOfType<ScrollBarAnimationCurvves>().easeCurve;
    }

    override protected void LateUpdate() {
        base.LateUpdate();
        if (this.verticalScrollbar) {
            this.verticalScrollbar.size = 0;
        }
    }

    override public void Rebuild(CanvasUpdate executing) {
        base.Rebuild(executing);
        if (this.verticalScrollbar) {
            this.verticalScrollbar.size = 0;
        }
    }

    /// <summary>
    /// Do action for all parents
    /// </summary>
    private void DoForParents<T>(Action<T> action) where T : IEventSystemHandler {
        Transform parent = transform.parent;
        while (parent != null) {
            foreach (var component in parent.GetComponents<Component>()) {
                if (component is T)
                    action((T)(IEventSystemHandler)component);
            }
            parent = parent.parent;
        }
    }

    /// <summary>
    /// Always route initialize potential drag event to parents
    /// </summary>
    public override void OnInitializePotentialDrag(PointerEventData eventData) {
        DoForParents<IInitializePotentialDragHandler>((parent) => { parent.OnInitializePotentialDrag(eventData); });
        base.OnInitializePotentialDrag(eventData);
    }

    /// <summary>
    /// Drag event
    /// </summary>
    public override void OnDrag(UnityEngine.EventSystems.PointerEventData eventData) {
        if (routeToParent)
            DoForParents<IDragHandler>((parent) => { parent.OnDrag(eventData); });
        else
            base.OnDrag(eventData);


        if (Input.touchCount > 0 && Input.GetTouch(0).phase.Equals(TouchPhase.Moved)) {
            Vector2 touchDelta = Input.GetTouch(0).deltaPosition;
            float touchDeltaNormalized = (touchDelta.x / Screen.width);
            lastTouchDeltaNormalized = touchDeltaNormalized;
        }

    }

    /// <summary>
    /// Begin drag event
    /// </summary>
    public override void OnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData) {
        CheckIsScrolling();
        if (!isScrolling) {
            if (!horizontal && Math.Abs(eventData.delta.x) > Math.Abs(eventData.delta.y)) {
                routeToParent = true;
                Debug.Log("Swiping");
                isSwiping = true;
            }

            else if (!vertical && Math.Abs(eventData.delta.x) < Math.Abs(eventData.delta.y))
                routeToParent = true;
            else
                routeToParent = false;

            if (routeToParent)
                DoForParents<IBeginDragHandler>((parent) => { parent.OnBeginDrag(eventData); });
            else
                base.OnBeginDrag(eventData);
        }

    }

    private void CheckIsScrolling() {
        Debug.Log("Scroll Check");
        ScrollClickPrevention[] scrolls;
        int scrollCount = 0;
        scrolls = gameObject.GetComponentsInChildren<ScrollClickPrevention>(true);
        for (int x = 0; x < scrolls.Length; x++) {
            if (scrolls[x].isScrolling == true) {
                scrollCount++;
            }
        }
        if (scrollCount > 0) {
            isScrolling = true;
        }
        else {
            isScrolling = false;
        }
    }

    /// <summary>
    /// End drag event
    /// </summary>
    /// 


    public override void OnEndDrag(UnityEngine.EventSystems.PointerEventData eventData) {
        //Debug.LogWarning(gameObject.name);

        if (routeToParent)
            DoForParents<IDragHandler>((parent) => { parent.OnDrag(eventData); });
        else
            base.OnEndDrag(eventData);

        routeToParent = false;

        if (gameObject.name == "ScrollListCTRL") {

            ScrollClickPrevention[] scrolls;
            scrolls = gameObject.GetComponentsInChildren<ScrollClickPrevention>(true);

            for (int x = 0; x < scrolls.Length; x++) {
                if (scrolls[x].isScrolling == true) {
                    //this gets rid of scrolling hitch when swiping while scrolling was enabled
                    scrolling++;
                }
            }
        }

        //Debug.LogWarning(gameObject.name);
        //Debug.LogWarning(first);
        //Debug.LogWarning(scrolling);

        if (gameObject.name == "ScrollListCTRL" && first == false && scrolling == 0) {
            
            first = true;
            float screenSize = Screen.width;
            //Debug.LogWarning("Screen Width: "+screenSize);

            float swipeThreshold = .03f;
            int flick = 0;

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
                Vector2 touchDelta = Input.GetTouch(0).deltaPosition;
                float touchDeltaNormalized = (touchDelta.x / Screen.width);

                //Debug.LogWarning("Touch Delta: " + touchDelta.x);

                //Debug.LogWarning("Last Touch Delta Normalized: " + lastTouchDeltaNormalized);
                //Debug.LogWarning("Ended Touch Delta Normalized: " + touchDeltaNormalized);

                if (Mathf.Abs(touchDeltaNormalized) < swipeThreshold) {
                    touchDeltaNormalized = lastTouchDeltaNormalized;
                }

                if (touchDeltaNormalized > swipeThreshold) {
                    flick = 1;
                }
                if (touchDeltaNormalized < -1 * swipeThreshold) {
                    flick = 2;
                }
            }

            if (flick == 0) {
                Debug.Log("no flick");
                CalculateClosestPosition();
                destination = new Vector3(destination.x, gameObject.transform.GetChild(0).localPosition.y, gameObject.transform.GetChild(0).localPosition.z);

                //gameObject.transform.GetChild(0).localPosition = destination;
                //first = false;
                //isSwiping = false;
                
                StartCoroutine(SmoothMove( destination));
            }
            if (flick == 1) {
                CheckIsScrolling();
                Debug.Log("flick left check");
                Debug.Log(gameObject.name);
                if (!isScrolling) {
                    Debug.Log("flick left");
                    CalculateRightPosition();
                    destination = new Vector3(destination.x, gameObject.transform.GetChild(0).localPosition.y, gameObject.transform.GetChild(0).localPosition.z);
                    StartCoroutine(SmoothMove(destination));
                }
            }
            if (flick == 2) {
                CheckIsScrolling();
                Debug.Log("flick right check");
                Debug.Log(gameObject.name);
                if (!isScrolling) {
                    Debug.Log("flick right");
                    CalculateLeftPosition();
                    destination = new Vector3(destination.x, gameObject.transform.GetChild(0).localPosition.y, gameObject.transform.GetChild(0).localPosition.z);
                    StartCoroutine(SmoothMove(destination));
                }
            }
        }

        if (gameObject.name == "ScrollListCTRL" && first == false && scrolling != 0) {
            scrolling = 0;
        }


        if (isSwiping == true) {
            Debug.Log("done swiping");
            isSwiping = false;
        }
        //Debug.Log(scrolling);
    }

    public void FlickRight() {
        isSwiping = true;
        first = true;
        //gameObject.GetComponent<ScrollRect>().horizontal = false;
        CalculateLeftPosition();
        destination = new Vector3(destination.x, gameObject.transform.GetChild(0).localPosition.y, gameObject.transform.GetChild(0).localPosition.z);
        StartCoroutine(SmoothMove( destination));
    }
    public void FlickLeft() {
        isSwiping = true;
        first = true;
        //gameObject.GetComponent<ScrollRect>().horizontal = false;
        CalculateRightPosition();
        destination = new Vector3(destination.x, gameObject.transform.GetChild(0).localPosition.y, gameObject.transform.GetChild(0).localPosition.z);
        StartCoroutine(SmoothMove( destination));
    }

    private void CalculateRightPosition() {
        GameObject SwipeGRP = gameObject.transform.GetChild(0).gameObject;
        GameObject pos1 = gameObject.transform.GetChild(1).gameObject;
        GameObject pos2 = gameObject.transform.GetChild(2).gameObject;
        GameObject pos3 = gameObject.transform.GetChild(3).gameObject;

        positionIndex++;
        if (positionIndex > 3) {
            positionIndex = 3;
        }
        if (positionIndex == 1) {
            destination = pos1.transform.localPosition;
        }
        if (positionIndex == 2) {
            destination = pos2.transform.localPosition;
        }
        if (positionIndex == 3) {
            destination = pos3.transform.localPosition;
        }
        //Debug.Log(positionIndex);
    }

    private void CalculateLeftPosition() {
        GameObject SwipeGRP = gameObject.transform.GetChild(0).gameObject;
        GameObject pos1 = gameObject.transform.GetChild(1).gameObject;
        GameObject pos2 = gameObject.transform.GetChild(2).gameObject;
        GameObject pos3 = gameObject.transform.GetChild(3).gameObject;

        positionIndex--;
        if (positionIndex < 1) {
            positionIndex = 1;
        }

        if (positionIndex == 1) {
            destination = pos1.transform.localPosition;
        }
        if (positionIndex == 2) {
            destination = pos2.transform.localPosition;
        }
        if (positionIndex == 3) {
            destination = pos3.transform.localPosition;
        }

        //Debug.Log(positionIndex);
    }

    private void CalculateClosestPosition() {
        GameObject SwipeGRP = gameObject.transform.GetChild(0).gameObject;
        GameObject pos1 = gameObject.transform.GetChild(1).gameObject;
        GameObject pos2 = gameObject.transform.GetChild(2).gameObject;
        GameObject pos3 = gameObject.transform.GetChild(3).gameObject;
        
        float dis1 = Vector3.Distance(SwipeGRP.transform.position, pos1.transform.position);
        float dis2 = Vector3.Distance(SwipeGRP.transform.position, pos2.transform.position);
        float dis3 = Vector3.Distance(SwipeGRP.transform.position, pos3.transform.position);


        if (dis1 == Mathf.Min(dis1, dis2, dis3)) {
            destination = pos1.transform.localPosition;
            positionIndex = 1;
        }
        if (dis2 == Mathf.Min(dis1, dis2, dis3)) {
            destination = pos2.transform.localPosition;
            positionIndex = 2;
        }
        if (dis3 == Mathf.Min(dis1, dis2, dis3)) {
            destination = pos3.transform.localPosition;
            positionIndex = 3;
        }
        //Debug.Log(positionIndex);
    }

    IEnumerator SmoothMove( Vector3 endPos) {
        float seconds = .19f;
        Vector3 startPos = gameObject.transform.GetChild(0).localPosition;
        //gameObject.GetComponent<ScrollBarFix>().inertia = false;
        //Debug.Log("smooth transition");

        for (float t = 0.0f; t< 1.0f; t += Time.deltaTime / seconds) {
            gameObject.transform.GetChild(0).localPosition = Vector3.Lerp(startPos, endPos, easeCurve.Evaluate(t));
            yield return null;
        }

        ChangeTitle();
        gameObject.transform.GetChild(0).localPosition = endPos;
        //gameObject.GetComponent<ScrollRect>().horizontal = true;
        first = false;
        isSwiping = false;
        //gameObject.GetComponent<ScrollBarFix>().inertia = true;
    }

    private void ChangeTitle() {
        //Debug.Log("positionindex "+positionIndex); // 3 2 1
        TitleMechanics[] titles;
        titles = transform.parent.GetComponentsInChildren<TitleMechanics>(true);
        int titleIndex = Mathf.Abs(positionIndex - 3);
        
        for (int x = 0; x < titles.Length; x++) {
            titles[x].UpdateTitle(titleIndex);
        }
    }
}


