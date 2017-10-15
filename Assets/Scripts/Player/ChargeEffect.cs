using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEffect : MonoBehaviour {

  ParticleSystem chargeeffect; //チャージエフェクト

	void Start () {
		chargeeffect = this.GetComponent<ParticleSystem> (); //コンポーネントの取得
    chargeeffect.Stop(); //止める
	}
	
	void Update () {
		//chargeeffect.Play(); これでParticleSystemを起動できる
	}
}
