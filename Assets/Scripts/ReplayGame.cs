using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayGame : MonoBehaviour {



    public void ReplayOnClick() {
        FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
        SceneManager.LoadScene("Game");
    }


}
