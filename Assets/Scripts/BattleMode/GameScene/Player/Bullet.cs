﻿using System.Collections;
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
    public string name;         // ショットの名前
    public AudioClip shotSound; // ショット音

    // 共通パラメータ
    public float shotDelay;         // ショット間隔
    public float lifeTime;          // 生存時間

    [SerializeField]
    protected float speed;
    public float Speed
    {
        get { return speed * 100; }
        set { speed = value; }
    }

    public float power;             // 攻撃力
    public bool isPenetrate;        // 貫通性の有無
    public ShotMode shotMode;       // ショットの種類
    public Vector3 initialPosition; // 自機を起点とした初期位置
    public Gage gage;               // ゲージに関する設定項目

    // ChargeShot系パラメータ
    public float chargeTime; // チャージ時間
    
    // LimitShot系パラメータ
	public int   bulletMaxNum; // 弾数制限
	public float reloadTime;   // リロード時間

    [System.NonSerialized] public PlayerSlot playerSlot;
}

public abstract class Bullet : MonoBehaviour
{
    // パラメータ
    [System.NonSerialized] public BulletParam param;
    protected Player player;

    protected void Start()
    {
        Init();

        // lifeTime秒後に削除
        Destroy(gameObject, param.lifeTime);
    }

    protected void Update()
    {
        Move();

        // 子要素（弾）がなくなったら削除
        if (transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }

        if (transform.position.y > GameManager.destroyArea.y)
        {
            Destroy(this.gameObject);
        }
    }

    // 初期設定関数
    protected virtual void Init() { }

    // move関数：弾の動きはここに書く 
    protected virtual void Move() { }

    // 気にしなくていい（弾生成時にパラメータを渡すための関数）
    public static Bullet Instantiate(Bullet _bullet, BulletParam _param, Transform _transform)
    {
        Bullet obj = Instantiate(_bullet, _transform.position + _param.initialPosition, _transform.rotation) as Bullet;
        obj.param = _param;
        obj.player = _transform.GetComponent<Player>();
        obj.gameObject.layer = LayerName.Default;
        foreach (Transform childTF in obj.transform)
        {
            childTF.gameObject.layer = LayerName.BulletPlayer;
        }
        GameManager.SetArea(obj.gameObject, obj.player.playerSlot);
        return obj;
    }

    protected virtual void OnDestroy()
    {
        
    }
}
