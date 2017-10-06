using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Held
{
	// 自動生成化したい!!!
	public class Json
	{
		public string name;

	}

	// シングルトン
	public class Held : MonoBehaviour
	{
		private string name;

		private static Held inst;
		private Held()
		{
			Json json = JsonUtility.FromJson<Json>("/");
			name = json.name;
		}
		public static Held Inst
		{
			get
			{
				if (inst == null) { inst = new Held(); }

				return inst;
			}
		}

		private void Start()
		{
			GameObject held = new GameObject("held");
			held.AddComponent<HeldPlayer>();
			held.AddComponent<HeldNSM>();
			held.AddComponent<HeldUSM>();
			held.AddComponent<HeldSkill>();
		}
	}
}