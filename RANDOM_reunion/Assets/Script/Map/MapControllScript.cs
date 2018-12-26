using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

[DataContract]
public class MapControllScript : MonoBehaviour, IJsonSaveLoadable, IJsonTemporarySaveLoadable, IJsonInitializable, IVisibleObject {
    [DataMember]
    public string MapName { get; private set; }//マップ名

    [DataMember]
    public MapBuildingScript MapSurface;//地表を取り扱うMapObject. 他のあらゆるMapObjectよりも奥で描写する.

    [DataMember]
    public Dictionary<string, MapBuildingScript> Buildings;//キー: 建物名, 値: 対応するMapObjectとする.

    //IJsonSaveLoadable
    public bool JsonExport(string path, string name, bool overwrite)//再帰的に適用する必要はない.
    {
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }
    public bool JsonImport(string path, string name)//再帰的に適用する必要はない.
    {
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }
    public bool SaveAs(string savename, bool overwrite)//再帰的に適用する必要はない.
    {
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }
    public bool LoadFrom(string savename)//再帰的に適用する必要はない.
    {
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }

    //IJsonTemporarySaveLoadable
    public bool SaveTemporary()//再帰的に適用する必要はない.
    {
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }
    public bool LoadTemporary()//再帰的に適用する必要はない.
    {
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }

    //IJsonInitializable
    public void Initialize(string mapname) //マップ名を引数にとり,対応するディレクトリからInitial.jsonを読み込み,Map,Buildingsに適用する.またMapNameも変える.
    {

    }
    //IVisibleObject
    public void Refresh()//メンバもRefresh()を持っていれば再帰的に適用する.
    {

    }
}