using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]


public class Appear : MonoBehaviour {
	public int small; //雑魚敵の出現回数
	public int medium; //中ボスの出現回数
	public int large; //ラスボスの（ry いらなそう
	public GameObject[] waves;
	private int smallFig;

	void Start ()
	{
		StartCoroutine ("SmallEnemy");  
	}

	public IEnumerator SmallEnemy()
	{
		if (waves.Length == 0) {
			yield break;
		}

		for (smallFig = 0; smallFig < small; smallFig++) {
			Debug.Log (smallFig);
			int rand = Random.Range (0, waves.Length); //ランダム
			GameObject wave = (GameObject)Instantiate (waves [rand], transform.position, Quaternion.identity);
			wave.transform.parent = transform;
			while (wave.transform.childCount != 0) {
				yield return new WaitForEndOfFrame ();
			}
			Destroy (wave);
			yield return new WaitForSeconds (2);
		}
	}
}
