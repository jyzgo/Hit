﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour {

    
    public Text _stateText;
    public Text _scoreText;
    public Text _coinText;

    private void Awake()
    {
        

    }

    internal void SetStateText(string v)
    {
        _stateText.text = v;
    }

    internal void UpdateUI()
    {
        _scoreText = SettingMgr.Instance.Score;
        _coinText = SettingMgr.Instance.Coin;
    }
}
