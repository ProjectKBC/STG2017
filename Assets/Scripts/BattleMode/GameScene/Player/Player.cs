﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSlot
{
    PC1, PC2
}

[RequireComponent(typeof(Skill))]
public abstract class Player : Ship
{
    // プレイヤー番号
    public PlayerSlot playerSlot;

    // ShotManagerを確保するリスト
    public Dictionary<string, ShotManager> shotManager = new Dictionary<string, ShotManager>();
    public Skill skill; // スキル

    [System.NonSerialized] public float hitPoint;
    [System.NonSerialized] public bool Started = false;
    private string state = "None";

    private void Init()
    {
        // ShotManagerの読み込み
        ShotManager[] tmp = GetComponents<ShotManager>();
        foreach (ShotManager x in tmp)
        {
            shotManager.Add(x.param.name, x);
        }

        // Skillの読み込み
        skill = GetComponent<Skill>();

        // HPの初期化
        hitPoint = base.maxHitPoint;

        Debug.Log("3");
        Started = true;
    }

    IEnumerator Start ()
    {
        Init();

        while (true)
        {
            InputManager();

            /* debug */ if (Input.GetKeyDown(KeyCode.Q)) { --hitPoint; }

            if (state == "Skill" || state == "Skill(KeyUp)")
            {
                skill.shot();
                yield return new WaitForSeconds(0.01f); ;
            }

            foreach (string key in shotManager.Keys)
            {
                if (state == key || state == key + "(KeyUp)")
                {
                    if (state == key + "(KeyUp)") { Debug.Log(key + "(KeyUp)"); }
                    shotManager[key].Shot(state);
                    continue;
                }
                else
                {
                    shotManager[key].Maintenance(state);
                }
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
	
	void Update ()
    {
        Move();
	}

    private string ConvertPlayerSlotToButtonCode()
    {
        switch (playerSlot)
        {
            case PlayerSlot.PC1: return "pl1_";
            case PlayerSlot.PC2: return "pl2_";
            default: return null;
        }
    }

    // _keyにショット名を渡すと対応したShotのボタンについてGetButtonDownできる
    private bool GetButtonDown(string _key)
    {
        if (_key == "Skill")
        {
            return Input.GetButtonDown(ConvertPlayerSlotToButtonCode() + ButtonCode.Skill.ToString());
        }
        return Input.GetButtonDown(ConvertPlayerSlotToButtonCode() + shotManager[_key].GetButtonCode());
    }

    // _keyにショット名を渡すと対応したShotのボタンについてGetButtonできる
    private bool GetButton(string _key)
    {
        if (_key == "Skill")
        {
            return Input.GetButton(ConvertPlayerSlotToButtonCode() + ButtonCode.Skill.ToString());
        }
        return Input.GetButton(ConvertPlayerSlotToButtonCode() + shotManager[_key].GetButtonCode());
    }

    // _keyにショット名を渡すと対応したShotのボタンについてGetButtonUpできる
    private bool GetButtonUp(string _key)
    {
        if (_key == "Skill")
        {
            return Input.GetButtonUp(ConvertPlayerSlotToButtonCode() + ButtonCode.Skill.ToString());
        }
        return Input.GetButtonUp(ConvertPlayerSlotToButtonCode() + shotManager[_key].GetButtonCode());
    }

    // 上手いことキーの重なりとか調整してくれる関数
    private void InputManager()
    {
        foreach (string key in shotManager.Keys)
        {
            if (state == key + "(KeyUp)")
            {
                state = "None";
            }

            if (GetButtonDown(key) && state != "Skill")
            {
                state = key;
            }
        }
        if (GetButtonDown("Skill"))
        {
            state = "Skill";
        }

        foreach (string key in shotManager.Keys)
        {
            if (GetButtonUp(key) && state == key)
            {
                state = key + "(KeyUp)";
            }
        }
        if (GetButtonUp("Skill") && state == "Skill")
        {
            state = "Skill(KeyUp)";
        }
        
    }

    // 移動処理
    void Move()
    {
        
        float x = Input.GetAxis(ConvertPlayerSlotToButtonCode() + "Horizontal");
        float y = Input.GetAxis(ConvertPlayerSlotToButtonCode() + "Vertical");

        Vector2 direction = new Vector2(x, y).normalized;

        Vector2 min = GameManager.Inst.GetAreaMin(playerSlot);
        Vector2 max = GameManager.Inst.GetAreaMax(playerSlot);
        
        Vector2 pos = transform.position;

        // Shiftで低速移動
        if (Input.GetKey(KeyCode.LeftShift))
        {
            pos += direction * speed/2 * Time.deltaTime;
        }
        else
        {
            pos += direction * speed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);
        
        transform.position = pos;
    }

    // 当たり判定
    void OnTriggerEnter2D(Collider2D c)
    {
        string layerName = LayerMask.LayerToName(c.gameObject.layer);
        
        switch (layerName)
        {
            case "Bullet (Enemy)":

                // todo: c.gameObjectからパラメータを抽出する(powerが欲しい)

                damage(0.0f);   // 自分のダメージ処理
                Destroy(c.gameObject); // 弾の削除

                break;

            case "Enemy":
                
                // todo: c.gameObjectからパラメータを抽出する(powerが欲しい)

                damage(0.0f); // 自分のダメージ処理
                break;
        }
    }

    void damage(float _damage)
    {
        hitPoint -= _damage;
        if (hitPoint < 0)
        {
            dead();
        }
    }

    void dead()
    {
        Destroy(this.gameObject);
        //FindObjectOfType<GameManager>().gameSet();
    }

    // 気にしなくていい（生成時にパラメータを渡すための関数）
    public static Player Instantiate(Player _player, PlayerSlot _slot, Vector3 _position, Quaternion _rotation)
    {
        Player obj = Instantiate(_player, _position, _rotation) as Player;
        obj.playerSlot = _slot;
        return obj;
    }
}
