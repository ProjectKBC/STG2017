using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSlot
{
    PC1, PC2
}

[RequireComponent(typeof(NormalShotManager))]
[RequireComponent(typeof(UniqueShotManager))]
[RequireComponent(typeof(Skill))]
public abstract class Player : Ship
{
    private PlayerSlot playerSlot;
    
    private NormalShotManager nsm;
    private UniqueShotManager usm;
    private Skill skill;

    private KeyCode NormalShotKey;
    private KeyCode UniqueShotKey;
    private KeyCode SkillKey;

    private string state = "None";

    private void init()
    {
        nsm   = GetComponent<NormalShotManager>();
        usm   = GetComponent<UniqueShotManager>();
        skill = GetComponent<Skill>();

        NormalShotKey = nsm.keyCode;
        UniqueShotKey = usm.keyCode;
        SkillKey = skill.keyCode;
    }

    IEnumerator Start ()
    {
        init();

        while (true)
        {
            InputManager();

            switch (state)
            {
                case "NormalShot":
                    nsm.shot();
                    break;

                case "UniqueShot":
                    usm.shot();
                    break;

                case "Skill":
                    break;
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
        if (Input.GetKeyDown(NormalShotKey) && state != "Skill")
        {
            state = "NormalShot";
            return;
        }
        if (Input.GetKeyDown(UniqueShotKey) && state != "Skill")
        {
            state = "UniqueShot";
            return;
        }
        if (Input.GetKeyDown(SkillKey))
        {
            state = "Skill";
        }


        if (Input.GetKeyUp(NormalShotKey) && state == "NormalShot")
        {
            state = "None";
        }
        if (Input.GetKeyUp(UniqueShotKey) && state == "UniqueShot")
        {
            state = "None";
        }
        if (Input.GetKeyUp(SkillKey) && state == "Skill")
        {
            state = "None";
        }
    }

    // 移動処理
    void move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(x, y).normalized;
        
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        //Vector2 min = GameManager.Inst.getAreaMin(this.playerSlot);
        //Vector2 max = GameManager.Inst.getAreaMax(this.playerSlot);

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
}
