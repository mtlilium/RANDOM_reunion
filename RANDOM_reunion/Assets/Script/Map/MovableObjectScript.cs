using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class MovableObjectScript : MonoBehaviour {
    Rigidbody2D rigid2D;
    Vector2 from = rigid2D.transform.position;

    public void Move(Vector2 to, MapCoordinate mapcoodinate)
    {
        Vector2.Leap(this.from, from + to, 1);
        mapcoodinate.x += to.x;
        mapcoodinate.y += to.y;
        return;
    }
	
}
