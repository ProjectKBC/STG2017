using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// f:旧FireLitStoneクラス
// f:名前がダサいのと、仕様の簡潔化をした。
// f:17/10/23
public class GameStarter : NoaBehaviour
{
    public string PC1Name;
    public string PC2Name;

    protected override IEnumerator Start()
    {
        // f:初期設定
        PlayerManager     plm  = PlayerManager.Inst;
        SoundManager      sm   = SoundManager.Inst;
        AppearManager     am   = AppearManager.Inst;
        PauseManager      pm   = PauseManager.Inst;
        BackGroundManager bm   = BackGroundManager.Inst;
        GameManager       gm   = GameManager.Inst;
        PlayerUIManager   puim = PlayerUIManager.Inst;
        // f:

        Debug.Log("1:target_loadingを生成する。");
        Instantiate(Resources.Load("Prefabs/target_loading"), GameObject.Find("LoadingCanvas").transform).name = "target_loading";

        plm.Starting();
        sm.Starting();
        am.Starting();
        // yield return new WaitUntil(() => plm.MyProc.started && sm.MyProc.started && am.MyProc.started);
        
        yield return pm.Starting();
        bm.Starting();

        GameManager.SetPCName(CharacterSelectManager.PC1Name ?? PC1Name, PlayerSlot.PC1);
        GameManager.SetPCName(CharacterSelectManager.PC2Name ?? PC2Name, PlayerSlot.PC2);
        gm.Starting();
        
        yield return new WaitUntil( () => GameManager.Pc1Player.MyProc.started && GameManager.Pc2Player.MyProc.started);
        puim.Starting();

        MyProc.started = true;

        Destroy(gameObject);
    }
}
