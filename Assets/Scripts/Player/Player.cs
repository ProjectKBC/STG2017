using System.Collections;
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
    public PlayerSlot playerSlot { get; set; }
    
    // ShotManagerを確保するリスト
    private Dictionary<string, ShotManager> shotManager = new Dictionary<string, ShotManager>();
    private Skill skill; // スキル

    private string state = "None";

    private void init()
    {
        // ShotManagerの読み込み
        ShotManager[] tmp = GetComponents<ShotManager>();
        foreach (ShotManager x in tmp)
        {
            shotManager.Add(x.param.name, x);
        }

        // Skillの読み込み
        skill = GetComponent<Skill>();
    }

    IEnumerator Start ()
    {
        init();

        while (true)
        {
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
                    shotManager[key].shot(); break;
                }
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
	
	void Update ()
    {
        move();
	}

    void InputManager()
    {
        foreach (string key in shotManager.Keys)
        {
            if (state == key + "(KeyUp)")
            {
                state = "None";
            }

            if (Input.GetKeyDown(shotManager[key].keyCode) && state != "Skill")
            {
                state = key;
            }
        }
        if (Input.GetKeyDown(skill.keyCode))
        {
            state = "Skill";
        }

        foreach (string key in shotManager.Keys)
        {
            if (Input.GetKeyUp(shotManager[key].keyCode) && state == key)
            {
                state = key + "(KeyUp)";
            }
        }
        if (Input.GetKeyUp(skill.keyCode) && state == "Skill")
        {
            state = "Skill(KeyUp)";
        }
        
    }

    // 移動処理
    void move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(x, y).normalized;

        Vector2 min = GameManager.Inst.getAreaMin(playerSlot);
        Vector2 max = GameManager.Inst.getAreaMax(playerSlot);
        
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
        this.hitPoint -= _damage;
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
