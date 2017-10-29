using System.Collections;
using System.Collections.Generic;
using UnityEngine;


<<<<<<< HEAD
public class PlayerEffectManager : MonoBehaviour
=======
public class PlayerEffectManager : NoaBehaviour
>>>>>>> test
{
    private enum EffectType
    {
        ChargeEffect,
    }

    private Player player;
    
    // チャージエフェクト
    private List<ParticleSystem> chargeEffect;
    private GameObject changeEffectObj;


    private void Init()
    {
        player = GetComponent<Player>();
        
        // チャージエフェクト
        changeEffectObj = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/ChargeEffect"), transform.position, Quaternion.identity);
        changeEffectObj.transform.parent = transform;
        chargeEffect = new List<ParticleSystem>(changeEffectObj.GetComponentsInChildren<ParticleSystem>());
        Stop(EffectType.ChargeEffect);
    }

<<<<<<< HEAD
    private void Start()
    {
        Init();
    }

    private void Update ()
    {
=======
    protected override IEnumerator Start()
    {
        yield return PlayerManager.Inst.MyProc.Stay();

        Init();
        MyProc.started = true;

        yield return NoaProcesser.StayBoss();
    }

    protected void Update ()
    {
        if (MyProc.IsStay() || NoaProcesser.IsStayBoss()) { return; }

>>>>>>> test
        foreach (string key in player.shotManager.Keys)
        {
            switch (player.shotManager[key].param.shotMode)
            {
                case ShotMode.SimpleShot:

                    break;

                case ShotMode.ChargeShot:
<<<<<<< HEAD
                    if (player.state == key + "(KeyUp)")
                    {
                        Stop(EffectType.ChargeEffect); break;
                    }
=======
>>>>>>> test
                    if (player.state == key)
                    {
                        foreach (ParticleSystem x in chargeEffect)
                        {
                            if (x.isStopped == false) { goto BREAK; }
                        }
                        Play(EffectType.ChargeEffect);
                    }
<<<<<<< HEAD
=======
                    else
                    {
                        Stop(EffectType.ChargeEffect); break;
                    }
>>>>>>> test
                    BREAK: ;
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
