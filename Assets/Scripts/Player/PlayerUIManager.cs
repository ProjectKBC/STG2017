using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    private Player player;
    
    public Canvas mainCanvas;

    void Start()
    {
        player = GetComponent<Player>();


        Canvas target = null;
        foreach (Transform child in mainCanvas.transform)
        {
            switch (child.name)
            {
                case "HPCanvas":
                    target = child.gameObject.GetComponent<Canvas>();
                    RectTransform rt = null;
                    Image i = null;
                    Text t = null;
                    foreach (Transform c_child in target.transform)
                    {
                        switch (c_child.name)
                        {
                            case "HPBar":
                                rt = c_child.gameObject.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector3(0, 0, 0);
                                rt.sizeDelta        = new Vector2(672, 72);
                                rt.anchorMin        = new Vector2(0, 1);
                                rt.anchorMax        = new Vector2(0, 1);
                                rt.pivot            = new Vector2(0, 1);
                                rt.rotation         = new Quaternion(0, 0, 0, 0);
                                rt.localScale       = new Vector3(1, 1, 1);

                                i = c_child.gameObject.GetComponent<Image>();
                                i.sprite        = null;
                                i.color         = new Color(204, 68, 68, 136);
                                i.material      = null;
                                i.raycastTarget = true;
                                break;


                            case "HPText":
                                rt = c_child.gameObject.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector3(0, 0, 0);
                                rt.sizeDelta        = new Vector2(160, 72);
                                rt.anchorMin        = new Vector2(0, 1);
                                rt.anchorMax        = new Vector2(0, 1);
                                rt.pivot            = new Vector2(0, 1);
                                rt.rotation         = new Quaternion(0, 0, 0, 0);
                                rt.localScale       = new Vector3(1, 1, 1);

                                t = c_child.gameObject.GetComponent<Text>();
                                t.text                 = ((float)player.hitPoint).ToString();
                                t.font                 = new Font("Arial");
                                t.fontStyle            = FontStyle.Normal;
                                t.fontSize             = 36;
                                t.supportRichText      = true;
                                t.alignment            = TextAnchor.UpperLeft;
                                t.alignByGeometry      = false;
                                t.horizontalOverflow   = HorizontalWrapMode.Wrap;
                                t.verticalOverflow     = VerticalWrapMode.Truncate;
                                t.resizeTextForBestFit = false;
                                t.color                = new Color(50, 50, 50, 255);
                                t.material             = null;
                                t.raycastTarget        = true;
                                break;

                        }
                    }
                    break;

                case "ChargeCanvas":
                    target = child.gameObject.GetComponent<Canvas>();
                    foreach (Transform c_child in target.transform)
                    {
                        switch (c_child.name)
                        {
                            case "ChargeBar":
                                rt = c_child.gameObject.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector3(0, 0, 0);
                                rt.sizeDelta        = new Vector2(72, 432);
                                rt.anchorMin        = new Vector2(0, 1);
                                rt.anchorMax        = new Vector2(0, 1);
                                rt.pivot            = new Vector2(0, 1);
                                rt.rotation         = new Quaternion(0, 0, 0, 0);
                                rt.localScale       = new Vector3(1, 1, 1);

                                i = c_child.gameObject.GetComponent<Image>();
                                i.sprite        = null;
                                i.color         = new Color(204, 204, 68, 136);
                                i.material      = null;
                                i.raycastTarget = true;
                                break;


                            case "ChargeText":
                                rt = c_child.gameObject.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector3(0, 0, 0);
                                rt.sizeDelta        = new Vector2(72, 540);
                                rt.anchorMin        = new Vector2(0, 1);
                                rt.anchorMax        = new Vector2(0, 1);
                                rt.pivot            = new Vector2(0, 1);
                                rt.rotation         = new Quaternion(0, 0, 0, 0);
                                rt.localScale       = new Vector3(1, 1, 1);

                                t = c_child.gameObject.GetComponent<Text>();
                                t.text                 = "";
                                t.font                 = new Font("Arial");
                                t.fontStyle            = FontStyle.Normal;
                                t.fontSize             = 36;
                                t.supportRichText      = true;
                                t.alignment            = TextAnchor.UpperLeft;
                                t.alignByGeometry      = false;
                                t.horizontalOverflow   = HorizontalWrapMode.Wrap;
                                t.verticalOverflow     = VerticalWrapMode.Truncate;
                                t.resizeTextForBestFit = false;
                                t.color                = new Color(50, 50, 50, 255);
                                t.material             = null;
                                t.raycastTarget        = true;
                                break;

                        }
                    }
                    break;

                case "IconCanvas":
                    target = child.gameObject.GetComponent<Canvas>();
                    foreach (Transform c_child in target.transform)
                    {
                        switch (c_child.name)
                        {
                            case "Icon":
                                rt = c_child.gameObject.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector3(0, 0, 0);
                                rt.sizeDelta        = new Vector2(108, 108);
                                rt.anchorMin        = new Vector2(0, 1);
                                rt.anchorMax        = new Vector2(0, 1);
                                rt.pivot            = new Vector2(0, 1);
                                rt.rotation         = new Quaternion(0, 0, 0, 0);
                                rt.localScale       = new Vector3(1, 1, 1);

                                i = c_child.gameObject.GetComponent<Image>();
                                i.sprite        = null;
                                i.color         = new Color(255, 255, 255, 136);
                                i.material      = null;
                                i.raycastTarget = true;
                                break;


                            case "IconText":
                                rt = c_child.gameObject.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector3(0, 0, 0);
                                rt.sizeDelta        = new Vector2(108, 108);
                                rt.anchorMin        = new Vector2(0, 1);
                                rt.anchorMax        = new Vector2(0, 1);
                                rt.pivot            = new Vector2(0, 1);
                                rt.rotation         = new Quaternion(0, 0, 0, 0);
                                rt.localScale       = new Vector3(1, 1, 1);

                                t = c_child.gameObject.GetComponent<Text>();
                                t.text                 = player.name;
                                t.font                 = new Font("Arial");
                                t.fontStyle            = FontStyle.Normal;
                                t.fontSize             = 18;
                                t.supportRichText      = true;
                                t.alignment            = TextAnchor.LowerLeft;
                                t.alignByGeometry      = false;
                                t.horizontalOverflow   = HorizontalWrapMode.Wrap;
                                t.verticalOverflow     = VerticalWrapMode.Truncate;
                                t.resizeTextForBestFit = false;
                                t.color                = new Color(50, 50, 50, 255);
                                t.material             = null;
                                t.raycastTarget        = true;
                                break;

                        }
                    }
                    break;
            }
        }
        
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
        switch (player.playerSlot)
        {
            case PlayerSlot.PC1:
                mainCanvas = Instantiate(Resources.Load("Prefabs/UI/PC1Canvas")) as Canvas;
                break;

            case PlayerSlot.PC2:
                mainCanvas = Instantiate(Resources.Load("Prefabs/UI/PC2Canvas")) as Canvas;
                break;
        }
    }
}
