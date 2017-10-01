using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager
{
}

// シングルトン
public sealed class GameManager : MonoBehaviour
{
    private static GameManager inst;
    private GameManager() { Debug.Log("manager created");}
    public static GameManager Inst
    {
        get
        {
            if (inst == null)
            {
                GameObject go = new GameObject("GameManager");
                inst = go.AddComponent<GameManager>();
            }

            return inst;
        }
    }

    public Vector4 pc1Area = new Vector4(-8.0f, -4.8f, -0.9f, 4.8f);
    public Vector4 pc2Area = new Vector4( 0.9f, -4.8f,  8.0f, 4.8f);

    private void Start()
    {
        Debug.Log("manager start");
    }

    public Vector2 getAreaMin(PlayerSlot PlayerSlot)
    {
        switch (PlayerSlot)
        {
            case PlayerSlot.PC1: return new Vector2(pc1Area.x, pc1Area.y);
            case PlayerSlot.PC2: return new Vector2(pc2Area.x, pc2Area.y);
            default: return new Vector2(-1, -1);
        }
    }

    public Vector2 getAreaMax(PlayerSlot PlayerSlot)
    {
        switch (PlayerSlot)
        {
            case PlayerSlot.PC1: return new Vector2(pc1Area.z, pc1Area.w);
            case PlayerSlot.PC2: return new Vector2(pc2Area.z, pc2Area.w);
            default: return new Vector2(-1, -1);
        }
    }

    public void gameSet()
    {

    }
}