using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// f:旧FireLitStoneクラス
// f:名前がダサいのと、仕様の簡潔化をした。
// f:17/10/23
public class GameStarter : NoaBehaviour
{
    public static new NoaProcesser MyProc = new NoaProcesser();
    public static bool IsSetPCName = false;
    public string PC1Name;
    public string PC2Name;
    
    protected override IEnumerator Start()
    {
        Debug.Log("1:GameStarterがLoadingManagerを呼び出す");
        LoadingManager lm = LoadingManager.Inst;
        yield return new WaitUntil(() => lm.MyProc.started);

        Debug.Log("3:GameStarterがPlayerManagerとSoundManagerを呼び出す");
        PlayerManager     plm  = PlayerManager.Inst;
        SoundManager      sm   = SoundManager.Inst;

        yield return new WaitUntil(() => plm.MyProc.started && sm.MyProc.started);

        Debug.Log("5:GameStarterがPauseManagerとBackGroundManagerを呼び出す");
        PauseManager      pm   = PauseManager.Inst;
        BackGroundManager bm   = BackGroundManager.Inst;

        yield return new WaitUntil(() => pm.MyProc.started && bm.MyProc.started);

        Debug.Log("7:GameStarterがGameManagerを呼び出す");
        GameManager       gm   = GameManager.Inst;

        GameManager.SetPCName(CharacterSelectManager.PC1Name ?? PC1Name, PlayerSlot.PC1);
        GameManager.SetPCName(CharacterSelectManager.PC2Name ?? PC2Name, PlayerSlot.PC2);
        Debug.Log("8:GameStarterがPCNameを渡す。");
        IsSetPCName = true;

        yield return new WaitUntil(() => gm.MyProc.started);
        Debug.Log("_9:Playerが生成される。");

        yield return new WaitUntil( () => GameManager.Pc1Player.MyProc.started && GameManager.Pc2Player.MyProc.started);
        Debug.Log("_10:Playerが初期設定をする。");

        Debug.Log("11:GameStarterがPlayerUIManagerを呼び出す");
        PlayerUIManager   puim = PlayerUIManager.Inst;

        yield return new WaitUntil( () => PlayerUIManager.Inst.MyProc.started);

        Debug.Log("_13:PlayerUIManagerがUIの初期設定をする。");
        MyProc.started = true;

        yield return new WaitUntil( () => NoaProcesser.BossProc.started);
        Debug.Log("14:ゲームを開始する。");
    }
}
