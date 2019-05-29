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
    
    [DataMember]
    public string InitialMapName = "MapDemo";//初期にロードするマップ名

    void Awake() {
        
        string DirectoryPath = SystemVariables.RootPath + "/Data";
        //JsonIO.JsonImport<SystemScript>(DirectoryPath, "System.json"); 現在読み込むべきシステム変数がないためコメントアウト
        SystemVariables.CopiedFrom(this);
        JsonIO.TiledJsonConvert();

        ///sprite読み込み///
        string jsonPath = SystemVariables.RootPath + "/TiledData";
        string jsonName = "MapDemo_Default";
        string tsxPath = SystemVariables.RootPath + "/TiledData/tsx";
        LoadSpriteList(jsonPath,jsonName,tsxPath);

    }//Awake時にシステム関係(アイテム情報やマップチップ情報など)をロードして,SystemVariable.Initialize(this)で適用する.
    
    void Start()
    {
        Initialize(InitialMapName);
    }

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
        string DirectoryPath = SystemVariables.RootPath + "/Data/" + savename;
        if (!JsonExport(DirectoryPath,savename,overwrite))
        {
            Debug.LogAssertion($"{DirectoryPath}へのセーブに失敗しました");
            return false;
        }
        return true;
    }
    public bool LoadFrom(string savename)
    {
        string DirectoryPath = SystemVariables.RootPath + "/Data"  ;
        if (!JsonImport(DirectoryPath, savename ))
        {
            Debug.LogAssertion($"{DirectoryPath}からのロードに失敗しました");
            return false;
        }
        return true;
    }

    //IJsonInitializable
    public void Initialize(string mapname) {
        MapControllScript mcs = GetComponent<MapControllScript>();
        if (mcs == null)
            mcs = gameObject.AddComponent<MapControllScript>();
        mcs.Initialize(mapname);

    } //マップ名を引数にとり,対応するディレクトリからDefault.jsonを読み込み,Map,Buildingsに適用する.またMapNameも変える.

    void LoadSpriteList(string jsonPath,string jsonName,string tsxPath)
    {
        string spritePathUnderResources = "Sprite/MapDemo/";
        try
        {
            List< Tuple<int,List<string>> > importResultList = TiledTsxImport.MapchipSourceImportFromTiled(jsonPath, jsonName, tsxPath);
            foreach (Tuple<int, List<string>> tp in importResultList)
            {
                //Debug.Log($"firstgid:{tp.Item1}");
                int firstgid = tp.Item1;
                int id = 0;
                ///ソースがマップチップデータでなければ複数のソースが返ってくる///
                ///ソースがマップチップデータでも(＝ソースが一つでも)問題なく動作する///
                foreach (string source in tp.Item2)
                {
                    ///ソースがマップチップデータなら一つのmultipleなspriteを読み込む必要がある///
                    ///マップチップデータでなくても(＝spriteがmultipleでなくても)問題なく動作する///
                    Sprite[] sprites= Resources.LoadAll<Sprite>(spritePathUnderResources+source);
                    foreach (var sp in sprites)
                    {
                        //firstgidから順にindexを増やしながらspriteを割り当てる
                        SpriteList[firstgid + id] = sp;
                        id++;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogAssertion(e.Message);
        }
    }
}
