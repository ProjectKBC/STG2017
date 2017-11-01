using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// f:Vector4用の列挙体
public enum V4Enum { x, y, z, w }

// f:シングルトン
// f:ゲーム中の座標関係やオブジェクト、スコアを管理するクラス
// f:基本的にstatic
public sealed class GameManager : NoaBehaviour
{
    private static GameManager inst;
    private GameManager() {}
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

    // f:画面サイズ
    private const float WIDTH = 1920;
    private const float HEIGHT = 1080;

    // f:World座標
    private const float TOP      = HEIGHT / 2;
    private const float BOTTOM   = -HEIGHT / 2;
    private const float LEFT     = -WIDTH / 2;
    private const float RIGHT    = WIDTH / 2;
    private const float CENTER_X = 0;
    private const float CENTER_Y = 0;

    public static readonly Vector4 RECT = new Vector4(LEFT, TOP, RIGHT, BOTTOM);

    // f:Local座標
    private const float L_TOP      = 0;
    private const float L_BOTTOM   = -1040;
    private const float L_LEFT     = 0;
    private const float L_RIGHT    = 780;
    private const float L_CENTER_X = L_BOTTOM / 2;
    private const float L_CENTER_Y = L_RIGHT / 2;

    public static readonly Vector4 L_RECT = new Vector4(L_LEFT, L_TOP, L_RIGHT, L_BOTTOM);

    // f:各プレイヤーの行動可能範囲
    private const float FRAME_W = 90f;
    private const float FRAME_H = 20f;

    public static readonly Vector4 pc1Area =
        new Vector4(LEFT + FRAME_W, TOP - FRAME_H, CENTER_X - FRAME_W, BOTTOM + FRAME_H);
    public static readonly Vector4 pc2Area =
        new Vector4(CENTER_X + FRAME_W, TOP - FRAME_H, RIGHT - FRAME_W, BOTTOM + FRAME_H);

    // f:選択されたキャラクターの名前
    public static string Pc1Name { get; private set; }
    public static string Pc2Name { get; private set; }

    // f:各プレイヤーのオブジェクトとPlayerクラス
    public static Player Pc1Player { get; private set; }
    public static Player Pc2Player { get; private set; }

    // f:各プレイヤーのスコア
    public static float Pc1Score { get; private set; }
    public static float Pc2Score { get; private set; }

    // f:撃破数を格納する連想配列
    public static Dictionary<EnemyType, int> PC1Kills = new Dictionary<EnemyType, int>();
    public static Dictionary<EnemyType, int> PC2Kills = new Dictionary<EnemyType, int>();

    // f:リザルト
    // 残りHP
    // 倒した敵機の数（種類別）
    // totalスコア
    // 勝ち負け

    public static bool IsGameSet = false;

    private void Init()
    {

        // f:Killsの初期セットアップ
        foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
        {
            PC1Kills.Add(enemyType, 0);
            PC2Kills.Add(enemyType, 0);
        }
    }

    protected override IEnumerator Start()
    {
        Init();

        yield return new WaitWhile( () => PlayerManager.Inst.MyProc.IsStay() && GameStarter.MyProc.IsStay());

        CreatePlayer(Pc1Name, Pc2Name);
        
        yield return new WaitWhile
            ( () =>
                PlayerUIManager.Inst.MyProc.IsStay() &&
                Pc1Player.MyProc.IsStay() &&
                Pc2Player.MyProc.IsStay()
            );

        MyProc.started = true;
        MyProc.Log(this, 2);

        yield return new WaitUntil( () => Loading.Inst.MyProc.ended);

        NoaProcesser.BossProc.started = true;
        Debug.Log("GameProc started!!");
    }

    private void Update()
    {
        // BossとPCの連動
        if (NoaProcesser.BossProc.started) { NoaProcesser.PC1Proc.started = NoaProcesser.PC2Proc.started = true; }
        else { NoaProcesser.PC1Proc.started = NoaProcesser.PC2Proc.started = false; }
        if (NoaProcesser.BossProc.ended) { NoaProcesser.PC1Proc.ended = NoaProcesser.PC2Proc.ended = true; }
        else { NoaProcesser.PC1Proc.ended = NoaProcesser.PC2Proc.ended = false; }

        // ポーズ
        if (!NoaProcesser.BossProc.ended && (Input.GetButtonDown("pl1_Pause") || Input.GetButtonDown("pl2_Pause")))
        {
            if (NoaProcesser.BossProc.pausing)
            {
                Pause.Inst.Active(false);
            }
            else
            {
                Pause.Inst.Active(true);
            }
        }

        // ゲームセット
        if (NoaProcesser.PC1Proc.ended && NoaProcesser.PC2Proc.ended) { GameAllSet(); }
    }

    /**/
    public static void SetPCName(string _name, PlayerSlot _playerSlot)
    {
        switch (_playerSlot)
        {
            case PlayerSlot.PC1: Pc1Name = _name; break;
            case PlayerSlot.PC2: Pc2Name = _name; break;
        }
    }

    /* f:Area系 ------------------------------------------------------------------------- */
    public static float GetAreaPoint(PlayerSlot _playerSlot, V4Enum _v4)
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

        return 0;
    }

    public static Vector2 GetAreaMin(PlayerSlot _playerSlot)
    {
        return new Vector4(GetAreaPoint(_playerSlot, V4Enum.x), GetAreaPoint(_playerSlot, V4Enum.w));
    }

    public static Vector2 GetAreaMax(PlayerSlot _playerSlot)
    {
        return new Vector4(GetAreaPoint(_playerSlot, V4Enum.z), GetAreaPoint(_playerSlot, V4Enum.y));
    }

    public static bool OutOfArea(Vector2 _position, PlayerSlot _playerSlot)
    {
        Vector2 min = GetAreaMin(_playerSlot);
        Vector2 max = GetAreaMax(_playerSlot);
        Vector2 margin = new Vector2(50, 50);
        return (_position.x < min.x - margin.x || _position.x > max.x + margin.x ||
                _position.y < min.y - margin.y || _position.y > max.y + margin.y);
    }

    public static void SetArea(GameObject _go, PlayerSlot _playerSlot)
    {
        _go.transform.parent = (_playerSlot == PlayerSlot.PC1)
                ? GameObject.Find(CanvasName.PC1AREA).transform
                : GameObject.Find(CanvasName.PC2AREA).transform;
    }

    /* f:Score系 ------------------------------------------------------------------------- */

    public static int GetScoreValue(EnemyType _enemyType)
    {
        switch(_enemyType)
        {
            case EnemyType.small:  return 100;
            case EnemyType.medium: return 500;
            case EnemyType.large:  return 3000;

            default: return 0;
        }
    }

    public static void SetParam(Enemy _deadEnemy)
    {
        switch(_deadEnemy.playerSlot)
        {
            case PlayerSlot.PC1:
                ++PC1Kills[_deadEnemy.enemyType];
                Pc1Score += _deadEnemy.score;
                break;
            case PlayerSlot.PC2:
                ++PC2Kills[_deadEnemy.enemyType];
                Pc2Score += _deadEnemy.score;
                break;
        }
    }
    
    /* f:Game系 --------------------------------------------------------------------------- */
    public static void GameSet(Player _player)
    {
        switch (_player.playerSlot)
        {
            case PlayerSlot.PC1:
                NoaProcesser.PC1Proc.ended = true;
                GameObject.Find(CanvasName.UI + "/PC1Shutter").GetComponent<Image>().color = new Color(255 / 255f, 0 / 255f, 0 / 255f, 100 / 255f);

                break;

            case PlayerSlot.PC2:
                NoaProcesser.PC2Proc.ended = true;
                GameObject.Find(CanvasName.UI + "/PC2Shutter").GetComponent<Image>().color = new Color(255 / 255f, 0 / 255f, 0 / 255f, 100 / 255f);

                break;
        }
    }

    private void GameAllSet()
    {
        IsGameSet = true;
        NoaProcesser.BossProc.ended = true;
        
        Instantiate(Resources.Load("Prefabs/UI/PC1Score"), GameObject.Find(CanvasName.UI).transform);
        Instantiate(Resources.Load("Prefabs/UI/PC2Score"), GameObject.Find(CanvasName.UI).transform);
    }

    /* f:Player系 ------------------------------------------------------------------------- */
    private static void CreatePlayer(string _pc1Name, string _pc2Name)
    {
        Pc1Player = Player.Instantiate(PlayerManager.GetCharacterPrefab(_pc1Name), PlayerSlot.PC1);
        Debug.Log("created " + Pc1Player);

        Pc2Player = Player.Instantiate(PlayerManager.GetCharacterPrefab(_pc2Name), PlayerSlot.PC2);
        Debug.Log("created " + Pc2Player);
    }
    
}