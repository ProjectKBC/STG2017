using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovePattern
{
	Straight,
	Circle,
	Chase,
}
	
public enum Spin
{
	Clockwise12,
	AntiClockwise12,
	Clockwise6,
	AntiClockwise6
}

[RequireComponent(typeof(EnemyShotManager))]
public abstract class Enemy : Ship
{
	public MovePattern movePattern;
	public Spin spin;
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
		switch (movePattern)
		{

		case MovePattern.Straight:
			direction = new Vector2(0, -1).normalized;
			pos = transform.position;
			pos += direction * speed * Time.deltaTime;

			transform.position = pos;
			break;

		case MovePattern.Circle:
			direction = new Vector2();
			switch (spin)
			{
				case Spin.Clockwise12:
					direction = new Vector2(Mathf.Cos(Time.time * speed), -Mathf.Sin(Time.time * speed)).normalized;
					break;
				case Spin.AntiClockwise12:
					direction = new Vector2(-Mathf.Cos(Time.time * speed), -Mathf.Sin(Time.time * speed)).normalized;
					break;
				case Spin.Clockwise6:
					direction = new Vector2(-Mathf.Cos(Time.time * speed), Mathf.Sin(Time.time * speed)).normalized;
					break;
				case Spin.AntiClockwise6:
					direction = new Vector2(Mathf.Cos(Time.time * speed), Mathf.Sin(Time.time * speed)).normalized;
					break;
			}
			pos = transform.position;
			pos += direction * speed * Time.deltaTime;

			transform.position = pos;
			break;

		case MovePattern.Chase:
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
