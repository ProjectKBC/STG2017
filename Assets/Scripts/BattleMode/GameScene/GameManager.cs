using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum V4Enum { x, y, z, w }

// シングルトン
public sealed class GameManager : MonoBehaviour
{
    // 画面サイズ
    private const float WIDTH  = 1920;
    private const float HEIGHT = 1080;

    // 画面に関する点座標
    private const float TOP    =  HEIGHT / 2;
    private const float BOTTOM = -HEIGHT / 2;
    private const float LEFT   = -WIDTH / 2;
    private const float RIGHT  =  WIDTH / 2;
    private const float CENTER_X = 0;
    private const float CENTER_Y = 0;

    // 各プレイヤーの行動可能範囲
    private const float FRAME_W = 90f;
    private const float FRAME_H = 20f;
    public readonly Vector4 pc1Area = new Vector4(LEFT     + FRAME_W, TOP    - FRAME_H,
                                                  CENTER_X - FRAME_W, BOTTOM + FRAME_H);

    public readonly Vector4 pc2Area = new Vector4(CENTER_X + FRAME_W, TOP    - FRAME_H,
                                                  RIGHT    - FRAME_W, BOTTOM + FRAME_H);

    // 
    private const float D_MARGIN = 20;
    public readonly Vector4 destroyArea = 
        new Vector4(LEFT - D_MARGIN, TOP + D_MARGIN, RIGHT + D_MARGIN,  BOTTOM - D_MARGIN);

    // 選択されたキャラクターの名前
    public readonly string pc1Name;
    public readonly string pc2Name;

    // 各プレイヤーのオブジェクトとPlayerクラス
    public GameObject Pc1GameObject { get; private set; }
    public GameObject Pc2GameObject { get; private set; }
    public Player Pc1Player { get; private set; }
    public Player Pc2Player { get; private set; }

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
            case PlayerSlot.PC1: return new Vector2(pc1Area.x, pc1Area.w);
            case PlayerSlot.PC2: return new Vector2(pc2Area.x, pc2Area.w);
            default: return new Vector2(-1, -1);
        }
    }

    public Vector2 GetAreaMax(PlayerSlot _playerSlot)
    {
        switch (_playerSlot)
        {
            case PlayerSlot.PC1: return new Vector2(pc1Area.z, pc1Area.y);
            case PlayerSlot.PC2: return new Vector2(pc2Area.z, pc2Area.y);
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
    
    /* Game系 --------------------------------------------------------------------------- */
    public void GameSet()
    {

    }

    /* Player系 ------------------------------------------------------------------------- */
    private void CreatePlayer(string _pc1Name, string _pc2Name)
    {
        Pc1GameObject = Instantiate
        (
            PlayerManager.Inst.GetCharacterPrefab(_pc1Name),
            new Vector3(LEFT/2, BOTTOM/2),
            new Quaternion()
        ) as GameObject;

        Pc1GameObject.name = PlayerManager.Inst.GetCharacterPrefab(_pc1Name).name;
        Pc1Player = Pc1GameObject.GetComponent<Player>();
        Pc1Player.playerSlot = PlayerSlot.PC1;

        Debug.Log("1:created " + Pc1GameObject);


        Pc2GameObject = Instantiate
        (
            PlayerManager.Inst.GetCharacterPrefab(_pc2Name),
            new Vector3(RIGHT/2, BOTTOM/2),
            new Quaternion()
        ) as GameObject;

        Pc2GameObject.name = PlayerManager.Inst.GetCharacterPrefab(_pc2Name).name;
        Pc2Player = Pc2GameObject.GetComponent<Player>();
        Pc2Player.playerSlot = PlayerSlot.PC2;

        Debug.Log("2:created " + Pc2GameObject);
    }
}