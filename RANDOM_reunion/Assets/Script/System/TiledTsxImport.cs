using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
//using System.Xml.Serialization;
using UnityEngine;

public class TiledTsxImport
{
    public static List<Tuple<int, List<string>>> MapchipSourceImportFromTiled(string jsonPath, string jsonName, string tsxPath)
    {
        var tilesets = JsonIO.JsonImport<TileMapData>(jsonPath, jsonName).Tilesets;
        var resultArray = new List<Tuple<int, List<string>>>();

        foreach (var ts in tilesets) {
            string[] s = ts.Source.Split('.');//拡張子(.tsx)を取り除くため分割

            if (s.Length != 2 || s[1] != "tsx")
            {
                throw new FormatException($"{jsonPath}/{jsonName}.json内のtileset.sourceの形式が不正です。tileset.source:{ts.Source}");
            }
            string tsxName = s[0];
            resultArray.Add( new Tuple<int,List<string>>(ts.FirstGId, TsxImageSourceImportFromTiled(tsxPath, tsxName)) );
        }
        return resultArray;
    }
	public static List<string> TsxImageSourceImportFromTiled(string path,string name){
		//tsxをxml形式でロード
		try{
			var tsx = XDocument.Load(path + '/' + name + ".tsx");
            var tsxTileset = tsx.Element("tileset");
            List<string> imageSource = new List<string>();
            
            if (IsTerrainData(tsx))
            {
                ///////地面のデータならtilesetの下にimageが置いてあるのでそこからsourceを取得
                string s = ImageSourceOptimize(tsxTileset.Element("image").Attribute("source").Value);
                imageSource.Add(s);
            }
            else
            {
                //////地面のデータじゃなければtileset内にtileが複数（一つかもしれないけど）あって、それぞれにimageがあるのでその中のsourceを取得
                foreach (var el in tsxTileset.Elements("tile"))
                {
                    string s = ImageSourceOptimize(el.Element("image").Attribute("source").Value);
                    imageSource.Add(s);
                }
            }
            return imageSource;
		}
		catch(Exception e){
			Debug.LogAssertion($"TiledTsxImporter.TsxImportFromTiled内で{path}/{name}.tsxを読み込み中に次の例外が発生:{e.Message}");
			return null;
		}
	}
    static string ImageSourceOptimize(string s)
    {
        ///上に関係のないパスが入ってるのでカット
        string[] ss = s.Split('/');

        string fileNameWithExtension = ss[ss.Length - 1];
        string fileName = fileNameWithExtension.Split('.')[0];
        return fileName;
    }
    private static bool IsTerrainData(XDocument xml)
    {
        return int.Parse(xml.Element("tileset").Attribute("columns").Value) > 0;
    }
}

