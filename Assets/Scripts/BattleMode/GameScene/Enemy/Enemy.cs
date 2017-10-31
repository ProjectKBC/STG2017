using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    small,
    medium,
    large
}

public enum MovePattern
{
	Straight,
	Slanting,
	Circle,
	Chase,
    Snake,
	Side,
}

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : NoaBehaviour
{
    Dictionary<ShotMovePattern, EnemyShotManager> enemyShotManager = new Dictionary<ShotMovePattern, EnemyShotManager>();
    public ShotMovePattern currentShotPattern; // 弾の動き
    public PlayerSlot playerSlot;
    public float maxHitPoint;
    [SerializeField]
    protected float _speed;
    public float Speed
    {
        get { return _speed * 100; }
        set { _speed = value; }
    }

    public float radius;
    public float score;
    public EnemyType enemyType;

    public MovePattern movePattern;
	public bool xAxisReverse; // x軸の反転の有無
	public bool yAxisReverse; // y軸の反転の有無
    public bool xTurn = true;
    public bool yTurn = true;
    Vector2 pos = new Vector2();

    /*
     * 
     * パターン化したい
     * 動き
     * ショット
    */

    // ShotManagerを確保するリスト
    protected float hitPoint;

    protected void Init()
    {        
        // ShotManagerの読み込み
        EnemyShotManager[] tmp = GetComponents<EnemyShotManager>();
        foreach (EnemyShotManager x in tmp)
        {
            x.param.playerSlot = playerSlot;
            enemyShotManager.Add(x.param.shotMovePattern, x);
        }

        hitPoint = maxHitPoint;
    }

    protected override IEnumerator Start()
    {
        Init();
        MyProc.started = true;

        yield return NoaProcesser.StayBoss();

        while (true)
        {
            NoaProcesser.StayBoss();

            Shot();
            yield return new WaitForSeconds(0.01f);
        }
	}

    protected void Update ()
    {
        if (MyProc.IsStay() || NoaProcesser.IsStayBoss()) { return; }

        Move();
        Shot();
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

    		// 直進
    		case MovePattern.Straight:
			    direction = new Vector2(0, xAxis * -1).normalized;
    			pos += direction * Speed * Time.deltaTime;
    			break;

			// 斜め移動
			case MovePattern.Slanting:
				direction = new Vector2(yAxis * 1, xAxis * -1).normalized;
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

                switch(playerSlot)
                {
                    case PlayerSlot.PC1:
                        pos = Vector2.Lerp(transform.position, player1.transform.position, Speed * Time.deltaTime);
                        break;

                    case PlayerSlot.PC2:
                        pos = Vector2.Lerp(transform.position, player2.transform.position, Speed * Time.deltaTime);
                        break;
                }
    			break;

                // 壁に沿って蛇行
            case MovePattern.Snake:
                if ((-830 < pos.x && pos.x < -130) || (130 < pos.x && pos.x < 830))
                {
                    direction = new Vector2(yAxis * -1, 0).normalized;
                    pos += direction * Speed * Time.deltaTime;
                }
                else
                {
                    direction = new Vector2(0, -1).normalized;
                    pos += direction * Speed * Time.deltaTime;
                    Invoke("Turn", 1 / _speed);
                }
                break;

			case MovePattern.Side:
				if ((-830 < pos.x && pos.x < -130) || (130 < pos.x && pos.x < 830))
				{
					direction = new Vector2 (yAxis * -1, 0).normalized;
					pos += direction * Speed * Time.deltaTime;
				}
                else
                {
					Turn ();
				}
				break;
			
		}
        transform.position = pos;
    }

    // ショットする条件やショットそのものの処理
    public virtual void Shot()
    {
        if (NoaProcesser.BossProc.IsStay()) { return; }
        enemyShotManager[currentShotPattern].Shot();
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
        GameManager.SetParam(this);
        NoaConsole.Log(this.score + ": " + this.name + " destroy", this.playerSlot);
        Destroy(this.gameObject);
    }
}
