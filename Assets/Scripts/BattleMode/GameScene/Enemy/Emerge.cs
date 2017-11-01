using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emerge : NoaBehaviour {

	Vector2 v;

	public int Ypos = 200;

    // f:
    private PlayerSlot playerSlot;

    // f:
    private void Init() {
        playerSlot = transform.parent.GetComponent<Appear>().playerSlot;
    }

	protected override IEnumerator Start () {
        Init(); // f:
        v = transform.position;

        yield return null;
	}
	
	void Update () {
		if (transform.position.y > Ypos) {
			Move ();
		}
	}

	void Move()
	{
		v.y -= transform.position.y * 1 * Time.deltaTime;
		transform.position = v;
	}
}
