using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyBulletParam
{
    public string name;         // ショットの名前
    public AudioClip shotSound; // ショット音

    // 共通パラメータ
    public float shotDelay;         // ショット間隔
    public float lifeTime;          // 生存時間
    public float speed;             // 弾丸速度
    public float power;             // 攻撃力
    public bool isPenetrate;        // 貫通性の有無

    public Vector3 initialPosition; // 自機を起点とした初期位置

}

public class EnemyBullet : MonoBehaviour
{
    [System.NonSerialized] public EnemyBulletParam param;
    private Enemy enemy;

    private void Start()
    {
        Init();
        Move();

        // lifeTime秒後に削除
        Destroy(gameObject, param.lifeTime);
    }

    private void Update()
    {
        // 子要素（弾）がすべてなくなったら削除
        if (transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }

        // デストロイですの
        if (transform.position.y < -10)
        {
            Destroy(this.gameObject);
        }

        // 
    }

    // 初期設定関数
    public virtual void Init()
    {
    }

    // move関数：弾の動きはここに書く 
    public virtual void Move()
    {
        // 真っすぐ進む
        GetComponent<Rigidbody2D>().velocity = transform.up.normalized * param.speed * -1;
    }

    // 弾生成時にパラメータを渡せるInstantiate関数
    public static EnemyBullet Instantiate(EnemyBullet _bullet, EnemyBulletParam _param, Transform _transform)
    {
        EnemyBullet obj = Instantiate(_bullet, _transform.position + _param.initialPosition, _transform.rotation) as EnemyBullet;
        obj.param = _param;
        obj.enemy = _transform.GetComponent<Enemy>();
        obj.gameObject.layer = LayerName.Default;
        foreach (Transform childTF in obj.transform)
        {
            childTF.gameObject.layer = LayerName.BulletEnemy;
        }
        return obj;
    }
}
