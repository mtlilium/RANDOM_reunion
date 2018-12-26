using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

[DataContract]
public class MapBuildingScript : MonoBehaviour, IJsonSaveLoadable, IJsonTemporarySaveLoadable, IJsonInitializable, IVisibleObject
{
    [IgnoreDataMember]
    public MapControllScript Parent { get; set; }//このスクリプトを管理するMapController

    [DataMember]
    public MapCoordinate Origin;//マップ座標系での基準点.MapController の MapSurface に充てられている場合は(X,Y)=(0,0)とすること.

    [DataMember]
    public string BuildingName;//建物などの名前(House1など)

    [DataMember]
    public string Status;//建物の状況名(InitialやUnderconstructionなど)

    [DataMember]
    public Field2D<MapChipScript> ChipField;//マップチップの管理

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

    //IJsonInitializable
    public void Initialize(string buildingname) //建物名を引数にとり,対応するディレクトリからInitial.jsonを読み込み,Status,ChipFieldに適用する.またbuildingnameも変える.
    {

    }

    //IVisibleObject
    public void Refresh()//メンバもRefresh()を持っていれば再帰的に適用する.
    {

    }
}