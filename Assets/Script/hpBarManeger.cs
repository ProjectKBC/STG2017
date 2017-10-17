using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpBarManeger : MonoBehaviour {

	public float hp1 = 100;
	public float MaxHP1 = 100;
	public float hp2 = 100;
	public float MaxHP2 = 100;

	LineRenderer hpBar;

	void Start(){
		hpBar = GameObject.Find("HP/hpLine").GetComponent<LineRenderer>();
	}

	//Player1を永遠と殺し続けるメソッド
	void Update(){
		if (hp1 >= 0) {
			hpBar.SetPosition (1, new Vector3 (4 * (hp1 / MaxHP1), 0.0f, 0.0f));
			hp1 -= 1;
		}
	}
}

