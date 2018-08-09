using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using MonsterLove.StateMachine;
using MTUnity.Actions;
using Destructible2D;
using MTUnity.Utils;

using System.Linq;

enum PlayState
{
    Ready,
    Playing,
    Shooting,
    Rotating,
    Lose
};

public class Unit
{
    public Unit(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public Unit up = null;
    public Unit down = null;
    public Unit left= null;
    public Unit right = null;
    public int x = 0;
    public int y = 0;
    public Cell cell = null;
    public bool isCenter = false;


}




public class LevelMgr :MonoBehaviour
{
    

    public static LevelMgr Current;

    public Unit[,] grids = new Unit[MAX_SIZE,MAX_SIZE];

    StateMachine<PlayState> _fsm;
    UIMgr uiMgr;
    public RotateCenter RotateCircle { get; private set; }
    public void Init()
    {
        Physics.gravity = new Vector3(0, -30.0F, 0);
        uiMgr = FindObjectOfType<UIMgr>();
        RotateCircle = GameObject.FindObjectOfType<RotateCenter>();
        _fsm = StateMachine<PlayState>.Initialize(this, PlayState.Ready);
        _indicator = GetComponentInChildren<Indicator>();
        _indicator.gameObject.SetActive(false);
        InitGrid();
   

    }

    void InitGrid()
    {
        for (int x = 0; x < MAX_SIZE; x++)
        {
            for (int y = 0; y < MAX_SIZE; y++)
            {
                grids[x, y] = new Unit(x,y);
            }
        }

        for (int x = 0; x < MAX_SIZE; x++)
        {
            for (int y = 0; y < MAX_SIZE; y++)
            {
            
                
                var curUnit = grids[x, y];
                if (x == 10 && y == 10)
                {
                    curUnit.isCenter = true;
                }
                if (x > 0)
                {
                    curUnit.left = grids[x - 1, y];
                }

                if (x < MAX_SIZE - 1)
                {
                    curUnit.right = grids[x + 1, y];
                }

                if (y > 0)
                {
                    curUnit.down = grids[x, y - 1];
                }

                if (y < MAX_SIZE - 1)
                {
                    curUnit.up = grids[x, y + 1];
                }
            }
        }
    }
    

    public Indicator _indicator;

    void Awake()
    {
        Current = this;
        Init();
    }

    List<Cell> _existCells = new List<Cell>();

    public void RemoveCell(Cell cell)
    {
        _existCells.Remove(cell);
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

        uiMgr.SetStateText("Get Ready!");
        Reset();
       // _fsm.ChangeState(PlayState.Playing);
    }

    bool _isNumberOk = false;
    internal void Hitted(int num)
    {
    }


    const int CELL_MAX_INDEX = 10;
    const int MAX_SIZE = CELL_MAX_INDEX * 2 + 1;

    public void SetCells(int x,int y,Cell cell)
    {
        Debug.Log("set cells x" + x + "y" + y);
        Debug.Assert(grids[x,y].cell == null);
        grids[x, y].cell = cell;
        cell.unit = grids[x, y];
        _existCells.Add(cell);


    }

    public void GenerateCellsAtEnter(int n = 1)
    {
        DataUtil.ShuffleList<Cell>(_existCells);
        List<Unit> _availableUnits = new List<Unit>();
        for (int i = 0; i < _existCells.Count; i++)
        {
            var curUnit = _existCells[i].unit;
            if (curUnit.up != null && !curUnit.up.isCenter && curUnit.up.cell == null)
            {
                _availableUnits.Add(curUnit.up);
            }

            if (curUnit.right != null && !curUnit.right.isCenter && curUnit.right.cell == null)
            {
                _availableUnits.Add(curUnit.right);
            }

            if (curUnit.down != null && !curUnit.down.isCenter && curUnit.down.cell == null)
            {
                _availableUnits.Add(curUnit.down);
            }

            if (curUnit.left != null && !curUnit.left.isCenter && curUnit.left.cell == null)
            {
                _availableUnits.Add(curUnit.left);
            }

        }

        for (int i = 0; i < _availableUnits.Count && i < n; i++)
        {
            var curUnit = _availableUnits[i];
            Debug.Log("cur unit x " + curUnit.x + " y " + curUnit.y);
            HashSet<int> numSet = new HashSet<int> { 1, 2, 3, 4, 5 };
            if (curUnit.up != null && curUnit.up.cell != null)
            {
               numSet.Remove(curUnit.up.cell.pow);

            }
            if (curUnit.right != null && curUnit.right.cell != null)
            {
                numSet.Remove(curUnit.right.cell.pow);
            }

            if (curUnit.down != null && curUnit.down.cell != null)
            {
                numSet.Remove(curUnit.down.cell.pow);
            }

            if (curUnit.left != null && curUnit.left.cell != null)
            {
                numSet.Remove(curUnit.left.cell.pow);
            }

      // Debug.Break();

            List<int> numList = numSet.ToList<int>();
            var newGenCell = GenerateCell(numList,true);
            
            curUnit.cell = newGenCell;
            newGenCell.unit = curUnit;
            newGenCell.transform.parent = RotateCircle.transform;
            newGenCell.SetPostion(curUnit.x, curUnit.y);
            newGenCell.SetTestText();
            _existCells.Add(newGenCell);
        }
        
        
       
        
    }

    

    public void CheckCellsMerge()
    {
        Debug.Log("check");
       // _fsm.ChangeState(PlayState.Rotating);
        //return;
        bool isChecked = false;

        for (int x = 0; x < MAX_SIZE; x++)
        {
            for (int y = 0; y < MAX_SIZE - 1; y++)
            {
                var curCell = grids[x, y].cell;
                if (curCell == null)
                {
                    continue;
                }
                var nextCell = grids[x, y + 1].cell;
                if (nextCell == null)
                {
                    y++;
                    continue;
                }

                Debug.Log("cur " + curCell.pow + " next " + nextCell.pow);
                if (curCell.pow == nextCell.pow)
                {
                    isChecked = true;
                    if (Math.Abs(curCell.corY - CELL_MAX_INDEX) > Math.Abs(nextCell.corY - CELL_MAX_INDEX))
                    {
                        curCell.MergeTo(nextCell);
                    }
                    else
                    {
                        nextCell.MergeTo(curCell);
                    }

                    y++;
                }
            }
        }

        for (int y = 0; y < MAX_SIZE ; y++)
        {
            for (int x = 0; x < MAX_SIZE-1; x++)
            {
                var curCell = grids[x, y].cell;
                if (curCell == null)
                {
                    continue;
                }
                var nextCell = grids[x + 1, y].cell;
                if (nextCell == null)
                {
                    x++;
                    continue;
                }

                if (curCell.pow == nextCell.pow)
                {
                    isChecked = true;
                    if (Math.Abs(curCell.corX - CELL_MAX_INDEX) > Math.Abs(nextCell.corX - CELL_MAX_INDEX))
                    {
                        curCell.MergeTo(nextCell);
                    }
                    else
                    {
                        nextCell.MergeTo(curCell);
                    }
                    
                    //Debug.Break();
                    x++;
                }
            }
        }

        if (isChecked)
        {
            StartCoroutine(DelayCheckCells());
        }
        else
        {
            _fsm.ChangeState(PlayState.Rotating);
        }

    }

    IEnumerator DelayCheckCells()
    {
        yield return new WaitForSeconds(0.15f);
        CheckCellsMerge();
    }

    float cell_y_pos = 0f;
    Cell GenerateCell(List<int> numCandidate,bool isAttached = false)
    {
        var cellGb = Instantiate<GameObject>(CellPrefab);
        var cell = cellGb.GetComponent<Cell>();
        cell.isAttached = isAttached;
        DataUtil.ShuffleList<int>(numCandidate);
        int ran = numCandidate[0];
        cell.SetPow(ran);
        //cell.transform.position = new Vector3(0.3f * ran, -1, 0);
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0));
        cell.transform.position = worldPoint + Vector3.up*0.5f +Vector3.forward * 10f;
        cell_y_pos = cell.transform.position.y;
        cell.transform.localScale = Vector3.one * 0.01f;
        
        cell.RunAction(GetScale());
        return cell;
    }

    public MTFiniteTimeAction GetScale()
    {
        return new MTSequence(new MTScaleTo(0.15f,1.2f),new MTScaleTo(0.15f,0.8f),new MTScaleTo(0.1f,1f));
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
        RotateCircle.transform.rotation = Quaternion.identity;

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
    bool _isIndicatorActive = false;
    IEnumerator Playing_Enter()
    {

        uiMgr.SetStateText("Playing");
        List<int> numList = new List<int> { 1, 2, 3 };
        _currentCell =  GenerateCell(numList,false);
        GenerateCellsAtEnter();
        yield return new WaitForSeconds(0.3f);
        _isIndicatorActive = false;
        _indicator.gameObject.SetActive(false);
    }




    const float CELL_WIDTH= 0.426f;
    void Playing_Update()
    {
        var touchPos = Input.mousePosition;
        if(_currentCell != null)
        {
            if (Input.GetMouseButton(0))
            {
                var pos = Camera.main.ScreenToWorldPoint(touchPos);
                float x = (int)(pos.x /CELL_WIDTH) * CELL_WIDTH;

                var tarPos = new Vector3(x, cell_y_pos, 0);
                _currentCell.transform.position =tarPos;
                _indicator.transform.position = tarPos;
                _isIndicatorActive = true;
                _indicator.gameObject.SetActive(true);
               
            }else
            {
                if (_isIndicatorActive)
                {
                    _currentCell.SetActive();
                    _currentCell = null;
                    _fsm.ChangeState(PlayState.Shooting);
                    _isIndicatorActive = false;
                    _indicator.gameObject.SetActive(false);
                }
            }
                 
        }

 

    }

    void Playing_OnExit()
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

    }

    #region Rotating
    const float ROTATE_INTERVAL = 0.4f;
    float _currentAngle = 0f;
    IEnumerator Rotating_Enter()
    {

        yield return new WaitForSeconds(ROTATE_INTERVAL);
        _currentAngle -= 90f;
        RotateCircle.RunActions(new MTRotateTo(ROTATE_INTERVAL, new Vector3(0,0,_currentAngle)));
        _fsm.ChangeState(PlayState.Playing);

    }
    #endregion

    #region Shooting
    IEnumerator Shooting_Enter()
    {

        yield return new WaitForSeconds(0.8f);
        CheckCellsMerge();

    }
    #endregion

}

