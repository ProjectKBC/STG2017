using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// f:CharacterSelectManager(CharacterSelectModes)
// f:CharacterSelectScene(Battle)において、シーン遷移を担うクラス
// f:by flanny 2017/10/16
public class CharacterSelectManager : MonoBehaviour {
	public static string player1Character;

	// Use this for initialization

	public void SelectChara1 () {
		player1Character = "";
		SceneManager.LoadScene ("GameUI");
	}	

	public void SelectChara2(){
		SceneManager.LoadScene ("GameUI");
	}

	public void SelectChara3(){
		SceneManager.LoadScene ("GameUI");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
