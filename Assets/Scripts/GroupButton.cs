using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupButton : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public GameObject firstSelect;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ActiveButtons(bool _IsOn)
    {
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
