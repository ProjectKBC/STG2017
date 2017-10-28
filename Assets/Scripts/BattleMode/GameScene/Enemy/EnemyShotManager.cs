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
        switch(param.shotMovepattern)
        {
            case ShotMovepattern.EveryDirection:
                for(float rad = 0; rad < 360; rad += param.angleInterval)
                {
                    InstBullet(rad);
                }
                break;
            default:
                InstBullet();
                break;
        }
     }

    protected void InstBullet()
    {
        lastShotTime = Time.time;
        if (param.shotSound != null) { audioSource.PlayOneShot(param.shotSound); }
        EnemyBullet b = EnemyBullet.Instantiate(bullet, param, transform);
    }

    protected void InstBullet(float _rad)
    {
        lastShotTime = Time.time;
        if (param.shotSound != null) { audioSource.PlayOneShot(param.shotSound); }
        EnemyBullet b = EnemyBullet.Instantiate(bullet, param, transform);
        b.transform.Rotate(new Vector2(Mathf.Cos(Mathf.PI / 180 * _rad), Mathf.Sin(Mathf.PI / 180 * _rad)));
    }
}
