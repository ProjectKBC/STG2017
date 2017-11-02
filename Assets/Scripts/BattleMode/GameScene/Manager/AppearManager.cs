using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AppearManager : NoaBehaviour
{
    private static AppearManager inst;
    private AppearManager() { }
    public static AppearManager Inst
    {
        get
        {
            if (inst == null)
            {
                GameObject go = new GameObject("AppearManager");
                go.transform.parent = GameObject.Find("Managers").transform;
                inst = go.AddComponent<AppearManager>();
            }

            return inst;
        }
    }

    protected override IEnumerator Start()
    {
        yield return new WaitUntil(() => PlayerManager.Inst.MyProc.started && SoundManager.Inst.MyProc.started);
        Debug.Log("_3:AppearManagerが呼び出される。");
        Instantiate(Resources.Load("Prefabs/PC1Emitter"), GameObject.Find("PC1Area/Canvas").transform).name = "target_pause";
        Instantiate(Resources.Load("Prefabs/PC2Emitter"), GameObject.Find("PC2Area/Canvas").transform).name = "target_pause";

        Debug.Log("4:AppearManagerがAppearを生成する。");
        MyProc.started = true;

        yield return new WaitUntil(() => NoaProcesser.BossProc.started);
        Destroy(gameObject);
    }
}
