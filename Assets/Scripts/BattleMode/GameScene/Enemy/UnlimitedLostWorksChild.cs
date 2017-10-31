using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlimitedLostWorksChild : EnemyBullet
{
    public override void Move()
    {
        Vector3 pos = transform.position;
        Quaternion angle = transform.rotation;

        Vector3 direction = new Vector2(angle.x, angle.y).normalized;

        pos += direction * param.Speed * Time.deltaTime;
        transform.position = pos;
    }
}
