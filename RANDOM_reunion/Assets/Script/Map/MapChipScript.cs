using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

[DataContract]
public class MapChipScript : MonoBehaviour , IVisibleObject {
    public static int CHIP_WIDTH = 1;//マップチップ間の横の大きさ(空白だとエラーのため適当に1にしています.要修正)
    public static int CHIP_HEIGHT = 1;//マップチップ間の縦の大きさ(空白だとエラーのため適当に1にしています.要修正)

    [IgnoreDataMember]
    public MapBuildingScript Parent { get; set; }//このスクリプトを管理するMapObjectScript

    [DataMember]
    public int SpriteID{get;set;}//適用するスプライトのID
    
    [DataMember]
    public bool Collision{get;set;}//衝突判定
    
    [DataMember]
    public MapCoordinate Coordinate{get;set;}//MapCoordinateでの座標
    
    //IVisibleObject
    public void Refresh() {
        Parent = transform.parent.GetComponent<MapBuildingScript>();
        GetComponent<SpriteRenderer>().sprite = SystemVariables.SpriteList?[SpriteID] ?? SystemVariables.SpriteList?[0];
        GetComponent<MeshCollider>().enabled = Collision;
        transform.position = Coordinate.ToVector3();
    }
}