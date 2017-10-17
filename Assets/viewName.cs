using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class viewName : MonoBehaviour {
	public Text text;

	// Use this for initialization
	void Start () {
		hoge ();
	}

	public void hoge(){
		text = this.GetComponent<Text> ();
		text.text = SelectCharacterSelectModes.selectCharacter[0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
