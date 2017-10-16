using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manager関連を生成するクラス 火つけ石
public class FireLitStone : MonoBehaviour
{
    public string _pc1Name;
    public string _pc2Name;

    void Start ()
    {
        Debug.Log(PlayerManager.Inst.name);
        Debug.Log(GameManager.Inst.name);

        GameManager.Inst.pc1Name = _pc1Name;
        GameManager.Inst.pc2Name = _pc2Name;
        Destroy(this.gameObject);
	}
}
