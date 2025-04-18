using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueHouses_Theme : MonoBehaviour{

    private void Start() {
        SetBlueHouse();
        SetBlueHouse_OrderInLayer();
    }


    private void SetBlueHouse() {
        GameObject blueHouseGO = ThemeManager.TM.GetBlueHouseGameObject();
        GameObject blueHouse=Instantiate(blueHouseGO, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        blueHouse.transform.localPosition = new Vector3(0f, 0.1f, 0f);
    }

    private void SetBlueHouse_OrderInLayer() {
        SquareMechanics squareData = gameObject.GetComponent<SquareMechanics>();
        GameObject blueHouse = gameObject.transform.GetChild(1).gameObject;
        Transform[] allChildrenTree = blueHouse.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildrenTree) {
            if (!child.name.Contains("Effect")) {
                child.GetComponent<ParticleSystemRenderer>().sortingOrder = (-1 * squareData.gamePositionY);
            }
        }
    }

}
