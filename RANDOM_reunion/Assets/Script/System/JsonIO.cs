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
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            using (StreamWriter exportResultStream = new StreamWriter(path + '/' + name + ".json", false, Encoding.Default))//ファイルを読み込み
            {
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));// T 用のjsonSerializerを準備
                jsonSerializer.WriteObject(exportResultStream.BaseStream, obj);//exportして書き込み
            }
        }
		catch(Exception e)
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
            Debug.LogAssertion($"JsonIO.JsonImport<{typeof(T)}>()内で{path + '/' + name + ".json"}を読み込もうとした際に次の例外が発生:{e.Message}");
            return default(T);//あらゆる例外に対してdefaultを返す
        }
        
        return importResultObj;
    }
    public static bool JsonExportFromTiled(string importname)//tiled(エディタ)のJsonデータをMapBuildingScriptに置き換えてJsonにExport
    {
        //Import元はTiledDataで統一する.
        //Exportについて,ファイルを'_'で区切り,前とレイヤー名をディレクトリ名,後ろをファイル名として処理する.
        //
        //例: AAA_BBB.json -> (SystemVariables.RootPath)/Data/Building/AAA/(各レイヤー名)/BBB.json
        string[] splittedName = importname.Split('_');
        string header = splittedName[0];
        string footer = splittedName[1];
        string importPath =SystemVariables.RootPath + "/TiledData";
        string importFileName = importname;
        try
        {
            TileMapData tiledData;//tiledのデータを保存するオブジェクト
            
            tiledData=JsonImport<TileMapData>(importPath, importFileName);//tiledのデータを読み込む
            
            bool doneSuccessfully=true;//すべてのレイヤーをうまく読み込めればtrue

            foreach(var layer in tiledData.Layers){
                var exportMapBuildingScriptForSerialization = new MapBuildingScript.MapBuildingScriptForSerialization();//一時保存用のMapBuildingScript.MapBuildingScriptForSerialization
                exportMapBuildingScriptForSerialization.BuildingName = layer.Name;//BuildingNameにLayerの名前を代入
                exportMapBuildingScriptForSerialization.Status = footer;

                int height = tiledData.Height,//マップデータの縦幅
                    width  = tiledData.Width; //　　　〃　　　横幅
                int minH = height-1, maxH = 0,//0でないデータのうち、最も上/下にあるデータの位置
                    minW = width-1, maxW = 0; //          〃           左/右      〃     位置
                
                exportMapBuildingScriptForSerialization.MapChipIDField = new Field2D<int>(width,height);
                
                for (int i=0;i<height;i++){
                    for(int j=0;j<width;j++){
                        
                        long data=layer.Data[i*width+j];
                        
                        if(data <=Int32.MaxValue){
                            exportMapBuildingScriptForSerialization.MapChipIDField.field[j][i] = (int)data;
                            if(data!=0){
                                minH = Math.Min(minH,i);
                                maxH = Math.Max(maxH,j);
                                minW = Math.Min(minW,i);
                                maxW = Math.Max(maxW,j);
                            }
                        }else{
                            exportMapBuildingScriptForSerialization.MapChipIDField.field[j][i] = -1;//intの範囲に収まらない場合-1
                        }
                    }
                }
                
                exportMapBuildingScriptForSerialization.MapChipIDField_Height = maxH-minH+1;//端にある0の行を無視した時のマップの縦幅
                exportMapBuildingScriptForSerialization.MapChipIDField_Width  = maxW-minW+1;//　　〃　　 列        〃          横幅
              
                exportMapBuildingScriptForSerialization.Origin = new MapCoordinate(width-(maxW+1) , height-(maxH+1));
                
                if(doneSuccessfully){//doneSuccesFullyがtrueなら更新　すでにfalseなら失敗が確定しているので更新の必要なし
                    doneSuccessfully = JsonExport(exportMapBuildingScriptForSerialization, Application.dataPath +"/Data/Building/" + header + "/" + layer.Name,footer);
                }
            }
            return doneSuccessfully;
        }
        catch(Exception e)
        {
			Debug.LogAssertion($"JsonIO.JsonExportFromTiled()内で{importPath}/{importname}.jsonを読み込もうとした際に次の例外が発生:{e.Message}");
            return false;//あらゆる例外に対してdefaultを返す
        }
    }
    public static void TiledJsonConvert()
    {
        string tiledPath = SystemVariables.RootPath + "/TiledData";
        string[] fileList = Directory.GetFiles(tiledPath, "*.json");

        foreach (var file in fileList)
        {
            JsonExportFromTiled(Path.GetFileNameWithoutExtension(Path.GetFileName(file)));
        }
    }
}