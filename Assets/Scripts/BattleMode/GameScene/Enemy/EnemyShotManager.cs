<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyShotManager : MonoBehaviour
=======
﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyShotManager : NoaBehaviour
>>>>>>> test
{
    public EnemyBullet bullet;
    [SerializeField] public EnemyBulletParam param;

<<<<<<< HEAD
    [System.NonSerialized] public bool Started = false;

=======
>>>>>>> test
    protected Enemy enemy;
    protected AudioSource audioSource;

    protected float lastShotTime = 0;
<<<<<<< HEAD
=======
    PlayerSlot playerSlot;
>>>>>>> test

    public virtual void Init()
    {
        enemy = GetComponent<Enemy>();
        audioSource = GetComponent<AudioSource>();
<<<<<<< HEAD
        Started = true;
    }

    protected IEnumerator Start()
    {
        Init();
        
        while (true)
        {
            Shot();
=======
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
            
>>>>>>> test
            yield return new WaitForSeconds(0.01f);
        }
    }
        
    public virtual void Shot()
    {
<<<<<<< HEAD
        if (Time.time - lastShotTime < param.shotDelay) { return; }

        InstBullet();
=======
        
        if (Time.time - lastShotTime < param.shotDelay) { return; }

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
>>>>>>> test
     }

    protected void InstBullet()
    {
        lastShotTime = Time.time;
<<<<<<< HEAD
        audioSource.PlayOneShot(param.shotSound);
        EnemyBullet b = EnemyBullet.Instantiate(bullet, param, transform);
=======
        if (param.shotSound != null) { audioSource.PlayOneShot(param.shotSound); }
        EnemyBullet b = EnemyBullet.Instantiate(bullet, param, transform);
    }

    protected void InstBullet(float _rad)
    {
        lastShotTime = Time.time;
        if (param.shotSound != null) { audioSource.PlayOneShot(param.shotSound); }
        EnemyBullet b = EnemyBullet.Instantiate(bullet, param, transform);
        b.transform.Rotate(new Vector2(Mathf.Cos(_rad), Mathf.Sin(_rad)));
>>>>>>> test
    }
}
