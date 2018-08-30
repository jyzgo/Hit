using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftDeadZone : DeadZone{

    // Use this for initialization
    protected override void InitPosition()
    {
       Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3( 0,Screen.height/2, 0));
        transform.position = worldPoint + Vector3.left*0.5f;
    }
}
