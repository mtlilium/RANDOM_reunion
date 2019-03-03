using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

[DataContract]
public class MapControllScript : IJsonSaveLoadable, IJsonTemporarySaveLoadable, IJsonInitializable, IVisibleObject {
    [DataMember]
    public string MapName { get; private set; }//マップ名

    [DataMember]
    public string[] BuildingName { get; private set; }//マップ内の建物名

    [IgnoreDataMember]
    public MapBuildingScript MapSurface;//地表を取り扱うMapObject. 他のあらゆるMapObjectよりも奥で描写する.

    [IgnoreDataMember]
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
        BuildingName = mcs?.BuildingName;

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
        string DirectoryPath = "Data/Building/" + MapName;

        if (MapSurface == null)
        {
      //      MapSurface = new GameObject().AddComponent<MapBuildingScript>();
      //      MapSurface.transform.parent = transform;
        }

        MapSurface.JsonImport(DirectoryPath + "/" + "Surface", "Default");

        if (Buildings == null)
            Buildings = new Dictionary<string, MapBuildingScript>();

        foreach (var b in BuildingName)
        {
            if (Buildings[b] == null)
            {
            //    Buildings.Add(b, new GameObject().AddComponent<MapBuildingScript>());
            }

          //  Buildings[b].transform.parent = transform;
            Buildings[b].JsonImport(DirectoryPath + "/" + b, "Default");
        }
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

	void LoadMap(string mapname){
		foreach (Transform child in this.gameObject.transform) {
			GameObject.Destroy (child.gameObject);
		}
		Initialize (mapname);
	}
}