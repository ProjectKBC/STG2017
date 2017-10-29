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

<<<<<<< HEAD
	public void SelectChara1 () {
		player1Character = "";
		SceneManager.LoadScene ("GameUI");
	}	

	public void SelectChara2(){
		SceneManager.LoadScene ("GameUI");
	}

	public void SelectChara3(){
		SceneManager.LoadScene ("GameUI");
=======
	public void SelectChara1() {
		GameManager.SetPCName("Veronica", PlayerSlot.PC1);
        GameManager.SetPCName("Veronica_CC", PlayerSlot.PC2);
        SceneManager.LoadScene ("BattleMode");
	}	

	public void SelectChara2() {
        GameManager.SetPCName("Anoma", PlayerSlot.PC1);
        GameManager.SetPCName("Veronica_CC", PlayerSlot.PC2);
        SceneManager.LoadScene ("BattleMode");
	}

	public void SelectChara3() {
        GameManager.SetPCName("Held", PlayerSlot.PC1);
        GameManager.SetPCName("Veronica_CC", PlayerSlot.PC2);
        SceneManager.LoadScene ("BattleMode");
>>>>>>> test
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
