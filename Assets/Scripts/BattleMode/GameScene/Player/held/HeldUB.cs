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

            // 真っすぐ進む
            GetComponent<Rigidbody2D>().velocity = transform.up.normalized * param.Speed;
        }

        override protected void Move()
        {
            base.Move();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
