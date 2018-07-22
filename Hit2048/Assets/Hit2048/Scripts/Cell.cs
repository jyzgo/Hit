using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour {

    public SpriteRenderer CellBg;
    public SpriteRenderer CellCenter;
    public Text text;

    public void SetBgColor(Color c)
    {
        CellBg.color = c;
    }

    public void SetCenterColor(Color c)
    {
        CellCenter.color = c;
    }

    int number = 2;
    public void SetNum(int n)
    {
        number = n;
        if (n < 10)
        {
            text.fontSize = 80;
        }
        else if (n >= 10 && n < 100)
        {
            text.fontSize = 70;
        }
        else if (n >= 100 && n < 1000)
        {
            text.fontSize = 55;
        } else
        {
            text.fontSize = 40;
        }
        text.text = n.ToString();
    }

}
