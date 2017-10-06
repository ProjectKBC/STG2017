using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Held
{
	public sealed class HeldUSM : ShotManager
	{
		override public void init()
		{
			base.init();
			/*
             * 自動生成化したい
            bullet = new GameObject().AddComponent<HeldUB>();
            */
		}
	}
}
