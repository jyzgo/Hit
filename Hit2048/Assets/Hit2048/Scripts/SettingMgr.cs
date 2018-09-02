using MTUnity;
using MTUnity.Utils;
using MTXxtea;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public enum SettingEnum
{
Coin,
MaxScore

}

public class SettingMgr : Singleton<SettingMgr>{

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

    public int Coin = 0;
    public int Score = 0;
    public int MaxScore = 0;

    void LoadSetting()
    {
        var bt = File.ReadAllBytes(GetPath());
        string content = MTXXTea.DecryptToString(bt, SKEY); //File.ReadAllText(GetPath());

        MTJSONObject setJs = MTJSON.Deserialize(content);
        MaxScore = setJs.GetInt(SettingEnum.MaxScore.ToString(), 0);
        Coin = setJs.GetInt(SettingEnum.Coin.ToString(), 0);

    }

    public void SaveToFile()
    {
        MTJSONObject setJs = MTJSONObject.CreateDict();
        setJs.Set(SettingEnum.Coin.ToString(), Coin);



        var bt = MTXXTea.Encrypt(setJs.ToString(), SKEY);
        File.WriteAllBytes(GetPath(), bt);

    }
    string GetPath()
    {
        Debug.Log("path " + Application.persistentDataPath);
        return Application.persistentDataPath + "/" + settingFileName;
    }

    const string settingFileName = "hitsetting.dt";

}
