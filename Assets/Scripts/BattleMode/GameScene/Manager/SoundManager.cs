using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SoundManager : NoaBehaviour
{
    private static SoundManager inst;
    private SoundManager() { }
    public static SoundManager Inst
    {
        get
        {
            if (inst == null)
            {
                GameObject go = new GameObject("SoundManager");
                go.transform.parent = GameObject.Find("Managers").transform;
                inst = go.AddComponent<SoundManager>();
            }

            return inst;
        }
    }

    private static AudioSource audioSource;
    private static List<AudioClip> BGMs = new List<AudioClip>();

    private void Init()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        BGMs.Add(Resources.Load<AudioClip>("Sounds/BGMs/gameBGM1"));
        BGMs.Add(Resources.Load<AudioClip>("Sounds/BGMs/gameBGM2"));
        BGMs.Add(Resources.Load<AudioClip>("Sounds/BGMs/gameBGM3"));
    }

    protected override IEnumerator Start()
    {
        yield return new WaitUntil(() => LoadingManager.Inst.MyProc.started);
        Debug.Log("_3:SoundManagerが呼び出される。");

        Init();

        Debug.Log("4:SoundManagerが音声を読み込む。");
        MyProc.started = true;

        yield return new WaitUntil(() => NoaProcesser.BossProc.started);

        System.Random r = new System.Random();
        audioSource.clip = BGMs[r.Next(BGMs.Count - 1)];
        audioSource.Play();
    }

    private void Update()
    {
        if (NoaProcesser.IsStayBoss()) { return; }
        
    }

    public static void PlayOneShot(AudioClip _audioClip)
    {
        audioSource.PlayOneShot(_audioClip);
    }
}
