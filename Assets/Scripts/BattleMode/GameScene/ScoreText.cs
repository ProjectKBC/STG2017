using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    private Text text;
    public PlayerSlot playerSlot;

    private void Start ()
    {
        text = GetComponent<Text>();
    }

    private void Update ()
    {
        text.text = playerSlot == PlayerSlot.PC1
            ? GameManager.Pc1Score.ToString()
            : GameManager.Pc2Score.ToString();
	}
}
