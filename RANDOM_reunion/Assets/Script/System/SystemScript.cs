using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class SystemScript : MonoBehaviour , IJsonSaveLoadable , IJsonInitializable//システム系を管理するスクリプトとして,何か(RootのオブジェクトやPlayerなど)に適用する
{
    public Sprite[] SpriteList;//スプライトのリスト
    public GameObject MapChipPrefab;//マップチップのプレハブ

    void Awake() { }//Awake時にシステム関係(アイテム情報やマップチップ情報など)をロードして,SystemVariable.Initialize(this)で適用する.

    //IJsonSaveLoadInitializable
    public bool JsonExport(string path, string name, bool overwrite)
    {
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }

    public bool JsonImport(string path, string name)
    {
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }

    public bool SaveAs(string savename, bool overwrite)
    {
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }
    public bool LoadFrom(string savename)
    {
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }

    //IJsonInitializable
    public void Initialize(string mapname) { } //マップ名を引数にとり,対応するディレクトリからInitial.jsonを読み込み,Map,Buildingsに適用する.またMapNameも変える.
}
