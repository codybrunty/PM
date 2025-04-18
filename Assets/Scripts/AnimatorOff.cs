using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOff : MonoBehaviour{

    public void TurnAnimatorOff() {
        gameObject.GetComponent<Animator>().enabled = false;
    }


}
