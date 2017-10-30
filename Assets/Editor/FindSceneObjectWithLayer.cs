// by yano_123
// https://qiita.com/yano_123/items/bd27606c2e3a3275f6ee

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FindSceneObjectsWithLayer : EditorWindow
{
    public int LayerToSearchFor = 0;

    public bool LimitResultCount = false;
    public int MaxResults = 1;

    public List<GameObject> Results;
    private Vector2 ResultScrollPos;

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.LabelField("Options", EditorStyles.boldLabel);
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Layer : ", GUILayout.MaxWidth(60));
                LayerToSearchFor = EditorGUILayout.LayerField(LayerToSearchFor);
                GUILayout.EndHorizontal();

                if (GUILayout.Button("Find"))
                    Find();

                if (LimitResultCount = EditorGUILayout.Foldout(LimitResultCount, "Limit Result Count (Limit:"
                        + (LimitResultCount ? MaxResults.ToString() : "None") + ")"))
                    MaxResults = EditorGUILayout.IntField("Result Max:", MaxResults);
            }

            EditorGUILayout.LabelField("Results", EditorStyles.boldLabel);
            {
                if (Results != null)
                {
                    EditorGUILayout.LabelField("Scene objects found:", Results.Count.ToString(), EditorStyles.boldLabel);

                    ResultScrollPos = EditorGUILayout.BeginScrollView(ResultScrollPos);
                    {
                        if (LimitResultCount)
                        {
                            for (int i = 0; i < Mathf.Min(MaxResults, Results.Count); i++)
                                EditorGUILayout.ObjectField(Results[i], typeof(GameObject), false);
                        }
                        else
                        {
                            foreach (GameObject go in Results)
                                EditorGUILayout.ObjectField(go, typeof(GameObject), false);
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
            }
        }
        EditorGUILayout.EndVertical();
    }

    void Find()
    {
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        Results = new List<GameObject>();
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == LayerToSearchFor)
            {
                Results.Add(goArray[i]);
            }
        }
    }

    [MenuItem("Tools/Find By Layer...")]
    static void Init()
    {
        FindSceneObjectsWithLayer window = EditorWindow.GetWindow<FindSceneObjectsWithLayer>("Find By Layer");
        window.ShowPopup();
        //window.ShowAuxWindow();
    }
}
