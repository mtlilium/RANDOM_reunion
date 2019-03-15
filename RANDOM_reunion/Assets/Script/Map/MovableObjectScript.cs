using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class MovableObjectScript : MonoBehaviour
{
    Rigidbody2D rb2d;
	SpriteRenderer sr;

    float movement = 1;

    public Sprite Sprite_Up        = null;
    public Sprite Sprite_UpLeft    = null;
    public Sprite Sprite_UpRight   = null;
    public Sprite Sprite_Left      = null;
    public Sprite Sprite_Right     = null;
    public Sprite Sprite_Down      = null;
    public Sprite Sprite_DownLeft  = null;
    public Sprite Sprite_DownRight = null;

	Dictionary<string, Sprite> stateDic = new Dictionary<string, Sprite>(); 

    public void Move(Vector2 direction)//引数の方向に移動に移動量movementだけ移動
    {
        rb2d.MovePosition(rb2d.position + direction.normalized * movement);

		string tmpState = GetState (direction);
		if (stateDic [tmpState] != null) {
			sr.sprite = stateDic [tmpState];
		}
    }
    public void Move(Vector2 direction, float q)//引数の方向に移動量Qだけ移動
    {
        rb2d.MovePosition(rb2d.position + direction.normalized * q);
		string tmpState = GetState (direction);
		if (stateDic [tmpState] != null) {
			sr.sprite = stateDic [tmpState];
		}
    }

    public void Move(MapCoordinate mapcoordinate)//MapCoordinateのToVector2の方向に移動量movementだけ移動
    {
        Move(mapcoordinate.ToVector2());
    }
    public void Move(MapCoordinate mapcoordinate, float q)//MapCoordinateのToVector2の方向に移動量Qだけ移動
    {
        Move(mapcoordinate.ToVector2(),q);
    }

    private void Awake()//起動時Rigidbody2Dを取得
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();

		//追加分0311
		sr = gameObject.GetComponent<SpriteRenderer>();
		//<どっちに動いているか,対応するSprite>
		stateDic.Add ("Up", Sprite_Up);
		stateDic.Add ("UpLeft", Sprite_UpLeft);
		stateDic.Add ("UpRight", Sprite_UpRight);
		stateDic.Add ("Left", Sprite_Left);
		stateDic.Add ("Right", Sprite_Right);
		stateDic.Add ("Down", Sprite_Down);
		stateDic.Add ("DownLeft", Sprite_DownLeft);
		stateDic.Add ("DownRight", Sprite_DownRight);

    }

	//0311追加分
	//Vector2から動いてる方向を取得
	string GetState(Vector2 direction){
		var x = direction.x;
		var y = direction.y;
		string state = "Idol";
		if (y > 0) {
			if (x == 0) {
				state = "Up";
			} else if (x < 0) {
				state = "UpLeft";
			} else if (x > 0) {
				state = "UpRight";
			}
		} else if (y == 0) {
			if (x < 0) {
				state = "Left";
			} else if (x > 0) {
				state = "Right";
			}
		} else if (y < 0) {
			if (x == 0) {
				state = "Down";
			} else if (x < 0) {
				state = "DownLeft";
			} else if (x > 0) {
				state = "DownRight";
			}
		}
		return state;
	}
}
