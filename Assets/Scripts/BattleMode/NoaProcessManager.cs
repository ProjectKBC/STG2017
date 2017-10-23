using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoaProcesser
{
    public static NoaProcesser BossProc = new NoaProcesser();
    public bool started = false;
    public bool pausing = false;
    public bool ended   = false;

    public static IEnumerator StayBoss()
    {
        if (!BossProc.started || BossProc.pausing || BossProc.ended) { yield return null; }
    }
    public static bool IsStayBoss()
    {
        return (!BossProc.started || BossProc.pausing || BossProc.ended);
    }

    public IEnumerator Stay()
    {
        if (!started || pausing || ended) { yield return null; }
    }
    public bool IsStay()
    {
        return (!started || pausing || ended);
    }

    /*
    public void Log(System.Object _this)
    {
        if (started) { Debug.Log(_this + " started"); }
        else { Debug.Log(_this + " NOT started"); }
    }
    */
    public void Log(System.Object _this, int _num)
    {
        if (started) { Debug.Log(_num + ": " + _this + " started"); return; }
        if (pausing) { Debug.Log(_num + ": " + _this + " pausing"); return; }
        if (ended)   { Debug.Log(_num + ": " + _this + " ended");   return; }
    }
    /*
    public void Log(System.Object _this, string _str)
    {
        if (started) { Debug.Log(_this + _str); }
        else { Debug.Log(_this + "NOT " + _str); }
    }
    */
}

public abstract class NoaBehaviour : MonoBehaviour
{
    public NoaProcesser MyProc = new NoaProcesser();

    protected abstract IEnumerator Start();

    protected virtual void Update() { }
}
