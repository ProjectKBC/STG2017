using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyShotManager))]
public abstract class Enemy : Ship
{
    /*
     * パターン化したい
     * 動き
     * ショット
    */

    // ShotManagerを確保するリスト
    private Dictionary<string, ShotManager> shotManager = new Dictionary<string, ShotManager>();

    IEnumerator Start ()
    {
        while(true)
        {
            shot();
            yield return new WaitForSeconds(0.01f);
        }
	}
	
	void Update ()
    {
        move();
    }

    // 移動軌跡などを書き込む関数
    public virtual void move()
    {

    }

    // ショットする条件やショットそのものの処理
    public virtual void shot()
    {

    }

    // 当たり判定
    private void OnTriggerEnter2D(Collider2D c)
    {
        string layerName = LayerMask.LayerToName(c.gameObject.layer);

        switch (layerName)
        {
            case "Bullet (Player)":

                // todo: c.gameObjectからパラメータを抽出する(powerが欲しい)
                damage(0.0f); // 自分のダメージ処理
                Destroy(c.gameObject); // 弾の削除

                break;
        }
    }

    private void damage(float _damage)
    {
        this.hitPoint -= _damage;
        if (hitPoint < 0)
        {
            dead();
        }
    }

    private void dead()
    {
        // todo: スコア処理
        Destroy(this.gameObject);
    }
}
