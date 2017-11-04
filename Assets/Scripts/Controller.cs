using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct AudioClips
{
    public List<AudioClip> BGM_Clips;
    public List<AudioClip> SE_Clips;
}

// f:シングルトン
public sealed class SoundPlayer : SingletonMonoBehaviour<SoundPlayer>
{
    public static SoundPlayer Inst { get; private set; }
    private SoundPlayer() { }

    private AudioSource audioSource;
    private AudioClips  header;

    // f:インスタンス生成時に呼び出されるStart()より呼び出しの早い関数
    private void Awake()
    {
        Initialization();
    }

    private void Start()
    {
        // f:GetComponent系はここで。
        Inst = GetComponent<SoundPlayer>();

    }

    private void Initialization()
    {

        // f:static変数の初期化
        audioSource = Inst.gameObject.AddComponent<AudioSource>();
        header = new AudioClips();
    }
}

public class Controller : SingletonMonoBehaviourFast<Controller>
{

}
