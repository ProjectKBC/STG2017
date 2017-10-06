using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Veronica
{
    public sealed class VeronicaNSM : ShotManager
    {
        override public void init()
        {
			base.init();
            /*
             * 自動生成化したい
            bullet = new GameObject().AddComponent<VeronicaNB>();
            */
        }
    }
}