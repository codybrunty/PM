using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaPanel : MonoBehaviour{
#if UNITY_IOS
	private RectTransform _rectTransform;

	private void Awake(){
		_rectTransform = GetComponent<RectTransform>();
		RefreshPanel(Screen.safeArea);
	}

	private void OnEnable(){
		SafeAreaDetection.OnSafeAreaChanged += RefreshPanel;
	}

	private void OnDisable(){
		SafeAreaDetection.OnSafeAreaChanged -= RefreshPanel;
	}

	private void RefreshPanel(Rect safeArea){




		if (Screen.height != Screen.safeArea.height){
			Vector2 anchorMin = safeArea.position;
			Vector2 anchorMax = safeArea.position + safeArea.size;
			anchorMin.x /= Screen.width;
		    anchorMax.x /= Screen.width;


			anchorMin.y /= Screen.height;

			if (Screen.height == 1792){
				anchorMax.y /= Screen.height - 30;
			}
			else{
				anchorMax.y /= Screen.height - 50;
			}


			_rectTransform.anchorMin = anchorMin;
			_rectTransform.anchorMax = anchorMax;
		}




	}
#endif
}
