using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anoma
{
    public sealed class AnomaNB : Bullet
    {
        override protected void Init()
        {
            base.Init();
        }

        override protected void Move()
        {
            base.Move();
            Vector2 pos = transform.position;
            pos.y += param.Speed;
            transform.position = pos;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
