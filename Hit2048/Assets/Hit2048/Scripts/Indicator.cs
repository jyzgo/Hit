using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour {

    SpriteRenderer _spRender;
    // Use this for initialization
    Color _currentColor;
    float _currentAlpha;
	void Start () {
        _spRender = GetComponent<SpriteRenderer>();
        _currentColor = _spRender.color;
        _currentAlpha = _currentColor.a;
	}
    float INTERVER = 0.02f;
	// Update is called once per frame
	void Update () {
        _currentColor = new Color(_currentColor.r, _currentColor.g, _currentColor.b, _currentAlpha);
        _spRender.color = _currentColor;
        _currentAlpha += INTERVER;
        if(_currentAlpha >1 || _currentAlpha <0)
        {
            INTERVER *= -1;
        }
		
	}
}
