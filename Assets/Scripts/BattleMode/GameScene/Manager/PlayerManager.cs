using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// シングルトン
public sealed class PlayerManager : NoaBehaviour
{
    // キャラクターのprefabを格納する連想配列
    private static Dictionary<string, GameObject> CharacterPrefabs = new Dictionary<string, GameObject>();

    private static PlayerManager inst;
    private PlayerManager() { }
    public static PlayerManager Inst
    {
        get
        {
            if (inst == null)
            {
                GameObject go = new GameObject("PlayerManager");
                go.transform.parent = GameObject.Find("Managers").transform;
                inst = go.AddComponent<PlayerManager>();
            }

            return inst;
        }
    }

    protected override IEnumerator Start()
    {
        yield return new WaitUntil(() => LoadingManager.Inst.MyProc.started);
        Debug.Log("_3:PlayerManagerが呼び出される。");

        System.Object[] tmp = Resources.LoadAll("Prefabs/Characters");

        for (int i = 0; i < tmp.Length; ++i)
        {
            GameObject x = tmp[i] as GameObject;

            // 重複時の処理
            while (true)
            {
                if (CharacterPrefabs.ContainsKey(x.name))
                {
                    x.name += "(duplicated)";
                    continue;
                }
                break;
            }

            CharacterPrefabs.Add(x.name, x);
        }

        Debug.Log("4:PlayerManagerがキャラクターを読み込む。");
        MyProc.started = true;

        yield return new WaitUntil( () => NoaProcesser.BossProc.started );
        Destroy(gameObject);
    }
    
    public static GameObject GetCharacterPrefab(string _name)
    {
        if (CharacterPrefabs.ContainsKey(_name) == false) { return null; }

        return CharacterPrefabs[_name];
    }
}