using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovePattern
{
	Straight,
	Circle,
	Chase,
}

[RequireComponent(typeof(EnemyShotManager))]
public abstract class Enemy : Ship
{
	public MovePattern movePattern;
	public bool xAxisReverse; // x軸の反転の有無
	public bool yAxisReverse; // y軸の反転の有無
    /*
     * パターン化したい
     * 動き
     * ショット
    */

    // ShotManagerを確保するリスト
    private Dictionary<string, ShotManager> shotManager = new Dictionary<string, ShotManager>();
    private float hitPoint;

    private void Init()
    {
        hitPoint = base.maxHitPoint;
    }

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
		Vector2 direction;
		Vector2 pos;
		int xAxis = 1;
		int yAxis = 1;
		if (xAxisReverse) xAxis = -1;
		if (yAxisReverse) yAxis = -1;

		switch (movePattern)
		{

		// 直進
		case MovePattern.Straight:
			direction = new Vector2(0, -1).normalized;
			pos = transform.position;
			pos += direction * speed * Time.deltaTime;

			transform.position = pos;
			break;

		// 円状に移動
		case MovePattern.Circle:
			direction = new Vector2(yAxis * Mathf.Cos(Time.time * speed), xAxis * Mathf.Sin(Time.time * speed)).normalized;
			pos = transform.position;
			pos += direction * speed * Time.deltaTime;

			transform.position = pos;
			break;

		// プレイヤーを追尾
		case MovePattern.Chase:
			GameObject player1 = GameObject.FindGameObjectWithTag("PC1");
			GameObject player2 = GameObject.FindGameObjectWithTag("PC2");

			if (transform.position.x < -1) transform.position = Vector2.Lerp(transform.position, player1.transform.position, speed * Time.deltaTime);
			else if (transform.position.x > 1) transform.position = Vector2.Lerp(transform.position, player2.transform.position, speed * Time.deltaTime);
			break;
		}
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
