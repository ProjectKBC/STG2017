using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyShotManager : NoaBehaviour
{
    public EnemyBullet bullet;
    [SerializeField] public EnemyBulletParam param;

    protected Enemy enemy;
    protected AudioSource audioSource;

    protected float lastShotTime = 0;
    PlayerSlot playerSlot;

    public virtual void Init()
    {
        enemy = GetComponent<Enemy>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override IEnumerator Start()
    {

        Init();
        MyProc.started = true;

        yield return enemy.MyProc.Stay();
        yield return NoaProcesser.StayBoss();

        while (true)
        {
            if (NoaProcesser.IsStayBoss()) { yield return null; }
            
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

            case ShotMovepattern.PlayerAim:
                break;
        }
=======

        InstBullet();
>>>>>>> 2f2adb1464b450d5d5af3691a44b0a0aeaf49299
     }

    protected void InstBullet()
    {
        lastShotTime = Time.time;
        if (param.shotSound != null) { audioSource.PlayOneShot(param.shotSound); }
        EnemyBullet b = EnemyBullet.Instantiate(bullet, param, transform);
    }
}
