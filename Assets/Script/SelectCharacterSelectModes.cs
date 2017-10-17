using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCharacterSelectModes : MonoBehaviour {
	int clickCount = 0;
	public static string[] selectCharacter = new string[2];
	//public static List<string> setChar = new List<string>(selectCharacter);

	public void onClickChara1(){
		//setChar.Add ("Chara1");
		check ("Chara1");
		clickCount++;
	}

	public void onClickChara2(){
		//setChar.Add ("Chara2");
		check ("Chara2");
		clickCount++;
	}

	public void onClickChara3(){
		//setChar.Add ("Chara3");
		check ("Chara3");
		clickCount++;
	}

	public void check(string name){
		if (clickCount == 0) {
			selectCharacter [0] = name;
		} else if (clickCount == 1) {
			selectCharacter [1] = name;
			SceneManager.LoadScene ("GameUI");
		} else {
		}
	}


	// Update is called once per frame
	void Update () {
		/*
		if(clickCount == 2){
			//selectCharacter = setChar.ToArray ();
			SceneManager.LoadScene ("GameUI");
		}
		*/
			
	}
}
