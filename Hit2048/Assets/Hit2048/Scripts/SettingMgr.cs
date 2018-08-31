using MTUnity.Utils;
using MTXxtea;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingMgr : MonoBehaviour {

    public static readonly string SKEY = "b8167365ee0a51e4dcc49";
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LoadFile()
    {
        var filePath = GetPath();
        if (!File.Exists(filePath))
        {

            SaveToFile();

        }
        LoadSetting();
    }



    void LoadSetting()
    {
        var bt = File.ReadAllBytes(GetPath());
        string content = MTXXTea.DecryptToString(bt, SKEY); //File.ReadAllText(GetPath());


        MTJSONObject setJs = MTJSON.Deserialize(content);

    }

    public void SaveToFile()
    {
        MTJSONObject setJs = MTJSONObject.CreateDict();
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
