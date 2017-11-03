using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emerge : MonoBehaviour {

	Vector2 v;

	public bool standbyPos = false;
	public bool xMove = false;

	[Range(-1000, 600)]
	public int StandbyXpos = 0;
	[Range(-1600, 0)]
	public int StandbyYpos = 0;

	[Range(-1000, 600)]
	public int Xpos = 0;
	[Range(-1600, 0)]
	public int Ypos = -400;

	public int xSpeed = 1;
	public int ySpeed = 1;


	void Start ()
	{
		v = transform.localPosition;
		if (standbyPos)
		{
			DefaultPos ();
		}
	}

	void Update ()
	{
		if (standbyPos == false && transform.localPosition.y > Ypos)
		{
			FirstMove ();
		}

		if (xMove == true)
		{
			XMove ();
		}
	}

	void DefaultPos()
	{
		v.x = StandbyXpos;
		v.y = StandbyYpos;
		transform.localPosition = v;
	}

	void FirstMove()
	{
		v.y += Ypos * Time.deltaTime;
		transform.localPosition = v;
	}

	void XMove()
	{
		v.x += xSpeed * Time.deltaTime;
		transform.localPosition = v;
	}

}