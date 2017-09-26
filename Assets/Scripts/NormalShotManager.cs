using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalShotManager : MonoBehaviour
{
    public Bullet bullet;

    public float shotDelay; // ショット間隔
    public float lifeTime;  // 生存時間
    public float speed;     // 弾丸速度
    public float power;     // 攻撃力
    public float chargeTime;// チャージ時間

    private BulletParam bp;
    
    private float timeElapsed = 0;

    void Start()
    {
        bp.shotDelay = shotDelay;
        bp.lifeTime = lifeTime;
        bp.speed = speed;
        bp.power = power;
        bp.chargeTime = chargeTime;
    }

    public void main()
    {

    }

    public void NormalInput()
    {

    }

    private float chargeBeginTime;
    private bool chargeBeginFlg = false;
    private bool chargeShotFlg  = false;
    // チャージショットが撃てる状態でtrueを返す。
    public bool ChargeInput(Transform origin)
    {
        // チャージ未開始
        if (Input.GetKeyDown(KeyCode.Z) && chargeBeginFlg == false)
        {
            chargeBeginFlg  = true;
            chargeBeginTime = Time.time;
            return false;
        }

        // チャージ中
        if (Input.GetKey(KeyCode.Z) && chargeShotFlg == false)
        {
            if (Time.time - chargeBeginTime >= chargeTime)
            {
                chargeShotFlg = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            if (chargeShotFlg)
            {
                chargeBeginTime = 0;
                chargeBeginFlg = false;
                chargeShotFlg = false;

                return true;
            }

            chargeBeginTime = 0;
            chargeBeginFlg = false;
        }

        return false;
    }

    public void shot(Transform origin)
    {
        if (Time.time - timeElapsed >= bp.shotDelay)
        {
            timeElapsed = Time.time;

            Bullet b = Bullet.Instantiate(bullet, bp, origin.position, origin.rotation);
            
            Debug.Log("nsm.shot");
        }
    }
}
