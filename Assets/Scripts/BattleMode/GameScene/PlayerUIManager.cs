using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GageBarType
{
    None,
    Top,
    Left,
    Circle,
}

public enum GageEffectType
{
    None,
    OnceRecharge,
    SeparatelyRecharge,
}

public enum GageCountType
{
    None,
    Increase,
    Decrease,
}

[System.Serializable]
public class Gage
{
    public float maxValue;
    public float value;
    public GageBarType    barType;
    public GageEffectType effectType;
    public GageCountType  countType;
}

public class PlayerUIManager : MonoBehaviour
{
    private Player player;
    
    [System.NonSerialized] public GameObject mainCanvas;
    [System.NonSerialized] public Image hitPointImage;
    [System.NonSerialized] public Text  hitPointText;
    [System.NonSerialized] public Image TopGageImage;
    [System.NonSerialized] public Text  TopGageText;
    [System.NonSerialized] public Image LeftGageImage;
    [System.NonSerialized] public Text  LeftGageText;
    [System.NonSerialized] public Image iconImage;
    [System.NonSerialized] public Text iconText;

    [System.NonSerialized] public Dictionary<string, Gage> gages = new Dictionary<string, Gage>();

    private bool isBarTypeTop = false;
    private bool isBarTypeLeft = false;
    private bool isBarTypeCircle = false;

    private float TopValue;
    private float TopMaxValue;
    
    private float LeftValue;
    private float LeftMaxValue;

    private const float width  = 780;
    private const float height = 1020;

    private void Start()
    {
        player = GetComponent<Player>();
        
        while (true) { if (player.Started) { break; } }

        foreach (string key in player.shotManager.Keys)
        {
            if (player.shotManager[key].param.shotMode == ShotMode.SimpleShot) { continue; }

            switch (player.shotManager[key].param.gage.barType)
            {
                case GageBarType.Top:
                    if (isBarTypeTop) { Debug.Log("BarTypeが重複しています。"); return; }

                    isBarTypeTop = true;

                    if (player.shotManager[key].param.shotMode == ShotMode.ChargeShot)
                    {
                        TopMaxValue = player.shotManager[key].param.chargeTime;
                    }

                    if (player.shotManager[key].param.shotMode == ShotMode.LimitShot)
                    {
                        TopMaxValue = player.shotManager[key].param.bulletMaxNum;
                    }
                    break;

                case GageBarType.Left:
                    if (isBarTypeLeft) { Debug.Log("BarTypeが重複しています。"); return; }

                    isBarTypeLeft = true;

                    if (player.shotManager[key].param.shotMode == ShotMode.ChargeShot)
                    {
                        LeftMaxValue = player.shotManager[key].param.chargeTime;
                    }

                    if (player.shotManager[key].param.shotMode == ShotMode.LimitShot)
                    {
                        LeftMaxValue = player.shotManager[key].param.bulletMaxNum;
                    }
                    break;
            }
                
        }

        CreateUI();
        SettingUI();
    }

    private void Update()
    {
        hitPointImage.fillAmount = player.hitPoint / player.maxHitPoint;
        hitPointText.text        = ((float)player.hitPoint).ToString();

        foreach (string key in player.shotManager.Keys)
        {
            if (player.shotManager[key].param.shotMode == ShotMode.SimpleShot) { continue; }

            switch (player.shotManager[key].param.gage.barType)
            {
                case GageBarType.Top:

                    if (player.shotManager[key].param.shotMode == ShotMode.ChargeShot)
                    {
                        if (player.shotManager[key].chargeBeginTime == 0)
                        {
                            TopValue = 0;
                        }
                        else if (Time.time - player.shotManager[key].chargeBeginTime < TopMaxValue)
                        {
                            TopValue = Time.time - player.shotManager[key].chargeBeginTime;
                        }
                        else
                        {
                            TopValue = TopMaxValue;
                        }
                        TopGageImage.fillAmount = TopValue / TopMaxValue;
                        TopGageText.text = ((int)((TopValue / TopMaxValue) * 100)).ToString();
                    }

                    if (player.shotManager[key].param.shotMode == ShotMode.LimitShot)
                    {
                        TopValue = player.shotManager[key].bulletNum;
                        TopGageImage.fillAmount = TopValue / TopMaxValue;
                        TopGageText.text = TopValue.ToString();
                    }
                    break;

                case GageBarType.Left:

                    if (player.shotManager[key].param.shotMode == ShotMode.ChargeShot)
                    {
                        if (player.shotManager[key].chargeBeginTime == 0)
                        {
                            LeftValue = 0;
                        }
                        else if (Time.time - player.shotManager[key].chargeBeginTime < LeftMaxValue)
                        {
                            LeftValue = Time.time - player.shotManager[key].chargeBeginTime;
                        }
                        else
                        {
                            LeftValue = LeftMaxValue;
                        }
                        LeftGageImage.fillAmount = LeftValue / LeftMaxValue;
                        LeftGageText.text = ((int)((LeftValue / LeftMaxValue) * 100)).ToString();
                    }
                    if (player.shotManager[key].param.shotMode == ShotMode.LimitShot)
                    {
                        LeftValue    = player.shotManager[key].bulletNum;
                        LeftGageImage.fillAmount = LeftValue / LeftMaxValue;
                        LeftGageText.text = LeftValue.ToString();
                    }
                    break;
            }
        }
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
        foreach (string key in gages.Keys)
        {
            switch (gages[key].barType)
            {
                case GageBarType.Top:
                    break;

                case GageBarType.Left:
                    if (isBarTypeLeft) { Debug.Log("BarTypeが重複しています。"); return; }

                    Debug.Log("アタッチ Left");
                    isBarTypeLeft = true;
                    break;

                case GageBarType.Circle:
                    if (isBarTypeCircle) { Debug.Log("BarTypeが重複しています。"); return; }

                    isBarTypeCircle = true;
                    Debug.Log(this+": "+ "GageBarType.Circleは未対応です。" );
                    break;
            }
        }

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
                                i.fillOrigin     = 0;
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

                case "TopGageCanvas":
                    target = child.gameObject.GetComponent<Canvas>();

                    if (isBarTypeTop == false) { Destroy(target.gameObject); Debug.Log("top"); break; }

                    foreach (Transform c_child in target.transform)
                    {
                        switch (c_child.name)
                        {
                            case "Bar":
                                rt = c_child.gameObject.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector3(0, 0, 0);
                                rt.sizeDelta        = new Vector2(672, 36);
                                rt.anchorMin        = new Vector2(0, 1);
                                rt.anchorMax        = new Vector2(0, 1);
                                rt.pivot            = new Vector2(0, 1);
                                rt.rotation         = new Quaternion(0, 0, 0, 0);
                                rt.localScale       = new Vector3(1, 1, 1);

                                i = c_child.gameObject.GetComponent<Image>();
                                i.sprite         = Resources.Load<Sprite>("Sprites/UI/GageBar");
                                i.color          = new Color(1, 1, 1, 1);
                                i.material       = null;
                                i.raycastTarget  = true;
                                i.type           = Image.Type.Filled;
                                i.fillMethod     = Image.FillMethod.Horizontal;
                                i.fillOrigin     = 0;
                                i.fillAmount     = 1.0f;
                                i.fillClockwise  = true;
                                i.preserveAspect = false;

                                TopGageImage = i;
                                break;


                            case "Text":
                                rt = c_child.gameObject.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector3(0, 0, 0);
                                rt.sizeDelta        = new Vector2(36, 540);
                                rt.anchorMin        = new Vector2(0, 1);
                                rt.anchorMax        = new Vector2(0, 1);
                                rt.pivot            = new Vector2(0, 1);
                                rt.rotation         = new Quaternion(0, 0, 0, 0);
                                rt.localScale       = new Vector3(1, 1, 1);

                                t = c_child.gameObject.GetComponent<Text>();
                                t.text                 = "";
                                t.font                 = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                                t.fontStyle            = FontStyle.Normal;
                                t.fontSize             = 24;
                                t.supportRichText      = true;
                                t.alignment            = TextAnchor.UpperLeft;
                                t.alignByGeometry      = false;
                                t.horizontalOverflow   = HorizontalWrapMode.Wrap;
                                t.verticalOverflow     = VerticalWrapMode.Truncate;
                                t.resizeTextForBestFit = false;
                                t.color                = new Color(0.196f, 0.196f, 0.196f, 1.0f);
                                t.material             = null;
                                t.raycastTarget        = true;

                                TopGageText = t;
                                break;

                        }
                    }
                    break;

                case "LeftGageCanvas":
                    target = child.gameObject.GetComponent<Canvas>();

                    if (isBarTypeLeft == false) { Destroy(target.gameObject); Debug.Log("left"); break; }

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
                                i.color          = new Color(1, 1, 1, 1);
                                i.material       = null;
                                i.raycastTarget  = true;
                                i.type           = Image.Type.Filled;
                                i.fillMethod     = Image.FillMethod.Vertical;
                                i.fillOrigin     = 1;
                                i.fillAmount     = 1.0f;
                                i.fillClockwise  = true;
                                i.preserveAspect = false;
                                
                                LeftGageImage = i;
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

                                LeftGageText = t;
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
