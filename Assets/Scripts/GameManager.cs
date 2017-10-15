using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// シングルトン
public sealed class GameManager : MonoBehaviour
{
    // 各プレイヤーの行動可能範囲
    public Vector4 pc1Area = new Vector4(-8.0f, -4.8f, -0.9f, 4.8f);
    public Vector4 pc2Area = new Vector4(0.9f,  -4.8f,  8.0f, 4.8f);

    // 選択されたキャラクターの名前
    public string pc1Name;
    public string pc2Name;

    private static GameManager inst;
    private GameManager() { Debug.Log("game_manager created");}
    public static GameManager Inst
    {
        get
        {
            if (inst == null)
            {
                GameObject go = new GameObject("GameManager");
                inst = go.AddComponent<GameManager>();
            }

            return inst;
        }
    }

    private void Start()
    {
        Debug.Log("game_manager start");
        CreatePlayer(pc1Name, pc2Name);
    }

    public Vector2 GetAreaMin(PlayerSlot _playerSlot)
    {
        switch (_playerSlot)
        {
            case PlayerSlot.PC1: return new Vector2(pc1Area.x, pc1Area.y);
            case PlayerSlot.PC2: return new Vector2(pc2Area.x, pc2Area.y);
            default: return new Vector2(-1, -1);
        }
    }

    public Vector2 GetAreaMax(PlayerSlot _playerSlot)
    {
        switch (_playerSlot)
        {
            case PlayerSlot.PC1: return new Vector2(pc1Area.z, pc1Area.w);
            case PlayerSlot.PC2: return new Vector2(pc2Area.z, pc2Area.w);
            default: return new Vector2(-1, -1);
        }
    }

    public void GameSet()
    {

    }

    private void CreatePlayer(string _pc1Name, string _pc2Name)
    {
        Player tmp;

        GameObject pc1 = Instantiate
        (
            PlayerManager.Inst.GetCharacterPrefab(_pc1Name),
            new Vector3(-4.5f, -2.5f),
            new Quaternion()
        ) as GameObject;

        pc1.name = PlayerManager.Inst.GetCharacterPrefab(_pc1Name).name; Debug.Log("1");
        tmp = pc1.GetComponent<Player>();
        tmp.playerSlot = PlayerSlot.PC1;

        GameObject pc2 = Instantiate
        (
            PlayerManager.Inst.GetCharacterPrefab(_pc2Name),
            new Vector3(4.5f, -2.5f),
            new Quaternion()
        );

        pc2.name = PlayerManager.Inst.GetCharacterPrefab(_pc2Name).name; Debug.Log("2");
        tmp = pc2.GetComponent<Player>();
        tmp.playerSlot = PlayerSlot.PC2;
    }
}