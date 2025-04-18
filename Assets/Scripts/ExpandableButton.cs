using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandableButton : MonoBehaviour {

    public bool expanded = false;
    public bool locked = true;
    GameObject body;
    [SerializeField] GameObject levelBlock = default;
    Animator bodyAnimator;
    private bool isScrolling = false;
    private bool isSwiping = false;
    public ScrollClickPrevention scrollClickPrevention;
    public bool disableExpand = false;
    [SerializeField] GameObject bodyMask = default;
    public bool moving = false;
    public bool adBlock = false;

    private void Start() {
        bodyAnimator = levelBlock.GetComponent<Animator>();
        Transform[] transforms;
        transforms = gameObject.transform.parent.transform.parent.GetComponentsInChildren<Transform>(true);
        int bodyIndex = 0;
        for (int i = 0; i < transforms.Length; i++) {
            if (transforms[i].name == "Body") {
                bodyIndex = i;
            }
        }
        body = transforms[bodyIndex].gameObject;
    }

    public void OnExpandClick() {
        //make sure we are not scrolling;
        CheckIsScrolling();
        CheckIsSwiping();
        if (!isScrolling && !isSwiping) {
            if (!disableExpand) {
                if (!moving) {
                    if (expanded) {
                        CollapseCommand();
                    }
                    else {
                        ExpandCommand();
                    }
                }
            }
        }
    }

    private void CheckIsScrolling() {
        isScrolling = scrollClickPrevention.isScrolling;
    }

    private void CheckIsSwiping() {
        isSwiping = scrollClickPrevention.isSwiping;
    }

    public void ExpandCommand() {
        bodyMask.SetActive(true);
        moving = true;
        expanded = true;
        if (locked) {
            bodyAnimator.SetInteger("Expand", 1);
            StartCoroutine(IsMoving(0.5f));
        }
        else {
            bodyAnimator.SetInteger("Expand", 2);
            StartCoroutine(IsMoving(0.5f));
        }
        FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
    }

    private void CollapseCommand() {
        moving = true;
        expanded = false;
        if (locked) {
            StartCoroutine(IsMoving(0.5f));
            StartCoroutine(BodyMaskTurnOff(0.5f));
        }
        else {
            StartCoroutine(IsMoving(0.5f));
            StartCoroutine(BodyMaskTurnOff(0.5f));
        }
        bodyAnimator.SetInteger("Expand", 0);
        FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
    }
    IEnumerator BodyMaskTurnOff(float seconds) {
        yield return new WaitForSeconds(seconds);
        bodyMask.SetActive(false);
    }
    IEnumerator IsMoving(float seconds) {
        yield return new WaitForSeconds(seconds);
        moving = false;
    }
}
