using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class SystemScript : MonoBehaviour, IJsonSaveLoadable, IJsonInitializable//システム系を管理するスクリプトとして,何か(RootのオブジェクトやPlayerなど)に適用する
{
    [IgnoreDataMember]
    public Sprite[] SpriteList;//スプライトのリスト
    [IgnoreDataMember]
    public GameObject MapChipPrefab;//マップチップのプレハブ

    void Awake() {
        string DirectoryPath = Application.dataPath + "/Data";
        JsonIO.JsonImport<SystemScript>(DirectoryPath, "System.json");
        SystemVariables.CopiedFrom(this);
        JsonIO.TiledJsonConvert();
    }//Awake時にシステム関係(アイテム情報やマップチップ情報など)をロードして,SystemVariable.Initialize(this)で適用する.
    

    //IJsonSaveLoadInitializable
    public bool JsonExport(string path, string name, bool overwrite)
    {
        string FilePath = path +"/"+name+".json";
        if (File.Exists(FilePath) && !overwrite)
        {
            return false;//(返り値なしだとエラーのため適当にしています.要修正)
        }
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return JsonIO.JsonExport(this, path, name);
    }

    public bool JsonImport(string path, string name)
    {
        string FilePath = path + "/" + name + ".json";
        if (!File.Exists(FilePath))
        {
            return false;
        }

        SystemScript ss=JsonIO.JsonImport<SystemScript>(path, name);
        SpriteList = ss?.SpriteList;
        MapChipPrefab = ss?.MapChipPrefab;
        return true;
    }

    public bool SaveAs(string savename, bool overwrite)
    {
        string DirectoryPath = Application.dataPath + "/Data/" + savename;
        if (!JsonExport(DirectoryPath,savename,overwrite))
        {
            Debug.LogAssertion($"{DirectoryPath}へのセーブに失敗しました");
            return false;
        }
        return true;
    }
    public bool LoadFrom(string savename)
    {
        string DirectoryPath = Application.dataPath + "/Data"  ;
        if (!JsonImport(DirectoryPath, savename ))
        {
            Debug.LogAssertion($"{DirectoryPath}からのロードに失敗しました");
            return false;
        }
        return true;
    }

    //IJsonInitializable
    public void Initialize(string mapname) {
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "Data/Building/" + mapname );
        DirectoryInfo[] diarr = di.GetDirectories();
        foreach (DirectoryInfo dri in diarr)
        {
            string DirectoryPath = Application.dataPath + "/Data/Building/" + mapname +"/"+dri.Name;
            string name = "Default,json";
            JsonIO.JsonImport<SystemScript>(DirectoryPath, name);
        }
    } //マップ名を引数にとり,対応するディレクトリからDefault.jsonを読み込み,Map,Buildingsに適用する.またMapNameも変える.
}
