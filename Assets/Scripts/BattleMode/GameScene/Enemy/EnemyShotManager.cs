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

        switch(param.shotMovePattern)
        {
            // 直進
            case ShotMovePattern.Straight:
                //InstBullet(Mathf.PI / 180 * 270);
                InstBullet();
                break;

                // 全方位
            case ShotMovePattern.EveryDirection:
                if(param.angleInterval > 0)
                {
                    for (float rad = 0; rad < 360; rad += param.angleInterval)
                    {
                        InstBullet(Mathf.PI / 180 * rad);
                    }
                }
                break;

                // 渦巻き状
            case ShotMovePattern.Tornado:
                InstBullet(Time.time * param.spinSpeed);
                break;

                // 自機狙い
            case ShotMovePattern.PlayerAim:
                float vx = GameManager.Pc1Player.transform.position.x - transform.position.x;
                float vy = GameManager.Pc1Player.transform.position.y - transform.position.y;
                InstBullet(vx, vy);
                break;
        }
     }

    protected void InstBullet()
    {
        lastShotTime = Time.time;
        if (param.shotSound != null) { audioSource.PlayOneShot(param.shotSound); }
        EnemyBullet b = EnemyBullet.Instantiate(bullet, param, transform);
    }

    // 主に角度を渡す
    protected void InstBullet(float _rad)
    {
        lastShotTime = Time.time;
        if (param.shotSound != null) { audioSource.PlayOneShot(param.shotSound); }
        EnemyBullet b = EnemyBullet.Instantiate(bullet, param, transform);
        b.transform.Rotate(new Vector2(Mathf.Cos(_rad), Mathf.Sin(_rad)));
    }

    // 主に座標を渡す
    protected void InstBullet(float _x, float _y)
    {
        lastShotTime = Time.time;
        if (param.shotSound != null) { audioSource.PlayOneShot(param.shotSound); }
        EnemyBullet b = EnemyBullet.Instantiate(bullet, param, transform);
        b.transform.Rotate(new Vector2(_x, _y).normalized);
    }
}
