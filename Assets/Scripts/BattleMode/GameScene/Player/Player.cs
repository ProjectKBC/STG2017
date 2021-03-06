using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSlot
{
    PC1, PC2
}

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Player : NoaBehaviour
{
    // プレイヤー番号
    public PlayerSlot playerSlot;

    public float maxHitPoint;

    [SerializeField] public float _speed;
    public float Speed
    {
        get { return _speed * 100; }
        set { _speed = value; }
    }

    // ShotManagerを確保するリスト
    public Dictionary<string, ShotManager> shotManager = new Dictionary<string, ShotManager>();
    public Skill skill; // スキル

    [System.NonSerialized] public float hitPoint;
    [System.NonSerialized] public string state = "None";
    [System.NonSerialized] public bool isStan = false;

    protected void Init()
    {
        // ShotManagerの読み込み
        ShotManager[] tmp = GetComponents<ShotManager>();
        foreach (ShotManager x in tmp)
        {
            x.param.playerSlot = playerSlot;
            shotManager.Add(x.param.name, x);
        }

        // Skillの読み込み
        skill = GetComponent<Skill>();

        // HPの初期化
        hitPoint = maxHitPoint;
    }

    protected override IEnumerator Start()
    {
        Init();
        MyProc.started = true;

        yield return new WaitUntil(() => NoaProcesser.BossProc.started);

        while (true)
        {
            if (NoaProcesser.IsStayBoss()) { yield return new WaitWhile(() => NoaProcesser.IsStayBoss()); }
            if (NoaProcesser.IsStayPC(playerSlot)) { yield return new WaitWhile(() => NoaProcesser.IsStayPC(playerSlot)); }

            InputManager();

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
                    shotManager[key].Shot();
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
	
	protected void Update ()
    {
        if (MyProc.IsStay() || NoaProcesser.IsStayBoss() || NoaProcesser.IsStayPC(playerSlot)) { return; }

        Move();
	}

    protected string ConvertPlayerSlotToButtonCode()
    {
        switch (playerSlot)
        {
            case PlayerSlot.PC1: return "pl1_";
            case PlayerSlot.PC2: return "pl2_";
            default: return null;
        }
    }

    // _keyにショット名を渡すと対応したShotのボタンについてGetButtonDownできる
    protected bool GetButtonDown(string _key)
    {
        if (_key == "Skill")
        {
            return Input.GetButtonDown(ConvertPlayerSlotToButtonCode() + ButtonCode.Skill.ToString());
        }
        if (_key == "Slow")
        {
            return Input.GetButtonDown(ConvertPlayerSlotToButtonCode() + ButtonCode.Slow.ToString());
        }
        return Input.GetButtonDown(ConvertPlayerSlotToButtonCode() + shotManager[_key].GetButtonCode());
    }

    // _keyにショット名を渡すと対応したShotのボタンについてGetButtonできる
    protected bool GetButton(string _key)
    {
        if (_key == "Skill")
        {
            return Input.GetButton(ConvertPlayerSlotToButtonCode() + ButtonCode.Skill.ToString());
        }
        if (_key == "Slow")
        {
            return Input.GetButton(ConvertPlayerSlotToButtonCode() + ButtonCode.Slow.ToString());
        }
        return Input.GetButton(ConvertPlayerSlotToButtonCode() + shotManager[_key].GetButtonCode());
    }

    // _keyにショット名を渡すと対応したShotのボタンについてGetButtonUpできる
    protected bool GetButtonUp(string _key)
    {
        if (_key == "Skill")
        {
            return Input.GetButtonUp(ConvertPlayerSlotToButtonCode() + ButtonCode.Skill.ToString());
        }
        if (_key == "Slow")
        {
            return Input.GetButtonUp(ConvertPlayerSlotToButtonCode() + ButtonCode.Slow.ToString());
        }
        return Input.GetButtonUp(ConvertPlayerSlotToButtonCode() + shotManager[_key].GetButtonCode());
    }

    // 上手いことキーの重なりとか調整してくれる関数
    protected void InputManager()
    {
        foreach (string key in shotManager.Keys)
        {
            if (state == key + "(KeyUp)")
            {
                state = "None";
            }

            if (GetButtonDown(key))
            {
                state = key;
            }

            if (GetButtonUp(key) && state == key)
            {
                state = key + "(KeyUp)";
            }
        }

        if (state == "Skill" + "(KeyUp)")
        {
            state = "None";
        }

        if (GetButtonDown("Skill"))
        {
            state = "Skill";
        }
        
        if (GetButtonUp("Skill") && state == "Skill")
        {
            state = "Skill(KeyUp)";
        }
        
    }

    // 移動処理
    void Move()
    {
        if (isStan) { return; }
        
        float x = Input.GetAxis(ConvertPlayerSlotToButtonCode() + "Horizontal");
        float y = Input.GetAxis(ConvertPlayerSlotToButtonCode() + "Vertical");

        Vector2 direction = new Vector2(x, y).normalized;

        Vector2 min = GameManager.GetAreaMin(playerSlot);
        Vector2 max = GameManager.GetAreaMax(playerSlot);
        
        Vector2 pos = transform.position;

        // Shiftで低速移動
        if (GetButton("Slow"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            pos += direction * Speed/2 * Time.deltaTime;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            pos += direction * Speed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);
        
        transform.position = pos;
    }

    // 当たり判定
    void OnTriggerEnter2D(Collider2D c)
    {
        switch (c.gameObject.layer)
        {
            case LayerName.BulletEnemy:
                EnemyBullet b = c.transform.parent.GetComponent<EnemyBullet>();
                StartCoroutine("Damage", b.param.power);
                Destroy(c.gameObject); // 弾の削除
                break;

            case LayerName.Enemy:

                StartCoroutine("Damage", 0.0f);
                break;
        }
    }

    IEnumerator Damage(float _damage)
    {
        hitPoint -= _damage;
        if (hitPoint <= 0)
        {
            Dead();
        }
        if (_damage != 0.0f)
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
            int count = 10;
            while (count > 0)
            {
                GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
                yield return new WaitForSeconds(0.05f);
                GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
                yield return new WaitForSeconds(0.05f);
                count--;
            }
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    void Dead()
    {
        Destroy(gameObject);
        GameManager.GameSet(this);
    }

    // f:生成時にパラメータを渡すことができるInstantiate関数
    public static Player Instantiate(Player _player, PlayerSlot _slot)
    {
        Vector3 position = (_slot == PlayerSlot.PC1)
                             ? new Vector3(GameManager.RECT.x / 2, GameManager.RECT.w / 2)
                             : new Vector3(GameManager.RECT.z / 2, GameManager.RECT.w / 2);
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        Player player = Instantiate(_player, position, rotation) as Player;

        SetupPlayer(player, _slot);

        return player;
    }

    protected static void SetupPlayer(Player _player, PlayerSlot _slot)
    {
        // f:名前の設定
        _player.name = _player.name.Replace("(Clone)", "");

        // f:スロットの設定
        _player.playerSlot = _slot;

        // f:レイヤー設定
        _player.gameObject.layer = LayerName.Player;

        // f:タグ設定
        //_player.gameObject.tag = TagName;

        // f:エリア設定
        GameManager.SetArea(_player.gameObject, _player.playerSlot);
    }

    public static Player Instantiate(GameObject _obj, PlayerSlot _slot)
    {
        Vector3 position = (_slot == PlayerSlot.PC1)
                             ? new Vector3(GameManager.RECT.x / 2, GameManager.RECT.w / 2)
                             : new Vector3(GameManager.RECT.z / 2, GameManager.RECT.w / 2);
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        GameObject obj = Instantiate(_obj, position, rotation);

        return SetupPlayer(obj, _slot);
    }

    protected static Player SetupPlayer(GameObject _obj, PlayerSlot _slot)
    {
        Player player = _obj.GetComponent<Player>();

        SetupPlayer(player, _slot);
        return player;       
    }
}
