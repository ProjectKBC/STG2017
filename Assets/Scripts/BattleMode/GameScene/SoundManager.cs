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
        Init();
        MyProc.started = true;

        yield return new WaitWhile(() => NoaProcesser.IsStayBoss());
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
