using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSlot
{
    PC1, PC2
}

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Player : MonoBehaviour
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
    [System.NonSerialized] public Starter starter = new Starter();
    [System.NonSerialized] public string state = "None";
    [System.NonSerialized] public bool isStan = false;

    protected void Init()
    {
        // レイヤー分類
        gameObject.layer = LayerName.Player;

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

    IEnumerator Start ()
    {
        starter.StayStarted(PlayerManager.starter);
        Init();
        starter.started = true;
        starter.Log(this, 3);

        yield return starter.StayStarted(GameManager.readier);

        while (true)
        {
            InputManager();
            if(state.Contains("(KeyUp)")) Debug.Log("p:"+Time.time);

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
	
	void Update ()
    {
        if (GameManager.readier.started == false) { return; }

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

        Vector2 min = GameManager.Inst.GetAreaMin(playerSlot);
        Vector2 max = GameManager.Inst.GetAreaMax(playerSlot);
        
        Vector2 pos = transform.position;

        // Shiftで低速移動
        if (GetButton("Slow"))
        {
            pos += direction * Speed/2 * Time.deltaTime;
        }
        else
        {
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
                Debug.Log(hitPoint);
                EnemyBullet b = c.transform.parent.GetComponent<EnemyBullet>();
                Damage(b.param.power);
                Destroy(c.gameObject); // 弾の削除
                break;

            case LayerName.Enemy:
                
                Damage(0.0f); // 自分のダメージ処理
                break;
        }
    }

    void Damage(float _damage)
    {
        hitPoint -= _damage;
        if (hitPoint < 0)
        {
            Dead();
        }
    }

    void Dead()
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
