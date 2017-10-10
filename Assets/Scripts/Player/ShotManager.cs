using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShotManager : MonoBehaviour
{
    public Bullet bullet;     // 弾のPrefab
    public BulletParam param; // パラメータクラス
    public KeyCode keyCode;   // 発射ボタン

    private string PlayerState;
    private AudioSource audioSource;

    public virtual void Init()
	{
		bulletNum = param.bulletMaxNum;
		audioSource = gameObject.GetComponent<AudioSource>();
	}

    IEnumerator Start()
    {
        Init();
        while (true)
        {

            yield return new WaitForSeconds(0.01f);
        }
    }
    
    // ショット判定と弾の生成を行う関数
    private float lastShotTime = 0;
    public void Shot()
    {
        if (param.isBulletLimit && param.isCharge)
        {
            Debug.Log("弾数制限とチャージショットは両立できません");
            return;
        }

        if (param.isBulletLimit)
        {
            ReloadShot();
            Debug.Log(bulletNum);
            return;
        }

        if (param.isCharge)
        {
            ChargeShot();
            return;
        }

        SimpleShot();
    }

    private float lastReloadTime = 0;
    private int   bulletNum = 0;
    private bool ReloadShot()
    {
        if (param.isBulletLimit == false) { return false; }

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
    
    private float chargeBeginTime = 0;
    private bool  canChargeShot = false;
    private bool ChargeShot()
    {
        if (param.isCharge == false) { return false; }

        // チャージ開始判定
        if (chargeBeginTime == 0)
        {
            chargeBeginTime = Time.time;
        }

        // チャージ完了判定
        if (Time.time - chargeBeginTime >= param.chargeTime)
        {
            Debug.Log("can shot");
            canChargeShot = true;
        }

        // チャージ発射判定
        if (Input.GetKeyUp(keyCode) && canChargeShot)
        {
            chargeBeginTime = 0;
            canChargeShot = false;
            return true;
        }

        // チャージ解除判定
        if (Input.GetKey(keyCode) == false)
        {
            chargeBeginTime = 0;
            canChargeShot = false;
        }

        return false;
    }

    private bool SimpleShot()
    {
        if (Time.time - lastShotTime < param.shotDelay) { return false; }

        InstBullet();
        return true;
    }

    private void InstBullet()
    {
        lastShotTime = Time.time;
        audioSource.PlayOneShot(param.shotSound);
        Bullet.Instantiate(bullet, param, transform.position, transform.rotation);
    }
}