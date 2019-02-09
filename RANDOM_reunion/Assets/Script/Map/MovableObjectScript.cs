using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class MovableObjectScript : MonoBehaviour
{
    Rigidbody2D rb2d;


    public void Move(Vector2 direction)//引数の方向に移動
    {
        rb2d.MovePosition(direction);
    }

    public void Move(MapCoordinate mapcoordinate)//MapCoordinateのToVector2の方向に移動
    {
        Move(mapcoordinate.ToVector2());
    }
    public void Move(Vector2 direction,float q)//引数の方向に移動量Qだけ移動
    {
        Move(direction.normalized * q);
    }

    public void Move(MapCoordinate mapcoordinate, float q)//MapCoordinateのToVector2の方向に移動量Qだけ移動
    {
        Move(mapcoordinate.ToVector2(),q);
    }

    private void Awake()//起動時Rigidbody2Dを取得
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }
}
