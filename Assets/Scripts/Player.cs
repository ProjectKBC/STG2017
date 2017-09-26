using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSlot
{
    PC1, PC2
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NormalShotManager))]
[RequireComponent(typeof(UniqueShotManager))]
public abstract class Player : Ship
{
    public float hitPoint;
    public float speed;

    private PlayerSlot playerSlot;

    private string activeKey = null;

    private NormalShotManager nsm;
    private UniqueShotManager usm;

    private void init()
    {
        nsm = GetComponent<NormalShotManager>();
        usm = GetComponent<UniqueShotManager>();
    }

    IEnumerator Start ()
    {
        init();

        while (true)
        {
            switch (keyInput())
            {
                case "NormalShot": Debug.Log("ns"); nsm.shot(transform); break;
                case "UniqueShot": Debug.Log("us"); usm.shot(transform); break;
            }
            
            yield return new WaitForSeconds(0.01f);
        }
    }
	
	void Update ()
    {
        move();
	}

    string keyInput()
    {
        if (Input.GetKeyDown(KeyCode.Z)) { activeKey = "Z"; }
        if (Input.GetKeyDown(KeyCode.X)) { activeKey = "X"; }

        if (Input.GetKeyUp(KeyCode.Z) && activeKey == "Z") { activeKey = null; }
        if (Input.GetKeyUp(KeyCode.X) && activeKey == "X") { activeKey = null; }

        if (Input.GetKey(KeyCode.Z) && (activeKey == "Z" || activeKey == null))
        {
            return "NormalShot";
        }
        if (Input.GetKey(KeyCode.X) && (activeKey == "X" || activeKey == null))
        {
            return "UniqueShot";
        }

        return null;
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

                // todo
                // c.gameObjectからパラメータを抽出する(powerが欲しい)

                Destroy(c.gameObject); // 弾の削除
                damage(); // 自分のダメージ処理
                break;

            case "Enemy":

                // todo
                // c.gameObjectからパラメータを抽出する(powerが欲しい)

                damage(); // 自分のダメージ処理
                break;
        }
    }

    void damage()
    {

    }

    void dead()
    {
        //FindObjectOfType<GameManager>().gameSet();
        this.Explosion();
        Destroy(gameObject);
    }
}
