using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSelectModes : MonoBehaviour {
	void start(){
	}

	public void GameStart () {
		SceneManager.LoadScene ("SelectMode");
	}	

	public void EndGame () {
		UnityEditor.EditorApplication.isPlaying = false;
	}
}
