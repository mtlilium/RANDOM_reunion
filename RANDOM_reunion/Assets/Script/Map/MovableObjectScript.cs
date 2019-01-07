using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class MovableObjectScript : MonoBehaviour {
    Rigidbody2D rigid2D;
    public static Vector2 from = rigid2D.transform.position;

    public void Move(Vector2Int to)
    {
        Vector2.Lerp(this.from, from + to, 1);
        
        return;
    }
    public override Vector2 ToVector2()
    {//Vector2に変換
        return new Vector2(
        (-x + y) / 2f / MapChipScript.CHIP_WIDTH,
        (x + y) / 2f / MapChipScript.CHIP_HEIGHT
     );
    }


}
