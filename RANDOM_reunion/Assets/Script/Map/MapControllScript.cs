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


    public Dictionary<string, string> Savesmap = new Dictionary<string, string>()//セーブデータ名とセーブ用ディレクトリを対応
    {
       { "Save1", @"Application.dataPath/Save/Save1" },
    };

    //IJsonSaveLoadable
    public bool JsonExport(string path, string name, bool overwrite)//再帰的に適用する必要はない.
    {
        if (overwrite)//同名ファイルの削除
        {
            string absolutepath = System.IO.Path.Combine(path, name);//絶対パスの作成
            System.IO.FileInfo(absolutepath).Delete();//作成したパスのファイル削除※ゴミ箱にいかない
        }
        return JsonIO.JsonExport<MapControllScript>(this, path, name);
    }
    public MapControllScript JsonImport(string path, string name)//再帰的に適用する必要はない.
    {
        return JsonIO.JsonImport<MapControllScript>(path, name);//入力したMapControllScriptクラスのデータを返す
    }

    public bool SaveAs(string savename, bool overwrite)//再帰的に適用する必要はない.
    {
        return this.JsonExport(Savesmap[savename], savename, overwrite);//（マップ毎の処理が必要）
    }
    public MapControllScript LoadFrom(string savename)//再帰的に適用する必要はない.
    {
        return JsonIO.JsonImport<MapControllScript>(Savesmap[savename], savename);//(マップ毎の処理が必要)
    }


    public Dictionary<string, string> Tempsmap = new Dictionary<string, string>()//マップ名と一時保存ディレクトリを対応
    {
       { "Map1", @"Application.dataPath/Temporary/Map1" },
    };

    //IJsonTemporarySaveLoadable
    public bool SaveTemporary()//再帰的に適用する必要はない.
    {
        return this.JsonExport(Tempsmap[this.MapName], this.MapName, overwrite);//一時保存ディレクトリに保存
    }
    public MapControllScript LoadTemporary()//再帰的に適用する必要はない.
    {
        return this.JsonImport(Tempsmap[this.MapName], this.MapName);//一時保存ディレクトリからのデータを返す
    }

    public Dictionary<string, path> Buildsmap = new Dictionary<string, path>()//マップ名とBuilding内のマップディレクトリを対応
    {
       { "Map1", @"Application.dataPath/Data/Building/Map1" },
    };

    //IJsonInitializable
    public void Initialize(string mapname) //マップ名を引数にとり,対応するディレクトリからInitial.jsonを読み込み,Map,Buildingsに適用する.またMapNameも変える.
    {
        MapControllScript Imported = this.JsonImport(Buildsmap[mapname], Initial);//読み込んだMapControllScriptクラスのデータをImportedに代入
        this.MapName = mapname;//MapName適応
        this.MapSurface = Imported.MapSurface;//MapSurface適応
        this.Buildings = Imported.Buildings;//Buildings適応
        return;
    }
    //IVisibleObject
    public void Refresh()//メンバもRefresh()を持っていれば再帰的に適用する.
    {

    }
}