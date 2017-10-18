﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovePattern
{
	Straight,
	Circle,
	Chase,
}

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{
    public PlayerSlot playerSlot;
    public float maxHitPoint;
    public float speed;
    public float score;

    public MovePattern movePattern;
	public bool xAxisReverse; // x軸の反転の有無
	public bool yAxisReverse; // y軸の反転の有無
    /*
     * パターン化したい
     * 動き
     * ショット
    */

    // ShotManagerを確保するリスト
    private Dictionary<string, EnemyShotManager> shotManager = new Dictionary<string, EnemyShotManager>();
    private float hitPoint;

    private void Init()
    {
        // レイヤー分類
        gameObject.layer = LayerName.Enemy;

        // ShotManagerの読み込み
        EnemyShotManager[] tmp = GetComponents<EnemyShotManager>();
        foreach (EnemyShotManager x in tmp)
        {
            shotManager.Add(x.param.name, x);
        }

        hitPoint = maxHitPoint;
    }

    IEnumerator Start ()
    {
        Init();
        while(true)
        {
            Shot();
            yield return new WaitForSeconds(0.01f);
        }
	}
	
	void Update ()
    {
        Move();
    }

    // 移動軌跡などを書き込む関数
    public virtual void Move()
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
			GameObject player1 = GameManager.Inst.Pc1GameObject;
			GameObject player2 = GameManager.Inst.Pc2GameObject;
            
            if (transform.position.x < -1)
            {
                transform.position = Vector2.Lerp(transform.position, player1.transform.position, speed * Time.deltaTime);
            }
            else if (transform.position.x > 1)
            {
                transform.position = Vector2.Lerp(transform.position, player2.transform.position, speed * Time.deltaTime);
            }
			break;
		}
    }

    // ショットする条件やショットそのものの処理
    public virtual void Shot()
    {
        shotManager["0"].Shot();
    }

    // 当たり判定
    private void OnTriggerEnter2D(Collider2D c)
    {
        switch (c.gameObject.layer)
        {
            case LayerName.BulletPlayer:
                Bullet b = c.transform.parent.GetComponent<Bullet>();
                Damage(b.param.power);
                Destroy(c.gameObject); // 弾の削除
                break;
        }
    }

    private void Damage(float _damage)
    {
        hitPoint -= _damage;
        Debug.Log(hitPoint);
        if (hitPoint < 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        // todo: スコア処理
        Destroy(this.gameObject);
    }
}
