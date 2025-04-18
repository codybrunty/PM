using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToStart : MonoBehaviour{

    public void ClickToStartCommand(){
        SceneManager.LoadScene("Menu_Level");
    }

}
