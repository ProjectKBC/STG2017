﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyBulletParam
{
    public string name;         // ショットの名前
    public AudioClip shotSound; // ショット音

    // 共通パラメータ
    public float shotDelay;         // ショット間隔
    public float lifeTime;          // 生存時間
    
    // 弾丸速度
    [SerializeField]
    protected float speed;
    public float Speed
    {
        get { return speed * 100; }
        set { speed = value; }
    }             

    public float power;             // 攻撃力
    public bool isPenetrate;        // 貫通性の有無

    public Vector3 initialPosition; // 自機を起点とした初期位置

    [System.NonSerialized] public PlayerSlot playerSlot;
}

public abstract class EnemyBullet : NoaBehaviour
{
    [System.NonSerialized] public EnemyBulletParam param;
    protected Enemy enemy;

    protected override IEnumerator Start()
    {
        yield return enemy.MyProc.Stay();

        Init();
        MyProc.started = true;

        yield return NoaProcesser.StayBoss();

        // lifeTime秒後に削除
        Destroy(gameObject, param.lifeTime);
    }

    protected void Update()
    {
        if (MyProc.IsStay() || NoaProcesser.IsStayBoss()) { return; }

        if (enemy.MyProc.started == false) { return; }

        Move();

        // f:範囲外判定
        foreach (Transform x in GetComponentInChildren<Transform>())
        {
            if (GameManager.OutOfArea(x.position, enemy.playerSlot))
            {
                Destroy(x.gameObject);
            }
        }
        
        // f:子要素（弾）がすべてなくなったら削除
        if (transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }
    }

    // f:初期設定関数
    public virtual void Init() { }

    // f: 
    public virtual void Move() { }

    // f:弾生成時にパラメータを渡せるInstantiate関数
    public static EnemyBullet Instantiate(EnemyBullet _bullet, EnemyBulletParam _param, Transform _transform)
    {
        EnemyBullet bullet = Instantiate(_bullet, _transform.position + _param.initialPosition, _transform.rotation) as EnemyBullet;
        SetupBullet(bullet, _param, _transform);
        return bullet;
    }

    protected static void SetupBullet(EnemyBullet _bullet, EnemyBulletParam _param, Transform _transform)
    {
        // f:パラメータとPlayerクラスの取得
        _bullet.param = _param;
        _bullet.enemy = _transform.GetComponent<Enemy>();

        // f:レイヤー設定
        _bullet.gameObject.layer = LayerName.Default;
        foreach (Transform c_transform in _bullet.transform)
        {
            c_transform.gameObject.layer = LayerName.BulletEnemy;
        }

        // f:タグ設定

        // f:エリア設定
        GameManager.SetArea(_bullet.gameObject, _bullet.enemy.playerSlot);
    }
}
