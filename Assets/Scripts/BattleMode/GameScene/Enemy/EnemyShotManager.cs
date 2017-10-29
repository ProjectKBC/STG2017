using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyShotManager : MonoBehaviour
{
    public EnemyBullet bullet;
    [SerializeField] public EnemyBulletParam param;

    [System.NonSerialized] public bool Started = false;

    protected Enemy enemy;
    protected AudioSource audioSource;

    protected float lastShotTime = 0;

    public virtual void Init()
    {
        enemy = GetComponent<Enemy>();
        audioSource = GetComponent<AudioSource>();
        Started = true;
    }

    protected IEnumerator Start()
    {
        Init();
        
        while (true)
        {
            Shot();
            yield return new WaitForSeconds(0.01f);
        }
    }
        
    public virtual void Shot()
    {
        if (Time.time - lastShotTime < param.shotDelay) { return; }

<<<<<<< HEAD
        switch(param.shotMovepattern)
        {
            // 直進
            case ShotMovepattern.Straight:
                InstBullet(Mathf.PI / 180 * 270);
                break;

                // 全方位
            case ShotMovepattern.EveryDirection:
                for(float rad = 0; rad < 360; rad += param.angleInterval)
                {
                    InstBullet(Mathf.PI / 180 * rad);
                }
                break;

                // 渦巻き状
            case ShotMovepattern.Tornado:
                InstBullet(Time.time * param.spinSpeed);
                break;
        }
=======
        InstBullet();
>>>>>>> test
     }

    protected void InstBullet()
    {
        lastShotTime = Time.time;
        audioSource.PlayOneShot(param.shotSound);
        EnemyBullet b = EnemyBullet.Instantiate(bullet, param, transform);
    }
}
