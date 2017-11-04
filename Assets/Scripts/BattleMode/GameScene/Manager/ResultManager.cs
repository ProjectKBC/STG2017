using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ResultManager : NoaBehaviour
{
    private static ResultManager inst;
    private ResultManager() { }
    public static ResultManager Inst
    {
        get
        {
            if (inst == null)
            {
                GameObject go = new GameObject("ResultManager");
                go.transform.parent = GameObject.Find("Managers").transform;
                inst = go.AddComponent<ResultManager>();
            }

            return inst;
        }
    }

    protected override IEnumerator Start()
    {
        yield return new WaitUntil(() => NoaProcesser.BossProc.started);
        ResultManager.DestroyMe(gameObject);
    }

    public void Starting()
    {
        Debug.Log("5:ResultManagerが呼び出される。");
        GameObject obj = Instantiate(Resources.Load("Prefabs/target_result"), GameObject.Find("ResultingCanvas").transform) as GameObject;
        obj.name = "target_result";
        
        MyProc.started = true;
    }

    public static void DestroyMe(GameObject _gameObject)
    {
        inst.MyProc.Reset();
        inst = null;
        Debug.Log("Destroy:ResultManager");
        Destroy(_gameObject);
    }
}
