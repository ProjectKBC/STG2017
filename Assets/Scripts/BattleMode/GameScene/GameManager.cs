using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum V4Enum { x, y, z, w }

// シングルトン
public sealed class GameManager : MonoBehaviour
{
    // 各プレイヤーの行動可能範囲
    public readonly Vector4 pc1Area = new Vector4(-8.0f, -4.8f, -0.9f, 4.8f);
    public readonly Vector4 pc2Area = new Vector4(0.9f,  -4.8f,  8.0f, 4.8f);

    // 選択されたキャラクターの名前
    public readonly string pc1Name;
    public readonly string pc2Name;

    // 各プレイヤーのスコア
    public float Pc1Score { get; private set; }
    public float Pc2Score { get; private set; }

    private static GameManager inst;
    private GameManager()
    {
        pc1Name = FireLitStone.fls.pc1Name;
        pc2Name = FireLitStone.fls.pc2Name;
        Debug.Log("game_manager created");
    }
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

    /* Area系 ------------------------------------------------------------------------- */
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

    public float GetAreaPoint(PlayerSlot _playerSlot, V4Enum _v4)
    {
        switch (_v4)
        {
            case V4Enum.x:
                switch (_playerSlot)
                {
                    case PlayerSlot.PC1: return pc1Area.x;
                    case PlayerSlot.PC2: return pc2Area.x;
                }
                break;

            case V4Enum.y:
                switch (_playerSlot)
                {
                    case PlayerSlot.PC1: return pc1Area.y;
                    case PlayerSlot.PC2: return pc2Area.y;
                }
                break;

            case V4Enum.z:
                switch (_playerSlot)
                {
                    case PlayerSlot.PC1: return pc1Area.z;
                    case PlayerSlot.PC2: return pc2Area.z;
                }
                break;

            case V4Enum.w:
                switch (_playerSlot)
                {
                    case PlayerSlot.PC1: return pc1Area.w;
                    case PlayerSlot.PC2: return pc2Area.w;
                }
                break;
        }

        return -1;
    }

    /* Score系 ------------------------------------------------------------------------- */

    public void AddScore(float _score, PlayerSlot _ps)
    {
        switch(_ps)
        {
            case PlayerSlot.PC1: Pc1Score += _score; break;
            case PlayerSlot.PC2: Pc2Score += _score; break;
        }
    }
    
    /* --------------------------------------------------------------------------------- */
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