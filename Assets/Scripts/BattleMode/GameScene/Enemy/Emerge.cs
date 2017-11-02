using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emerge : MonoBehaviour {

	Vector2 v;

	public int Ypos = 200;

	void Start () {
		v = transform.position;
	}
	
	void Update () {
		if (transform.position.y > Ypos)
		{
			Move ();
		}
	}

	void Move()
	{
		v.y -= transform.position.y * 1 * Time.deltaTime;
		transform.position = v;
	}
}
