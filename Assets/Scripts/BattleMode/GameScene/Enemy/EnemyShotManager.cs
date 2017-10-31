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
    int count = 0;
    bool delaySwitch = false;
    float player1posX = GameManager.Pc1Player.transform.position.x;
    float player1posY = GameManager.Pc1Player.transform.position.y;
    float player2posX = GameManager.Pc2Player.transform.position.x;
    float player2posY = GameManager.Pc2Player.transform.position.y;

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
        if(delaySwitch)
        {
            if (Time.time - lastShotTime < param.shotDelay2) { return; }
        }
        else if (Time.time - lastShotTime < param.shotDelay) { return; }

        switch(param.shotMovePattern)
        {
            // 直進
            case ShotMovePattern.Straight:
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
                float vx = 0;
                float vy = 0;

                // 追加ディレイがある場合はグループで処理
                if(param.delayShotCount != 0)
                {
                    if(count == 0)
                    {
                        player1posX = GameManager.Pc1Player.transform.position.x;
                        player1posY = GameManager.Pc1Player.transform.position.y;
                        player2posX = GameManager.Pc2Player.transform.position.x;
                        player2posY = GameManager.Pc2Player.transform.position.y;
                    }
                }

                else
                {
                    player1posX = GameManager.Pc1Player.transform.position.x;
                    player1posY = GameManager.Pc1Player.transform.position.y;
                    player2posX = GameManager.Pc2Player.transform.position.x;
                    player2posY = GameManager.Pc2Player.transform.position.y;
                }

                switch(enemy.playerSlot)
                {
                    case PlayerSlot.PC1:
                        vx = player1posX - transform.position.x;
                        vy = player1posY - transform.position.y;
                        break;

                    case PlayerSlot.PC2:
                        vx = player2posX - transform.position.x;
                        vy = player2posY - transform.position.y;
                        break;
                }
                InstBullet(vx, vy);
                break;
        }

        count++;
        delaySwitch = false;
        if (count == param.delayShotCount)
        {
            count = 0;
            delaySwitch = true;
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
