using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// f:CharacterSelectManager(CharacterSelectModes)
// f:CharacterSelectScene(Battle)において、シーン遷移を担うクラス
// f:by flanny 2017/10/16
public class CharacterSelectManager : MonoBehaviour
{
    public static string PC1Name = null;
    public static string PC2Name = null;

    // Use this for initialization

    public void SelectChara1() {
        PC1Name = "Veronica";
        PC2Name = "Veronica_CC";
        SceneManager.LoadScene ("BattleMode");
	}	

	public void SelectChara2() {
        PC1Name = "Anoma";
        PC2Name = "Veronica_CC";
        SceneManager.LoadScene ("BattleMode");
	}

	public void SelectChara3() {
        PC1Name = "Held";
        PC2Name = "Veronica_CC";
        SceneManager.LoadScene ("BattleMode");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
