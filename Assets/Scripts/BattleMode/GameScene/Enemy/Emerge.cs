using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emerge : MonoBehaviour {


	void Start () {
		
	}
	
	void Update () {
		Vector2 v = new Vector2 (transform.position.x, transform.position.y);
		while (v.y > 200)
		{
			v = new Vector2 (transform.position.x, transform.position.y - 5);
			transform.position = v;
		}
	}
}
