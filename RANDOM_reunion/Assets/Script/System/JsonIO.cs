using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;


public static class JsonIO
{
    public static bool JsonExport<T>(T obj, string path, string name)//objをpathディレクトリに[naem].jsonとして保存 成功すればtrue 失敗すればfalse
    {
        try
        {
            using (StreamWriter exportResultStream = new StreamWriter(path + '/' + name + ".json", false, Encoding.Default))//ファイルを読み込み
            {
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));// T 用のjsonSerializerを準備
                jsonSerializer.WriteObject(exportResultStream.BaseStream, obj);//exportして書き込み
            }
        }
        catch
        {
            return false;//あらゆる例外に対してfalseを返す
        }
        return true;
    }
    public static T JsonImport<T>(string path, string name)//pathディレクトリの[naem].jsonを読み込む 失敗などで読み込めなければ,LogAssertionで警告を表示しdefault(T)を返す
    {
        T importResultObj = default(T);
        try
        {
            using (StreamReader importerStream = new StreamReader(path + '/' + name + ".json", Encoding.Default))//ファイルを読み込み
            {
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));// T 用のjsonSerializerを準備
                importResultObj = (T)jsonSerializer.ReadObject(importerStream.BaseStream);//読み込みしてimport
            }
        }
        catch (Exception e)
        {
            Debug.LogAssertion($"{e.Message}///JsonIO.JsonImport<{typeof(T)}>()内で{path + '/' + name + ".json"}を読み込もうとした際に例外が発生.");
            return default(T);//あらゆる例外に対してdefaultを返す
        }
        
        return importResultObj;
    }
    public static bool JsonExportFromTiled(string path,string name)//tiled(エディタ)のJsonデータをMapBuildingScriptに置き換えてJsonにExport
    {
        try
        {
            TileMapData tiledData;//tiledのデータを保存するオブジェクト

            tiledData=JsonImport<TileMapData>(Application.dataPath,"sample");//tiledのデータを読み込む

            MapBuildingScript exportMapBuilding = new MapBuildingScript();//一時保存用のMapBuildingScript

            exportMapBuilding.BuildingName = tiledData.Layers[0].Name;
            exportMapBuilding.Status = "Default";
            
            exportMapBuilding.MapChipIDField = new Field2D<int>();

            int height = tiledData.Height,//マップデータの縦幅
                width  = tiledData.Width; //　　　〃　　　横幅
            int minH = 0, maxH = height,//0でないデータのうち、最も上/下にあるデータの位置
                minW = 0, maxW = width; //          〃           左/右      〃     位置

            for(int i=0;i<height;i++){
                for(int j=0;j<width;j++){
                    exportMapBuilding.MapChipIDField.field[i][j] = tiledData.Layers[0].Data[i*width+j];
                    if(tiledData.Layers[0].Data[i*width+j]!=0){
                        minH = Math.Min(minH,i);
                        maxH = Math.Max(maxH,j);
                        minW = Math.Min(minW,i);
                        maxW = Math.Max(maxW,j);
                    }
                }
            }

            exportMapBuilding.MapChipIDField_Height = maxH-minW+1;//端にある0の行を無視した時のマップの縦幅
            exportMapBuilding.MapChipIDField_Width  = maxW-minW+1;//　　〃　　 列        〃          横幅
            
            exportMapBuilding.Origin = new MapCoordinate(width-(maxW+1) , height-(maxH+1));

            return JsonExport(exportMapBuilding,path,name);
        }
        catch(Exception e)
        {
            Debug.LogAssertion($"JsonIO.JsonExportFromTiled()内で{Application.dataPath}/sample.jsonを読み込もうとした際に例外が発生.");
            return false;//あらゆる例外に対してdefaultを返す
        }
        return true;
    }
}