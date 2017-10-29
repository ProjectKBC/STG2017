<<<<<<< HEAD
﻿using System.Collections;
=======
﻿using System;
using System.Collections;
>>>>>>> test
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
<<<<<<< HEAD
public class BackGround : MonoBehaviour
=======
public class BackGround : NoaBehaviour
>>>>>>> test
{
    public float ScrollSpeed = 2f;
    private MeshRenderer mr;

    float offset = 0f;
<<<<<<< HEAD

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    void Update()
    {
=======
    
    protected override IEnumerator Start()
    {
        mr = GetComponent<MeshRenderer>();
        MyProc.started = true;

        yield return NoaProcesser.StayBoss();
    }

    private void Update()
    {
        if (MyProc.IsStay() || NoaProcesser.IsStayBoss()) { return; }

>>>>>>> test
        offset = Mathf.Repeat(Time.time * ScrollSpeed, 1f);
        mr.material.SetTextureOffset("_MainTex", new Vector2(0f, offset));
    }
}

