using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotMode
{
    SimpleShot,
    ChargeShot,
    LimitShot,
}

[System.Serializable]
public class BulletParam
{
    public string name;         // f:ショットの名前
    public AudioClip shotSound; // f:ショット音

    // f:共通パラメータ
    public float shotDelay; // f:ショット間隔
    public float lifeTime;  // f:生存時間

    // f:移動速度 Speedを使うことを推奨
    public float speed;
    public float Speed
    {
        get { return speed * 1.5f; }
        set { speed = value; }
    }

    public float power;                // f:攻撃力
    public bool isPenetrate;           // f:貫通性の有無
    public ShotMode shotMode;          // f:ショットの種類
    public Vector3 initialPosition;    // f:自機を起点とした初期位置
    public Quaternion initialRotation; // f:自機を起点とした初期位置
    public Gage gage;                  // f:ゲージに関する設定項目

    // f:ChargeShot系パラメータ
    public float chargeTime;   // f:チャージ時間
    public float rechargeTime; // f:ショットを放ってから次にチャージ可能になるまでの時間
    
    // f:LimitShot系パラメータ
	public int   bulletMaxNum; // f:弾数制限
	public float reloadTime;   // f:リロード時間

    [System.NonSerialized] public PlayerSlot playerSlot;
}

public abstract class Bullet : NoaBehaviour
{
    [System.NonSerialized] public BulletParam param;
    protected Player player;

    // f: lifeTimeをpause機能に対応させる
    protected float bornTime = 0;
    protected float pausingTime = 0;

    protected override IEnumerator Start()
    {
        yield return new WaitWhile( () => player.MyProc.IsStay());

        Init();
        MyProc.started = true;

        yield return new WaitWhile(() => NoaProcesser.IsStayBoss() | NoaProcesser.IsStayPC(player.playerSlot));
    }

    protected void Update()
    {
        float tmpTime = 0;
        if (NoaProcesser.BossProc.pausing) { tmpTime = Time.time; }

        if (MyProc.IsStay() || NoaProcesser.IsStayBoss() || NoaProcesser.IsStayPC(param.playerSlot)) { return; }

        if (tmpTime != 0) { pausingTime += Time.time - tmpTime; }

        Move();
        
        // f:時間経過で削除
        if (Time.time - (bornTime + pausingTime) >= param.lifeTime)
        {
            Destroy(gameObject);
        }

        // f:範囲外判定
        foreach (Transform x in GetComponentInChildren<Transform>())
        {
            if (GameManager.OutOfArea(x.position, player.playerSlot))
            {
                Destroy(x.gameObject);
            }
        }

        // f:子要素（弾）がなくなったら削除
        if (transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }
    }

    // f:初期設定関数
    protected virtual void Init()
    {
        bornTime = Time.time;
    }

    // f: 
    protected virtual void Move() { }

    // f:弾生成時にパラメータを渡すことができるInstantiate関数
    public static Bullet Instantiate(Bullet _bullet, BulletParam _param, Transform _transform)
    {
        Bullet bullet = Instantiate(_bullet, _transform.position + _param.initialPosition, _transform.rotation) as Bullet;
        SetupBullet(bullet, _param, _transform);
        return bullet;
    }

    protected static void SetupBullet(Bullet _bullet, BulletParam _param, Transform _transform)
    {
        // f:パラメータとPlayerクラスの取得
        _bullet.param  = _param;
        _bullet.player = _transform.GetComponent<Player>();

        // f:レイヤー設定
        _bullet.gameObject.layer = LayerName.Default;
        foreach (Transform c_transform in _bullet.transform)
        {
            c_transform.gameObject.layer = LayerName.BulletPlayer;
        }

        // f:タグ設定

        // f:エリア設定
        GameManager.SetArea(_bullet.gameObject, _bullet.player.playerSlot);
    }

    protected virtual void OnDestroy()
    {
        
    }
}
