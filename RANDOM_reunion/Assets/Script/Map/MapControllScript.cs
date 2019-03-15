using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class MapControllScript : MonoBehaviour, IJsonSaveLoadable, IJsonTemporarySaveLoadable, IJsonInitializable, IVisibleObject {
    public string MapName { get; private set; }//マップ名
    
    public List<string> BuildingName { get; private set; }//マップ内の建物名
    
    public MapBuildingScript MapSurface;//地表を取り扱うMapObject. 他のあらゆるMapObjectよりも奥で描写する.
    
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

        MapControllScriptForSerialization mcss = new MapControllScriptForSerialization();

        mcss.MapName = MapName;
        mcss.BuildingName = BuildingName;

        return JsonIO.JsonExport(mcss, path, name);
    }
    public bool JsonImport(string path, string name)
    {
        string FilePath = path + "/" + name + ".json";
        if (!File.Exists(FilePath))
        {
            return false;
        }
        MapControllScriptForSerialization mcss = JsonIO.JsonImport<MapControllScriptForSerialization>(path, name);

        MapName = mcss.MapName;
        BuildingName = mcss.BuildingName;

        return true;
    }

    public bool SaveAs(string savename, bool overwrite)
    {
        string DirectoryPath = SystemVariables.RootPath + "Save/" + savename + "/" + MapName;
        if (!JsonExport(DirectoryPath, MapName, overwrite))
        {
            Debug.LogAssertion($"{DirectoryPath}へのセーブに失敗しました");
            return false;
        }
        return true;
    }
    public bool LoadFrom(string savename)
    {
        string DirectoryPath = SystemVariables.RootPath + "Save/" + savename + "/" + MapName;
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
		string DirectoryPath = SystemVariables.RootPath + "Temporary/" + MapName;
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
    public void Initialize(string mapname) //マップ名を引数にとり,対応するディレクトリからDefault.jsonを読み込み,Map,Buildingsに適用する.またMapNameも変える.
    {
        MapName = mapname;

        if (MapSurface != null)
            Destroy(MapSurface.gameObject);

        if (Buildings != null)
        {
            foreach(var i in Buildings)
                Destroy(i.Value.gameObject);
        }
        Buildings = new Dictionary<string, MapBuildingScript>();
        BuildingName = new List<string>();

        string mapPath = SystemVariables.RootPath + "/Data/Building/" + mapname;

        DirectoryInfo[] subdirs = new DirectoryInfo(mapPath).GetDirectories();
        foreach (DirectoryInfo dri in subdirs)
        {            
            string directoryPath = mapPath +"/"+ dri.Name;

            if (dri.Name == "Surface")
            {
                MapSurface = new GameObject().AddComponent<MapBuildingScript>();
                MapSurface.transform.parent = transform;
                MapSurface.Initialize("Surface");
            }
            else
            {
                MapBuildingScript structure = new GameObject().AddComponent<MapBuildingScript>();
                structure.transform.parent = transform;
                Buildings.Add(dri.Name, structure);
                BuildingName.Add(dri.Name);
                structure.Initialize(dri.Name);
            }            
        }
        Refresh();
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

    [DataContract]
    public class MapControllScriptForSerialization
    {
        [DataMember]
        public string MapName;//マップ名

        [DataMember]
        public List<string> BuildingName;//マップ内の建物名

        public MapControllScriptForSerialization() { }
    }
}