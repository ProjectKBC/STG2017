using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// f:旧FireLitStoneクラス
// f:名前がダサいのと、仕様の簡潔化をした。
// f:17/10/23
public class GameStarter : NoaBehaviour
{
    public static new NoaProcesser MyProc = new NoaProcesser();
    public string PC1Name;
    public string PC2Name;

    private void Init()
    {
        // f:ただの呼び出し
        PlayerManager   pm   = PlayerManager.Inst;
        GameManager     gm   = GameManager.Inst;
        PlayerUIManager puim = PlayerUIManager.Inst;
    }

    protected override IEnumerator Start()
    {
        Init();

        if (PlayerManager.Inst.MyProc.IsStay()) yield return null;

        if (GameManager.Pc1Name == null || GameManager.Pc2Name == null)
        {
            GameManager.SetPCName(PC1Name, PlayerSlot.PC1);
            GameManager.SetPCName(PC2Name, PlayerSlot.PC2);
        }

        MyProc.started = true;
        MyProc.Log(this, 1);
        Destroy(this.gameObject);
	}
}
