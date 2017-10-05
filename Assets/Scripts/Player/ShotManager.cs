using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShotManager : MonoBehaviour
{
    public Bullet bullet;     // 弾のPrefab
    [SerializeField]
    public BulletParam param; // パラメータクラス

    public KeyCode keyCode;   // 発射ボタン
    private AudioSource audioSource;

    public virtual void init() { }

    IEnumerator Start()
    {
        init();
        audioSource = gameObject.GetComponent<AudioSource>();
        while (true)
        {

            yield return new WaitForSeconds(0.01f);
        }
    }

    private bool NormalInput()
    {
        return Input.GetKey(keyCode);
    }

    // チャージ関連を管理する関数 trueでショット発射
    private float chargeBeginTime = 0;
    private bool  canChargeShot = false;
    private bool ChargeInput()
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
            canChargeShot = true;
        }

        // チャージ発射判定
        if (Input.GetKeyUp(keyCode) && canChargeShot)
        {
            chargeBeginTime = 0;
            canChargeShot   = false;
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

    // ショット判定と弾の生成を行う関数
    private float timeElapsed = 0;
    public void shot()
    {
        if (Time.time - timeElapsed >= param.shotDelay)
        {
            // チャージショット
            if (param.isCharge)
            {
                if (ChargeInput())
                {
                    audioSource.PlayOneShot(param.shotSound);
                    timeElapsed = Time.time;
                    Bullet.Instantiate(bullet, param, transform.position, transform.rotation);
                }
            }
            // 通常ショット
            else
            {
                if (NormalInput())
                {
                    audioSource.PlayOneShot(param.shotSound);
                    timeElapsed = Time.time;
                    Bullet.Instantiate(bullet, param, transform.position, transform.rotation);
                }
            }
        }
    }
}