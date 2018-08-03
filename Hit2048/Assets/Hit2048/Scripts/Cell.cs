using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MTUnity.Actions;

public class Cell : MonoBehaviour {

    public SpriteRenderer CellBg;
    public SpriteRenderer CellCenter;
    public Transform CellCenterTrans;
    public SpriteRenderer Number;

    RotateCenter _center;
    private void Start()
    {
        _center = LevelMgr.Current.RotateCircle;
    }

    public void SetBgColor(Color c)
    {
        CellBg.color = c;
    }

    public void SetCenterColor(Color c)
    {
        CellCenter.color = c;
    }

    int number = 2;
    int pow = 0;
    public void SetNum(int n)
    {
        Debug.Assert(n > 0);
        pow = n;
        var bgColor = ResMgr.Current.cbg[n - 1];
        var cellColor = ResMgr.Current.Colors[n - 1];
        SetBgColor(bgColor);
        SetCenterColor(cellColor);


        number = (int)System.Math.Pow(2,n);
        Number.sprite = ResMgr.Current.Numbers[n - 1];
   
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
        CellCenterTrans.rotation = Quaternion.identity; 
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
            transform.position = center.transform.position + Vector3.down * 0.426f;
            transform.SetParent(center.transform);
            transform.SetParent(_center.transform);
            this.RunAction(GetScale());
        }

        Cell anotherCell = collision.GetComponent<Cell>();
        if(anotherCell !=null)
        {
            isAttached = true;
            isCellActive = false;
            if (anotherCell.number != this.number)
            {
                transform.position = anotherCell.transform.position + Vector3.down * 0.426f;
                transform.SetParent(_center.transform);
                this.RunAction(GetScale());
            }else
            {
                this.RunActions(new MTMoveTo(0.1f, anotherCell.transform.position + Vector3.back),new MTCallFunc(()=> {
                    anotherCell.IncreaseNum();
                }),new MTDestroy());
            }
        }
    }

    private void IncreaseNum()
    {
        SetNum(pow + 1);
        this.RunAction(GetScale());
    }

    MTFiniteTimeAction GetScale()
    {
        return new MTSequence(new MTScaleTo(0.1f,0.8f),new MTScaleTo(0.08f,1f));
    }
}
