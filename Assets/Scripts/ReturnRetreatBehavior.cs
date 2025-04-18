using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnRetreatBehavior : StateMachineBehaviour {

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {

        bool menuVisible = animator.GetComponent<ReturnPopUp>().visible;

        if (!menuVisible) {
            animator.gameObject.SetActive(false);
        }
    }

}
