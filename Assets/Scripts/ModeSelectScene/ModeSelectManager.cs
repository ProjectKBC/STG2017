using UnityEngine;
using System.Collections;
using UnityEngine.UI; //uGUIを使うとき必ず必要

// f:ModeSelectManager(SelectSelectScene)
// f:ModeSelectSceneにおいて、UIを担うクラス
// f:by flanny 2017/10/16
public class ModeSelectManager : MonoBehaviour {
	Button solo;
	Button buttle;
	Button setting;

	public void Start () {
		GetComponent<Button>().interactable = true;
		SceneStart();
	}
		
	public void SceneStart(){

		/*
		solo = GameObject.Find("SelectUI/Solo").GetComponent<Button>();

		buttle = GameObject.Find("SelectUI/Buttle").GetComponent<Button>();

		setting = GameObject.Find ("SelectUI/Setting").GetComponent<Button> ();

		*/
		//solo.Select();

		Button MenuButton = GetComponent<Button>();    // 対象のボタン
		MenuButton.animator.SetTrigger ("Highlighted");

	}

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
