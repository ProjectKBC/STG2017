using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// f:CharacterSelectManager(CharacterSelectModes)
// f:CharacterSelectScene(Battle)において、シーン遷移を担うクラス
// f:by flanny 2017/10/16
public class CharacterSelectManager : MonoBehaviour {
	public static string player1Character;
	Button chara1;
	Button chara2;
	Button chara3;

	public static string PC1Name = null;
	public static string PC2Name = null;

	public int selectCnt = 0;


	public void Start(){
		chara1 = GameObject.Find ("CharacterButtons/Chara1").GetComponent<Button>();;
		chara2 = GameObject.Find ("CharacterButtons/Chara2").GetComponent<Button>();;
		chara3 = GameObject.Find ("CharacterButtons/Chara3").GetComponent<Button>();;

		chara1.Select ();
	}

	public void SelectChara1() {
		selectCnt++;
		if (selectCnt == 1) {
			PC1Name = "Veronica";
			makeIcon ("PC1", "Veronica");
		} else {
			PC2Name = "Veronica";
			makeIcon ("PC2", "Veronica");
			SceneManager.LoadScene ("BattleMode");
		}
	}	

	public void SelectChara2() {
		selectCnt++;
		if (selectCnt == 1) {
			PC1Name = "Anoma";
			makeIcon ("PC1", "Anoma");
		} else {
			PC2Name = "Anoma";
			makeIcon ("PC2", "Anoma");
			SceneManager.LoadScene ("BattleMode");
		}
	}

	public void SelectChara3() {
		selectCnt++;
		if (selectCnt == 1) {
			PC1Name = "Held";
			makeIcon ("PC1", "Held");
		} else {
			PC2Name = "Held";
			makeIcon ("PC2", "Held");
			SceneManager.LoadScene ("BattleMode");
		}
	}

	void makeIcon(string PC1, string selectName){
		GameObject work_image = new GameObject("WorkImage");
		work_image.transform.parent = GameObject.Find("Canvas").transform;
		if (PC1 == "PC1") {
			work_image.AddComponent<RectTransform> ().anchoredPosition = new Vector3 (-400, -250, 0);
		} else {
			work_image.AddComponent<RectTransform> ().anchoredPosition = new Vector3 (400, -250, 0);
		}
		work_image.GetComponent<RectTransform>().localScale = new Vector3(4, 4, 1);
		work_image.AddComponent<Image>().sprite = Resources.Load<Sprite>("sprites/UI/"+selectName+"_stand");
		work_image.GetComponent<Image>().preserveAspect = true;
		work_image.GetComponent<Image>().SetNativeSize();
	}

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