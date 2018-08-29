using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTUnity.Actions;
using System;

public class Coin : MonoBehaviour {
    internal bool isDestroying = false;

    // Use this for initialization
    void Start () {
        this.RunActions(new MTRepeatForever(new MTScaleTo(0.8f, 0.6f), new MTScaleTo(0.8f, 0.8f)));
        LevelMgr.Current.AddCoin(this);
	}
	

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Cell cell = collision.GetComponent<Cell>();
        if (cell != null)
        {
            if (!cell.isAttached)
            {
                SoundMgr.Current.PlayCoinTaken();
                Destroy(gameObject);
            }
        }
    }



    private void OnDestroy()
    {
        LevelMgr.Current.RemoveCoin(this);
    }

    internal void DestorySelf()
    {
        isDestroying = true;
        Destroy(gameObject);
    }
}
