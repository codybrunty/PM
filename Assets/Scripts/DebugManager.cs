using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour{

    [SerializeField] List<GameObject> debugButtons = new List<GameObject>();
    private int DebugOn;


    private void Start() {
        DebugOn = PlayerPrefs.GetInt("DebugOn",0);
        if (DebugOn == 1) {
            for (int i = 0; i < debugButtons.Count; i++) {
                debugButtons[i].SetActive(true);
            }
        }
    }

}
