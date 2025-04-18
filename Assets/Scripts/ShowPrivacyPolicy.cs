using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPrivacyPolicy : MonoBehaviour{
    public void PrivacyOnClick() {
        Application.OpenURL("https://www.bombchomp.com/terms");
    }
}
