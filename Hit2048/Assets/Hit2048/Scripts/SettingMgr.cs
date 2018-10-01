using MTUnity;
using MTUnity.Utils;
using MTXxtea;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public enum SettingEnum
{
    totalCoin,
    recordCoin,
    recordScore,
    recordBomb,
    recordRound,
    recordMatch

}

public class SettingMgr : Singleton<SettingMgr>
{

    public static readonly string SKEY = "b8167365ee0a51e4dcc49";


    public void LoadFile()
    {
        var filePath = GetPath();
        if (!File.Exists(filePath))
        {

            SaveToFile();

        }
        LoadSetting();
    }


    public int currentCoin = 0;
    public int recordCoin = 0;
    public int totalCoin = 0;

    public int currentScore = 0;
    public int recordScore = 0;

    public int currentBomb = 0;
    public int recordBomb = 0;

    public int currentRound = 0;
    public int recordRound = 0;

    public int currentMatch = 0;
    public int recordMatch = 0;
    void LoadSetting()
    {
        var bt = File.ReadAllBytes(GetPath());
        string content = MTXXTea.DecryptToString(bt, SKEY); //File.ReadAllText(GetPath());
       // Debug.Log("get content " + content);
        MTJSONObject setJs = MTJSON.Deserialize(content);
        totalCoin= setJs.GetInt(SettingEnum.totalCoin.ToString(), 0);
        //Debug.Log("get totoal coin " + totalCoin);
        recordCoin = setJs.GetInt(SettingEnum.recordCoin.ToString(), 0);

        recordScore = setJs.GetInt(SettingEnum.recordScore.ToString(), 0);
        recordBomb = setJs.GetInt(SettingEnum.recordBomb.ToString(), 0);
        recordRound = setJs.GetInt(SettingEnum.recordRound.ToString(), 0);
        recordMatch = setJs.GetInt(SettingEnum.recordMatch.ToString(), 0);
        


    }

    public void SaveToFile()
    {
        MTJSONObject setJs = MTJSONObject.CreateDict();
        setJs.Set(SettingEnum.totalCoin.ToString(),totalCoin);

        setJs.Set(SettingEnum.recordCoin.ToString(), recordCoin);

        setJs.Set(SettingEnum.recordScore.ToString(), recordScore);

        setJs.Set(SettingEnum.recordBomb.ToString(), recordBomb);
        setJs.Set(SettingEnum.recordRound.ToString(), recordRound);
        setJs.Set(SettingEnum.recordMatch.ToString(), recordMatch);



        var bt = MTXXTea.Encrypt(setJs.ToString(), SKEY);
        File.WriteAllBytes(GetPath(), bt);

    }
    string GetPath()
    {
       // Debug.Log("path " + Application.persistentDataPath);
        return Application.persistentDataPath + "/" + settingFileName;
    }

    const string settingFileName = "hitsetting.dt";

    
    internal void AddCoinCollected()
    {
        currentCoin += 1;
        totalCoin += 1;
        SaveToFile();
    }
    
    internal void OnGameOverShowed()
    {
        if(currentCoin > recordCoin)
        {
            recordCoin = currentCoin;
        }

        if(currentRound > recordRound)
        {
            recordRound = currentRound;
        }

        if(currentBomb > recordBomb)
        {
            recordBomb = currentBomb;
        }

        if(currentScore > recordScore)
        {
            recordScore = currentScore;
        }

        if(currentMatch > recordMatch)
        {
            recordMatch = currentMatch;
        }

        SaveToFile();
    }

    internal void LastMartchTime(int matchTime)
    {
        if(currentMatch < matchTime)
        {
            currentMatch = matchTime;
        }

    }
}
