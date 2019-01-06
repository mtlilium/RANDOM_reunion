using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;
using System.IO;

[DataContract]
public class MapChipScript : MonoBehaviour , IJsonSaveLoadable ,IJsonTemporarySaveLoadable, IVisibleObject {
    public static readonly int CHIP_WIDTH = 1;//マップチップ間の横の大きさ(空白だとエラーのため適当に1にしています.要修正)
    public static readonly int CHIP_HEIGHT = 1;//マップチップ間の縦の大きさ(空白だとエラーのため適当に1にしています.要修正)

    [IgnoreDataMember]
    public MapBuildingScript Parent { get; set; }//このスクリプトを管理するMapObjectScript

    [DataMember]
    public int SpriteID{get;set;}//適用するスプライトのID
    
    [DataMember]
    public bool Collision{get;set;}//衝突判定
    
    [DataMember]
    public MapCoordinate Coordinate{get;set;}//MapCoordinateでの座標
    
    //IJsonSaveLoadable
    //ただのスラッシュでいいのかバックスラッシュじゃないのか
    public bool JsonExport(string path, string name, bool overwrite)
    {
        if((File.Exists(path+"/"+name)&& overwrite)||!(File.Exists(path + "/" + name)))
        {
            if (JsonIO.JsonExport<MapChipScript>(this, path, name))
            {
                return true;
            }
        }
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }
    public bool JsonImport(string path, string name)
    {
        if (File.Exists(path + "/" + name)&&JsonIO.JsonImport<MapChipScript>(path, name))
        {
           return true;
        }
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }
    public bool SaveAs(string savename, bool overwrite)
    {
        if (JsonIO.JsonExport<MapChipScript>(this, "Save/" + savename + "/" + Parent.Parent.MapName, savename)&&overwrite)
        {
            return true;
        }
        else
        {
            Debug.LogAssertion("せーぶできませんでした");
            return false;
        }
        //(返り値なしだとエラーのため適当にしています.要修正)
    }
    public bool LoadFrom(string savename)
    {
        if(JsonIO.JsonImport<MapChipScript>("Save/" + savename + "/" + Parent.Parent.MapName, savename))
        {
            return true;
        }
        else
        {
            return false;
        }
        //Parent.BuildingNameかも
        //(返り値なしだとエラーのため適当にしています.要修正)
    }

    //IJsonTemporarySaveLoadable
    public bool SaveTemporary()
    {   
        if(JsonIO.JsonExport<MapChipScript>(this, "Temporary/" + Parent.Parent.MapName, Parent.Parent.MapName))
        {
            return true;
        }
        else
        {
            return false;
        }
        //(返り値なしだとエラーのため適当にしています.要修正)
    }
    public bool LoadTemporary()
    {
        if (File.Exists("Temporary/"+ Parent.Parent.MapName+"/"+ Parent.Parent.MapName))
        {
            JsonIO.JsonImport<MapChipScript>("Temporary/" + Parent.Parent.MapName, Parent.Parent.MapName);
            return true;
        }
        else
        {
            Debug.LogAssertion("てんぽらりーせーぶできませんでした");
            return false;
        }
        //(返り値なしだとエラーのため適当にしています.要修正)
    }

    //IVisibleObject
    public void Refresh() { }//メンバもRefresh()を持っていれば再帰的に適用する.
}