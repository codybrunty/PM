using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager levelManager;

    public int scroll;
    public int block;
    public int level;

    void Awake() {
        if (levelManager == null) {
            DontDestroyOnLoad(gameObject);
            levelManager = this;
        }
        else if (levelManager != this) {
            Destroy(gameObject);
        }

    }
}
