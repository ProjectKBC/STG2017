<<<<<<< HEAD
﻿using System.Collections;
=======
﻿using System;
using System.Collections;
>>>>>>> test
using System.Collections.Generic;
using UnityEngine;

public enum MovePattern
{
	Straight,
	Circle,
	Chase,
    Snake,
}

[RequireComponent(typeof(Rigidbody2D))]
<<<<<<< HEAD
public abstract class Enemy : MonoBehaviour
{
=======
public abstract class Enemy : NoaBehaviour
{
    Dictionary<ShotMovepattern, EnemyShotManager> enemyShotManager = new Dictionary<ShotMovepattern, EnemyShotManager>();
    public ShotMovepattern currentShotPattern; // 弾の動き
>>>>>>> test
    public PlayerSlot playerSlot;
    public float maxHitPoint;
    [SerializeField]
    protected float _speed;
    public float Speed
    {
        get { return _speed * 100; }
        set { _speed = value; }
    }
<<<<<<< HEAD
=======

    public float radius;
>>>>>>> test
    public float score;

    public MovePattern movePattern;
	public bool xAxisReverse; // x軸の反転の有無
	public bool yAxisReverse; // y軸の反転の有無
<<<<<<< HEAD
    public bool xTurn = true;
    public bool yTurn = true;
    Vector2 pos = new Vector2();

=======
    public bool xTurn = true;     public bool yTurn = true;     Vector2 pos = new Vector2(); 
>>>>>>> test
    /*
     * 
     * パターン化したい
     * 動き
     * ショット
    */

    // ShotManagerを確保するリスト
<<<<<<< HEAD
    public Dictionary<string, EnemyShotManager> shotManager = new Dictionary<string, EnemyShotManager>();
    public Starter starter = new Starter();
=======
>>>>>>> test
    protected float hitPoint;

    protected void Init()
    {        
<<<<<<< HEAD
        // レイヤー分類
        gameObject.layer = LayerName.Enemy;

=======
>>>>>>> test
        // ShotManagerの読み込み
        EnemyShotManager[] tmp = GetComponents<EnemyShotManager>();
        foreach (EnemyShotManager x in tmp)
        {
            x.param.playerSlot = playerSlot;
<<<<<<< HEAD
            shotManager.Add(x.param.name, x);
=======
            enemyShotManager.Add(x.param.shotMovepattern, x);
>>>>>>> test
        }

        hitPoint = maxHitPoint;
    }

<<<<<<< HEAD
    protected IEnumerator Start ()
    {
        yield return starter.StayStarted(PlayerUIManager.starter);
        Init();
        starter.started = true;
        starter.Log(this, 5);

        yield return starter.StayStarted(GameManager.readier);

        while (true)
        {                         
=======
    protected override IEnumerator Start()
    {
        Init();
        MyProc.started = true;

        yield return NoaProcesser.StayBoss();

        while (true)
        {
            NoaProcesser.StayBoss();

>>>>>>> test
            Shot();
            yield return new WaitForSeconds(0.01f);
        }
	}

    protected void Update ()
    {
<<<<<<< HEAD
        if (GameManager.readier.started == false) { return; }

        Move();
    }

    void Turn()
    {
        if (xTurn) xAxisReverse = !xAxisReverse;
        if (yTurn) yAxisReverse = !yAxisReverse;
        if (-920 < pos.x && pos.x <= -830) pos.x = -829;
        else if (-130 <= pos.x && pos.x < 0) pos.x = -131;
        else if (0 < pos.x && pos.x <= 130) pos.x = 131;
        else if (830 <= pos.x && pos.x < 920) pos.x = 829;
        transform.position = pos;
        CancelInvoke();
    }
=======
        if (MyProc.IsStay() || NoaProcesser.IsStayBoss()) { return; }

        Move();
        Shot();
    }

    void Turn()     {         if (xTurn) xAxisReverse = !xAxisReverse;         if (yTurn) yAxisReverse = !yAxisReverse;         if (-920 < pos.x && pos.x <= -830) pos.x = -829;         else if (-130 <= pos.x && pos.x < 0) pos.x = -131;         else if (0 < pos.x && pos.x <= 130) pos.x = 131;         else if (830 <= pos.x && pos.x < 920) pos.x = 829;         transform.position = pos;         CancelInvoke();     }
>>>>>>> test

    // 移動軌跡などを書き込む関数
    public virtual void Move()
    {
		Vector2 direction;
        pos = transform.position;
		int xAxis = 1;
		int yAxis = 1;
		if (xAxisReverse) xAxis = -1;
		if (yAxisReverse) yAxis = -1;

		switch (movePattern)
		{

<<<<<<< HEAD
		// 直進
		case MovePattern.Straight:
			direction = new Vector2(0, -1).normalized;
			pos += direction * Speed * Time.deltaTime;
			break;

                // 円状に移動
		case MovePattern.Circle:
			direction = new Vector2(yAxis * Mathf.Cos(Time.time * _speed), xAxis * Mathf.Sin(Time.time * _speed)).normalized;
			pos += direction * Speed * Time.deltaTime;
			break;

		// プレイヤーを追尾
		case MovePattern.Chase:
			GameObject player1 = GameManager.Inst.Pc1GameObject;
			GameObject player2 = GameManager.Inst.Pc2GameObject;
            
            if (transform.position.x < -1)
            {
                pos = Vector2.Lerp(transform.position, player1.transform.position, Speed * Time.deltaTime);
            }
            else if (transform.position.x > 1)
            {
                pos = Vector2.Lerp(transform.position, player2.transform.position, Speed * Time.deltaTime);
            }
			break;

         // 壁に沿って蛇行
		case MovePattern.Snake:
			if ((-830 < pos.x && pos.x < -130) || (130 < pos.x && pos.x < 830)) {
				direction = new Vector2 (yAxis * -1, 0).normalized;
				pos += direction * Speed * Time.deltaTime;
			} else {
				direction = new Vector2 (0, -1).normalized;
				pos += direction * Speed * Time.deltaTime;
				Invoke ("Turn", 1 / _speed);
			}
			break;
		break;
=======
    		// 直進
    		case MovePattern.Straight:
    			direction = new Vector2(0, -1).normalized;
    			pos += direction * Speed * Time.deltaTime;
    			break;

                // 円状に移動
    		case MovePattern.Circle:
    			direction = new Vector2(yAxis * Mathf.Cos(Time.time * _speed) * radius, xAxis * Mathf.Sin(Time.time * _speed) * radius).normalized;
    			pos = transform.position;
                pos += direction * Speed * Time.deltaTime;

    			transform.position = pos;
                
    			direction = new Vector2(yAxis * Mathf.Cos(Time.time * _speed), xAxis * Mathf.Sin(Time.time * _speed)).normalized;
    			pos += direction * Speed * Time.deltaTime;
    			break;

    		    // プレイヤーを追尾
    		case MovePattern.Chase:
    			Player player1 = GameManager.Pc1Player;
                Player player2 = GameManager.Pc2Player;
                
                if (transform.position.x < -1)
                {
                    pos = Vector2.Lerp(transform.position, player1.transform.position, Speed * Time.deltaTime);
                }
                else if (transform.position.x > 1)
                {
                    pos = Vector2.Lerp(transform.position, player2.transform.position, Speed * Time.deltaTime);
                }
    			break;

                // 壁に沿って蛇行
            case MovePattern.Snake:                 if ((-830 < pos.x && pos.x < -130) || (130 < pos.x && pos.x < 830))                 {
                    direction = new Vector2(yAxis * -1, 0).normalized;                     pos += direction * Speed * Time.deltaTime;                 }                 else                 {                     direction = new Vector2(0, -1).normalized;                     pos += direction * Speed * Time.deltaTime;                     Invoke("Turn", 1 / _speed);                 }                 break;
>>>>>>> test
		}
        transform.position = pos;
    }

    // ショットする条件やショットそのものの処理
    public virtual void Shot()
    {
<<<<<<< HEAD
        foreach (string key in shotManager.Keys)
        {
            shotManager[key].Shot();
        }
=======
        if (NoaProcesser.BossProc.IsStay()) { return; }
        enemyShotManager[currentShotPattern].Shot();
>>>>>>> test
    }

    // 当たり判定
    protected void OnTriggerEnter2D(Collider2D c)
    {
        switch (c.gameObject.layer)
        {
            case LayerName.BulletPlayer:
                Bullet b = c.transform.parent.GetComponent<Bullet>();
                Damage(b.param.power);
                if (b.param.isPenetrate)
                {

                }
                else
                {
                    Destroy(c.gameObject); // 弾の削除
                }
                break;
        }
    }

    protected void Damage(float _damage)
    {
        hitPoint -= _damage;
        Debug.Log(hitPoint);
        if (hitPoint < 0)
        {
            Dead();
        }
    }

    protected void Dead()
    {
        // todo: スコア処理
        Destroy(this.gameObject);
    }
}
