using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Veronica
{
    public sealed class VeronicaUB : Bullet
    {

        override protected void Init()
        {
            base.Init();
            player.isStan = true;
        }

        override protected void Move()
        {
            base.Move();
            transform.position = player.transform.position;
        }

        override protected void OnDestroy()
        {
            base.OnDestroy();
            player.isStan = false;
        }
    }
}
