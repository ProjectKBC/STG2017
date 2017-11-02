using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingManager : NoaBehaviour
{
    private static LoadingManager inst;
    private LoadingManager() { }
    public static LoadingManager Inst
    {
        get
        {
            if (inst == null)
            {
                GameObject go = new GameObject("LoadingManager");
                go.transform.parent = GameObject.Find("Managers").transform;
                inst = go.AddComponent<LoadingManager>();
            }

            return inst;
        }
    }

    protected override IEnumerator Start()
    {
        Debug.Log("_1:LoadingManagerが呼び出される。");
        Instantiate(Resources.Load("Prefabs/target_loading"), GameObject.Find("LoadingCanvas").transform).name = "target_loading";
        Debug.Log("2:LoadingManagerがtarget_loadingを生成する。");
        MyProc.started = true;

        yield return new WaitUntil( () => NoaProcesser.BossProc.started);
        Destroy(gameObject);
    }
}
