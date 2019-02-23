using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(JsonIO.JsonExportFromTiled(Application.dataPath,"JsonExportFromTiledTestResult")){
			Debug.Log("success");
		}else{
			Debug.Log("failed");
		}
		//MapBuildingScript obj=JsonIO.JsonImport<MapBuildingScript>("","JsonExportFromTiledTestResult");
		//Debug.Log(obj.MapChipIDField_Width+","+obj.MapChipIDField_Height);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
