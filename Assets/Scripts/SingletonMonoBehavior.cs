/*  参考：http://tsubakit1.hateblo.jp/entry/20140709/1404915381
 *  Unityで少しだけ高速なシングルトン(Singleton) - テラシュールブログ
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    Debug.LogWarning(typeof(T) + "is nothing");
                }
            }

            return instance;
        }
    }

    protected void Awake()
    {
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = this as T;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }

        Destroy(this);
        return false;
    }
}

public abstract class SingletonMonoBehaviourFast<T> : MonoBehaviour where T : SingletonMonoBehaviourFast<T>
{
    protected static readonly string[] findTags =
    {
        "GameController",
    };

    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {

                Type type = typeof(T);

                foreach (var tag in findTags)
                {
                    GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);

                    for (int j = 0; j < objs.Length; j++)
                    {
                        instance = objs[j].GetComponent<T>();
                        if (instance != null)
                            return instance;
                    }
                }

                Debug.LogWarning(string.Format("{0} is not found", type.Name));
            }

            return instance;
        }
    }

    virtual protected void Awake()
    {
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = this as T;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }

        Destroy(this);
        return false;
    }
}
