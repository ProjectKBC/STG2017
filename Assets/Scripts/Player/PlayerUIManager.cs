using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    private Player player;
    
    public Canvas canvas;

    void Start()
    {
        Text target = null;
        foreach (Transform child in canvas.transform)
        {
            if (child.name == "Main Title")
            {
                target = child.gameObject.GetComponent<Text>();
                target.text = "AAAAAAA";
            }
            else if (child.name == "Sub Title")
            {
                target = child.gameObject.GetComponent<Text>();
                target.text = "BBBBBBB";
            }
        }
        

        player = GetComponent<Player>();
        //this.initParameter();

    }

    /*
    void Update()
    {
        lifeGage.fillAmount = player.hitPoint / player.maxHitPoint;
    }

    private void initParameter()
    {
        lifeGage = GameObject.Find("LifeGage").GetComponent<Image>();
        lifeGage.fillAmount = 1;

        lifeRedGage = GameObject.Find("LifeRedGage").GetComponent<Image>();
        lifeRedGage.fillAmount = 1;
    }
    */

    private void CreateUI()
    {
        Canvas c = new Canvas
        {
            renderMode = RenderMode.ScreenSpaceCamera,
            pixelPerfect = true
        };
    }
}
