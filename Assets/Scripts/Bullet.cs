using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BulletParam
{
    public float shotDelay; // ショット間隔
    public float lifeTime;  // 生存時間
    public float speed;     // 弾丸速度
    public float power;     // 攻撃力
    public float chargeTime;// チャージ時間
}

public abstract class Bullet : MonoBehaviour
{
    BulletParam param; // パラメータ構造体

    void Start()
    {
        move();

        // lifeTime秒後に削除
        Destroy(gameObject, param.lifeTime);
    }

    void Update()
    {
        // 子要素（弾）がなくなったら削除
        if (transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }
    }

    // move関数：弾の動きはここに書く 
    private void move()
    {
        // 真っすぐ進む
        GetComponent<Rigidbody2D>().velocity = transform.up.normalized * param.speed;
    }

    // 気にしなくていい（弾生成時にパラメータを渡すための関数）
    public static Bullet Instantiate(Bullet _bullet, BulletParam _param, Vector3 _position, Quaternion _rotation)
    {
        Bullet obj = Instantiate(_bullet, _position, _rotation) as Bullet;
        obj.param = _param;
        return obj;
    }
}
