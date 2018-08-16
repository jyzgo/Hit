using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MTUnity.Actions;

public enum BombType
{
    
    Square,
    Up,
    Right,
    Down,
    Left,
    None
}

public enum CellType
{
    Cell,
    Bomb
}

public class Cell : MonoBehaviour {

    public SpriteRenderer CellBg;
    public SpriteRenderer CellCenter;
    public Transform CellCenterTrans;
    public SpriteRenderer Number;
    public Text TestText;

    public CellType _cellType = CellType.Cell;
    public BombType _bombType = BombType.None;

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

    public Unit unit = null;
    const int BOMBINDEX = 6;
    int number = 2;
    public int pow = 0;
    public void SetPow(int n)
    {
        pow = n;
        var bgColor = ResMgr.Current.cbg[n  ];
        var cellColor = ResMgr.Current.Colors[n];
        SetCenterColor(cellColor);
        SetBgColor(bgColor);
        if (pow < BOMBINDEX)
        {
            number = (int)System.Math.Pow(2, n);
            Number.sprite = ResMgr.Current.Numbers[n];
        }
        else
        {
            _cellType = CellType.Bomb;
            int index = MTRandom.GetRandomInt(0, 4);
            _bombType = (BombType)index;
            Number.sprite = ResMgr.Current.Bombs[index];
            

        }
   
    }

    internal void MoveToUnit()
    {
        var tarPos = TargetPos();
        this.RunAction(new MTMoveTo(0.1f, tarPos));
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
            LevelMgr.Current.DelayCheckMerge();
            return;
        }

        Cell anotherCell = collision.GetComponent<Cell>();
        if(anotherCell !=null)
        {
            isAttached = true;
            isCellActive = false;
            if (anotherCell.pow!= this.pow)
            {


                transform.position = anotherCell.transform.position + Vector3.down * 0.426f;
                transform.SetParent(_center.transform);
                this.RunAction(GetScale());

                SetLocalCoord();
                if (anotherCell._cellType == CellType.Bomb)
                {
                    anotherCell.BombTrigger();
                }
            }
            else
            {

                MergeTo(anotherCell);
            }
            LevelMgr.Current.DelayCheckMerge();

        }

    }

    bool isBombTriggering = false;
    private void BombTrigger()
    {
        isBombTriggering = true;
        if (_bombType == BombType.Square)
        {
            int startX = unit.x-1;
            int startY = unit.y - 1;
            int endX = unit.x + 1;
            int endY = unit.y + 1;
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (x < 0 || x >= LevelMgr.MAX_SIZE)
                    {
                        continue;
                    }
                    if (y < 0 || y >= LevelMgr.MAX_SIZE)
                    {
                        continue;
                    }

                    LevelMgr.Current.Explode(x, y);
                                        
                }
            }
        }
        else if (_bombType == BombType.Up)
        {

        }
        else if (_bombType == BombType.Right)
        { }
        else if (_bombType == BombType.Down)
        { }
        else if (_bombType == BombType.Left)
        {
        }
        
    }

    bool isDestroying = false;
    private void OnDestroying()
    {
        isDestroying = true;
    }

    public const float MERGE_TIME = 0.1f;
    public bool isMerging = false;
    public void MergeTo(Cell anotherCell)
    {
        isMerging = true;
        this.RunActions(new MTMoveTo(MERGE_TIME, anotherCell.transform.position+ Vector3.back,true), new MTCallFunc(() =>
        {
            if (unit != null)
            {
                unit.cell = null;
            }

            anotherCell.IncreaseNum();
                }),new MTDestroy());
    }

    public int corX = 0;
    public int corY = 0;
    public void SetLocalCoord()
    {
        corX = (int)(Math.Round(transform.localPosition.x / CELL_SIZE)) + 10;
        corY =(int)(Math.Round(transform.localPosition.y / CELL_SIZE)) + 10;
        SetTestText();
        LevelMgr.Current.SetCells(corX, corY, this);
    }

    public void SetTestText()
    {
        TestText.text = corX.ToString() +" "+  corY.ToString();
    }

    public void SetPostion(int x, int y)
    {
        corX = x;
        corY = y;
        transform.localPosition = TargetPos();
        transform.localRotation = Quaternion.identity;
    }

    Vector3 TargetPos()
    {
        return new Vector3((corX - 10) * CELL_SIZE, (corY - 10) * CELL_SIZE, 0);
    }

    private void IncreaseNum()
    {
        SetPow(pow + 1);
        this.RunAction(GetScale());
    }

    MTFiniteTimeAction GetScale()
    {
        return new MTSequence(new MTScaleTo(0.1f,0.8f),new MTScaleTo(0.08f,1f));
    }
}
