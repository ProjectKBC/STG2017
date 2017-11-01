using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : NoaBehaviour
{
    // f:シングルトンではない!?
    public static Pause Inst = null;

    // f:ポーズ時間を計測
    public  static float pausingTime;
    private static float tmpTime;

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
            transform.GetChild(0).gameObject.SetActive(true);
            groupButton.ActiveButtons(true);

            // f:ポーズ経過時間の初期化とポーズ開始時刻の保存
            pausingTime = 0;
            tmpTime = Time.time;

            NoaProcesser.BossProc.pausing = true;
        }
        else
        {
            groupButton.ActiveButtons(false);
            transform.GetChild(0).gameObject.SetActive(false);

            // f:ポーズ経過時間の保存とポーズ開始時刻の初期化
            pausingTime = Time.time - tmpTime;
            tmpTime = 0;
            Debug.Log("Pause: " + pausingTime);

            NoaProcesser.BossProc.pausing = false;
        }
    }
}
