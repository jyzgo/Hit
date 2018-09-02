//#define TEST_RES
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResMgr : MonoBehaviour {


    public static ResMgr Current;
    private void Awake()
    {
        Current = this;
    }

    public GameObject FloatText;
    public GameObject FireWorkPrefab;
    public GameObject CoinPrefab;
    public GameObject SquareExplosionPrefab;
    public Color[] cbg;
    public Color[] Colors;

    public GameObject Cell;

    Cell[] cells = new Cell[10];
    private void Start()
    {
#if (TEST_RES)
        Test();
#endif
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
            cell.SetPow(test[i % test.Length]);
        }
    }

    int[] test = new int[4] { 5, 55, 555, 2048 };

    private void Update()
    {
        //TestUpdate();
    }

    public Sprite[] Numbers;
    public Sprite[] Bombs;



}
