using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private GameManager() {}
    public static GameManager Inst
    {
        get
        {
            if (inst == null)
            {
                GameObject go = GameObject.Find("GameManager");
                inst = go.AddComponent<GameManager>();
            }

            return inst;
        }
    }

    public static Starter starter = new Starter();
    public static Starter readier = new Starter();

    // 画面サイズ
    private const float WIDTH = 1920;
    private const float HEIGHT = 1080;

    // World座標
    private const float TOP = HEIGHT / 2;
    private const float BOTTOM = -HEIGHT / 2;
    private const float LEFT = -WIDTH / 2;
    private const float RIGHT = WIDTH / 2;
    private const float CENTER_X = 0;
    private const float CENTER_Y = 0;

    // Local座標
    private const float L_TOP      = 0;
    private const float L_BOTTOM   = -1040;
    private const float L_LEFT     = 0;
    private const float L_RIGHT    = 780;
    public static readonly Vector4 L_RECT = new Vector4(L_LEFT, L_TOP, L_RIGHT, L_BOTTOM);
    private const float L_CENTER_X = L_BOTTOM / 2;
    private const float L_CENTER_Y = L_RIGHT / 2;

    // 各プレイヤーの行動可能範囲
    private const float FRAME_W = 90f;
    private const float FRAME_H = 20f;

    public static readonly Vector4 pc1Area =
        new Vector4(LEFT - FRAME_W, TOP - FRAME_H, CENTER_X - FRAME_W, BOTTOM + FRAME_H);
    public static readonly Vector4 pc2Area =
        new Vector4(CENTER_X + FRAME_W, TOP - FRAME_H, RIGHT - FRAME_W, BOTTOM + FRAME_H);

    // 
    private const float D_MARGIN = 20;
    public static readonly Vector4 destroyArea =
        new Vector4(L_LEFT - D_MARGIN, L_TOP + D_MARGIN, L_RIGHT + D_MARGIN, L_BOTTOM - D_MARGIN);

    // 選択されたキャラクターの名前
    public static string Pc1Name { get; private set; }
    public static string Pc2Name { get; private set; }

    // 各プレイヤーのオブジェクトとPlayerクラス
    public static GameObject Pc1GameObject { get; private set; }
    public static GameObject Pc2GameObject { get; private set; }
    public static Player Pc1Player { get; private set; }
    public static Player Pc2Player { get; private set; }

    // 各プレイヤーのスコア
    public static float Pc1Score { get; private set; }
    public static float Pc2Score { get; private set; }

    private IEnumerator Start()
    {
        yield return starter.StayStarted(PlayerManager.starter);

        CreatePlayer(Pc1Name, Pc2Name);

        starter.started = true;
        starter.Log(this, 2);

        yield return starter.StayStarted(PlayerUIManager.starter);
        yield return starter.StayStarted(Pc1Player.starter);
        yield return starter.StayStarted(Pc2Player.starter);

        readier.started = true;
        readier.Log(this, "ready");
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

    /* Area系 ------------------------------------------------------------------------- */
    public static Vector2 GetAreaMin(PlayerSlot _playerSlot)
    {
        switch (_playerSlot)
        {
            case PlayerSlot.PC1: return new Vector2(pc1Area.x, pc1Area.w);
            case PlayerSlot.PC2: return new Vector2(pc2Area.x, pc2Area.w);
            default: return new Vector2(-1, -1);
        }
    }

    public static Vector2 GetAreaMax(PlayerSlot _playerSlot)
    {
        switch (_playerSlot)
        {
            case PlayerSlot.PC1: return new Vector2(pc1Area.z, pc1Area.y);
            case PlayerSlot.PC2: return new Vector2(pc2Area.z, pc2Area.y);
            default: return new Vector2(-1, -1);
        }
    }

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

        return -1;
    }

    public static void SetArea(GameObject _go, PlayerSlot _playerSlot)
    {
        switch(_playerSlot)
        {
            case PlayerSlot.PC1: _go.transform.parent = GameObject.Find(CanvasName.PC1AREA).transform; break;
            case PlayerSlot.PC2: _go.transform.parent = GameObject.Find(CanvasName.PC2AREA).transform; break;
        }
    }

    /* Score系 ------------------------------------------------------------------------- */

    public static void AddScore(float _score, PlayerSlot _ps)
    {
        switch(_ps)
        {
            case PlayerSlot.PC1: Pc1Score += _score; break;
            case PlayerSlot.PC2: Pc2Score += _score; break;
        }
    }
    
    /* Game系 --------------------------------------------------------------------------- */
    public static void GameSet()
    {

    }

    /* Player系 ------------------------------------------------------------------------- */
    private static void CreatePlayer(string _pc1Name, string _pc2Name)
    {
        Pc1GameObject = Player.Instantiate
        (
            PlayerManager.GetCharacterPrefab(_pc1Name),
            new Vector3(LEFT / 2, BOTTOM / 2),
            new Quaternion()
        ) as GameObject;

        Pc1GameObject.name = PlayerManager.GetCharacterPrefab(_pc1Name).name;
        Pc1Player = Pc1GameObject.GetComponent<Player>();
        Pc1Player.playerSlot = PlayerSlot.PC1;
        SetArea(Pc1GameObject, Pc1Player.playerSlot);

        Debug.Log("created " + Pc1GameObject);


        Pc2GameObject = Player.Instantiate
        (
            PlayerManager.GetCharacterPrefab(_pc2Name),
            new Vector3(RIGHT / 2, BOTTOM / 2),
            new Quaternion()
        ) as GameObject;

        Pc2GameObject.name = PlayerManager.GetCharacterPrefab(_pc2Name).name;
        Pc2Player = Pc2GameObject.GetComponent<Player>();
        Pc2Player.playerSlot = PlayerSlot.PC2;
        SetArea(Pc2GameObject, Pc2Player.playerSlot);

        Debug.Log("created " + Pc2GameObject);
    }
}