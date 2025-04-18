using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NoInternetButton : MonoBehaviour{

    [SerializeField] GameObject dcImage = default;
    [SerializeField] TextMeshProUGUI text = default;
    [SerializeField] GameObject extraStuff = default;

    void Start(){
        StartCoroutine(CheckInternet());

    }

    IEnumerator CheckInternet() {

        yield return new WaitForSeconds(0.5f);
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            dcImage.SetActive(true);
            text.text = "";
            gameObject.GetComponent<Button>().interactable = false;
            gameObject.GetComponent<Image>().raycastTarget = false;
            if (extraStuff != null) {
                extraStuff.SetActive(false);
            }
        }
    }

}
