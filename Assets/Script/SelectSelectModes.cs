using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectSelectModes : MonoBehaviour {
	public void SoloStart () {
		//SceneManager.LoadScene ("SelectCharacter");
	}	

	public void ButttleStart(){
		SceneManager.LoadScene ("SelectCharacter");
	}

	public void SettingStart(){
		//SceneManager.LoadScene ("Setting");
	}
}
