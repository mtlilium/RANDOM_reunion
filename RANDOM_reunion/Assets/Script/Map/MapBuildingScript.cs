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
		string FilePath = path + "/" + name + ".json";
		if((File.Exists(FilePath) && overwrite) || !(File.Exists(path + "/" + name)))
		{
			JsonIO.JsonExport<MapBuildingScript> (this, path, name);
		}
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }
    public bool JsonImport(string path, string name)
    {
		string FilePath = path + "/" + name + ".json";
		if (File.Exists (FilePath)) 
		{
			JsonIO.JsonImport<MapBuildingScript> (path, name);
		}
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }
    public bool SaveAs(string savename, bool overwrite)
    {
		string DirectoryPath = "Save/" + savename + "/" + Parent.MapName;
		if (Directory.Exists (DirectoryPath) && overwrite) 
		{
			JsonIO.JsonExport<MapBuildingScript> (this, DirectoryPath, Parent.MapName);
		} else {
			Directory.CreateDirectory (DirectoryPath);
			JsonIO.JsonExport<MapBuildingScript> (this, DirectoryPath, Parent.MapName);
		}
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }
    public bool LoadFrom(string savename)
    {
		string DirectoryPath = "Save/" + savename + "/" + Parent.MapName;
		if (Directory.Exists (DirectoryPath)) 
		{
			JsonIO.JsonImport<MapBuildingScript> (DirectoryPath, Parent.MapName);
		}
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }

    //IJsonTemporarySaveLoadable
    public bool SaveTemporary()
    {
		string DirectoryPath = "Temporary/" + Parent.MapName;
		if (Directory.Exists (DirectoryPath)) 
		{
			JsonIO.JsonExport<MapBuildingScript> (this, DirectoryPath, Parent.MapName);
		}
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }
    public bool LoadTemporary()
    {
		string DirectoryPath = "Temporary/" + Parent.MapName;
		if(Directory.Exists (DirectoryPath))
		{
			JsonIO.JsonImport<MapBuildingScript> (DirectoryPath, Parent.MapName);
		}
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }

    //IJsonInitializable
    public void Initialize(string buildingname) //建物名を引数にとり,対応するディレクトリからInitial.jsonを読み込み,Status,ChipFieldに適用する.またbuildingnameも変える.
    {
		string DirectoryPath = "Data/Building/" + Parent.MapName + buildingname;
		MapBuildingScript tmp = JsonIO.JsonImport<MapBuildingScript> (DirectoryPath + "/Initial.json", buildingname);
		this.BuildingName = tmp.BuildingName;
		this.Status = tmp.Status;
		this.ChipField = tmp.ChipField;
    }

    //IVisibleObject
    public void Refresh()//メンバもRefresh()を持っていれば再帰的に適用する.
    {
		//ChipField.Foreach (x => x.Refresh());
    }
}