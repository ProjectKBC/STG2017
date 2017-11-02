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
        yield return new WaitUntil(() => NoaProcesser.BossProc.started);
        PauseManager.DestroyMe(gameObject);
    }

    public IEnumerator Starting()
    {
        Debug.Log("5:PauseManagerが呼び出される。");
        GameObject obj = Instantiate(Resources.Load("Prefabs/target_pause"), GameObject.Find("PausingCanvas").transform) as GameObject;
        obj.name = "target_pause";

        yield return new WaitUntil(() => obj.GetComponent<Pause>().MyProc.started);
        MyProc.started = true;
    }

    public static void DestroyMe(GameObject _gameObject)
    {
        inst.MyProc.Reset();
        inst = null;
        Debug.Log("Destroy:PauseManager");
        Destroy(_gameObject);
    }
}
