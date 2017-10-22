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
    
    // 弾丸速度
    [SerializeField]
    protected float speed;
    public float Speed
    {
        get { return speed * 100; }
        set { speed = value; }
    }             

    public float power;             // 攻撃力
    public bool isPenetrate;        // 貫通性の有無

    public Vector3 initialPosition; // 自機を起点とした初期位置

    [System.NonSerialized] public PlayerSlot playerSlot;
}

public abstract class EnemyBullet : MonoBehaviour
{
    [System.NonSerialized] public EnemyBulletParam param;
    protected Enemy enemy;

    protected void Start()
    {
        Init();
        Move();

        // lifeTime秒後に削除
        Destroy(gameObject, param.lifeTime);
    }

    protected void Update()
    {
        // 子要素（弾）がすべてなくなったら削除
        if (transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }

        // デストロイですの
        if (transform.position.y < GameManager.destroyArea.w)
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
        GameManager.SetArea(obj.gameObject, PlayerSlot.PC1);
        //obj.gameObject.transform.parent = GameObject.Find(CanvasName.PC1AREA).transform;
        return obj;
    }
}
