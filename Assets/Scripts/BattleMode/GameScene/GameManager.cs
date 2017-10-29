<<<<<<< HEAD
﻿using System.Collections;
=======
﻿using System;
using System.Collections;
>>>>>>> test
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

<<<<<<< HEAD
public enum V4Enum { x, y, z, w }

public class Starter
{
    public bool started = false;

    public IEnumerator StayStarted()
    {
        while (true)
        {
            if (started) { break; }
            yield return null;
        }
    }

    public IEnumerator StayStarted(Starter _starter)
    {
        if (started == false)
        {
            while (true)
            {
                if (_starter.started) { break; }
                yield return null;
            }
        }
    }

    public void Log(Object _this)
    {
        if(started) { Debug.Log(_this + " started"); }
        else { Debug.Log(_this + " NOT started"); }
    }

    public void Log(Object _this, int _num)
    {
        if (started) { Debug.Log(_num + ": " + _this + " started"); }
        else { Debug.Log(_num + ": " + _this + " NOT started"); }
    }

    public void Log(Object _this, string _str)
    {
        if (started) { Debug.Log(_this + _str); }
        else { Debug.Log( _this + "NOT " + _str); }
    }
}

// シングルトン
public sealed class GameManager : MonoBehaviour
{
    private static GameManager inst;
    private GameManager()
    {
        pc1Name = FireLitStone.fls.pc1Name;
        pc2Name = FireLitStone.fls.pc2Name;
        Debug.Log("game_manager created");
    }
=======
// f:Vector4用の列挙体
public enum V4Enum { x, y, z, w }

// f:シングルトン
// f:ゲーム中の座標関係やオブジェクト、スコアを管理するクラス
// f:基本的にstatic
public sealed class GameManager : NoaBehaviour
{
    private static GameManager inst;
    private GameManager() {}
>>>>>>> test
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

<<<<<<< HEAD
    public static Starter starter = new Starter();
    public static Starter readier = new Starter();

    // 画面サイズ
    private const float WIDTH = 1920;
    private const float HEIGHT = 1080;

    // 画面に関する点座標
    private const float TOP = HEIGHT / 2;
    private const float BOTTOM = -HEIGHT / 2;
    private const float LEFT = -WIDTH / 2;
    private const float RIGHT = WIDTH / 2;
    private const float CENTER_X = 0;
    private const float CENTER_Y = 0;

    // 各プレイヤーの行動可能範囲
    private const float FRAME_W = 90f;
    private const float FRAME_H = 20f;
    public readonly Vector4 pc1Area = new Vector4(LEFT + FRAME_W, TOP - FRAME_H,
                                                  CENTER_X - FRAME_W, BOTTOM + FRAME_H);

    public readonly Vector4 pc2Area = new Vector4(CENTER_X + FRAME_W, TOP - FRAME_H,
                                                  RIGHT - FRAME_W, BOTTOM + FRAME_H);

    // 
    private const float D_MARGIN = 20;
    public readonly Vector4 destroyArea =
        new Vector4(LEFT - D_MARGIN, TOP + D_MARGIN, RIGHT + D_MARGIN, BOTTOM - D_MARGIN);

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

    private IEnumerator Start()
    {
        yield return starter.StayStarted(PlayerManager.starter);

        CreatePlayer(pc1Name, pc2Name);

        starter.started = true;
        starter.Log(this, 2);

        yield return starter.StayStarted(PlayerUIManager.starter);
        yield return starter.StayStarted(Pc1Player.starter);
        yield return starter.StayStarted(Pc2Player.starter);

        readier.started = true;
        readier.Log(this, "ready");
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
=======
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

    protected override IEnumerator Start()
    {
        yield return PlayerManager.Inst.MyProc.Stay();
        yield return GameStarter.MyProc.Stay();

        CreatePlayer(Pc1Name, Pc2Name);

        MyProc.started = true;
        MyProc.Log(this, 2);

        yield return PlayerUIManager.Inst.MyProc.Stay();
        yield return Pc1Player.MyProc.Stay();
        yield return Pc2Player.MyProc.Stay();

        NoaProcesser.BossProc.started = true;
        Debug.Log("GameProc started!!");
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
>>>>>>> test
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

<<<<<<< HEAD
        return -1;
    }

    /* Score系 ------------------------------------------------------------------------- */

    public void AddScore(float _score, PlayerSlot _ps)
=======
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

    public static void AddScore(float _score, PlayerSlot _ps)
>>>>>>> test
    {
        switch(_ps)
        {
            case PlayerSlot.PC1: Pc1Score += _score; break;
            case PlayerSlot.PC2: Pc2Score += _score; break;
        }
    }
    
<<<<<<< HEAD
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

        Debug.Log("created " + Pc1GameObject);


        Pc2GameObject = Instantiate
        (
            PlayerManager.Inst.GetCharacterPrefab(_pc2Name),
            new Vector3(RIGHT/2, BOTTOM/2),
            new Quaternion()
        ) as GameObject;

        Pc2GameObject.name = PlayerManager.Inst.GetCharacterPrefab(_pc2Name).name;
        Pc2Player = Pc2GameObject.GetComponent<Player>();
        Pc2Player.playerSlot = PlayerSlot.PC2;

        Debug.Log("created " + Pc2GameObject);
=======
    /* f:Game系 --------------------------------------------------------------------------- */
    public static void GameSet(Player _loser)
    {
        NoaProcesser.BossProc.ended = true;

        GameObject.Find(CanvasName.UI + "/PC1Shutter").GetComponent<Image>().color = new Color(255 / 255f, 0 / 255f, 0 / 255f, 100 / 255f);
        GameObject.Find(CanvasName.UI + "/PC2Shutter").GetComponent<Image>().color = new Color(255 / 255f, 0 / 255f, 0 / 255f, 100 / 255f);
    }

    /* f:Player系 ------------------------------------------------------------------------- */
    private static void CreatePlayer(string _pc1Name, string _pc2Name)
    {
        Pc1Player = Player.Instantiate(PlayerManager.GetCharacterPrefab(_pc1Name), PlayerSlot.PC1);
        Debug.Log("created " + Pc1Player);

        Pc2Player = Player.Instantiate(PlayerManager.GetCharacterPrefab(_pc2Name), PlayerSlot.PC2);
        Debug.Log("created " + Pc2Player);
>>>>>>> test
    }
}