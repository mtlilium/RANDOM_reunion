using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class MovableObjectScript : MonoBehaviour
{
    Rigidbody2D rb2d;


    public void Move(Vector2 direction)//引数の方向に瞬間移動
    {
        rb2d.MovePosition(direction);
        return;
    }

    public void Move(MapCoordinate mapcoordinate)//MapCoordinateのToVector2の方向に瞬間移動
    {
        rb2d.MovePosition(mapcoordinate.ToVector2());
    }

    private void Awake()//起動時Rigidbody2Dを取得
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }
}
