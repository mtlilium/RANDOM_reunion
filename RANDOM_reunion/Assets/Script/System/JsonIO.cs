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
		catch(Exception e)
        {
			Debug.LogAssertion ($"{e.Message}///JsonExport");
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
    public static bool JsonExportFromTiled(string importname)//tiled(エディタ)のJsonデータをMapBuildingScriptに置き換えてJsonにExport
    {
        //Import元はTiledDataで統一する.
        //Exportについて,ファイルを"_"で区切り,最後をファイル名,それ以外をディレクトリ名として処理する.
        //
        //例: AAA_BBB_CCC.json -> (Application.dataPath)/AAA/BBB/CCC.json
        string[] splittedName = importname.Split('_');
        Queue<string> q = new Queue<string>();
        for (int i = 0; i < splittedName.Length - 1; i++)
            q.Enqueue(splittedName[i]);
        string exportFileName = splittedName[splittedName.Length - 1];
        string exportPath = Application.dataPath;

        while (q.Count > 0)
            exportPath += $"/{q.Dequeue()}";

        try
        {
            TileMapData tiledData;//tiledのデータを保存するオブジェクト
            string importPath =Application.dataPath + "/TiledData";
            string importFileName = importname;
            tiledData=JsonImport<TileMapData>(importPath, importFileName);//tiledのデータを読み込む

            MapBuildingScript exportMapBuilding = new MapBuildingScript();//一時保存用のMapBuildingScript

            exportMapBuilding.BuildingName = tiledData.Layers[0].Name;//BuildingNameにLayer0の名前を代入
            exportMapBuilding.Status = "Default";//StatusはDefaultに

            int height = tiledData.Height,//マップデータの縦幅
                width  = tiledData.Width; //　　　〃　　　横幅
            int minH = height-1, maxH = 0,//0でないデータのうち、最も上/下にあるデータの位置
                minW = width-1, maxW = 0; //          〃           左/右      〃     位置

			exportMapBuilding.MapChipIDField = new Field2D<int>(width,height);

            for(int i=0;i<height;i++){
                for(int j=0;j<width;j++){
					long data=tiledData.Layers[0].Data[i*width+j];
					if(data <=Int32.MaxValue){
						exportMapBuilding.MapChipIDField.field[j][i] = (int)data;
						if(data!=0){
							minH = Math.Min(minH,i);
							maxH = Math.Max(maxH,j);
							minW = Math.Min(minW,i);
							maxW = Math.Max(maxW,j);
						}
					}else{
						exportMapBuilding.MapChipIDField.field[j][i] = -1;
					}
                }
            }

            exportMapBuilding.MapChipIDField_Height = maxH-minH+1;//端にある0の行を無視した時のマップの縦幅
            exportMapBuilding.MapChipIDField_Width  = maxW-minW+1;//　　〃　　 列        〃          横幅
            
            exportMapBuilding.Origin = new MapCoordinate(width-(maxW+1) , height-(maxH+1));

            return JsonExport(exportMapBuilding, exportPath, exportFileName);
        }
        catch(Exception e)
        {
			Debug.LogAssertion($"{e.Message}///JsonIO.JsonExportFromTiled()内で{Application.dataPath}/sample.jsonを読み込もうとした際に例外が発生.");
            return false;//あらゆる例外に対してdefaultを返す
        }
    }
    public static void TiledJsonConvert()
    {
        string tiledPath = Application.dataPath + "/TiledData";
        string[] fileList = Directory.GetFiles(tiledPath, "*.json");

        foreach (var i in fileList) 
            JsonExportFromTiled(Path.GetFileNameWithoutExtension(Path.GetFileName(tiledPath)));
    }
}