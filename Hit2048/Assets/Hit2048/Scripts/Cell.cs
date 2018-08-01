using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour {

    public SpriteRenderer CellBg;
    public SpriteRenderer CellCenter;
    public Text text;

    public void SetBgColor(Color c)
    {
        CellBg.color = c;
    }

    public void SetCenterColor(Color c)
    {
        CellCenter.color = c;
    }

    int number = 2;
    public void SetNum(int n)
    {
        Debug.Assert(n > 0);
        var bgColor = ResMgr.Current.cbg[n - 1];
        var cellColor = ResMgr.Current.Colors[n - 1];
        SetBgColor(bgColor);
        SetCenterColor(cellColor);


        number = (int)System.Math.Pow(2,n);
        if (number< 10)
        {
            text.fontSize = 80;
        }
        else if (number >= 10 && n < 100)
        {
            text.fontSize = 60;
        }
        else if (number >= 100 && n < 1000)
        {
            text.fontSize = 55;
        } else
        {
            text.fontSize = 40;
        }
        text.text = number.ToString();
    }

    bool isCellActive = false;
    internal void SetActive()
    {
        isCellActive = true;
    }
    private void Update()
    {
        if(isCellActive)
        {
            transform.Translate(Vector3.up * 0.1f);
        }
    }
    bool isAttached = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttached)
        {
            return;
        }

        RotateCenter center = collision.GetComponent<RotateCenter>();
        if(center != null)
        {
            isAttached = true;
            isCellActive = false;
            transform.position = center.transform.position + Vector3.down * 1.29f;
            transform.SetParent(center.transform);
        }
    }
}
