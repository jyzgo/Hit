﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResMgr : MonoBehaviour {

    public static ResMgr Current;
    private void Awake()
    {
        Current = this;
    }
    public GameObject SmallSquare;

    public Color[] cbg;
    public Color[] Colors;

    public GameObject Cell;

    Cell[] cells = new Cell[10];
    private void Start()
    {
        Test();
    }

    

    void Test()
    {
        for (int i = 0; i < 10; i++)
        {
            var gb = Instantiate<GameObject>(Cell);
            gb.transform.position = new Vector3(0, 1 - i * 0.6f, 0);
            var cell = gb.GetComponent<Cell>();
            cells[i] = cell;
        }
    }

    void TestUpdate()
    {
        for(int i =0; i <cells.Length;i++)
        {
            var cell = cells[i];
            cell.SetBgColor(cbg[i]);
            cell.SetCenterColor(Colors[i]);
        }
    }

    private void Update()
    {
        TestUpdate();
    }




}
