<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public KeyCode keyCode;

=======
﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : NoaBehaviour
{
    public KeyCode keyCode;


    protected override IEnumerator Start()
    {
        yield return NoaProcesser.StayBoss();
        MyProc.started = true;
    }

    protected void Update()
    {
        if (MyProc.IsStay() || NoaProcesser.IsStayBoss()) { return; }
    }

>>>>>>> test
    public void shot()
    {

    }
}
