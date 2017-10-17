using UnityEngine;
using System.Collections;
using UnityEngine.UI; //uGUIを使うとき必ず必要

public class SelectSelectScene : MonoBehaviour {
	Button solo;
	/*

	public void Start () {
		//GetComponent<Button>().interactable = true;
		SceneStart();
	}
		
	public void SceneStart(){

		Button MenuButton = GetComponent<Button>();    // 対象のボタン
		MenuButton.animator.SetTrigger ("Highlighted");

	}
	*/

	//public Object firstSelect;

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
