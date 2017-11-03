using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashing : NoaBehaviour
{
    public float maxValue;
    public float minValue;
    public float intervalTime;
    public float bornTime;

    private Image image;
    private float ElapseTime() { return Time.time - bornTime; }

    protected override IEnumerator Start ()
    {
        maxValue %= 255;
        minValue %= 255;
        image = GetComponent<Image>();
        yield return null;	
	}
	
	void Update ()
    {
        image.color = new Color(0,0,0, ((ElapseTime() % intervalTime) + minValue) / (maxValue - minValue));
	}
}
