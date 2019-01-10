using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class MovableObjectScript : MonoBehaviour {

    public Rigidbody2D rigid2D { get; set; }
    Vector2 from = new Vector2(
        transform.position.x,
        transform.position.y
    );

    
    public void Move(Vector2 to)
    {
        this.GetComponent<Rigidbody2D>().MovePosition(from + to);//現在地からtoの方向に進む
        
        return;
    }

    /*mapcoordinate.x = -MapChipScript.CHIP_WIDTH* rbx + MapChipScript.CHIP_HEIGHT* rby;//MapCoordinateのx,yの更新
    mapcoordinate.y = MapChipScript.CHIP_WIDTH* rbx + MapChipScript.CHIP_HEIGHT* rby;*/

    public void Move(MapCoordinate mapcoordinate)
    {

    }
}
