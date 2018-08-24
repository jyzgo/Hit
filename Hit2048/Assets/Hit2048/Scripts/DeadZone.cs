using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour {

    private void Awake()
    {
        
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3( Screen.width/2,Screen.height, 0));
        transform.position = worldPoint - Vector3.down*0.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Cell cell = collision.GetComponent<Cell>();
        if (cell != null)
        {
            if (!cell.isAttached)
            {
                Destroy(cell.gameObject);
                LevelMgr.Current.ChangeToRotating();
            }
        }
    }
}
