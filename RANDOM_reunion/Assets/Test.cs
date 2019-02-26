using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//MapBuildingScript map=new MapBuildingScript();
		//map=JsonIO.JsonImport<MapBuildingScript>(Application.dataPath,"mapBuildingScriptTest");
		
		string fileName="sample";
		//TileMapData tileMapData=new TileMapData();
		//JsonIO.JsonExport(tileMapData,Application.dataPath,fileName);
		/* TileMapData tileMapData=JsonIO.JsonImport<TileMapData>(Application.dataPath,fileName);
		Debug.Log("Width:"+tileMapData.Width); */
		
		if(JsonIO.JsonExportFromTiled(Application.dataPath,"JsonExportFromTiledTestResult")){
			Debug.Log("success");
		}else{
			Debug.Log("failed");
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
