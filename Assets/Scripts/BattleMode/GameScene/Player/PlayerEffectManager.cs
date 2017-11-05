using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerEffectManager : NoaBehaviour
{
    private enum EffectType
    {
        ChargeEffect,
    }

    private Player player;

    // チャージエフェクト
    private List<ParticleSystem> chargeEffect;
    private List<ParticleSystem> chargeEffect_ready;
    private GameObject changeEffectObj;
    private GameObject changeEffect_readyObj;


    private void Init()
    {
        player = GetComponent<Player>();

        // チャージエフェクト
        changeEffectObj = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/ChargeEffect"), transform.position, Quaternion.identity);
        changeEffectObj.transform.parent = transform;
        chargeEffect = new List<ParticleSystem>(changeEffectObj.GetComponentsInChildren<ParticleSystem>());
        changeEffect_readyObj = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/ChargeEffect_ready"), transform.position, Quaternion.identity);
        changeEffect_readyObj.transform.parent = transform;
        chargeEffect_ready = new List<ParticleSystem>(changeEffect_readyObj.GetComponentsInChildren<ParticleSystem>());

        changeEffectObj.gameObject.SetActive(false);
        foreach (ParticleSystem x in chargeEffect) { x.Stop(); }
        changeEffect_readyObj.gameObject.SetActive(false);
        foreach (ParticleSystem x in chargeEffect_ready) { x.Stop(); }
    }

    protected override IEnumerator Start()
    {
        yield return new WaitWhile( () => PlayerManager.Inst.MyProc.IsStay());

        Init();
        MyProc.started = true;

        yield return new WaitWhile(() => NoaProcesser.IsStayBoss() || NoaProcesser.IsStayPC(player.playerSlot));
    }

    protected void Update ()
    {
        if (MyProc.IsStay() || NoaProcesser.IsStayBoss() || NoaProcesser.IsStayPC(player.playerSlot)) { return; }

        foreach (string key in player.shotManager.Keys)
        {
            switch (player.shotManager[key].param.shotMode)
            {
                case ShotMode.SimpleShot:

                    break;

                case ShotMode.ChargeShot:
                    if (player.state == key && player.shotManager[key].CanChargeShot == false)
                    {
                        if (chargeEffect[0].isStopped)
                        {
                            foreach (ParticleSystem x in chargeEffect) { x.Play(); }
                            changeEffectObj.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        changeEffectObj.gameObject.SetActive(false);
                        foreach (ParticleSystem x in chargeEffect) { x.Stop(); }
                    }

                    if (player.state == key && player.shotManager[key].CanChargeShot == true)
                    {

                        if (chargeEffect_ready[0].isStopped)
                        {
                            foreach (ParticleSystem x in chargeEffect_ready) { x.Play(); }
                            changeEffect_readyObj.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        changeEffect_readyObj.gameObject.SetActive(false);
                        foreach (ParticleSystem x in chargeEffect_ready) { x.Stop(); }
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
                foreach(ParticleSystem x in chargeEffect) { x.Play(); }
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
                foreach (ParticleSystem x in chargeEffect) { x.Stop(); }
                break;
        }
    }

    private void Pause(EffectType _type)
    {
        switch (_type)
        {
            case EffectType.ChargeEffect:
                changeEffectObj.gameObject.SetActive(false);
                foreach (ParticleSystem x in chargeEffect) { x.Pause(); }
                break;
        }
    }
}
