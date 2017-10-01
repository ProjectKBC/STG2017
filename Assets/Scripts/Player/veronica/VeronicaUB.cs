using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeronicaUB : Bullet
{

    override public void init()
    {
        /*
        param.name       = "";    // ショットの名前
        param.shotDelay  = 0.0f;  // ショット間隔
        param.lifeTime   = 0.0f;  // 生存時間
        param.speed      = 0.0f;  // 弾丸速度
        param.power      = 0.0f;  // 攻撃力
        param.isCharge   = false; // チャージ有無
        param.chargeTime = 0.0f;  // チャージ時間
        param.shotSound  = null;  // ショット音
        */
    }

    override public void move()
    {
        // 真っすぐ進む
        GetComponent<Rigidbody2D>().velocity = transform.up.normalized * param.speed;
    }
}
