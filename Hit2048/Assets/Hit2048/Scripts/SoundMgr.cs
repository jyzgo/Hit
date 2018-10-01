using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour {

    public AudioClip impactsound;
    public AudioClip mergersound;
    public AudioClip coinCameOutSound;
    public AudioClip coinTakenSound;
    public AudioClip rocketSound;
    public AudioClip bombSound;


    SFXPool _sfx;
    public static SoundMgr Current;
    private void Awake()
    {
        Current = this;
        _sfx = SFXPool.Instance;
        _sfx.Init(5);
    }

    public void PlayImpactSound()
    {

        _sfx.PlayClip(impactsound);
       
    }

    public void PlayMergeSound()
    {
        
        _sfx.PlayClip(mergersound,1,LevelMgr.Current._mergeTime *0.1f + 0.9f);
    }

    public void PlayCoinCameOut()
    {
        _sfx.PlayClip(coinCameOutSound);
    }

    public void PlayCoinTaken()
    {
        _sfx.PlayClip(coinTakenSound);
    }

    public void PlayRocketSound()
    {
        _sfx.PlayClip(rocketSound);
    }

    public void PlayBombSound()
    {
        _sfx.PlayClip(bombSound);
    }


}
