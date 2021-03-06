﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonCode
{
    Shot1,
    Shot2,
    Shot3,
    Skill,
    Slow,
    Pause,
    Submit,
    Cancel,
}

public abstract class ShotManager : NoaBehaviour
{
    public Bullet bullet;         // 弾のPrefab
    public BulletParam param;     // パラメータクラス
    public ButtonCode buttonCode; // 発射ボタン

    protected Player player;
    [System.NonSerialized] public bool Started = false;

    protected float lastShotTime = 0;

    public virtual void Init()
    {
        player = GetComponent<Player>();
        bulletNum = param.bulletMaxNum;
        CanChargeShot = false;
    }

    protected override IEnumerator Start()
    {
        Init();
        MyProc.started = true;

        yield return player.MyProc.Stay();
        yield return NoaProcesser.StayBoss();
    }
    
    public string GetButtonCode()
    {
        switch (buttonCode)
        {
            case ButtonCode.Shot1: return ButtonCode.Shot1.ToString();
            case ButtonCode.Shot2: return ButtonCode.Shot2.ToString();
            case ButtonCode.Shot3: return ButtonCode.Shot3.ToString();

            default: return null;
        }
    }

    // ショット判定と弾の生成を行う関数
    public void Shot()
    {
        switch (param.shotMode)
        {
            case ShotMode.SimpleShot: SimpleShot(); break;

            case ShotMode.ChargeShot: ChargeShot(); break;

            case ShotMode.LimitShot:  LimitShot(); break;
        }
    }

    protected bool SimpleShot()
    {
        if (Time.time - lastShotTime < param.shotDelay) { return false; }

        InstBullet();
        return true;
    }

    public float chargeBeginTime = 0;
    public bool CanChargeShot { get; private set; }
    protected bool ChargeShot()
    {
        // チャージ開始判定
        if (chargeBeginTime == 0)
        {
            chargeBeginTime = Time.time;
        }

        // チャージ完了判定
        if (Time.time - chargeBeginTime >= param.chargeTime)
        {
            Debug.Log("can shot");
            CanChargeShot = true;
        }

        // チャージ発射判定
        if (player.state == param.name + "(KeyUp)" && CanChargeShot)
        {
            InstBullet();
            chargeBeginTime = 0;
            CanChargeShot = false;
            return true;
        }

        return false;
    }

    public float lastReloadTime = 0;
    public int bulletNum = 0;
    protected bool LimitShot()
    {
        // 残弾がなく、リロードタイムを過ぎた場合 -> リロードする
        if (Time.time - lastReloadTime >= param.reloadTime && bulletNum <= 0)
        {
            bulletNum = param.bulletMaxNum;
        }

        // 残弾がない場合のシールド
        if (bulletNum <= 0) { return false; }

        // ショット間隔をあけるためのシールド
        if (Time.time - lastShotTime < param.shotDelay) { return false; }

        // 弾を撃つ
        InstBullet();
        bulletNum--;

        // 残弾０以下の場合 -> リロード開始
        if (bulletNum <= 0)
        {
            lastReloadTime = Time.time;
        }

        return true;
    }

    protected void InstBullet()
    {
        lastShotTime = Time.time;
        SoundManager.PlayOneShot(param.shotSound);
        Bullet b = Bullet.Instantiate(bullet, param, transform);
    }


    public void Maintenance(string _playerState)
    {
        switch (param.shotMode)
        {
            case ShotMode.SimpleShot: SimpleMaintenance(); return;

            case ShotMode.ChargeShot: ChargeMaintenance(); return;

            case ShotMode.LimitShot: LimitMaintenance(); return;
        }
    }

    protected void SimpleMaintenance()
    {
    }

    protected void ChargeMaintenance()
    {
        chargeBeginTime = 0;
        CanChargeShot = false;
    }

    protected void LimitMaintenance()
    {
        // 残弾がなく、リロードタイムを過ぎた場合 -> リロードする
        if (Time.time - lastReloadTime >= param.reloadTime && bulletNum <= 0)
        {
            bulletNum = param.bulletMaxNum;
        }
    }
}