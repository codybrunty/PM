﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextMechanics : MonoBehaviour {
    public float destroyTime = 0.5f;

    void Start () {
        Destroy(gameObject,destroyTime);
	}

}
