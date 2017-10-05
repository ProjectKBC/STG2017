using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Veronica
{
    // 自動生成化したい!!!
    public class Json
    {
        public string name;

    }

    // シングルトン
    public class Veronica : MonoBehaviour
    {
        private string name;

        private static Veronica inst;
        private Veronica()
        {
            Json json = JsonUtility.FromJson<Json>("/");
            name = json.name;
        }
        public static Veronica Inst
        {
            get
            {
                if (inst == null) { inst = new Veronica(); }

                return inst;
            }
        }

        private void Start()
        {
            GameObject veronica = new GameObject("veronica");
            veronica.AddComponent<VeronicaPlayer>();
            veronica.AddComponent<VeronicaNSM>();
            veronica.AddComponent<VeronicaUSM>();
            veronica.AddComponent<VeronicaSkill>();
        }
    }
}