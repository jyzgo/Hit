using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTUnity.Actions;

public class FireWork : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 2f);
        this.RunActions(new MTScaleBy(0.1f, 1.5f), new MTScaleBy(0.12f, 1f));
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.up * 0.1f;
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Cell cell = collision.GetComponent<Cell>();
        if (cell != null && cell.isAttached)
        {
            cell.DestoryAndGenCoin();
        }
    }

   
}
