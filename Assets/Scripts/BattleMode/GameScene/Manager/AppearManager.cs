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
        yield return new WaitUntil(() => NoaProcesser.BossProc.started);
        AppearManager.DestroyMe(gameObject);
    }

    public void Starting()
    {
        Debug.Log("4:AppearManagerが呼び出される。");
        Instantiate(Resources.Load("Prefabs/Enemys/Emitters/PC1Emitter"), GameObject.Find("PC1Area/Canvas").transform).name = "target_pause";
        Instantiate(Resources.Load("Prefabs/Enemys/Emitters/PC2Emitter"), GameObject.Find("PC2Area/Canvas").transform).name = "target_pause";
        
        MyProc.started = true;

    }

    public static void DestroyMe(GameObject _gameObject)
    {
        inst.MyProc.Reset();
        inst = null;
        Debug.Log("Destroy:AppearManager");
        Destroy(_gameObject);
    }
}
