using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEffect : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.layer == LayerName.UI)
        {

        }
    }
}
