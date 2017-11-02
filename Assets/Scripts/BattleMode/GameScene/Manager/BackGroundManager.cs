using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BackGroundManager : NoaBehaviour
{
    private static BackGroundManager inst;
    private BackGroundManager() { }
    public static BackGroundManager Inst
    {
        get
        {
            if (inst == null)
            {
                GameObject go = new GameObject("BackGroundManager");
                go.transform.parent = GameObject.Find("Managers").transform;
                inst = go.AddComponent<BackGroundManager>();
            }

            return inst;
        }
    }

    protected override IEnumerator Start()
    {
        yield return new WaitUntil(() => PlayerManager.Inst.MyProc.started && SoundManager.Inst.MyProc.started);
        Debug.Log("_5:BackGroundManagerが呼び出される。");
        Instantiate(Resources.Load("Prefabs/UI/PC1BackGround"), GameObject.Find(CanvasName.UI).transform).name = "PC1BackGround";
        Instantiate(Resources.Load("Prefabs/UI/PC2BackGround"), GameObject.Find(CanvasName.UI).transform).name = "PC2BackGround";

        Debug.Log("6:BackGroundManagerがステージ背景を生成する。");
        MyProc.started = true;

        yield return new WaitUntil( () => NoaProcesser.BossProc.started);
        Destroy(gameObject);
    }
}
