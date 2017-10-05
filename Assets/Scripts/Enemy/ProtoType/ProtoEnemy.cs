using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoEnemy : Enemy
{
    public override void move()
    {
        Vector2 direction = new Vector2(0, -1).normalized;
        Vector2 pos = transform.position;
        pos += direction * speed * Time.deltaTime;

        transform.position = pos;
    }

    public override void shot()
    {
        
    }
}
