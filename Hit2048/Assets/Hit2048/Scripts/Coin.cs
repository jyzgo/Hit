using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTUnity.Actions;

public class Coin : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.RunActions(new MTRepeatForever(new MTScaleTo(0.8f, 0.6f), new MTScaleTo(0.8f, 0.8f)));
	}
	

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Cell cell = collision.GetComponent<Cell>();
        if (cell != null)
        {
            if (!cell.isAttached)
            {
                Destroy(gameObject);
            }
        }
    }
}
