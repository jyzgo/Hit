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
        _indicator = GetComponentInChildren<Indicator>();
        _indicator.gameObject.SetActive(false);

    }

    public Indicator _indicator;

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

    float cell_y_pos = 0f;
    Cell GenerateCell()
    {
        var cellGb = Instantiate<GameObject>(CellPrefab);
        var cell = cellGb.GetComponent<Cell>();
        int ran = MTRandom.GetRandomInt(1, 9);
        cell.SetNum(ran);
        //cell.transform.position = new Vector3(0.3f * ran, -1, 0);
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0));
        cell.transform.position = worldPoint + Vector3.up*0.5f;
        cell_y_pos = cell.transform.position.y;
        return cell;
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
    Cell _currentCell = null;
    void Playing_Enter()
    {
        Debug.Log("Playing");
        uiMgr.SetStateText("Playing");
        _currentCell =  GenerateCell();
 _indicator.gameObject.SetActive(true);
    }


    const float CELL_WIDTH= 0.5f;
    void Playing_Update()
    {
        var touchPos = Input.mousePosition;
        if(_currentCell != null)
        {
            if (Input.GetMouseButton(0))
            {
                var viewPos = new Vector3(touchPos.x / Screen.width, touchPos.y / Screen.height, 0f);
                var pos = Camera.main.ScreenToWorldPoint(touchPos);
                float x = (int)(pos.x /CELL_WIDTH) * CELL_WIDTH;

                var tarPos = new Vector3(x, cell_y_pos, 0);
                _currentCell.transform.position =tarPos;
                _indicator.transform.position = tarPos;

               
            }else
            {
               
            }
                 
        }

 

    }

    void Playing_OnExit()
    {
        _indicator.gameObject.SetActive(false);
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
            //_fsm.ChangeState(PlayState.Shooting);

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

