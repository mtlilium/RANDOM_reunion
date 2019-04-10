using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

[DataContract]
public class MapChipScript : MonoBehaviour , IVisibleObject {
    public static int CHIP_WIDTH = 64;//マップチップ間の横の大きさ(空白だとエラーのため適当に1にしています.要修正)
    public static int CHIP_HEIGHT = 32;//マップチップ間の縦の大きさ(空白だとエラーのため適当に1にしています.要修正)

    [IgnoreDataMember]
    public MapBuildingScript Parent { get; set; }//このスクリプトを管理するMapObjectScript

    [DataMember]
    public int SpriteID;//適用するスプライトのID
    
    [DataMember]
    public bool Collision{get;set;}//衝突判定
    
    [DataMember]
    public MapCoordinate Coordinate{get;set;}//MapCoordinateでの座標
    
    //IVisibleObject
    public void Refresh() {
        Parent = transform.parent.GetComponent<MapBuildingScript>();
        if (0 <= SpriteID && SpriteID < SystemVariables.SpriteList.Length)
            GetComponent<SpriteRenderer>().sprite = SystemVariables.SpriteList[SpriteID];
        else
            GetComponent<SpriteRenderer>().sprite = null;

        GetComponent<MeshCollider>().enabled = Collision;
        transform.position = Coordinate.ToVector3();
    }
}