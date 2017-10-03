using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シングルトン
public sealed class PlayerManager : MonoBehaviour
{
    private Dictionary<string, GameObject> CharacterPrefabs = new Dictionary<string, GameObject>();

    private static PlayerManager inst;
    private PlayerManager() { Debug.Log("player_manager created"); }
    public static PlayerManager Inst
    {
        get
        {
            if (inst == null)
            {
                GameObject go = new GameObject("PlayerManager");
                inst = go.AddComponent<PlayerManager>();
            }

            return inst;
        }
    }

    private void Start()
    {
        Debug.Log("player_manager start");
        Debug.Log(Resources.LoadAll("Prefabs/Characters")[0].name);
        Object[] tmp = Resources.LoadAll("Prefabs/Characters");

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
        
    }
    
    public GameObject getCharacterPrefab(string _name)
    {
        if (CharacterPrefabs.ContainsKey(_name) == false) { return null; }

        return CharacterPrefabs[_name];
    }
}