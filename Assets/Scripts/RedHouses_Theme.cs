using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHouses_Theme : MonoBehaviour{

    
    private void Start() {
        SetRedHouse();
        SetRedHouse_OrderInLayer();
    }



    private void SetRedHouse() {
        GameObject redHouseGO = ThemeManager.TM.GetRedHouseGameObject();
        GameObject redHouse = Instantiate(redHouseGO, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        redHouse.transform.localPosition = new Vector3(0f,0.1f,0f);
    }

    private void SetRedHouse_OrderInLayer() {
        SquareMechanics squareData = gameObject.GetComponent<SquareMechanics>();
        GameObject redHouse = gameObject.transform.GetChild(1).gameObject;
        Transform[] allChildrenTree = redHouse.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildrenTree) {
            if (!child.name.Contains("Effect")) {
                child.GetComponent<ParticleSystemRenderer>().sortingOrder = (-1 * squareData.gamePositionY);
            }
        }
    }


}
