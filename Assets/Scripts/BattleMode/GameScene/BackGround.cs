using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BackGround : NoaBehaviour
{
    public float ScrollSpeed = 2f;
    public PlayerSlot playerSlot;
    private MeshRenderer mr;

    float offset = 0f;
    
    protected override IEnumerator Start()
    {
        mr = GetComponent<MeshRenderer>();
        MyProc.started = true;

        yield return new WaitWhile( () => NoaProcesser.IsStayBoss() || NoaProcesser.IsStayPC(playerSlot));
    }

    private void Update()
    {
        if (MyProc.IsStay() || NoaProcesser.IsStayBoss() || NoaProcesser.IsStayPC(playerSlot)) { return; }

        offset = Mathf.Repeat(Time.time * ScrollSpeed, 1f);
        mr.material.SetTextureOffset("_MainTex", new Vector2(0f, offset));
    }
}

