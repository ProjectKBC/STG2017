using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appear : NoaBehaviour
{
	public int[] SmallMediumNumbers;
	public int large; //ラスボスの（ry いらなそう
	public GameObject[] SmallWaves;
	public GameObject[] MediumWaves;
	public GameObject[] LargeWaves;

	private List<GameObject> ExistWaves = new List<GameObject>();
	public PlayerSlot playerSlot;

	private int smallFig = 0;
	private int mediumFig = 0;
	private int largeFig = 0;
	private int CurrentEnemy;

	private bool SmallEnemys = false;
	private bool MediumEnemys = false;

	protected override IEnumerator Start ()
	{
		if (NoaProcesser.IsStayBoss ()) 
		{
			yield return null;
		}

		if (SmallMediumNumbers.Length % 2 == 1)
		{
			for (CurrentEnemy = 0; CurrentEnemy < SmallMediumNumbers.Length; CurrentEnemy += 2)
			{
				StartCoroutine ("SmallTime", SmallMediumNumbers [CurrentEnemy]);
				while (SmallEnemys)
				{
					yield return new WaitForEndOfFrame ();
				}
				StartCoroutine ("MediumEnemy", SmallMediumNumbers [CurrentEnemy + 1]);
				while (MediumEnemys)
				{
					yield return new WaitForEndOfFrame ();
				}
			}
			StartCoroutine ("SmallTime", SmallMediumNumbers [SmallMediumNumbers.Length - 1]);
			while (SmallEnemys)
			{
				yield return new WaitForEndOfFrame ();
			}
		} else {
			for (CurrentEnemy = 0; CurrentEnemy < SmallMediumNumbers.Length; CurrentEnemy += 2)
			{
				StartCoroutine ("SmallTime", SmallMediumNumbers [CurrentEnemy]);
				while (SmallEnemys)
				{
					yield return new WaitForEndOfFrame ();
				}
				StartCoroutine ("MediumEnemy", SmallMediumNumbers [CurrentEnemy + 1]);
				while (MediumEnemys)
				{
					yield return new WaitForEndOfFrame ();
				}
			}
		}
		StartCoroutine ("LargeEnemy");
	}

	void Update()
	{
		if (NoaProcesser.IsStayBoss ())
		{
			return;
		}
		if (ExistWaves.Count == 0 && SmallEnemys == true && smallFig == SmallMediumNumbers [CurrentEnemy])
		{
			SmallEnemys = false;
		} else if (ExistWaves.Count == 0 && MediumEnemys == true && mediumFig == SmallMediumNumbers [CurrentEnemy + 1])
		{
			MediumEnemys = false;
		}
		for (int test = 0; test < ExistWaves.Count; test++)
		{
			if (ExistWaves [test] != null)
			{
				if (ExistWaves [test].transform.childCount <= 0) {
					Destroy (ExistWaves [test]);
					ExistWaves.RemoveAt (test);
				}
			}
		}
	}

	//時間で敵がでる
	public IEnumerator SmallTime(int small)
	{
		SmallEnemys = true;
		if (SmallWaves.Length == 0)
		{
			yield break;
		}

		for (smallFig = 0; smallFig < small; smallFig++)
		{
			int rand = Random.Range (0, SmallWaves.Length); //ランダム
			ExistWaves.Add ((GameObject)Instantiate (SmallWaves [rand], transform.position, Quaternion.identity));
			ExistWaves[ExistWaves.Count - 1].transform.parent = transform;
			Transform[] t = ExistWaves[ExistWaves.Count - 1].transform.GetComponentsInChildren<Transform>();
			foreach (Transform child in t)
			{
				Enemy[] t2 = child.transform.GetComponentsInChildren<Enemy> ();
				foreach (Enemy child2 in t2)
				{
					child2.GetComponent<Enemy> ().playerSlot = playerSlot;
				}
			}
			yield return new WaitForSeconds (2);
		}
	}
	/*
public IEnumerator SmallEnemy(int small)
	{
		SmallEnemys = true;
		if (SmallWaves.Length == 0)
		{
			yield break;
		}

		for (smallFig = 0; smallFig < small; smallFig++)
		{
			int rand = Random.Range (0, SmallWaves.Length); //ランダム
			ExistWaves.Add ((GameObject)Instantiate (SmallWaves [rand], transform.position, Quaternion.identity));
			ExistWaves[ExistWaves.Count - 1].transform.parent = transform;
			Transform[] t = ExistWaves[ExistWaves.Count - 1].transform.GetComponentsInChildren<Transform>();
			foreach (Transform child in t) {
				Enemy[] t2 = child.transform.GetComponentsInChildren<Enemy> ();
				foreach (Enemy child2 in t2)
				{
					child2.GetComponent<Enemy> ().playerSlot = playerSlot;
				}
			}
			yield return new WaitForSeconds (2);
		}
	}
	*/


	//1つのwaveが消えると次のwaveが出る　いらないかも
	public IEnumerator SmallEnemy(int small)
	{
		int smallFig = 0;
		SmallEnemys = true;
		if (SmallWaves.Length == 0)
		{
			yield break;
		}

		for (smallFig = 0; smallFig < small; smallFig++)
		{
			int rand = Random.Range (0, SmallWaves.Length); //ランダム
			GameObject wave = (GameObject)Instantiate (SmallWaves [rand], transform.position, Quaternion.identity);
			wave.transform.parent = transform;
			Transform[] t = ExistWaves[ExistWaves.Count - 1].transform.GetComponentsInChildren<Transform>();
			foreach (Transform child in t)
			{
				Enemy[] t2 = child.transform.GetComponentsInChildren<Enemy> ();
				foreach (Enemy child2 in t2)
				{
					child2.GetComponent<Enemy> ().playerSlot = playerSlot;
				}
			}
			while (wave.transform.childCount != 0)
			{
				yield return new WaitForEndOfFrame ();
			}
			Destroy (wave);
			yield return new WaitForSeconds (2);
		}
		SmallEnemys = false;
	}

	/*
	//中ボスが出る、1つの中ボスのwaveで絶対に1つのwaveまで
	public IEnumerator MediumEnemy (int medium)
	{
		int mediumFig;
		MediumEnemys = true;
		if (MediumWaves.Length == 0)
		{
			yield break;
		}

		for (mediumFig = 0; mediumFig < medium; mediumFig++)
		{
			ExistWaves.Add((GameObject)Instantiate (MediumWaves [mediumFig], transform.position, Quaternion.identity));
			ExistWaves[ExistWaves.Count - 1].transform.parent = transform;
			Transform[] t = ExistWaves[ExistWaves.Count - 1].transform.GetComponentsInChildren<Transform>();
			foreach (Transform child in t)
			{
				Enemy[] t2 = child.transform.GetComponentsInChildren<Enemy> ();
				foreach (Enemy child2 in t2)
				{
					child2.GetComponent<Enemy> ().playerSlot = playerSlot;
				}
			}
			while (ExistWaves[ExistWaves.Count - 1].transform.childCount != 0)
			{
				yield return new WaitForEndOfFrame ();
			}
			Destroy (ExistWaves[ExistWaves.Count - 1]);
			ExistWaves.RemoveAt (ExistWaves.Count - 1);
			yield return new WaitForSeconds (2);
		}
		MediumEnemys = false;
	}
	*/

	// 中ボス、テスト改良系、雑魚敵と同じ気がするけど便宜上
	public IEnumerator MediumEnemy (int medium)
	{
		MediumEnemys = true;
		if (MediumWaves.Length == 0)
		{
			yield break;
		}

		for (mediumFig = 0; mediumFig < medium; mediumFig++)
		{
			ExistWaves.Add((GameObject)Instantiate (MediumWaves [mediumFig], transform.position, Quaternion.identity));
			ExistWaves[ExistWaves.Count - 1].transform.parent = transform;
			Transform[] t = ExistWaves[ExistWaves.Count - 1].transform.GetComponentsInChildren<Transform>();
			foreach (Transform child in t)
			{
				Enemy[] t2 = child.transform.GetComponentsInChildren<Enemy> ();
				foreach (Enemy child2 in t2)
				{
					child2.GetComponent<Enemy> ().playerSlot = playerSlot;
				}
			}
			yield return new WaitForSeconds (2);
		}
	}

	/*
	//中ボスが出る、1つのwaveづつ出る
	public IEnumerator MediumEnemy (int medium)
	{
		MediumEnemys = true;
		if (MediumWaves.Length == 0)
		{
			yield break;
		}

		for (mediumFig = 0; mediumFig < medium; mediumFig++)
		{
			ExistWaves.Add((GameObject)Instantiate (MediumWaves [mediumFig], transform.position, Quaternion.identity));
			ExistWaves[ExistWaves.Count - 1].transform.parent = transform;
			Transform[] t = ExistWaves[ExistWaves.Count - 1].transform.GetComponentsInChildren<Transform>();
			foreach (Transform child in t)
			{
				Enemy[] t2 = child.transform.GetComponentsInChildren<Enemy> ();
				foreach (Enemy child2 in t2)
				{
					child2.GetComponent<Enemy> ().playerSlot = playerSlot;
				}
			}
			while (ExistWaves[0].transform.childCount != 0)
			{
				yield return new WaitForEndOfFrame ();
			}
			Destroy (ExistWaves[0]);
			ExistWaves.RemoveAt (0);
			yield return new WaitForSeconds (2);
		}
		MediumEnemys = false;
	}
	*/

	public IEnumerator LargeEnemy ()
	{
		if (LargeWaves.Length == 0)
		{
			yield break;
		}

		for (largeFig = 0; largeFig < large; largeFig++)
		{
			ExistWaves.Add((GameObject)Instantiate (LargeWaves [largeFig], transform.position, Quaternion.identity));
			ExistWaves[0].transform.parent = transform;
			Transform[] t = ExistWaves[0].transform.GetComponentsInChildren<Transform>();
			foreach (Transform child in t)
			{
				Enemy[] t2 = child.transform.GetComponentsInChildren<Enemy> ();
				foreach (Enemy child2 in t2)
				{
					child2.GetComponent<Enemy> ().playerSlot = playerSlot;
				}
			}
			while (ExistWaves.Count != 0 && ExistWaves [0].transform.childCount != 0) {
					yield return new WaitForEndOfFrame ();
			}
			Destroy (ExistWaves[0]);
			ExistWaves.RemoveAt (0);
			yield return new WaitForSeconds (2);
		}
	}
}
