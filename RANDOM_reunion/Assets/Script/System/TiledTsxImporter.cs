using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
//using System.Xml.Serialization;
using UnityEngine;

public class TiledTsxImporter
{
	public static string TsxImageSourceImportFromTiled(string path,string name){
		//tsxをxml形式でロード
		try{
			var xml = XDocument.Load(path + '/' + name + ".xml");

            string imageSource = xml.Element("tileset").Element("image").Attribute("source").Value;

            //////全てのtilesetに関してimage.sourceをlistとして取得//////////////
			/*var xmlTilesetData=xml.Elements("tileset");

			List<TiledObjectForTsxSerialization> importResults=xmlTilesetData.Select(data => new TiledObjectForTsxSerialization{
				version = data.Element("version").Value,
				source = data.Element("Image").Element("source").Value
			}).ToList();
			foreach (var imR in importResults){
				Debug.Log($"sorce:{imR.source}");
			}*/
            ////////////////////////////////////////////////////////////////////
            

            return imageSource;
		}
		catch(Exception e){
			Debug.LogAssertion($"TiledTsxImporter.TsxImportFromTiled内で{path}/{name}.xmlを読み込み中に次の例外が発生:{e.Message}");
			return null;
		}

		/*var importResultTsxObj=new TiledObjectForTsxSerialization();
		try{
			using(var importStream =new StreamReader(path + '/' + name + ".xml", Encoding.Default)){
				var serializer = new XmlSerializer(typeof(TiledObjectForTsxSerialization));
				importResultTsxObj = (TiledObjectForTsxSerialization)serializer.Deserialize(importStream.BaseStream);
			}
			Debug.Log("sorce:"+importResultTsxObj.image.source+"a");
			return importResultTsxObj;
		}
		catch(Exception e){
			Debug.LogAssertion($"TiledTsxImporter.TsxImportFromTiled内で{path}/{name}.xmlを読み込み中に次の例外が発生:{e.Message}");
			return null;
		}*/
	}
}

