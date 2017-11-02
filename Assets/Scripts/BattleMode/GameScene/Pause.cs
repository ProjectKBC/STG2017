using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : NoaBehaviour
{
    // f:シングルトンではない!?
    public static Pause Inst = null;

    // f:ポーズ時間
    public static float allElapsedTime = 0;
    private static float startTime;

    // f:ポーズ関連のUI
    private static GroupButton groupButton;
    private static Button ReturnToGame;
    private static Button QuitAndReturnToTitle;

    protected override IEnumerator Start()
    {
        if (Inst != null) { Destroy(gameObject); yield break; }

        Inst = this;

        groupButton  = GameObject.Find("PausingCanvas/target_pause/Pausing/Buttons").GetComponent<GroupButton>();
        ReturnToGame = GameObject.Find("PausingCanvas/target_pause/Pausing/Buttons/ReturnToGame_Button").GetComponent<Button>();
        QuitAndReturnToTitle = GameObject.Find("PausingCanvas/target_pause/Pausing/Buttons/QuitAndReturnToTitle_Button").GetComponent<Button>();

        GroupButton gb = new GroupButton();
        foreach (Transform x in transform.GetChild(0).GetComponentInChildren<Transform>())
        {
            if (x.name == "Buttons")
            {
                gb = x.GetComponent<GroupButton>();
                break;
            }
        }
        yield return new WaitUntil(() => gb.MyProc.started);

        transform.GetChild(0).gameObject.SetActive(false);

        MyProc.started = true;
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
            startTime = Time.time;

            NoaProcesser.BossProc.pausing = true;
            transform.GetChild(0).gameObject.SetActive(true);
            groupButton.ActiveButtons(true);
        }
        else
        {
            groupButton.ActiveButtons(false);
            transform.GetChild(0).gameObject.SetActive(false);
            NoaProcesser.BossProc.pausing = false;

            allElapsedTime += Time.time - startTime;
        }
    }
}
