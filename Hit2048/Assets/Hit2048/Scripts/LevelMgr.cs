using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using MonsterLove.StateMachine;
using MTUnity.Actions;
using Destructible2D;

enum PlayState
{
    Ready,
    Playing,
    Lose
};
public class LevelMgr :MonoBehaviour
{
    

    public static LevelMgr Current;


    StateMachine<PlayState> _fsm;
    UIMgr uiMgr;
    Camera mainCam;
    public void Init()
    {
        Physics.gravity = new Vector3(0, -30.0F, 0);
        uiMgr = FindObjectOfType<UIMgr>();

        _fsm = StateMachine<PlayState>.Initialize(this, PlayState.Ready);
        mainCam = Camera.main;

        _rotateCircle = GameObject.FindObjectOfType<RotateCircle>();


    }

    RotateCircle _rotateCircle;

    void Awake()
    {
        Current = this;
        Init();
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Ready_Enter()
    {
        Debug.Log("Ready");
        uiMgr.SetStateText("Get Ready!");
        Reset();
        //_fsm.ChangeState(PlayState.Playing);
    }

    bool _isNumberOk = false;
    internal void Hitted(int num)
    {
        GenerateNumber();
    }

    void Ready_Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnClick(Vector3.zero);
        }
    }
    private void Reset()
    {

        //_player.transform.position = new Vector3(0, 1, 0);
    }

    internal void ToLose()
    {
        _fsm.ChangeState(PlayState.Lose);
    }

    IEnumerator Lose_Enter()
    {
        uiMgr.SetStateText("Lose");
        yield return new WaitForSeconds(2f);
        _fsm.ChangeState(PlayState.Ready);
    }


    const float SPEED = 0.05f;
    const int FractureCount = 2;

    #region Prefabs

    public GameObject NumberPrefab;


    #endregion


    #region Playing

    void Playing_Enter()
    {
        Debug.Log("Playing");
        uiMgr.SetStateText("Playing");
        GenerateNumber();
    }

    void Playing_Update()
    {

        // Required key is down?
        if (Input.GetKeyDown(KeyCode.Mouse0) == true)
        {
            if (_isNumberOk)
            {
                _isNumberOk = false;
                _number.Fire();
            }
           
        }

        _rotateCircle.Playing_Update();
    }

    #endregion


    Number _number = null;
    void GenerateNumber()
    {
        var gb = Instantiate<GameObject>(NumberPrefab);
        _number = gb.GetComponent<Number>();
        _number.Init(2);
        _isNumberOk = true;

    }
    public void Touch()
    {
        Debug.Log("Touch");
    }


    public void OnClick(Vector3 x)
    {
        if (_fsm.State == PlayState.Ready)
        {
            _fsm.ChangeState(PlayState.Playing);
        }
        else if (_fsm.State == PlayState.Playing)
        {


        }
    }

}

