using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public class UGSManager : MonoBehaviour{

    static UGSManager instance;

    private void Awake() {
        if (instance != null) { Destroy(this); }
        else {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public string environment = "production";
    async void Start() {
        try {
            var options = new InitializationOptions().SetEnvironmentName(environment);
            await UnityServices.InitializeAsync(options);
        }
        catch (Exception exception) {
            // An error occurred during services initialization.
        }
    }
}
