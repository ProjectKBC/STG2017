using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Held
{
	public sealed class HeldNSM : ShotManager
	{
		override public void Init()
		{
			base.Init();
			/*
             * 自動生成化したい
            bullet = new GameObject().AddComponent<HeldNB>();
            */
		}
	}
}