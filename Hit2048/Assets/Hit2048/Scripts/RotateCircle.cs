using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCircle : MonoBehaviour {


    public void Playing_Update()
   {
        transform.Rotate(new Vector3(0, 0, -1f));
   }

	
}
