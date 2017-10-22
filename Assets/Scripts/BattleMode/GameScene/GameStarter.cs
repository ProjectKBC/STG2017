using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public string PC1Name;
    public string PC2Name;
    public static Starter starter = new Starter();

    private void Init()
    {
        PlayerManager pm = PlayerManager.Inst;
        GameManager gm = GameManager.Inst;
        PlayerUIManager puim = PlayerUIManager.Inst;
    }

    IEnumerator Start ()
    {
        Init();

        yield return starter.StayStarted(PlayerManager.starter);
        GameManager.SetPCName(PC1Name, PlayerSlot.PC1);
        GameManager.SetPCName(PC2Name, PlayerSlot.PC2);
        starter.started = true;
        starter.Log(this, 0);
	}
}
