using System;
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

public class PCCanvas
{
    public GameObject mainCanvas;
    public Player player;

    public Text ScoreText;
    public Image hitPointImage;
    public Text  hitPointText;
    public Image TopGageImage;
    public Image TopSubGageImage;
    public Text  TopGageText;
    public Image LeftGageImage;
    public Image LeftSubGageImage;
    public Text  LeftGageText;
    public Image iconImage;
    public Text  iconText;

    public bool isBarTypeTop    = false;
    public bool isBarTypeLeft   = false;
    public bool isBarTypeCircle = false;

    public float TopValue;
    public float TopMaxValue;
    public float TopSubValue;
    public float TopSubMaxValue;

    public float LeftValue;
    public float LeftMaxValue;
    public float LeftSubValue;
    public float LeftSubMaxValue;

    public Dictionary<string, Gage> gages = new Dictionary<string, Gage>();
}

public sealed class PlayerUIManager : NoaBehaviour
{
    private static PlayerUIManager inst;
    private PlayerUIManager() { }
    public static PlayerUIManager Inst
    {
        get
        {
            if (inst == null)
            {
                GameObject go = new GameObject("PlayerUIManager");
                go.transform.parent = GameObject.Find("Managers").transform;
                inst = go.AddComponent<PlayerUIManager>();
            }

            return inst;
        }
    }

    public static PCCanvas pc1Canvas = new PCCanvas();
    public static PCCanvas pc2Canvas = new PCCanvas();

    private const float width  = 780;
    private const float height = 1020;

    protected override IEnumerator Start() { yield return null; }

    public void Starting()
    {
        Debug.Log("8:PlayerUIManagerが呼び出される。");
        
        CreateUI();
        pc1Canvas.player = GameManager.Pc1Player;
        pc2Canvas.player = GameManager.Pc2Player;
        
        CheckStatus(pc1Canvas);
        CheckStatus(pc2Canvas);

        SettingUI(pc1Canvas);
        SettingUI(pc2Canvas);

        MyProc.started = true;
    }

    private bool lastUpdateFlgPC1 = false;
    private bool lastUpdateFlgPC2 = false;
    private void Update()
    {
        if (MyProc.IsStay()) { return; }

        if (!NoaProcesser.IsStayPC(PlayerSlot.PC1) || !lastUpdateFlgPC1) { UpdateUI(pc1Canvas); }
        if (!NoaProcesser.IsStayPC(PlayerSlot.PC2) || !lastUpdateFlgPC2) { UpdateUI(pc2Canvas); }

        if (NoaProcesser.IsStayPC(PlayerSlot.PC1)) { lastUpdateFlgPC1 = true; }
        if (NoaProcesser.IsStayPC(PlayerSlot.PC2)) { lastUpdateFlgPC2 = true; }

        if (NoaProcesser.IsStayBoss()) { return; }
    }
    
    private void UpdateUI(PCCanvas _c)
    {
        _c.hitPointImage.fillAmount = _c.player.hitPoint / _c.player.maxHitPoint;
        _c.hitPointText.text        = ((float)_c.player.hitPoint).ToString();

        foreach (string key in _c.player.shotManager.Keys)
        {
            if (_c.player.shotManager[key].param.shotMode == ShotMode.SimpleShot) { continue; }

            switch (_c.player.shotManager[key].param.gage.barType)
            {
                case GageBarType.Top:
                    // トップ＆チャージショット
                    if (_c.player.shotManager[key].param.shotMode == ShotMode.ChargeShot)
                    {
                        if (_c.player.shotManager[key].chargeBeginTime == 0)
                        {
                            _c.TopValue = 0;
                        }
                        else if (Time.time - _c.player.shotManager[key].chargeBeginTime < _c.TopMaxValue)
                        {
                            _c.TopValue = Time.time - _c.player.shotManager[key].chargeBeginTime;
                        }
                        else
                        {
                            _c.TopValue = _c.TopMaxValue;
                        }
                        _c.TopGageImage.fillAmount = _c.TopValue / _c.TopMaxValue;
                        _c.TopGageText.text        = ((int)((_c.TopValue / _c.TopMaxValue) * 100)).ToString();
                    }

                    // トップ＆リミットショット
                    if (_c.player.shotManager[key].param.shotMode == ShotMode.LimitShot)
                    {
                        if (_c.player.shotManager[key].bulletNum == 0)
                        {
                            _c.TopSubValue = Time.time - _c.player.shotManager[key].lastReloadTime;
                        }
                        else
                        {
                            _c.TopSubValue = 0;
                        }

                        _c.TopValue = _c.player.shotManager[key].bulletNum;
                        _c.TopGageImage.fillAmount    = _c.TopValue / _c.TopMaxValue;
                        _c.TopSubGageImage.fillAmount = _c.TopSubValue / _c.TopSubMaxValue;
                        _c.TopGageText.text           = _c.TopValue.ToString();
                    }
                    break;

                case GageBarType.Left:

                    // レフト＆チャージショット
                    if (_c.player.shotManager[key].param.shotMode == ShotMode.ChargeShot)
                    {
                        if (_c.player.shotManager[key].chargeBeginTime == 0)
                        {
                            _c.LeftValue = 0;
                        }
                        else if (Time.time - _c.player.shotManager[key].chargeBeginTime < _c.LeftMaxValue)
                        {
                            _c.LeftValue = Time.time - _c.player.shotManager[key].chargeBeginTime;
                        }
                        else
                        {
                            _c.LeftValue = _c.LeftMaxValue;
                        }
                        _c.LeftGageImage.fillAmount = _c.LeftValue / _c.LeftMaxValue;
                        _c.LeftGageText.text        = ((int)((_c.LeftValue / _c.LeftMaxValue) * 100)).ToString();
                    }

                    // レフト＆リミットショット
                    if (_c.player.shotManager[key].param.shotMode == ShotMode.LimitShot)
                    {
                        if (_c.player.shotManager[key].bulletNum == 0)
                        {
                            _c.LeftSubValue = Time.time - _c.player.shotManager[key].lastReloadTime;
                        }
                        else
                        {
                            _c.LeftSubValue = 0;
                        }

                        _c.LeftValue = _c.player.shotManager[key].bulletNum;
                        _c.LeftGageImage.fillAmount    = _c.LeftValue / _c.LeftMaxValue;
                        _c.LeftSubGageImage.fillAmount = _c.LeftSubValue / _c.LeftSubMaxValue;
                        _c.LeftGageText.text           = _c.LeftValue.ToString();
                    }
                    break;
            }
        }

        _c.ScoreText.text = _c.player.playerSlot == PlayerSlot.PC1
            ? GameManager.Pc1Score.ToString()
            : GameManager.Pc2Score.ToString();
    }

    private void CheckStatus(PCCanvas _c)
    {
        foreach (string key in _c.player.shotManager.Keys)
        {
            Debug.Log(_c.player.shotManager[key]);
            if (_c.player.shotManager[key].param.shotMode == ShotMode.SimpleShot) { continue; }

            switch (_c.player.shotManager[key].param.gage.barType)
            {
                case GageBarType.Top:
                    if (_c.isBarTypeTop) { Debug.Log("BarTypeが重複しています。"); return; }

                    _c.isBarTypeTop = true;

                    // トップ＆チャージショット
                    if (_c.player.shotManager[key].param.shotMode == ShotMode.ChargeShot)
                    {
                        _c.TopMaxValue = _c.player.shotManager[key].param.chargeTime;
                    }

                    // トップ＆リミットショット
                    if (_c.player.shotManager[key].param.shotMode == ShotMode.LimitShot)
                    {
                        _c.TopMaxValue = _c.player.shotManager[key].param.bulletMaxNum;
                        _c.TopSubMaxValue = _c.player.shotManager[key].param.reloadTime;
                    }
                    break;

                case GageBarType.Left:
                    if (_c.isBarTypeLeft) { Debug.Log("BarTypeが重複しています。"); return; }

                    _c.isBarTypeLeft = true;

                    // レフト＆チャージショット
                    if (_c.player.shotManager[key].param.shotMode == ShotMode.ChargeShot)
                    {
                        _c.LeftMaxValue = _c.player.shotManager[key].param.chargeTime;
                    }

                    // レフト＆リミットショット
                    if (_c.player.shotManager[key].param.shotMode == ShotMode.LimitShot)
                    {
                        _c.LeftMaxValue = _c.player.shotManager[key].param.bulletMaxNum;
                        _c.LeftSubMaxValue = _c.player.shotManager[key].param.reloadTime;
                    }
                    break;
            }

        }
    }

    private void CreateUI()
    {
        pc1Canvas.mainCanvas = 
            Instantiate(Resources.Load(ResourcesPath.Ui("PC1Canvas")), GameObject.Find(CanvasName.UI).transform) as GameObject;
        pc1Canvas.mainCanvas.name = "PC1Canvas";
        pc1Canvas.mainCanvas.AddComponent<PlayerUI>().playerSlot = PlayerSlot.PC1;

        pc2Canvas.mainCanvas = 
            Instantiate(Resources.Load(ResourcesPath.Ui("PC2Canvas")), GameObject.Find(CanvasName.UI).transform) as GameObject;
        pc2Canvas.mainCanvas.name = "PC2Canvas";
        pc2Canvas.mainCanvas.AddComponent<PlayerUI>().playerSlot = PlayerSlot.PC2;
    }

    private void SettingUI(PCCanvas _c)
    {
        Canvas target = null;
        foreach (Transform child in _c.mainCanvas.GetComponentsInChildren<Transform>())
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
                                rt.sizeDelta        = new Vector2(336, 72);
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
                                _c.hitPointImage = i;
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
                                t.text                 = ((float)_c.player.hitPoint).ToString();
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

                                _c.hitPointText = t;
                                break;

                        }
                    }
                    break;

                case "TopGageCanvas":
                    target = child.gameObject.GetComponent<Canvas>();

                    if (_c.isBarTypeTop == false) { Destroy(target.gameObject); Debug.Log("top"); break; }

                    foreach (Transform c_child in target.transform)
                    {
                        switch (c_child.name)
                        {
                            case "Bar":
                                rt = c_child.gameObject.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector3(0, 0, 0);
                                rt.sizeDelta = new Vector2(336, 36);
                                rt.anchorMin = new Vector2(0, 1);
                                rt.anchorMax = new Vector2(0, 1);
                                rt.pivot = new Vector2(0, 1);
                                rt.rotation = new Quaternion(0, 0, 0, 0);
                                rt.localScale = new Vector3(1, 1, 1);

                                i = c_child.gameObject.GetComponent<Image>();
                                i.sprite = Resources.Load<Sprite>("Sprites/UI/GageBar");
                                i.color = new Color(1, 1, 1, 1);
                                i.material = null;
                                i.raycastTarget = true;
                                i.type = Image.Type.Filled;
                                i.fillMethod = Image.FillMethod.Horizontal;
                                i.fillOrigin = 0;
                                i.fillAmount = 1.0f;
                                i.fillClockwise = true;
                                i.preserveAspect = false;

                                _c.TopGageImage = i;
                                break;

                            case "Bar_sb":
                                rt = c_child.gameObject.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector3(0, 0, 0);
                                rt.sizeDelta = new Vector2(336, 36);
                                rt.anchorMin = new Vector2(0, 1);
                                rt.anchorMax = new Vector2(0, 1);
                                rt.pivot = new Vector2(0, 1);
                                rt.rotation = new Quaternion(0, 0, 0, 0);
                                rt.localScale = new Vector3(1, 1, 1);

                                i = c_child.gameObject.GetComponent<Image>();
                                i.sprite = Resources.Load<Sprite>("Sprites/UI/GageBar");
                                i.color = new Color(1, 0, 0, 0.25f);
                                i.material = null;
                                i.raycastTarget = true;
                                i.type = Image.Type.Filled;
                                i.fillMethod = Image.FillMethod.Horizontal;
                                i.fillOrigin = 0;
                                i.fillAmount = 0.0f;
                                i.fillClockwise = true;
                                i.preserveAspect = false;

                                _c.TopSubGageImage = i;
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

                                _c.TopGageText = t;
                                break;

                        }
                    }
                    break;

                case "LeftGageCanvas":
                    target = child.gameObject.GetComponent<Canvas>();

                    if (_c.isBarTypeLeft == false) { Destroy(target.gameObject); break; }

                    foreach (Transform c_child in target.transform)
                    {
                        switch (c_child.name)
                        {
                            case "Bar":
                                rt = c_child.gameObject.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector3(0, 0, 0);
                                rt.sizeDelta = new Vector2(72, 336);
                                rt.anchorMin = new Vector2(0, 1);
                                rt.anchorMax = new Vector2(0, 1);
                                rt.pivot = new Vector2(0, 1);
                                rt.rotation = new Quaternion(0, 0, 0, 0);
                                rt.localScale = new Vector3(1, 1, 1);

                                i = c_child.gameObject.GetComponent<Image>();
                                i.sprite = Resources.Load<Sprite>("Sprites/UI/GageBar");
                                i.color = new Color(1, 1, 1, 1);
                                i.material = null;
                                i.raycastTarget = true;
                                i.type = Image.Type.Filled;
                                i.fillMethod = Image.FillMethod.Vertical;
                                i.fillOrigin = 1;
                                i.fillAmount = 1.0f;
                                i.fillClockwise = true;
                                i.preserveAspect = false;

                                _c.LeftGageImage = i;
                                break;

                            case "Bar_sb":
                                rt = c_child.gameObject.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector3(0, 0, 0);
                                rt.sizeDelta = new Vector2(72, 336);
                                rt.anchorMin = new Vector2(0, 1);
                                rt.anchorMax = new Vector2(0, 1);
                                rt.pivot = new Vector2(0, 1);
                                rt.rotation = new Quaternion(0, 0, 0, 0);
                                rt.localScale = new Vector3(1, 1, 1);

                                i = c_child.gameObject.GetComponent<Image>();
                                i.sprite = Resources.Load<Sprite>("Sprites/UI/GageBar");
                                i.color = new Color(1, 0, 0, 0.25f);
                                i.material = null;
                                i.raycastTarget = true;
                                i.type = Image.Type.Filled;
                                i.fillMethod = Image.FillMethod.Vertical;
                                i.fillOrigin = 1;
                                i.fillAmount = 0.0f;
                                i.fillClockwise = true;
                                i.preserveAspect = false;

                                _c.LeftSubGageImage = i;
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

                                _c.LeftGageText = t;
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
                                i.sprite        = Resources.Load<Sprite>("Sprites/UI/" + _c.player.name + "_icon");
                                i.color         = i.sprite == null ? new Color(1.0f, 1.0f, 1.0f, 0.5333f) 
                                                                   : new Color(1.0f, 1.0f, 1.0f, 1.0f);
                                i.material      = null;
                                i.raycastTarget = true;

                                _c.iconImage = i;
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
                                t.text                 = _c.player.name;
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

                                _c.iconText = t;
                                break;

                        }
                    }
                    break;

                case "ScoreCanvas":
                    target = child.gameObject.GetComponent<Canvas>();
                    foreach (Transform c_child in target.transform)
                    {
                        switch (c_child.name)
                        {
                            case "Text":
                                /*
                                rt = c_child.gameObject.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector3(0, 0, 0);
                                rt.sizeDelta = new Vector2(108, 108);
                                rt.anchorMin = new Vector2(0, 1);
                                rt.anchorMax = new Vector2(0, 1);
                                rt.pivot = new Vector2(0, 1);
                                rt.rotation = new Quaternion(0, 0, 0, 0);
                                rt.localScale = new Vector3(1, 1, 1);
                                */
                                t = c_child.gameObject.GetComponent<Text>();
                                /*
                                t.text = "0";
                                t.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                                t.fontStyle = FontStyle.Normal;
                                t.fontSize = 18;
                                t.supportRichText = true;
                                t.alignment = TextAnchor.LowerLeft;
                                t.alignByGeometry = false;
                                t.horizontalOverflow = HorizontalWrapMode.Wrap;
                                t.verticalOverflow = VerticalWrapMode.Truncate;
                                t.resizeTextForBestFit = false;
                                t.color = new Color(0.196f, 0.196f, 0.196f, 1.0f);
                                t.material = null;
                                t.raycastTarget = true;
                                */
                                _c.ScoreText = t;
                                break;
                        }
                    }
                    break;
            }
        }
    }

    public static void DestroyMe(GameObject _gameObject)
    {
        pc1Canvas = new PCCanvas();
        pc2Canvas = new PCCanvas();

        inst.MyProc.Reset();
        inst = null;
        Debug.Log("Destroy:PlayerUIManager");
        Destroy(_gameObject);
    }
}
