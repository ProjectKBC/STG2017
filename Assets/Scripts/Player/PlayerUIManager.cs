using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    private Player player;
    
    [System.NonSerialized] public GameObject mainCanvas;
    [System.NonSerialized] public Image hitPointImage;
    [System.NonSerialized] public Text  hitPointText;
    [System.NonSerialized] public Image gageImage;
    [System.NonSerialized] public Text  gageText;
    [System.NonSerialized] public Image iconImage;
    [System.NonSerialized] public Text  iconText;

    private const float width  = 780;
    private const float height = 1020;

    void Start()
    {
        player = GetComponent<Player>();
        
        while (true) { if (player.Started) { break; } }

        CreateUI();
        SettingUI();
    }

    void Update()
    {
        hitPointImage.fillAmount = player.hitPoint / player.maxHitPoint;
        hitPointText.text = ((float)player.hitPoint).ToString();
        
        // todo: ShotManagerからparamを持ってきて、チャージ要素を検出する
    }
    
    private void CreateUI()
    {
        switch (player.playerSlot)
        {
            case PlayerSlot.PC1:
                mainCanvas = Instantiate
                    (
                        Resources.Load("Prefabs/UI/PC1Canvas"),
                        GameObject.Find("Canvas").transform
                    ) as GameObject;
                break;

            case PlayerSlot.PC2:
                mainCanvas = Instantiate
                    (
                        Resources.Load("Prefabs/UI/PC2Canvas"),
                        GameObject.Find("Canvas").transform
                    ) as GameObject;
                break;
        }
    }

    private void SettingUI()
    {
        Canvas target = null;
        foreach (Transform child in mainCanvas.GetComponentsInChildren<Transform>())
        {
            switch (child.name)
            {
                case "HPCanvas":
                    target = child.gameObject.GetComponent<Canvas>();
                    RectTransform rt = null;
                    Image i = null;
                    Text t = null;

                    foreach (Transform c_child in target.GetComponentsInChildren<Transform>())
                    {
                        switch (c_child.name)
                        {
                            case "Bar":
                                rt = c_child.gameObject.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector3(0, 0, 0);
                                rt.sizeDelta        = new Vector2(672, 72);
                                rt.anchorMin        = new Vector2(0, 1);
                                rt.anchorMax        = new Vector2(0, 1);
                                rt.pivot            = new Vector2(0, 1);
                                rt.rotation         = new Quaternion(0, 0, 0, 0);
                                rt.localScale       = new Vector3(1, 1, 1);

                                i = c_child.gameObject.GetComponent<Image>();
                                i.sprite         = Resources.Load<Sprite>("Sprites/UI/HPBar");
                                i.color          = new Color(1,1,1,1);
                                i.material       = null;
                                i.raycastTarget  = true;
                                i.type           = Image.Type.Filled;
                                i.fillMethod     = Image.FillMethod.Horizontal;
                                i.fillOrigin     = 3;
                                i.fillAmount     = 1.0f;
                                i.fillClockwise  = true;
                                i.preserveAspect = false;

                                hitPointImage = i;
                                break;


                            case "Text":
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
                                t.font                 = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                                t.fontStyle            = FontStyle.Normal;
                                t.fontSize             = 36;
                                t.supportRichText      = true;
                                t.alignment            = TextAnchor.UpperLeft;
                                t.alignByGeometry      = false;
                                t.horizontalOverflow   = HorizontalWrapMode.Wrap;
                                t.verticalOverflow     = VerticalWrapMode.Truncate;
                                t.resizeTextForBestFit = false;
                                t.color                = new Color(0.196f, 0.196f, 0.196f, 1.0f);
                                t.material             = null;
                                t.raycastTarget        = true;

                                hitPointText = t;
                                break;

                        }
                    }
                    break;

                case "GageCanvas":
                    target = child.gameObject.GetComponent<Canvas>();
                    foreach (Transform c_child in target.transform)
                    {
                        switch (c_child.name)
                        {
                            case "Bar":
                                rt = c_child.gameObject.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector3(0, 0, 0);
                                rt.sizeDelta        = new Vector2(72, 432);
                                rt.anchorMin        = new Vector2(0, 1);
                                rt.anchorMax        = new Vector2(0, 1);
                                rt.pivot            = new Vector2(0, 1);
                                rt.rotation         = new Quaternion(0, 0, 0, 0);
                                rt.localScale       = new Vector3(1, 1, 1);

                                i = c_child.gameObject.GetComponent<Image>();
                                i.sprite         = Resources.Load<Sprite>("Sprites/UI/GageBar");
                                i.color          = new Color(1,1,1,1);
                                i.material       = null;
                                i.raycastTarget  = true;
                                i.type           = Image.Type.Filled;
                                i.fillMethod     = Image.FillMethod.Horizontal;
                                i.fillOrigin     = 3;
                                i.fillAmount     = 1.0f;
                                i.fillClockwise  = true;
                                i.preserveAspect = false;

                                gageImage = i;
                                break;


                            case "Text":
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
                                t.font                 = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                                t.fontStyle            = FontStyle.Normal;
                                t.fontSize             = 36;
                                t.supportRichText      = true;
                                t.alignment            = TextAnchor.UpperLeft;
                                t.alignByGeometry      = false;
                                t.horizontalOverflow   = HorizontalWrapMode.Wrap;
                                t.verticalOverflow     = VerticalWrapMode.Truncate;
                                t.resizeTextForBestFit = false;
                                t.color                = new Color(0.196f, 0.196f, 0.196f, 1.0f);
                                t.material             = null;
                                t.raycastTarget        = true;

                                gageText = t;
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
                                i.color         = new Color(1.0f, 1.0f, 1.0f, 0.5333f);
                                i.material      = null;
                                i.raycastTarget = true;

                                iconImage = i;
                                break;


                            case "Text":
                                rt = c_child.gameObject.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector3(0, 0, 0);
                                rt.sizeDelta        = new Vector2(108, 108);
                                rt.anchorMin        = new Vector2(0, 1);
                                rt.anchorMax        = new Vector2(0, 1);
                                rt.pivot            = new Vector2(0, 1);
                                rt.rotation         = new Quaternion(0, 0, 0, 0);
                                rt.localScale       = new Vector3(1, 1, 1);

                                t = c_child.gameObject.GetComponent<Text>();
                                t.text                 = player.name; Debug.Log("4 " + t.text);
                                t.font                 = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                                t.fontStyle            = FontStyle.Normal;
                                t.fontSize             = 18;
                                t.supportRichText      = true;
                                t.alignment            = TextAnchor.LowerLeft;
                                t.alignByGeometry      = false;
                                t.horizontalOverflow   = HorizontalWrapMode.Wrap;
                                t.verticalOverflow     = VerticalWrapMode.Truncate;
                                t.resizeTextForBestFit = false;
                                t.color                = new Color(0.196f, 0.196f, 0.196f, 1.0f);
                                t.material             = null;
                                t.raycastTarget        = true;

                                iconText = t;
                                break;

                        }
                    }
                    break;
            }
        }
    }

}
