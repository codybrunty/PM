using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopingImages : MonoBehaviour{

    public List<GameObject> images = new List<GameObject>();
    public List<GameObject> hideImages = new List<GameObject>();
    public float waitTime = default;
    public int imageIndex = 0;
    public Coroutine coroutine;

    public IEnumerator SwitchImages() {
        while (true) {
            yield return new WaitForSeconds(waitTime);
            TurnOnImage();
        }
    }
    
    private void TurnOnImage() {
        imageIndex++;

        if (imageIndex == images.Count) {
            TurnOffImages();
            imageIndex = 0;
        }
        HideImages(imageIndex);
        images[imageIndex].SetActive(true);
        //gameObject.GetComponent<Image>().sprite = images[imageIndex];
    }

    private void HideImages(int index) {
        //hide images around turns
        if (hideImages[index] != null) {
            hideImages[index].SetActive(false);
        }
    }

    private void TurnOffImages() {
        //unhide all hidden images
        for (int i = 0; i < hideImages.Count; i++) {
            if (hideImages[i] != null) {
                hideImages[i].SetActive(true);
            }
        }
        //reset image groups
        for (int i = 0; i < images.Count; i++) {
            images[i].SetActive(false);
        }

    }
}
