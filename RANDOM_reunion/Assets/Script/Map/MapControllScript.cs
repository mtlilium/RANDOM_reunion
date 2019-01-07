using System;
using System.IO;
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
    public Dictionary<string, MapBuildingScript> Buildings;//キー: 建物名, 値: 対応するMapBuildingScriptとする.

    //IJsonSaveLoadable
    public bool JsonExport(string path, string name, bool overwrite)
    {
        string filePath = path + "/" + name + ".json";

        if (File.Exists(filePath) && !overwrite)
        {
            return false;
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
        MapControllScript mcs = JsonIO.JsonImport<MapControllScript>(path, name);

        MapName = mcs?.MapName;
        MapSurface = mcs?.MapSurface;
        Buildings = mcs?.Buildings;

        return true;
    }

    public bool SaveAs(string savename, bool overwrite)
    {
        string DirectoryPath = Application.dataPath + "Save/" + savename + "/" + MapName;
        if (!JsonExport(DirectoryPath, MapName, overwrite))
        {
            Debug.LogAssertion($"{DirectoryPath}へのセーブに失敗しました");
            return false;
        }
        return true;
    }
    public bool LoadFrom(string savename)
    {
        string DirectoryPath = Application.dataPath + "Save/" + savename + "/" + MapName;
        if (!JsonImport(DirectoryPath, MapName))
        {
            Debug.LogAssertion($"{DirectoryPath}からのロードに失敗しました");
            return false;
        }
        return true;
    }

    //IJsonTemporarySaveLoadable
    public bool SaveTemporary()
    {
		string DirectoryPath = Application.dataPath + "Temporary/" + MapName;
        if (!JsonExport(DirectoryPath, MapName, true))
        {
            Debug.LogAssertion($"{DirectoryPath}へのセーブに失敗しました");
            return false;
        }
        return true;
    }
    public bool LoadTemporary()
    {
        string DirectoryPath = "Temporary/" + MapName;
        if (!JsonImport(DirectoryPath, MapName))
        {
            Debug.LogAssertion($"{DirectoryPath}からのロードに失敗しました");
            return false;
        }
        return true;
    }

    //IJsonInitializable
    public void Initialize(string mapname) //マップ名を引数にとり,対応するディレクトリからInitial.jsonを読み込み,Map,Buildingsに適用する.またMapNameも変える.
    {
        MapName = mapname;
        string DirectoryPath = "Data/Building/" + MapName + "/" + "Surface/Default";
        JsonImport(DirectoryPath, MapName);
    }
    //IVisibleObject
    public void Refresh()//メンバもRefresh()を持っていれば再帰的に適用する.
    {
        MapSurface.Refresh();
        foreach(var x in Buildings)
        {
            x.Value.Refresh();
        }
    }
}