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
    public Text TestText;

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

    public int number = 2;
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
    public bool isAttached = false;

    const float CELL_SIZE = 0.426f;
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
            transform.position = center.transform.position + Vector3.down * CELL_SIZE;
            transform.SetParent(_center.transform);
            this.RunAction(GetScale());
            SetLocalCoord();
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
                MergeTo(anotherCell); 
            }
            SetLocalCoord();

        }

    }

    public void MergeTo(Cell anotherCell)
    {
        this.RunActions(new MTMoveTo(0.1f, anotherCell.transform.position + Vector3.back), new MTCallFunc(() =>
        {
            anotherCell.IncreaseNum();
                }),new MTDestroy());
    }

    public int corX = 0;
    public int corY = 0;
    void SetLocalCoord()
    {
        corX = (int)(Math.Round(transform.localPosition.x / CELL_SIZE)) + 10;
        corY =(int)(Math.Round(transform.localPosition.y / CELL_SIZE)) + 10;

        TestText.text = corX.ToString() +" "+  corY.ToString();
        LevelMgr.Current.SetCells(corX, corY, this);
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
