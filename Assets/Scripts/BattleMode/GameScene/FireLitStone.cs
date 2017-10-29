using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manager関連を生成するクラス 火つけ石
public class FireLitStone : MonoBehaviour
{
    public static FireLitStone fls;
    public string pc1Name;
    public string pc2Name;
    
    void Start ()
    {
        fls = gameObject.GetComponent<FireLitStone>();
        string tmp;
        tmp = PlayerManager.Inst.name;
        tmp = GameManager.Inst.name;
        tmp = PlayerUIManager.Inst.name;
        Destroy(this.gameObject);
	}
}
