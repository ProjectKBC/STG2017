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

        Debug.Log(PlayerManager.Inst.name);
        Debug.Log(GameManager.Inst.name);

        Destroy(this.gameObject);
	}
}
