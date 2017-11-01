using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Held
{
	public sealed class HeldUB : Bullet
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
