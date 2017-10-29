// by kurisankaku
// http://www.kurisankaku.xyz/entry/2017/03/22/213712

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NoaConsoleHeader
{
    public PlayerUI UI;

    public string oldLogs = "";
    public ScrollRect scrollRect;
    public Text textLog;
}

public class NoaConsole : MonoBehaviour
{
    // ScrollViewに表示するログ
    public static Dictionary<PlayerSlot ,string> Logs = new Dictionary<PlayerSlot, string>();
    public static List<NoaConsole> consoles = new List<NoaConsole>();
    public NoaConsoleHeader msh;
    public bool Started = false;

    // Start時に各オブジェクトを取得
    void Start()
    {
        consoles.Add(this);

        msh.UI = transform.parent.gameObject.GetComponent<PlayerUI>();
        Logs.Add(msh.UI.playerSlot, null);

        msh.scrollRect = gameObject.GetComponent<ScrollRect>();
        msh.textLog    = msh.scrollRect.content.GetComponentInChildren<Text>();
        Started = true;
    }

    void Update()
    {
        // logsとoldLogsが異なるときにTextを更新
        if (msh.scrollRect != null && Logs[msh.UI.playerSlot] != msh.oldLogs)
        {
            msh.textLog.text = Logs[msh.UI.playerSlot];
            // Textが追加されたときに５フレーム後に自動でScrollViewのBottomに移動するようにする。
            StartCoroutine(DelayMethod(5, () =>
            {
                msh.scrollRect.verticalNormalizedPosition = 0;
            }));
            msh.oldLogs = Logs[msh.UI.playerSlot];
        }
    }

    //ログを表示
    public static void Log(string _massage, PlayerSlot _ps)
    {
        Logs[_ps] += (_massage + "\n");
        Debug.Log(_ps + " : " + _massage);
    }

    public static void Log(object _massage, PlayerSlot _ps)
    {
        Logs[_ps] += (_massage + "\n");
        Debug.Log(_ps + " : " + _massage);
    }

    // 指定したフレーム数後にActionが実行される
    private IEnumerator DelayMethod(int delayFrameCount, Action action)
    {
        for (var i = 0; i < delayFrameCount; i++)
        {
            yield return null;
        }
        action();
    }
}