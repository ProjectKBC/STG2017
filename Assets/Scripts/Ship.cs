﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Ship : MonoBehaviour
{
    public float maxHitPoint;
    public float speed;
}
