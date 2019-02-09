using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

[DataContract]
public class MapCoordinate{
    [DataMember]
    public int x;
    [DataMember]
    public int y;

    public Vector2 ToVector2(){//Vector2に変換
        return new Vector2(
            (-x + y)/ (2f / MapChipScript.CHIP_WIDTH),
            ( x + y)/ (2f / MapChipScript.CHIP_HEIGHT)
        );
    }
    public float Depth(){//マップチップの標準的な深さに変換
        return x + y;
    }
    public Vector3 ToVector3(){//Vector3に変換
        return new Vector3(
            (-x + y) / (2f / MapChipScript.CHIP_WIDTH),
            (x + y) / (2f / MapChipScript.CHIP_HEIGHT),
             Depth()
        );
    }
}