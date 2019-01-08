﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class MovableObjectScript : MonoBehaviour {
    Rigidbody2D rigid2D = GetComponent<RigidBody2D>();
    int rbx = rigid2D.position.x;
    int rby = rigid2D.position.y;
    Vector2 from = new Vector2(
        rbx,
        rby
    );

    public void Move(Vector2 to, MapCoordinate mapcoordinate)
    {
        rigid2D.MovePosition(from + to);
        mapcoordinate.x = -MapChipScript.CHIP_WIDTH * rbx + MapChipScript.CHIP_HEIGHT * rby;
        mapcoordinate.y = MapChipScript.CHIP_WIDTH * rbx + MapChipScript.CHIP_HEIGHT * rby;
        return;
    }
    

}
