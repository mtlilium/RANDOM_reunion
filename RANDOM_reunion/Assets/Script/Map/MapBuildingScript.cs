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
    public string Status;//建物の状況名(DefaultやUnderconstructionなど)
    
    [DataMember]
    public int MapChipIDField_Width { get; set; } = 0;//マップチップIDの管理フィールドの横の大きさ(建物の横の大きさ)

    [DataMember]
    public int MapChipIDField_Height { get; set; } = 0;//マップチップID管理フィールドの縦の大きさ(建物の縦の大きさ)

    [DataMember]
    public Field2D<int> MapChipIDField;//マップチップIDの管理フィールド

    [DataMember]
    public Field2D<bool> CollisionField;//マップチップの衝突判定の管理フィールド

    [IgnoreDataMember]
    public Field2D<MapChipScript> MapChipField;//マップチップの管理

    public bool ApplyInfoToMapChip()//自身の持つマップチップ情報を子のマップチップに適用
    {
        if (MapChipField == null)
            return false;
        else
        {
            try
            {
                for (int i = 0; i < MapChipIDField_Width; i++)
                {
                    for (int j = 0; j < MapChipIDField_Height; j++)
                    {
                        MapChipField.field[i][j].SpriteID = MapChipIDField.field[i][j];
                        //MapChipField.field[i][j].Collision = CollisionField.field[i][j]; //現在当たり判定のデータの取り扱いが不明なためコメントアウト中
                        MapChipField.field[i][j].transform.position = (Origin + new MapCoordinate(i, j)).ToVector3();
                    }
                }
            }
            catch
            {
                Debug.Log("マップチップの更新に失敗");
                return false;
            }
            return true;
        }
    }

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

        Origin = scr.Origin;
        BuildingName = scr.BuildingName;
        Status = scr.Status;
        MapChipIDField_Width = scr.MapChipIDField_Width;
        MapChipIDField_Height = scr.MapChipIDField_Height;
        MapChipIDField = scr.MapChipIDField;
        CollisionField = scr.CollisionField;
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
        InstantiateMapChip();
    }

    private void InstantiateMapChip()//MapChipの生成
    {
        if (MapChipField != null)
        {
            return;
        }
        MapChipField = new Field2D<MapChipScript>(MapChipIDField_Width, MapChipIDField_Height);
        for (int i = 0; i < MapChipIDField_Width; i++)
        {
            for (int j = 0; j < MapChipIDField_Height; j++)
            {
                MapChipField.field[i][j] = Instantiate(SystemVariables.MapChipPrefab, transform).GetComponent<MapChipScript>();                
            }
        }
        ApplyInfoToMapChip();
    }

    //IVisibleObject
    public void Refresh()//メンバもRefresh()を持っていれば再帰的に適用する.
    {
        Parent = transform.parent.GetComponent<MapControllScript>();
        transform.position = Origin.ToVector3();
        if (!(MapChipField != null))
        {
            InstantiateMapChip();
        }
        MapChipField.Foreach (x => x.Refresh());
    }
}