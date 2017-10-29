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
}
