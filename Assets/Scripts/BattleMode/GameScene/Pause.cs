using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : NoaBehaviour
{
    public static Pause Inst = null;

    private static GroupButton groupButton;
    private static Button ReturnToGame;
    private static Button QuitAndReturnToTitle;

    private void Init()
    {
        if (Inst != null) { Destroy(gameObject); return; }

        Inst = this;

        groupButton          = GameObject.Find("PausingCanvas/Pausing/Buttons").GetComponent<GroupButton>();
        ReturnToGame         = GameObject.Find("PausingCanvas/Pausing/Buttons/ReturnToGame_Button").GetComponent<Button>();
        QuitAndReturnToTitle = GameObject.Find("PausingCanvas/Pausing/Buttons/QuitAndReturnToTitle_Button").GetComponent<Button>();

        
        transform.GetChild(0).gameObject.SetActive(false);
    }

    protected override IEnumerator Start()
    {
        Init();
        yield return null;
    }

    private void Update()
    {
        if (NoaProcesser.BossProc.pausing && !MyProc.started)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            groupButton.ActiveButtons(true);
            MyProc.started = true;
        }

        if (!NoaProcesser.BossProc.pausing && MyProc.started)
        {
            MyProc.started = false;
            groupButton.ActiveButtons(false);
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void OnClick_ReturnToGame()
    {
        MyProc.started = false;
        groupButton.ActiveButtons(false);
        transform.GetChild(0).gameObject.SetActive(false);
        NoaProcesser.BossProc.pausing = false;
    }

    public void OnClick_QuitAndReturnToTitle()
    {

    }
}
