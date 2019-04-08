using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

[DataContract]
public class MapCoordinate{
    [DataMember]
    public float x;
    [DataMember]
    public float y;

    public static int ConstOfPositionTransform { get; } = 192;

    public MapCoordinate(float _x, float _y)
    {
        x = _x;
        y = _y;
    }
    public MapCoordinate(Vector2 v)
    {
        x = v.x;
        y = v.y;
    }

    public static MapCoordinate operator + (MapCoordinate lhs, MapCoordinate rhs)
    {
        return new MapCoordinate(lhs.x + rhs.x, lhs.y + rhs.y);
    }
    public static MapCoordinate operator - (MapCoordinate lhs, MapCoordinate rhs)
    {
        return new MapCoordinate(lhs.x - rhs.x, lhs.y - rhs.y);
    }

    public Vector2 ToVector2(){//Vector2に変換
        return new Vector2(
            (-x + y)/ (ConstOfPositionTransform / MapChipScript.CHIP_WIDTH),
            ( x + y)/ (ConstOfPositionTransform / MapChipScript.CHIP_HEIGHT)
        );
    }
    public static MapCoordinate FromVector2(Vector2 vec)//Vector2から変換
    {
        return new MapCoordinate(
            (vec.y / MapChipScript.CHIP_HEIGHT - vec.x / MapChipScript.CHIP_WIDTH) * ConstOfPositionTransform / 2,
            (vec.y / MapChipScript.CHIP_HEIGHT + vec.x / MapChipScript.CHIP_WIDTH) * ConstOfPositionTransform / 2
            );
    }

    public float Depth(){//マップチップの標準的な深さに変換
        return x + y;
    }
    public Vector3 ToVector3(){//Vector3に変換
        return new Vector3(
            (-x + y) / (ConstOfPositionTransform / MapChipScript.CHIP_WIDTH),
            (x + y) / (ConstOfPositionTransform / MapChipScript.CHIP_HEIGHT),
             Depth()
        );
    }

    public new string ToString()
    {
        return $"{x},{y}";
    }
}