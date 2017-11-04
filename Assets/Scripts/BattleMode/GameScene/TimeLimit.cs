using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLimit : MonoBehaviour
{
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        string s =  string.Format("{0:00}:{1:00}:{2:00}", (int)GameManager.TimeLimit / 3600, (int)GameManager.TimeLimit / 60, (int)GameManager.TimeLimit % 60); // c:なんか1秒ズレてるように見えるけど気のせいだよ
        text.text = s;
        if (GameManager.TimeLimit <= 10) { text.color = new Color(255, 0, 0); }
    }
}
