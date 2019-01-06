using System;
using System.IO;
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
        MapBuildingScript scr = JsonIO.JsonImport<MapBuildingScript>(path, name);

        Origin = scr?.Origin;
        BuildingName = scr?.BuildingName;
        Status = scr?.Status;
        ChipField = scr?.ChipField;

        return true;
    }
    public bool SaveAs(string savename, bool overwrite)
    {
		string DirectoryPath = Application.dataPath + "Save/" + savename + "/" + Parent.MapName;
        if(!JsonExport(DirectoryPath, BuildingName, overwrite))
        {
            Debug.LogAssertion($"{DirectoryPath}へのセーブに失敗しました");
            return false;
        }
        return true;
    }
    public bool LoadFrom(string savename)
    {
        string DirectoryPath = Application.dataPath + "Save/" + savename + "/" + Parent.MapName;
        if (!JsonImport(DirectoryPath, BuildingName))
        {
            Debug.LogAssertion($"{DirectoryPath}からのロードに失敗しました");
            return false;
        }
        return true;
    }

    //IJsonTemporarySaveLoadable
    public bool SaveTemporary()
    {
		string DirectoryPath = Application.dataPath + "Temporary/" + Parent.MapName;
        if (!JsonExport(DirectoryPath, BuildingName, true))
        {
            Debug.LogAssertion($"{DirectoryPath}へのセーブに失敗しました");
            return false;
        }
        return true;
    }
    public bool LoadTemporary()
    {
		string DirectoryPath = "Temporary/" + Parent.MapName;
        if (!JsonImport(DirectoryPath, BuildingName))
        {
            Debug.LogAssertion($"{DirectoryPath}からのロードに失敗しました");
            return false;
        }
        return true;
    }

    //IJsonInitializable
    public void Initialize(string buildingname) //建物名を引数にとり,対応するディレクトリからInitial.jsonを読み込み,Status,ChipFieldに適用する.またbuildingnameも変える.
    {
        string DirectoryPath = "Data/Building/" + Parent.MapName + "/" + buildingname + "/Default";
        BuildingName = buildingname;
        JsonImport(DirectoryPath, BuildingName);

        //MapChipの生成
        for (int i = 0; i < ChipField.X; i++)
        {
            for (int j = 0; j < ChipField.Y; j++)
            {
                MapChipScript mcs = Instantiate(SystemVariables.MapChipPrefab, transform).GetComponent<MapChipScript>();

                mcs.Parent = this;
                mcs.SpriteID = ChipField.field[i][j].SpriteID;
                mcs.Collision = ChipField.field[i][j].Collision;
                mcs.Coordinate = ChipField.field[i][j].Coordinate;
                mcs.Refresh();
            }
        }
    }

    //IVisibleObject
    public void Refresh()//メンバもRefresh()を持っていれば再帰的に適用する.
    {
        ChipField.Foreach (x => x.Refresh());
    }
}