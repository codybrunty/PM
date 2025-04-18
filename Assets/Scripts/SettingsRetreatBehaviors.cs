using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsRetreatBehaviors : StateMachineBehaviour {

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {

        bool menuVisible = animator.GetComponent<SettingsPopUp>().visible;

        if (!menuVisible) {
            animator.gameObject.SetActive(false);
        }
    }


}
