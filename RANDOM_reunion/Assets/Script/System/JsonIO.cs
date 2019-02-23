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

            tiledData=JsonImport<TileMapData>(Application.dataPath,"sample");

            /* using (var jsonStream=new StreamReader(Application.dataPath+"/sample.json",Encoding.Default) )//tiledのデータを読み込み
            {
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(TileMapData));
                tiledData = (TileMapData)jsonSerializer.ReadObject(jsonStream.BaseStream);
            } */

            /* MapBuildingScript exportObj=new MapBuildingScript();//一時保存用のMapBuildingScript

            exportObj.BuildingName=tiledData.Layers[0].Name;
            exportObj.Status="Default";
            
            exportObj.MapChipIDField=new Field2D<int>();
            int height=tiledData.Height,width=tiledData.Width;
            int minH=0,maxH=height,minW=0,maxW=width;
            for(int i=0;i<height;i++){
                for(int j=0;j<width;j++){
                    exportObj.MapChipIDField.field[i][j] = tiledData.Layers[0].Data[i*height+j];
                    if(tiledData.Layers[0].Data[i*height+j]!=0){
                        minH = Math.Min(minH,i);
                        maxH = Math.Max(maxH,j);
                        minW = Math.Min(minW,i);
                        maxW = Math.Max(maxW,j);
                    }
                }
            }

            exportObj.MapChipIDField_Width  = maxW-minW+1;
            exportObj.MapChipIDField_Height = maxH-minW+1;
            exportObj.Origin=new MapCoordinate(width-(maxW+1),height-(maxH+1)); */

            //return JsonExport(exportObj,path,name);
            return true;
        }
        catch(Exception e)
        {
            Debug.LogAssertion($"{e.Message}/////JsonIO.JsonExportFromTiled()内で{Application.dataPath}/sample.jsonを読み込もうとした際に例外が発生.");
            return false;//あらゆる例外に対してdefaultを返す
        }
        return true;
    }
}