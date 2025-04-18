using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHint : MonoBehaviour{
    public void HintButton2OnClick() {
        FindObjectOfType<CreateBoard>().ShowTreesOnGameBoard();
    }
}
