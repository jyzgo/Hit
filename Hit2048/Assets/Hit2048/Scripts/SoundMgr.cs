using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour {

    public AudioClip impactsound;
    public AudioClip mergersound;
    public AudioClip coinSound;
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
        _sfx.PlayClip(mergersound);
    }



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
