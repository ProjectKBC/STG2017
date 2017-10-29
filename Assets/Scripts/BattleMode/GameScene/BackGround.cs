using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BackGround : MonoBehaviour
{
    public float ScrollSpeed = 2f;
    private MeshRenderer mr;

    float offset = 0f;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        offset = Mathf.Repeat(Time.time * ScrollSpeed, 1f);
        mr.material.SetTextureOffset("_MainTex", new Vector2(0f, offset));
    }
}

