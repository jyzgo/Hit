using System.Collections;
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

    private void Start()
    {
        
    }

    

    void Test()
    {

    }




}
