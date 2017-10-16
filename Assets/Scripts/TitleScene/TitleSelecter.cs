using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

// f:TitleSelecter(TitleSelectModes)
// f:TitleSelecter、シーン遷移を担うクラス
// f:by flanny 2017/10/16
public class TitleSelecter : MonoBehaviour
{
    void start()
    {
    }

    public void GameStart()
    {
        SceneManager.LoadScene("ModeSelect");
    }

    public void EndGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}