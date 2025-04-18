using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSrollListMechanics : MonoBehaviour{


    public void Start() {
        gameObject.GetComponent<Scrollbar>().value = 1;
    }


}
