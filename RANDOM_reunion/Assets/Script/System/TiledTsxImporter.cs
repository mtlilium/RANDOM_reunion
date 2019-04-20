using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class TiledTsxImporter
{
	public static TiledObjectForTsxSerialization TsxImportFromTiled(string path,string name){
		var importResultTsxObj=new TiledObjectForTsxSerialization();
		try{
			using(var importStream =new StreamReader(path + '/' + name + ".tsx", Encoding.Default)){
				var serializer = new XmlSerializer(typeof(TiledObjectForTsxSerialization));
				importResultTsxObj = (TiledObjectForTsxSerialization)serializer.Deserialize(importStream.BaseStream);
				Debug.Log("sorce:"+importResultTsxObj.tileSet.image.source);
			}
			return importResultTsxObj;
		}
		catch(Exception e){
			Debug.LogAssertion("");
			return null;
		}
	}

}


public class TiledObjectForTsxSerialization
{
	[XmlElement("tileset")]
	public TileSet tileSet;

	public class TileSet{
		[XmlElement("image")]
		public Image image;

		public class Image{
			[XmlElement("source")]
			public string source;
		}
	}
}

