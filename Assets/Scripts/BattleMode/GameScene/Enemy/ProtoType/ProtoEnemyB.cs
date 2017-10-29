using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoEnemyB : EnemyBullet
{
    override public void Init()
    {
        base.Init();
    }

    override public void Move()
    {
        base.Move();
<<<<<<< HEAD
        GetComponent<Rigidbody2D>().velocity = transform.up.normalized * param.Speed * -1;
=======
>>>>>>> test
    }
}
