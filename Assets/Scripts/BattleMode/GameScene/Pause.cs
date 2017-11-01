using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : NoaBehaviour
{
    // f:シングルトンではない!?
    public static Pause Inst = null;
    
    // f:ポーズ関連のUI
    private static GroupButton groupButton;
    private static Button ReturnToGame;
    private static Button QuitAndReturnToTitle;

    private void Init()
    {
        if (Inst != null) { Destroy(gameObject); return; }

        Inst = this;

        groupButton = GameObject.Find("PausingCanvas/Pausing/Buttons").GetComponent<GroupButton>();
        ReturnToGame = GameObject.Find("PausingCanvas/Pausing/Buttons/ReturnToGame_Button").GetComponent<Button>();
        QuitAndReturnToTitle = GameObject.Find("PausingCanvas/Pausing/Buttons/QuitAndReturnToTitle_Button").GetComponent<Button>();
        
        transform.GetChild(0).gameObject.SetActive(false);
    }

    protected override IEnumerator Start()
    {
        Init();
        yield return null;
    }

    public void OnClick_ReturnToGame()
    {
        Active(false);
    }

    public void OnClick_QuitAndReturnToTitle()
    {

    }

    public void Active(bool _IsOn)
    {
        if (_IsOn)
        {
            NoaProcesser.BossProc.pausing = true;
            transform.GetChild(0).gameObject.SetActive(true);
            groupButton.ActiveButtons(true);
        }
        else
        {
            groupButton.ActiveButtons(false);
            transform.GetChild(0).gameObject.SetActive(false);
            NoaProcesser.BossProc.pausing = false;
        }
    }
}
