using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result : NoaBehaviour
{
    // f:シングルトンではない!?
    public static Result Inst = null;

    // f:ポーズ関連のUI
    private static GroupButton groupButton;

    protected override IEnumerator Start()
    {
        if (Inst != null) { Destroy(gameObject); yield break; }

        Inst = this;

        groupButton = GameObject.Find("ResultingCanvas/target_result/Resulting/Buttons").GetComponent<GroupButton>();
        
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
    
    public void OnClick_QuitAndReturnToTitle()
    {
        GameManager.GameEsc();
        SceneManager.LoadScene("ModeSelect");
    }

    public void Active(bool _IsOn)
    {
        if (_IsOn)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            groupButton.ActiveButtons(true);
        }
        else
        {
            groupButton.ActiveButtons(false);
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}
