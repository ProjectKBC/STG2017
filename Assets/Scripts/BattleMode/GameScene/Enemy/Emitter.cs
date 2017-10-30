using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmergePattern
{
	Down,
	Suddenly,
}

[System.Serializable]
public class EnemyData
{
	public EmergePattern emergePattern;
	public GameObject wave;
}

public class Emitter : MonoBehaviour
{
	public int small; //雑魚敵の出現回数
	public int medium; //中ボスの出現回数
	public int large; //ラスボスの（ry いらなそう
	public GameObject[] waves;
	[SerializeField]
	public EnemyData [] wavess;
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
			if (smallFig < waves.Length) {
				GameObject wave = (GameObject)Instantiate (waves [smallFig], transform.position, Quaternion.identity);
				wave.transform.parent = transform;
				while (wave.transform.childCount != 0) {
					yield return new WaitForEndOfFrame ();
				}
				Destroy (wave);
				yield return new WaitForSeconds (2); //2秒待つ、待ち時間がないと敵が一気に駆逐される、残ったビームに焼かれてるのかも？
			} else {
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
}