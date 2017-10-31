using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : NoaBehaviour
{
    public static Loading Inst; 
    public float minLoadingTime;
    private float startTime;

    protected override IEnumerator Start()
    {
        Inst = this;
        startTime = Time.time;
        MyProc.started = true;

        yield return null;
    }

    private void Update()
    {
        if (GameManager.Inst.MyProc.started && (Time.time - startTime) >= minLoadingTime)
        {
            MyProc.ended = true;
        }

        if (NoaProcesser.BossProc.started)
        {
            Destroy(this.gameObject);
        }
    }
}
