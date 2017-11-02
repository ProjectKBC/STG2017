using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlimitedLostWorks : EnemyBullet
{
    public UnlimitedLostWorksChild bullet;
    private float startTime = 0;

    protected override IEnumerator Start()
    {
        base.Start();
        startTime = Time.time; Debug.Log(startTime);
        MyProc.started = true;

        if (MyProc.IsStay()) yield return null;
    }

    protected void Update()
    {
        if (MyProc.IsStay() || NoaProcesser.IsStayBoss()) { return; }

        if (enemy.MyProc.started == false) { return; }

        Move();

        // f:範囲外判定
        foreach (Transform x in GetComponentInChildren<Transform>())
        {
            if (GameManager.OutOfArea(x.position, enemy.playerSlot))
            {
                Destroy(x.gameObject);
            }
        }

        // f:子要素（弾）がすべてなくなったら削除
        if (transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }
    }

    public override void Move()
    {
        base.Move();

        if (Time.time - startTime >= 1)
        {
            Debug.Log("unlimited......");
            UnlimitedLostWorksChild b;
            b = EnemyBullet.Instantiate(bullet, this.param, transform) as UnlimitedLostWorksChild;
            b.transform.Rotate(new Vector3(0 ,0, 1), 210);

            b = EnemyBullet.Instantiate(bullet, this.param, transform) as UnlimitedLostWorksChild;
            b.transform.Rotate(new Vector3(0, 0, 1), 180);

            b = EnemyBullet.Instantiate(bullet, this.param, transform) as UnlimitedLostWorksChild;
            b.transform.Rotate(new Vector3(0, 0, 1), 150);

            Destroy(this.gameObject);
        }
    }
}