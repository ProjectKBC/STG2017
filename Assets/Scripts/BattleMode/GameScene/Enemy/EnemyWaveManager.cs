using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シングルトン
public class EnemyWaveManager : MonoBehaviour
{
    private static EnemyWaveManager inst;

    private EnemyWaveManager() { Debug.Log("EnemyWaveManager created"); }

    public static EnemyWaveManager Inst
    {
        get
        {
            if (inst == null)
            {
                GameObject go = new GameObject("EnemyWaveManager");
                inst = go.AddComponent<EnemyWaveManager>();
            }

            return inst;
        }
    }

	public GameObject[] waves;
	private int currentWave;

	IEnumerator Start ()
	{
		if (waves.Length == 0)
		{
			yield break;
		}
		while (true)
		{
			GameObject wave = (GameObject)Instantiate (waves [currentWave], transform.position, Quaternion.identity);
			wave.transform.parent = transform;
			while (wave.transform.childCount != 0)
			{
				yield return new WaitForEndOfFrame ();
			}
			Destroy (wave);
			if (waves.Length <= currentWave + 1) {
				currentWave = 0;
			}
		}
	}
}
