using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 特殊ショット用抽象クラス
public class UniqueShotManager : MonoBehaviour
{
    public Bullet bullet;
    private BulletParam bp;

    // 入力用の変数 (script側で入力するならいらない)
    public float shotDelay; // ショット間隔
    public float lifeTime;  // 生存時間
    public float speed;     // 弾丸速度
    public float power;     // 攻撃力

    private float timeElapsed = 0;

    void Start()
    {
        bp.shotDelay = shotDelay;
        bp.lifeTime  = lifeTime;
        bp.speed     = speed;
        bp.power     = power;
    }

    public void shot(Transform origin)
    {
        if (Time.time - timeElapsed >= bp.shotDelay)
        {
            timeElapsed = Time.time;

            Bullet.Instantiate(bullet, bp, origin.position, origin.rotation);
            Debug.Log("usm.shot");
        }
    }
}
