using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestGame : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("BattleMode");
    }
}
