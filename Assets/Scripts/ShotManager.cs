using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShotManager : MonoBehaviour
{
    public Bullet bullet;  // 弾のPrefab
    [SerializeField] public BulletParam param; // パラメータクラス
    public KeyCode keyCode;

    private void init()
    {
    }

    IEnumerator Start()
    {
        init();
        while (true)
        {

            yield return new WaitForSeconds(0.01f);
        }
    }

    // ZキーとXキーの同時押しを対処する入力仕分け関数
    private KeyCode activeKey = KeyCode.None;
    KeyCode keyInput()
    {
        if (Input.GetKeyDown(KeyCode.Z)) { activeKey = KeyCode.Z; }
        if (Input.GetKeyDown(KeyCode.X)) { activeKey = KeyCode.X; }

        if (Input.GetKeyUp(KeyCode.Z) && activeKey == KeyCode.Z) { activeKey = KeyCode.None; }
        if (Input.GetKeyUp(KeyCode.X) && activeKey == KeyCode.X) { activeKey = KeyCode.None; }

        if (Input.GetKey(KeyCode.Z) && (activeKey == KeyCode.Z || activeKey == KeyCode.None))
        {
            return KeyCode.Z;
        }
        if (Input.GetKey(KeyCode.X) && (activeKey == KeyCode.X || activeKey == KeyCode.None))
        {
            return KeyCode.X;
        }

        return KeyCode.None;
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
        if (Input.GetKeyDown(keyCode))
        {
            chargeBeginTime = Time.time;
        }

        // チャージ完了判定
        if (Time.time - chargeBeginTime >= param.chargeTime)
        {
            Debug.Log("can shot");
            canChargeShot = true;
        }

        // チャージ発射or解除判定
        if (Input.GetKeyUp(keyCode))
        {
            if (canChargeShot) { return true; }

            chargeBeginTime = 0;
            canChargeShot   = false;
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
                    timeElapsed = Time.time;
                    Bullet.Instantiate(bullet, param, transform.position, transform.rotation);
                }
            }
            // 通常ショット
            else
            {
                if (NormalInput())
                {
                    timeElapsed = Time.time;
                    Bullet.Instantiate(bullet, param, transform.position, transform.rotation);
                }
            }
        }
    }
}