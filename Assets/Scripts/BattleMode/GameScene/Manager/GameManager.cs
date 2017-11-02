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
                go.transform.parent = GameObject.Find("Managers").transform;
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

    private AudioSource audioSource;
    private List<AudioClip> BGMs = new List<AudioClip>();

    private void Init()
    {
        gameObject.AddComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
        BGMs.Add(Resources.Load<AudioClip>("Sounds/BGMs/gameBGM1"));
        BGMs.Add(Resources.Load<AudioClip>("Sounds/BGMs/gameBGM2"));
        BGMs.Add(Resources.Load<AudioClip>("Sounds/BGMs/gameBGM3"));

        // f:Killsの初期セットアップ
        foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
        {
            PC1Kills.Add(enemyType, 0);
            PC2Kills.Add(enemyType, 0);
        }

        System.Random r = new System.Random(4098);
        audioSource.clip = BGMs[1];
        audioSource.loop = true;
    }

    protected override IEnumerator Start()
    {
        yield return new WaitUntil(() => PauseManager.Inst.MyProc.started && BackGroundManager.Inst.MyProc.started);
        Debug.Log("_7:GameManagerが呼び出される。");
        Init();

        yield return new WaitUntil(() => GameStarter.IsSetPCName);
        Debug.Log("_8:GameManagerがPCNameを渡される。");
        CreatePlayer(Pc1Name, Pc2Name);

        Debug.Log("9:GameManagerがPlayerを生成する。");
        MyProc.started = true;
        
        yield return new WaitUntil( () => GameStarter.MyProc.started);

        NoaProcesser.PC1Proc.started = true;
        NoaProcesser.PC2Proc.started = true;
        NoaProcesser.BossProc.started = true;
        Debug.Log("_14:ゲームを開始する。");

        audioSource.Play();
    }

    private void Update()
    {
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
        if (NoaProcesser.PC1Proc.ended && NoaProcesser.PC2Proc.ended && !IsGameSet) { GameAllSet(); }
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
                Debug.Log(_deadEnemy.enemyType + ": " + PC1Kills[_deadEnemy.enemyType]);
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
        
        GameObject sc1 = Instantiate(Resources.Load("Prefabs/UI/PC1Score"), GameObject.Find(CanvasName.UI).transform) as GameObject;
        GameObject sc2 = Instantiate(Resources.Load("Prefabs/UI/PC2Score"), GameObject.Find(CanvasName.UI).transform) as GameObject;

        Text t;

        Pc1Score += Pc1Player.hitPoint * 1000;
        foreach (Transform x in sc1.GetComponentsInChildren<Transform>())
        {
            Debug.Log(x.name);
            switch (x.name)
            {
                case "small_kills":
                    t = x.gameObject.GetComponent<Text>();
                    t.text = PC1Kills[EnemyType.small].ToString();
                    break;

                case "medium_kills":
                    t = x.gameObject.GetComponent<Text>();
                    t.text = PC1Kills[EnemyType.medium].ToString();
                    break;

                case "large_kills":
                    t = x.gameObject.GetComponent<Text>();
                    t.text = PC1Kills[EnemyType.large].ToString();
                    break;

                case "HP_bonus":
                    t = x.gameObject.GetComponent<Text>();
                    t.text = (Pc1Player.hitPoint * 1000).ToString();
                    break;

                case "total_score":
                    t = x.gameObject.GetComponent<Text>();
                    t.text = Pc1Score.ToString();
                    break;
            }
        }

        Pc2Score += Pc2Player.hitPoint * 1000;
        foreach (Transform x in sc2.GetComponentsInChildren<Transform>())
        {
            switch (x.name)
            {
                case "small_kills":
                    x.gameObject.GetComponent<Text>().text = PC2Kills[EnemyType.small].ToString();
                    break;
                case "medium_kills":
                    x.gameObject.GetComponent<Text>().text = PC2Kills[EnemyType.medium].ToString();
                    break;
                case "large_kills":
                    x.gameObject.GetComponent<Text>().text = PC2Kills[EnemyType.large].ToString();
                    break;
                case "HP_bonus":
                    x.gameObject.GetComponent<Text>().text = (Pc2Player.hitPoint * 1000).ToString();
                    break;
                case "total_score":
                    x.gameObject.GetComponent<Text>().text = Pc2Score.ToString();
                    break;
            }
        }
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