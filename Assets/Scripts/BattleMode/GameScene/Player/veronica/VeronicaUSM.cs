﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Veronica
{
    public sealed class VeronicaUSM : ShotManager
    {
        override public void Init()
        {
			base.Init();
            /*
             * 自動生成化したい
            bullet = new GameObject().AddComponent<VeronicaUB>();
            */
        }
    }
}
 