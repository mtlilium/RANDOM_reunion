using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class MapBuildingScript : MonoBehaviour, IJsonSaveLoadable, IJsonTemporarySaveLoadable, IJsonInitializable, IVisibleObject
{
    public MapControllScript Parent { get; set; }//このスクリプトを管理するMapController
    
    public MapCoordinate Origin = new MapCoordinate(0, 0);//マップ座標系での基準点.MapController の MapSurface に充てられている場合は(X,Y)=(0,0)とすること.
    
    private string _BuildingName;
    public string BuildingName//建物などの名前(House1など)
    {
        get
        {
            return _BuildingName;
        }
        private set
        {
            _BuildingName = value;
            gameObject.name = _BuildingName;
        }
    }

    public string Status;//建物の状況名(DefaultやUnderconstructionなど)
    
    public int MapChipIDField_Width { get; set; } = 0;//マップチップIDの管理フィールドの横の大きさ(建物の横の大きさ)
    
    public int MapChipIDField_Height { get; set; } = 0;//マップチップID管理フィールドの縦の大きさ(建物の縦の大きさ)
    
    public Field2D<int> MapChipIDField;//マップチップIDの管理フィールド
    
    public Field2D<bool> CollisionField;//マップチップの衝突判定の管理フィールド
    
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
						if(MapChipField.field [i] [j]!=null){
							MapChipField.field [i] [j].SpriteID = MapChipIDField.field [i] [j];
							//MapChipField.field[i][j].Collision = CollisionField.field[i][j]; //現在当たり判定のデータの取り扱いが不明なためコメントアウト中
							MapChipField.field[i][j].Coordinate = new MapCoordinate(i, j);
						}
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
        MapBuildingScriptForSerialization exporter = new MapBuildingScriptForSerialization();

        exporter.Origin = Origin;
        exporter.BuildingName = BuildingName;
        exporter.Status = Status;
        exporter.MapChipIDField_Width = MapChipIDField_Width;
        exporter.MapChipIDField_Height = MapChipIDField_Height;
        exporter.MapChipIDField = MapChipIDField;
        exporter.CollisionField = CollisionField;

        return JsonIO.JsonExport(exporter, path, name);
    }
    public bool JsonImport(string path, string name)
    {
        string FilePath = path + "/" + name + ".json";
        if (!File.Exists(FilePath))
        {
            return false;
        }
        MapBuildingScriptForSerialization scr = JsonIO.JsonImport<MapBuildingScriptForSerialization>(path, name);

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
		string DirectoryPath = SystemVariables.RootPath + "Save/" + savename + "/" + Parent.MapName;
        if(!JsonExport(DirectoryPath, BuildingName, overwrite))
        {
            Debug.LogAssertion($"{DirectoryPath}へのセーブに失敗しました");
            return false;
        }
        return true;
    }
    public bool LoadFrom(string savename)
    {
        string DirectoryPath = SystemVariables.RootPath + "Save/" + savename + "/" + Parent.MapName;
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
		string DirectoryPath = SystemVariables.RootPath + "Temporary/" + Parent.MapName;
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
    public void Initialize(string buildingname) //建物名を引数にとり,対応するディレクトリからDefault.jsonを読み込み,Status,ChipFieldに適用する.またbuildingnameも変える.
    {
        Parent = transform.parent.GetComponent<MapControllScript>();
        string DirectoryPath = SystemVariables.RootPath + "/Data/Building/" + Parent.MapName + "/" + buildingname;
        BuildingName = buildingname;
        JsonImport(DirectoryPath, "Default");
        InstantiateMapChip();
    }

    private void InstantiateMapChip()//MapChipの生成
    {
        if (MapChipField == null)
        {
			MapChipField = new Field2D<MapChipScript>(MapChipIDField_Width, MapChipIDField_Height);
        }

        for (int i = 0; i < MapChipIDField_Width; i++)
        {
			for (int j = 0; j < MapChipIDField_Height; j++) {

				if (MapChipIDField.field [i] [j] != 0 && MapChipField.field[i][j] == null) {
					MapChipField.field [i] [j] = Instantiate (SystemVariables.MapChipPrefab, transform).GetComponent<MapChipScript> ();                
				}
				else if (MapChipField.field [i] [j] != null) {
					Destroy (MapChipField.field [i] [j].gameObject);
				}
			}
        }
        ApplyInfoToMapChip();
    }

    //IVisibleObject
    public void Refresh()//メンバもRefresh()を持っていれば再帰的に適用する.
    {
        transform.position = Origin.ToVector3();
        if (!(MapChipField != null))
        {
            InstantiateMapChip();
        }
        MapChipField.Foreach (x => x?.Refresh());
    }
    
    [DataContract]
    public class MapBuildingScriptForSerialization
    {
        [DataMember]
        public MapCoordinate Origin;
        [DataMember]
        public string BuildingName;
        [DataMember]
        public string Status;
        [DataMember]
        public int MapChipIDField_Width;
        [DataMember]
        public int MapChipIDField_Height;
        [DataMember]
        public Field2D<int> MapChipIDField;
        [DataMember]
        public Field2D<bool> CollisionField;

        public MapBuildingScriptForSerialization() { }
    }
}