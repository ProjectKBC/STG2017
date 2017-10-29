using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Anoma
{
    // 自動生成化したい!!!
    public class Json
    {
        public string name;

    }

    // シングルトン
    public class Anoma : MonoBehaviour
    {
        private static Anoma inst;
        private Anoma()
        {
            Json json = JsonUtility.FromJson<Json>("/");
            name = json.name;
        }
        public static Anoma Inst
        {
            get
            {
                if (inst == null) { inst = new Anoma(); }

                return inst;
            }
        }

        private void Start()
        {
            GameObject Anoma = new GameObject("Anoma");
            Anoma.AddComponent<AnomaPlayer>();
            Anoma.AddComponent<AnomaNSM>();
            Anoma.AddComponent<AnomaUSM>();
            Anoma.AddComponent<AnomaSkill>();
        }
    }
}
