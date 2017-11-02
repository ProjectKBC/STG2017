using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PauseManager : NoaBehaviour
{
    private static PauseManager inst;
    private PauseManager() { }
    public static PauseManager Inst
    {
        get
        {
            if (inst == null)
            {
                GameObject go = new GameObject("PauseManager");
                go.transform.parent = GameObject.Find("Managers").transform;
                inst = go.AddComponent<PauseManager>();
            }

            return inst;
        }
    }

    protected override IEnumerator Start()
    {
        yield return new WaitUntil(() => PlayerManager.Inst.MyProc.started && SoundManager.Inst.MyProc.started);
        Debug.Log("_5:PauseManagerが呼び出される。");
        Instantiate(Resources.Load("Prefabs/target_pause"), GameObject.Find("PausingCanvas").transform).name = "target_pause";

        Debug.Log("6:PauseManagerがtarget_pauseを生成する。");
        MyProc.started = true;

        yield return new WaitUntil(() => NoaProcesser.BossProc.started);
        Destroy(gameObject);
    }
}
