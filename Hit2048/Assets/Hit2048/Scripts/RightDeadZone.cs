using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightDeadZone : DeadZone{

    protected override void InitPosition()
    {
       Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3( Screen.width,Screen.height/2, 0));
        transform.position = worldPoint + Vector3.right*0.8f;
    }
}
