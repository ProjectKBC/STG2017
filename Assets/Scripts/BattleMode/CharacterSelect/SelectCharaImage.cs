using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharaImage : MonoBehaviour {
	public static SpriteRenderer MainSpriteRenderer;
	public static Sprite VeronicaIcon;
	public static Sprite AnomaIcon;
	public static Sprite HeldIcon;

	void Start () {
		MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	}
}
