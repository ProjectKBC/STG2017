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
        yield return new WaitUntil(() => NoaProcesser.BossProc.started);
        PlayerManager.DestroyMe(gameObject);
    }

    public void Starting()
    {
        Debug.Log("2:PlayerManagerが呼び出される。");

        System.Object[] tmp = Resources.LoadAll("Prefabs/Characters");

        for (int i = 0; i < tmp.Length; ++i)
        {
            GameObject x = tmp[i] as GameObject;

            CharacterPrefabs.Add(x.name, x);
        }
        MyProc.started = true;
    }
    
    public static GameObject GetCharacterPrefab(string _name)
    {
        if (CharacterPrefabs.ContainsKey(_name) == false) { return null; }

        return CharacterPrefabs[_name];
    }

    public static void DestroyMe(GameObject _gameObject)
    {
        CharacterPrefabs = new Dictionary<string, GameObject>();

        inst.MyProc.Reset();
        inst = null;
        Debug.Log("Destroy:PlayerManager");
        Destroy(_gameObject);
    }
}
