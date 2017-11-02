using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupButton : NoaBehaviour
{
    private CanvasGroup canvasGroup;
    public GameObject firstSelect;

    protected override IEnumerator Start()
    {
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        //canvasGroup.interactable = false;
        MyProc.started = true;

        yield return null;
    }

    public void ActiveButtons(bool _IsOn)
    {
        //yield return new WaitUntil( () => MyProc.started);

        if (_IsOn)
        {
            canvasGroup.interactable = true;
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(firstSelect);
        }
        else
        {
            canvasGroup.interactable = false;
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
