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
        yield return new WaitWhile( () => IsStayBoss());
    }
    public static bool IsStayBoss()
    {
        return (!BossProc.started || BossProc.pausing || BossProc.ended);
    }

    public IEnumerator Stay()
    {
        yield return new WaitWhile(() => IsStay());
    }
    public bool IsStay()
    {
        return (!started || pausing || ended);
    }
    
    public void Log(System.Object _this, int _num)
    {
        if (started) { Debug.Log(_num + ": " + _this + " started"); return; }
        if (pausing) { Debug.Log(_num + ": " + _this + " pausing"); return; }
        if (ended)   { Debug.Log(_num + ": " + _this + " ended");   return; }
    }

    public static NoaProcesser PC1Proc = new NoaProcesser();
    public static NoaProcesser PC2Proc = new NoaProcesser();

    public static IEnumerator StayPC(PlayerSlot _playerSlot)
    {
        yield return new WaitWhile(() => IsStayPC(_playerSlot));
    }
    public static bool IsStayPC(PlayerSlot _playerSlot)
    {
        return _playerSlot == PlayerSlot.PC1
            ? (!PC1Proc.started || PC1Proc.pausing || PC1Proc.ended)
            : (!PC2Proc.started || PC2Proc.pausing || PC2Proc.ended);
    }
}

public abstract class NoaBehaviour : MonoBehaviour
{
    public NoaProcesser MyProc = new NoaProcesser();

    protected abstract IEnumerator Start();
}
