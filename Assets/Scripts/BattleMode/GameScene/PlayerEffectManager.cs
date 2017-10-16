using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerEffectManager : MonoBehaviour
{
    private enum EffectType
    {
        ChargeEffect,
    }

    private Player player;

    // チャージエフェクト
    private ParticleSystem chargeEffect;
    private GameObject changeEffectObj;

    private void Init()
    {
        player = GetComponent<Player>();
        
        changeEffectObj = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/ChargeEffect"), transform.position, Quaternion.identity);
        changeEffectObj.transform.parent = transform;
        chargeEffect = changeEffectObj.GetComponent<ParticleSystem>();

        Stop(EffectType.ChargeEffect);
    }

    private void Start()
    {
        Init();
    }

    private void Update ()
    {
        foreach (string key in player.shotManager.Keys)
        {
            switch (player.shotManager[key].param.shotMode)
            {
                case ShotMode.SimpleShot:

                    break;

                case ShotMode.ChargeShot:
                    if (player.state == key + "(KeyUp)")
                    {
                        Stop(EffectType.ChargeEffect); break;
                    }
                    if (player.state == key && chargeEffect.isStopped)
                    {
                        Play(EffectType.ChargeEffect);
                    }
                    break;

                case ShotMode.LimitShot:

                    break;
            }

        }
    }

    private void Play(EffectType _type)
    {
        switch (_type)
        {
            case EffectType.ChargeEffect:
                chargeEffect.Play();
                changeEffectObj.gameObject.SetActive(true);
                break;
        }
    }

    private void Stop(EffectType _type)
    {
        switch (_type)
        {
            case EffectType.ChargeEffect:
                changeEffectObj.gameObject.SetActive(false);
                chargeEffect.Stop();
                break;
        }
    }

    private void Pause(EffectType _type)
    {
        switch (_type)
        {
            case EffectType.ChargeEffect:
                changeEffectObj.gameObject.SetActive(false);
                chargeEffect.Pause();
                break;
        }
    }
}
