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


   /* public Dictionary<string, string> Savesmap = new Dictionary<string, string>()//セーブデータ名とセーブ用ディレクトリを対応
    {
       { "Save1", @"Application/dataPath/Save/Save1" },
    };*/

    //IJsonSaveLoadable
    public bool JsonExport(string path, string name, bool overwrite)//再帰的に適用する必要はない.
    {
        if (File.Exists(System.IO.Path.Combine(path, name, ".json")) && overwrite)//同名ファイル存在&&上書きする場合
        {
          /*  string willdeletepath = System.IO.Path.Combine(path, name, ".json");//絶対パスの作成
            System.IO.FileInfo(willdeletepath).Delete();//作成したパスのファイル削除（）上書き処理手動の場合*/

            return JsonIO.JsonExport<MapControllScript>(this, path, name);//パス先に保存
        }
        if(!File.Exists(System.IO.Path.Combine(path, name, ".json"))) return JsonIO.JsonExport<MapControllScript>(path, name);//同名ファイルがないならばパス先に保存
    }
    public bool JsonImport(string path, string name)//再帰的に適用する必要はない.
    {
        MapControllScript Imported = JsonIO.JsonImport<MapControllScript>(path, name);//読み込んだMapControllScriptクラスのデータをImportedに代入
        if (Imported == default ( MapControllScript )) return false;//読み込み失敗したらfalseを返す
        this.MapSurface = Imported.MapSurface;//MapSurface適応
        this.Buildings = Imported.Buildings;//Buildings適応
        return true;
        
    }

    public bool SaveAs(string savename, bool overwrite)//再帰的に適用する必要はない.
    {
        if (this.JsonExport(@"Application/dataPath/Save", savename, overwrite)) return true;//パス先にセーブ、成功したらtrue返す
        Debug.LogAssertion( "セーブに失敗しました"　);//失敗したらLogAssertion出す
        return false;//失敗でfalse返す
    }
    public bool LoadFrom(string savename)//再帰的に適用する必要はない.
    {
        if (this.JsonImport(@"Application/dataPath/Save", savename)) return true;//パス先からロード、成功したらtrue返す
        Debug.LogAssertion("ロードに失敗しました");//失敗したらLogAssertion出す
        return false;//失敗でfalse返す
    }


    public Dictionary<string, string> Tempsmap = new Dictionary<string, string>()//マップ名と一時保存ディレクトリを対応
    {
       { "Map1", @"Application/dataPath/Temporary/Map1" },
    };

    //IJsonTemporarySaveLoadable
    public bool SaveTemporary()//再帰的に適用する必要はない.
    {
        if (this.JsonExport(Tempsmap[this.MapName], this.MapName, true)) return true;//一時保存ディレクトリに保存、成功でtrue返す
        Debug.LogAssertion("セーブに失敗しました");//失敗したらLogAssertion出す
        return false;//失敗でfalse返す
    }
    public bool LoadTemporary()//再帰的に適用する必要はない.
    {
        if (this.JsonImport(Tempsmap[this.MapName], this.MapName)) return true;//一時保存ディレクトリからのデータを適応、成功でtrue
        Debug.LogAssertion("ロードに失敗しました");//失敗したらLogAssertion出す
        return false;//失敗でfalse返す
    }

    public Dictionary<string, path> Buildsmap = new Dictionary<string, path>()//マップ名とBuilding内のマップディレクトリを対応
    {
       { "Map1", @"Application/dataPath/Data/Building/Map1" },
    };

    //IJsonInitializable
    public void Initialize(string mapname) //マップ名を引数にとり,対応するディレクトリからInitial.jsonを読み込み,Map,Buildingsに適用する.またMapNameも変える.
    {
        if (this.JsonImport(Buildsmap[mapname], Initial))//Initial.jsonの読み込み
        {
            this.MapName = mapname;//成功すればMapName適応
        }
        return;
    }
    //IVisibleObject
    public void Refresh()//メンバもRefresh()を持っていれば再帰的に適用する.
    {
        MapSurface.Refresh();
    }
}