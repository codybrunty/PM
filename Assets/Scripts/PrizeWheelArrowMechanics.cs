using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeWheelArrowMechanics : MonoBehaviour{

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name.Contains("SoundTrigger")) {
            int randomIndex = UnityEngine.Random.Range(0, 2);
            if (randomIndex == 1) {
                FindObjectOfType<SoundManager>().PlayOneShotSound("WheelClick1");
                FindObjectOfType<SoundManager>().PlayOneShotSound("WheelClick2");
            }
            else {
                FindObjectOfType<SoundManager>().PlayOneShotSound("WheelClick1");
                FindObjectOfType<SoundManager>().PlayOneShotSound("WheelClick2");
            }
        }
    }

}
