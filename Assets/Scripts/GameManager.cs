using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シングルトン
public sealed class GameManager : MonoBehaviour
{
    private Vector2 MarginAreaTop    { get; set; }
    private Vector2 MarginAreaLeft   { get; set; }    
    private Vector2 MarginAreaRight  { get; set; }
    private Vector2 MarginAreaBottom { get; set; }
    private Vector2 MarginAreaCenter { get; set; }

    private Vector2 PC1AreaMin { get; set; }
    private Vector2 PC1AreaMax { get; set; }
    private Vector2 PC2AreaMin { get; set; }
    private Vector2 PC2AreaMax { get; set; }

    private static GameManager inst = new GameManager();
    public static GameManager Inst { get { return inst; } }
    private GameManager()
    {
        MarginAreaTop    = new Vector2(Screen.width, 10);
        MarginAreaLeft   = new Vector2(10, Screen.height);
        MarginAreaRight  = new Vector2(10, Screen.height);
        MarginAreaBottom = new Vector2(Screen.width, 10);
        MarginAreaCenter = new Vector2(10, Screen.height);

        PC1AreaMin = new Vector2(0 + MarginAreaTop.x,                   0 + MarginAreaTop.y);
        PC1AreaMax = new Vector2(Screen.width/2 - MarginAreaCenter.x/2, Screen.height - MarginAreaBottom.y);
        PC2AreaMin = new Vector2(Screen.width/2 + MarginAreaCenter.x/2, 0 + MarginAreaTop.y);
        PC2AreaMax = new Vector2(Screen.width - MarginAreaRight.x,      Screen.height - MarginAreaBottom.y);

        Debug.Log("manager created");
    }

    public Vector2 getAreaMin(PlayerSlot PlayerSlot)
    {
        switch (PlayerSlot)
        {
            case PlayerSlot.PC1: return PC1AreaMin;
            case PlayerSlot.PC2: return PC2AreaMin;
            default: return new Vector2(-1, -1);
        }
    }

    public Vector2 getAreaMax(PlayerSlot PlayerSlot)
    {
        switch (PlayerSlot)
        {
            case PlayerSlot.PC1: return PC1AreaMax;
            case PlayerSlot.PC2: return PC2AreaMax;
            default: return new Vector2(-1, -1);
        }
    }

    public void gameSet()
    {

    }
}