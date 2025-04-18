using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepyButton : MonoBehaviour
{
    public void SleepyButtonOnClick() {
        PlayerPrefs.SetInt("SleepyClicked",1);
        FindObjectOfType<SoundManager>().PlayOneShotSound("select1");
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=com.BombChomp.SleepyHeadz");

#elif UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.apple.com/app/id1528662701");
#endif
    }
}
