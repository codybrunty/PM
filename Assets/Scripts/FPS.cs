﻿using UnityEngine;
using System.Collections;

public class FPS : MonoBehaviour {
    //float deltaTime = 0.0f;

    private void Awake() {
        Application.targetFrameRate = 60;
    }
    /*
    void Update() {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }
    
    void OnGUI() {

        int debugOn = PlayerPrefs.GetInt("DebugOn", 0);

        if (debugOn == 1) {
            int w = Screen.width, h = Screen.height;
            GUIStyle style = new GUIStyle();
            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
        }
    }
    */
}