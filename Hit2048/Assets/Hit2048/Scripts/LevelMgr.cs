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
    public Unit left = null;
    public Unit right = null;
    public int x = 0;
    public int y = 0;
    public Cell cell = null;
    public bool isCenter = false;

    public bool isLinked = false;


    

    public bool isCheckLinKed = false;


    private void SetCell(Cell c)
    {
        //if (c == null || c.unit == null || cell == null)
        //{
        //    return;

        //}
        Debug.Assert(c.unit == null);
        Debug.Assert(this.cell == null);
        this.cell = c;
        cell.corX = x;
        cell.corY = y;
        this.cell.unit = this;
    }

    Cell CleanCell()
    {
        var tcell = cell;
        tcell.unit = null;
        this.cell = null;

        return tcell;
    }
    internal void MoveUp()
    {
        if (cell != null)
        {
            var tcell = CleanCell();
            up.cell = null;
            up.SetCell(tcell);
            tcell.MoveToUnit();


            if (down != null)
            {
                down.MoveUp();
            }
        }

    }

    internal void MoveDown()
    {
        if (cell != null)
        {
            var tcell = CleanCell();
            down.cell = null;
            down.SetCell(tcell);
            tcell.MoveToUnit();

            if (up != null)
            {
                up.MoveDown();
            }
        }


    }

    internal void MoveLeft()
    {
        if (cell != null)
        {
            var tcell = CleanCell();
            left.cell = null;
            left.SetCell(tcell);
            tcell.MoveToUnit();
            if (right != null)
            {
                right.MoveLeft();
            }
        }


    }

    internal void MoveRight()
    {
        if (cell != null)
        {
            var tcell = CleanCell();
            right.cell = null;
            right.SetCell(tcell);
            tcell.MoveToUnit();
            if (left != null)
            {
                left.MoveRight();
            }
        }
    }

    int CenterX = 10;
    int CenterY = 10;
    internal void CheckLink()
    {
        isCheckLinKed = true;
        if (cell == null)
        {
            isLinked = false;
        }
        else
        {
            isLinked = true;
            if (up != null && !up.isCheckLinKed)
            {
                up.CheckLink();
            }

            if (down != null &&!down.isCheckLinKed)
            {
                down.CheckLink();
            }

            if (right!=null && !right.isCheckLinKed)
            {
                right.CheckLink();
            }

            if (left != null && !left.isCheckLinKed)
            {
                left.CheckLink();
            }
        }
        
    }

    internal void Clean()
    {
        if (cell != null)
        {
            GameObject.Destroy(cell.gameObject);
            cell = null;
        }
        isLinked = false;
        isCheckLinKed = false;
    }
}




public class LevelMgr : MonoBehaviour
{


    public static LevelMgr Current;

    public Unit[,] grids = new Unit[MAX_SIZE, MAX_SIZE];

    StateMachine<PlayState> _fsm;
    UIMgr _uiMgr;
    public RotateCenter RotateCircle { get; private set; }
    SettingMgr _settingMgr;
    public void Init()
    {
        InitGrid();
        //Physics.gravity = new Vector3(0, -30.0F, 0);
        _uiMgr = FindObjectOfType<UIMgr>();
        _settingMgr =  SettingMgr.Instance;
        _settingMgr.LoadFile();
        _uiMgr.UpdateUI();
        
        RotateCircle = GameObject.FindObjectOfType<RotateCenter>();
        _fsm = StateMachine<PlayState>.Initialize(this, PlayState.Ready);
        _indicator = GetComponentInChildren<Indicator>();
        _indicator.gameObject.SetActive(false);

        AdMgr.RegisterAllAd();
        AdMgr.ShowAdmobBanner();


    }

    void InitGrid()
    {
        for (int x = 0; x < MAX_SIZE; x++)
        {
            for (int y = 0; y < MAX_SIZE; y++)
            {
                grids[x, y] = new Unit(x, y);
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

    internal void Explode(int x, int y)
    {

        var unit = grids[x, y];
        if (unit.cell != null)
        {
            AddScore(unit.cell);
            unit.cell.DestoryAndGenCoin();
        }
    }

    HashSet<Coin> _coinSet = new HashSet<Coin>();
    public void AddCoin(Coin c)
    {
        _coinSet.Add(c);
        
    }

    public void RemoveCoin(Coin c)
    {
        _coinSet.Remove(c);
    }

    public void RemoveAllCoin()
    {
        foreach (var c in _coinSet)
        {
            if (!c.isDestroying)
            {
                c.DestorySelf();
            }
        }
        _coinSet.Clear();
    }

    #region Ready
    int _initCellNum = 10;
    void Ready_Enter()
    {
        AdMgr.PreloadAdmobInterstitial();
        _uiMgr.SetStateText("Get Ready!");
        _settingMgr.Score = 0;
        _initCellNum = 10;
        _uiMgr.UpdateUI();
        Reset();
        // _fsm.ChangeState(PlayState.Playing);
    }

    internal void Hitted(int num)
    {
    }


    public const int CELL_MAX_INDEX = 10;
    public const int MAX_SIZE = CELL_MAX_INDEX * 2 + 1;

    public void SetCells(int x, int y, Cell cell)
    {
        Debug.Log("set cells x" + x + "y" + y);
       // Debug.Assert(grids[x, y].cell == null);
        if (grids[x, y] == null)
        {
            return;
        }
        grids[x, y].cell = cell;
        cell.unit = grids[x, y];


    }



    public void GenerateCellsAtEnter(int n = 1)
    {
       HashSet<Unit> candidateUnitSet = new HashSet<Unit>();
        
        for (int x = 0; x < MAX_SIZE; x++)
        {
            for (int y = 0; y < MAX_SIZE; y++)
            {
                var curUnit = grids[x, y];
                if (curUnit.isCenter)
                {
                    continue;
                }

                if (curUnit.cell != null)
                {
                    continue;
                }
                if (curUnit.up != null && (curUnit.up.cell != null || curUnit.up.isCenter))
                {
                    candidateUnitSet.Add(curUnit);
                    continue;
                }

                if (curUnit.right != null && (curUnit.right.cell != null|| curUnit.right.isCenter))
                {
                    candidateUnitSet.Add(curUnit);
                    continue;
                }

                if (curUnit.down != null && (curUnit.down.cell != null|| curUnit.down.isCenter))
                {
                    candidateUnitSet.Add(curUnit);
                    continue;
                }

                if (curUnit.left != null && (curUnit.left.cell != null || curUnit.left.isCenter))
                {
                    candidateUnitSet.Add(curUnit);
                }
            }
        }

        List<Unit> candidateUnits = candidateUnitSet.ToList<Unit>();
        DataUtil.ShuffleList<Unit>(candidateUnits);


        for (int i = 0; i < candidateUnits.Count && i < n; i++)
        {
            var curUnit = candidateUnits[i];

            HashSet<int> numSet = new HashSet<int> {1,2,3 };
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
            var newGenCell = GenerateCell(numList, true);
            if (newGenCell == null)
            {
                return;
            }

            curUnit.cell = newGenCell;
            newGenCell.unit = curUnit;
            newGenCell.transform.parent = RotateCircle.transform;
            newGenCell.SetPostion(curUnit.x, curUnit.y);
            newGenCell.SetTestText();
        }


    }

    public void AddScore(Cell cell)
    {
        _settingMgr.Score += cell.number;
        PopupFloatText(cell.transform.position, cell.number.ToString());
        _uiMgr.UpdateUI();

    }

    public void PopupFloatText(Vector3 pos, string str)
    {
        var text = Instantiate<GameObject>(ResMgr.Current.FloatText);
        var t = text.GetComponent<FloatFont>();
        t.SetShow(pos, str);
    }

    void CheckLinked()
    {
        for (int x = 0; x < MAX_SIZE; x++)
        {
            for (int y = 0; y < MAX_SIZE; y++)
            {
                grids[x, y].isLinked = false;
                grids[x, y].isCheckLinKed = false;
            }
        }


        //move up
        var upUnit = grids[10, 11];
        if (upUnit != null)
        {
            upUnit.isLinked = true;
            upUnit.CheckLink();
        }

        //move down
        var downUnit = grids[10, 9];
        if (downUnit.cell != null)
        {
            downUnit.isLinked = true;
            downUnit.CheckLink();
        }

        //move left
        var leftUnit = grids[9, 10];
        if (leftUnit.cell != null)
        {
            leftUnit.isLinked = true;
            leftUnit.CheckLink();
        }
        //move right
        var rightUnit = grids[11, 10];
        if (rightUnit.cell != null)
        {
            rightUnit.isLinked = true;
            rightUnit.CheckLink();
        }

        for (int x = 0; x < MAX_SIZE; x++)
        {
            for (int y = 0; y < MAX_SIZE; y++)
            {
                var unit = grids[x, y];
                if (!unit.isLinked)
                {
                    Explode(x, y);
                }
            }
        }


    }



    void CheckCorY(Unit u)
    {
        if (u == null)
        {
            return;
        }
        if (u.y > CELL_MAX_INDEX)
        {
            if (u == null)
            {
                return;
            }

            if (u.up != null)
            {
                u.up.MoveDown();
            }

        }
        else
        {
            if (u.down != null)
            {
                u.down.MoveUp();
            }
        }
    }

    void CheckCorX(Unit u)
    {
        if (u == null)
        {
            return;
        }
        if (u.x > CELL_MAX_INDEX)
        {
            if (u.right != null)
            {
                u.right.MoveLeft();
            }
        }
        else
        {
            if (u.left != null)
            {
                u.left.MoveRight();
            }
        }
    }

    public void DelayCheckMerge()
    {
        StartCoroutine(DelayCheckCells());
    }

    public void CheckCellsMerge()
    {

        // _fsm.ChangeState(PlayState.Rotating);
        //return;
        bool isChecked = false;

        for (int x = 0; x < MAX_SIZE; x++)
        {
            for (int y = 0; y < MAX_SIZE - 1; y++)
            {
                var curCell = grids[x, y].cell;
                if (curCell == null || curCell.isMerging || curCell._cellType == CellType.Bomb)
                {
                    continue;
                }
                var nextCell = grids[x, y + 1].cell;
                if (nextCell == null || nextCell.isMerging || nextCell._cellType == CellType.Bomb)
                {
                    //y++;
                    continue;
                }

                if (curCell.pow == nextCell.pow)
                {
                    isChecked = true;
                    if (Math.Abs(curCell.corY - CELL_MAX_INDEX) > Math.Abs(nextCell.corY - CELL_MAX_INDEX))
                    {
                        var curUnit = curCell.unit;
                        curCell.MergeTo(nextCell);
                        curCell.unit = null;
                        CheckCorY(curUnit);
                    }
                    else
                    {
                        var curUnit = nextCell.unit;
                        nextCell.MergeTo(curCell);
                        nextCell.unit = null;
                        CheckCorY(curUnit);

                    }
                    y++;
                }
            }
        }

        if (isChecked)
        {
            StartCoroutine(DelayCheckCells());
            return;
        }

        for (int y = 0; y < MAX_SIZE; y++)
        {
            for (int x = 0; x < MAX_SIZE - 1; x++)
            {
                var curCell = grids[x, y].cell;
                if (curCell == null || curCell.isMerging || curCell._cellType == CellType.Bomb)
                {
                    continue;
                }
                var nextCell = grids[x + 1, y].cell;
                if (nextCell == null || nextCell.isMerging || nextCell._cellType == CellType.Bomb)
                {
                    //x++;
                    continue;
                }

                if (curCell.pow == nextCell.pow)
                {
                    isChecked = true;
                    if (Math.Abs(curCell.corX - CELL_MAX_INDEX) > Math.Abs(nextCell.corX - CELL_MAX_INDEX))
                    {

                        var curUnit = curCell.unit;
                        curCell.MergeTo(nextCell);
                        curCell.unit = null;
                        CheckCorX(curUnit);
                    }
                    else
                    {
                        var curUnit = nextCell.unit;
                        nextCell.MergeTo(curCell);
                        nextCell.unit = null;
                        CheckCorX(curUnit);

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
            if (CheckIfLose())
            {
                _fsm.ChangeState(PlayState.Lose);
            }
            else
            {
                CheckLinked();
                _fsm.ChangeState(PlayState.Rotating);
            }
        }

    }

    IEnumerator DelayCheckCells()
    {
        yield return new WaitForSeconds(0.2f);
        CheckCellsMerge();
    }

    float cell_y_pos = 0f;
    Cell GenerateCell(List<int> numCandidate, bool isAttached = false)
    {
        if (numCandidate.Count == 0)
        {
            return null;
        }
        var cellGb = Instantiate<GameObject>(CellPrefab);
        var cell = cellGb.GetComponent<Cell>();
        cell.isAttached = isAttached;
        DataUtil.ShuffleList<int>(numCandidate);
        int ran = numCandidate[0];

        cell.SetPow(ran);
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0));
        cell.transform.position = worldPoint + Vector3.up * 0.5f + Vector3.forward * 10f;
        cell_y_pos = cell.transform.position.y;
        cell.transform.localScale = Vector3.one * 0.01f;

        cell.RunAction(GetScale());
        return cell;
    }

    public MTFiniteTimeAction GetScale()
    {
        return new MTSequence(new MTScaleTo(0.15f, 1.2f), new MTScaleTo(0.15f, 0.8f), new MTScaleTo(0.1f, 1f));
    }

    void Ready_Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick(Vector3.zero);
        }
    }

    #endregion Ready
    private void Reset()
    {
        _currentAngle = 0f;
        RotateCircle.transform.rotation = Quaternion.identity;

        for (int x = 0; x < MAX_SIZE; x++)
        {
            for (int y = 0; y < MAX_SIZE; y++)
            {
                grids[x, y].Clean();
            }
        }

        RemoveAllCoin();

        //_player.transform.position = new Vector3(0, 1, 0);
    }

   


    const float SPEED = 0.05f;
    const int FractureCount = 2;

    #region Prefabs

    public GameObject CellPrefab;

    #endregion

    #region Lose
    internal void ToLose()
    {
        _fsm.ChangeState(PlayState.Lose);
    }

    IEnumerator Lose_Enter()
    {
        AdMgr.ShowAdmobInterstitial();
        _uiMgr.SetStateText("Lose");
        yield return new WaitForSeconds(2f);
        _fsm.ChangeState(PlayState.Ready);
    }

    bool CheckIfLose()
    {
        for (int x = 0; x < MAX_SIZE; x++)
        {
            for (int y = 0; y < MAX_SIZE; y++)
            {
                if (x == 0 || x == MAX_SIZE - 1 || y == 0 || y == MAX_SIZE - 1)
                {
                    if (grids[x, y].cell != null)
                    {
                        _fsm.ChangeState(PlayState.Lose);
                        return true;
                    }
                }
            }
        }
        return false;
    }


    #endregion

    #region Playing
    Cell _currentCell = null;
    bool _isIndicatorActive = false;
    IEnumerator Playing_Enter()
    {

        _uiMgr.SetStateText("Playing");
        List<int> numList = new List<int> { 5 };
        _currentCell = GenerateCell(numList, false);
        for (int i = 0; i < _initCellNum; i++)
        {
            GenerateCellsAtEnter();
            yield return new WaitForSeconds(0.1f);
        }
        _initCellNum = 1;
        yield return new WaitForSeconds(0.3f);
        _isIndicatorActive = false;
        _indicator.gameObject.SetActive(false);
    }




    const float CELL_WIDTH = 0.426f;
    void Playing_Update()
    {
        var touchPos = Input.mousePosition;
        if (_currentCell != null)
        {
            if (Input.GetMouseButton(0))
            {
                var pos = Camera.main.ScreenToWorldPoint(touchPos);
                float x = (int)(pos.x / CELL_WIDTH) * CELL_WIDTH;

                var tarPos = new Vector3(x, cell_y_pos, 0);
                _currentCell.transform.position = tarPos;
                _indicator.transform.position = tarPos;
                _isIndicatorActive = true;
                _indicator.gameObject.SetActive(true);

            }
            else
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
        _uiMgr.SetStateText("Rotate Before");
        yield return new WaitForSeconds(ROTATE_INTERVAL);

        _uiMgr.SetStateText("Rotate After");
        _currentAngle -= 90f;
        RotateCircle.RunActions(new MTRotateTo(ROTATE_INTERVAL, new Vector3(0, 0, _currentAngle)));
        yield return new WaitForSeconds(ROTATE_INTERVAL);
        _fsm.ChangeState(PlayState.Playing);

    }
    #endregion

    #region Shooting
    IEnumerator Shooting_Enter()
    {
        _uiMgr.SetStateText("Shooting Bef");
        yield return new WaitForSeconds(0.1f);
        _uiMgr.SetStateText("Shotting Aft");
        // CheckCellsMerge();

    }

    internal void ChangeToRotating()
    {
        Debug.Log("change to ro");
        _fsm.ChangeState(PlayState.Rotating);
    }
    #endregion

}

