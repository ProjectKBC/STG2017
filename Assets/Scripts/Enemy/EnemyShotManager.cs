using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotManager : MonoBehaviour
{
    public Bullet bullet;     // 弾のPrefab
    [SerializeField]
    public BulletParam param; // パラメータクラス
}
