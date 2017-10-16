using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI; //uGUIを使うとき必ず必要

public class SelectStartScene : MonoBehaviour {

	public Object firstSelect;

	public void ButtonActive() {
		for(var i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).GetComponent<Button>().interactable = true;
		}
	}

	void ButtonNotActive() {
		for(var i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).GetComponent<Button>().interactable = false;
		}
	}
}