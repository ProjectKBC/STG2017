using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appear : NoaBehaviour
{
	public int[] figs;
	public int large; //ラスボスの（ry いらなそう
	public GameObject[] SmallWaves;
	public GameObject[] MediumWaves;
	public GameObject[] LargeWaves;

	private int smallFig = 0;
	private int mediumFig = 0;
	private int CurrentEnemy;

	private bool SmallEnemys = false;
	private bool MediumEnemys = false;
	private bool LargeEnemys = false;

	public PlayerSlot playerSlot;

	List<GameObject> ExistEnemys = new List<GameObject>();


	protected override IEnumerator Start ()
	{
		if (NoaProcesser.IsStayBoss ()) 
		{
			yield return null;
		}

		if (figs.Length % 2 == 1) {
			for (CurrentEnemy = 0; CurrentEnemy < figs.Length - 1; CurrentEnemy += 2) {
				while (MediumEnemys) {
					yield return new WaitForEndOfFrame ();
				}
				StartCoroutine ("SmallTime", figs [CurrentEnemy]);
				while (SmallEnemys) {
					yield return new WaitForEndOfFrame ();
				}
				StartCoroutine ("MediumEnemy", figs [CurrentEnemy + 1]);
			}
			while (MediumEnemys) {
				yield return new WaitForEndOfFrame ();
			}
			StartCoroutine ("SmallTime", figs [figs.Length - 1]);
			while (SmallEnemys) {
				yield return new WaitForEndOfFrame ();
			}
		} else {
			for (CurrentEnemy = 0; CurrentEnemy < figs.Length; CurrentEnemy += 2) {
				while (MediumEnemys) {
					yield return new WaitForEndOfFrame ();
				}
				StartCoroutine ("SmallTime", figs [CurrentEnemy]);
				while (SmallEnemys) {
					yield return new WaitForEndOfFrame ();
				}
				StartCoroutine ("MediumEnemy", figs [CurrentEnemy + 1]);
				while (MediumEnemys) {
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
		if (ExistEnemys.Count == 0 && SmallEnemys == true && smallFig == figs [CurrentEnemy]) {
			SmallEnemys = false;
		} else if (ExistEnemys.Count == 0 && MediumEnemys == true && mediumFig == figs [CurrentEnemy]) {
			MediumEnemys = false;
		}
		if (SmallEnemys == true)
		{
			for (int test = 0; test < ExistEnemys.Count; test++) {
				if (ExistEnemys [test].transform.childCount == 0) {
					Destroy (ExistEnemys [test]);
					ExistEnemys.RemoveAt (test);
				}
			}
		}
	}

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
			ExistEnemys.Add ((GameObject)Instantiate (SmallWaves [rand], transform.position, Quaternion.identity));
			ExistEnemys[ExistEnemys.Count - 1].transform.parent = transform;
			Transform[] t = ExistEnemys[ExistEnemys.Count - 1].transform.GetComponentsInChildren<Transform>();
			foreach (Transform child in t) {
				Enemy[] t2 = child.transform.GetComponentsInChildren<Enemy> ();
				foreach (Enemy child2 in t2)
				{
					child2.GetComponent<Enemy> ().playerSlot = playerSlot;
				}
			}
			yield return new WaitForSeconds (7);
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
			ExistEnemys.Add ((GameObject)Instantiate (SmallWaves [rand], transform.position, Quaternion.identity));
			ExistEnemys[ExistEnemys.Count - 1].transform.parent = transform;
			Transform[] t = ExistEnemys[ExistEnemys.Count - 1].transform.GetComponentsInChildren<Transform>();
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
			while (wave.transform.childCount != 0)
			{
				yield return new WaitForEndOfFrame ();
			}
			Destroy (wave);
			yield return new WaitForSeconds (2);
		}
		SmallEnemys = false;
	}

	public IEnumerator MediumEnemy (int medium)
	{
		int mediumFig = 0;
		MediumEnemys = true;
		if (MediumWaves.Length == 0) {
			yield break;
		}

		for (mediumFig = 0; mediumFig < medium; mediumFig++)
		{
			ExistEnemys.Add((GameObject)Instantiate (MediumWaves [mediumFig], transform.position, Quaternion.identity));
			ExistEnemys[ExistEnemys.Count - 1].transform.parent = transform;
			Transform[] t = ExistEnemys[ExistEnemys.Count - 1].transform.GetComponentsInChildren<Transform>();
			foreach (Transform child in t) {
				Enemy[] t2 = child.transform.GetComponentsInChildren<Enemy> ();
				foreach (Enemy child2 in t2)
				{
					child2.GetComponent<Enemy> ().playerSlot = playerSlot;
				}
			}
			while (ExistEnemys[ExistEnemys.Count - 1].transform.childCount != 0)
			{
				yield return new WaitForEndOfFrame ();
			}
			Destroy (ExistEnemys[ExistEnemys.Count - 1]);
			ExistEnemys.RemoveAt (ExistEnemys.Count - 1);
			yield return new WaitForSeconds (2);
		}
		MediumEnemys = false;
	}

	public IEnumerator LargeEnemy ()
	{
		if (LargeWaves.Length == 0)
		{
			yield break;
		}

		for (int LargeFig = 0; LargeFig < large; LargeFig++)
		{
			GameObject wave = (GameObject)Instantiate (LargeWaves [LargeFig], transform.position, Quaternion.identity);
			wave.transform.parent = transform;
			while (wave.transform.childCount != 0) 
			{
				yield return new WaitForEndOfFrame ();
			}
			Destroy (wave);
			yield return new WaitForSeconds (2);
		}
	}
}