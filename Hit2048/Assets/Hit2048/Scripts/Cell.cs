using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    public SpriteRenderer CellBg;
    public SpriteRenderer CellCenter;

    public void SetBgColor(Color c)
    {
        CellBg.color = c;
    }

    public void SetCenterColor(Color c)
    {
        CellCenter.color = c;
    }


}
