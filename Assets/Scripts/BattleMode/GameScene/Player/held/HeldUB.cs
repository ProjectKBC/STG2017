using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Held
{
	public sealed class HeldUB : Bullet
	{
		
		override public void Init()
		{
            base.Init();
		}

		override public void Move()
		{
			// 真っすぐ進む
			GetComponent<Rigidbody2D>().velocity = transform.up.normalized * param.speed;
		}
	}
}
