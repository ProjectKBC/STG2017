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

        InstBullet();
        ManageScroll.Log("shot " + gameObject.name, enemy.playerSlot);
    }

    protected void InstBullet()
    {
        lastShotTime = Time.time;
        audioSource.PlayOneShot(param.shotSound);
        EnemyBullet b = EnemyBullet.Instantiate(bullet, param, transform);
        ManageScroll.Log(gameObject, b.param.playerSlot);
    }
}
