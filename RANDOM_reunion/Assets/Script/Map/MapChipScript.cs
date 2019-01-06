using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

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
    public bool JsonExport(string path, string name, bool overwrite)
    {
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }
    public bool JsonImport(string path, string name)
    {
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }
    public bool SaveAs(string savename, bool overwrite)
    {
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }
    public bool LoadFrom(string savename)
    {
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }

    //IJsonTemporarySaveLoadable
    public bool SaveTemporary()
    {
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }
    public bool LoadTemporary()
    {
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }

    //IVisibleObject
    public void Refresh() { }//メンバもRefresh()を持っていれば再帰的に適用する.
}