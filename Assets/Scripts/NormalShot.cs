using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NormalShot : MonoBehaviour
{
    public int speed;
    public int power;

    // ゲームオブジェクト生成から削除するまでの時間
    public float lifeTime;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up.normalized * speed;
        Destroy(gameObject, lifeTime);
    }
    
    public void Shot(Transform origin)
    {
        Instantiate(this, origin.position, origin.rotation);
        GetComponent<AudioSource>().Play();
    }
}
