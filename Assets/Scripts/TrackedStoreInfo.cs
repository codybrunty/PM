using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedStoreInfo : MonoBehaviour {

    public int TimesPurchased500 = 0;
    public int TimesPurchased1500 = 0;
    public int TimesPurchased5000 = 0;

    private void Start() {
        UpdateTracking();
    }

    public void UpdateTracking(){
        TimesPurchased500 = PlayerPrefs.GetInt("Times_Purchased_Gold_500", 0);
        TimesPurchased1500 = PlayerPrefs.GetInt("Times_Purchased_Gold_1500", 0);
        TimesPurchased5000 = PlayerPrefs.GetInt("Times_Purchased_Gold_5000", 0);
    }

}
