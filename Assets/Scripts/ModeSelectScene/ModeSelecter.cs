using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// f:ModeSelecter(SelectSelectModes)
// f:ModeSelecter、シーン遷移を担うクラス
// f:by flanny 2017/10/16
public class ModeSelecter : MonoBehaviour {
	public void SoloStart () {
		SceneManager.LoadScene ("CharacterSelect_BattleMode");
	}	

	public void BattleStart(){
		SceneManager.LoadScene ("CharacterSelect_BattleMode");
	}

	public void SettingStart(){
		SceneManager.LoadScene ("Setting");
	}
}
