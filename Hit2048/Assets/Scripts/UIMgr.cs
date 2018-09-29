using MTUnity.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour {

    
    public Text _stateText;
    public Text _scoreText;
    public Text _coinText;
    public GameObject _loseUI;

    public GameObject Press;
    public GameObject Move;

    private void Awake()
    {
        

    }

    internal void SetStateText(string v)
    {
        _stateText.text = v;
    }

    internal void UpdateUI()
    {
        _coinText.text = SettingMgr.Instance.totalCoin.ToString();
        _scoreText.text = SettingMgr.Instance.currentScore.ToString();
        
    }
    internal void OnReady()
    {
        _loseUI.SetActive(false);
        Press.gameObject.SetActive(true);
        Press.gameObject.RunActions(new MTRepeatForever(new MTScaleTo(0.8f, 2.3f), new MTScaleTo(0.8f, 1.8f)));
    }

    internal void ShowMoveHint()
    {
        Move.gameObject.SetActive(true);
        Move.gameObject.RunActions(new MTRepeatForever(new MTScaleTo(0.8f, 2.3f), new MTScaleTo(0.8f, 1.8f)));
    }

    internal void HideMoveHint()
    {
        Move.StopAllActions();
        Move.gameObject.SetActive(false);

    }

    internal void FirstPress()
    {

    }

    internal void OnReadyExit()
    {
        Press.StopAllActions();
        Press.gameObject.SetActive(false);
    }

    public void OnReplay()
    {
        AdMgr.ShowAdmobInterstitial();
        LevelMgr.Current.ToReady();
    }

    public void ToLose()
    {
       
        _loseUI.SetActive(true);
    }
}
