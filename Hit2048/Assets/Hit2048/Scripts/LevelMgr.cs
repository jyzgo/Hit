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
    Shooting,
    Rotating,
    Lose
};
public class LevelMgr :MonoBehaviour
{
    

    public static LevelMgr Current;


    StateMachine<PlayState> _fsm;
    UIMgr uiMgr;
    RotateCenter _rotateCircle;
    Camera mainCam;
    public void Init()
    {
        Physics.gravity = new Vector3(0, -30.0F, 0);
        uiMgr = FindObjectOfType<UIMgr>();
        _rotateCircle = GameObject.FindObjectOfType<RotateCenter>();
        _fsm = StateMachine<PlayState>.Initialize(this, PlayState.Ready);
        mainCam = Camera.main;

    }



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

    #region Ready
    void Ready_Enter()
    {
        Debug.Log("Ready");
        uiMgr.SetStateText("Get Ready!");
        Reset();
       // _fsm.ChangeState(PlayState.Playing);
    }

    bool _isNumberOk = false;
    internal void Hitted(int num)
    {
    }

    void GenerateCell()
    {
        var cellGb = Instantiate<GameObject>(CellPrefab);
        var cell = cellGb.GetComponent<Cell>();
        int ran = MTRandom.GetRandomInt(1, 9);
        cell.SetNum(ran);
        cell.transform.position = new Vector3(0.3f * ran, -1, 0);
    }

    void Ready_Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnClick(Vector3.zero);
        }
    }

    #endregion Ready
    private void Reset()
    {
        _currentAngle = 0f;
        _rotateCircle.transform.rotation = Quaternion.identity;

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

    public GameObject CellPrefab;

    #endregion


    #region Playing

    void Playing_Enter()
    {
        Debug.Log("Playing");
        uiMgr.SetStateText("Playing");
        GenerateCell();

    }

    

    void Playing_Update()
    {
        
 

    }

    #endregion


    Number _number = null;
 
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
            _fsm.ChangeState(PlayState.Shooting);

        }
    }

    #region Rotating
    const float ROTATE_INTERVAL = 0.4f;
    float _currentAngle = 0f;
    IEnumerator Rotating_Enter()
    {
        Debug.Log("Rotating enter");
        yield return new WaitForSeconds(ROTATE_INTERVAL);
        _currentAngle -= 90f;
        _rotateCircle.RunActions(new MTRotateTo(ROTATE_INTERVAL, new Vector3(0,0,_currentAngle)));
        _fsm.ChangeState(PlayState.Playing);

    }
    #endregion

    #region Shooting
    IEnumerator Shooting_Enter()
    {
        Debug.Log("shooting");
        yield return new WaitForSeconds(0f);
        _fsm.ChangeState(PlayState.Rotating);

    }
    #endregion

}

