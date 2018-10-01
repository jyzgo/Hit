using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MTUnity.Actions;

public class FloatFont : MonoBehaviour {

    public Text text;
    // Use this for initialization
    const float FADE_TIME = 1.5f;
    public void SetShow(Vector3 pos,string str)
    {
        transform.position = pos;
        text.text = str;
        this.RunActions(new MTScaleBy(0.1f,2f),new MTScaleBy(0.1f,0.4f), new MTMoveBy(FADE_TIME , Vector3.up *1.5f),new MTDestroy());
        text.gameObject.RunAction(new MTFontFadeTo(FADE_TIME,0));
	       
    }


    
	

}
