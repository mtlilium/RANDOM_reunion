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

	Dictionary<directions, Sprite> stateDic = new Dictionary<directions, Sprite>(); 

	enum directions{
		Up,
		UpLeft,
		UpRight,
		Left,
		Right,
		Down,
		DownLeft,
		DownRight
	}

	directions state;

    public void Move(Vector2 direction)//引数の方向に移動に移動量movementだけ移動
    {
        Move(direction, movement);
    }
    public void Move(Vector2 direction, float q)//引数の方向に移動量Qだけ移動
    {
        rb2d.MovePosition(rb2d.position + direction.normalized * q);
		GetState (direction);
		if (stateDic [state] != null) {
			sr.sprite = stateDic [state];
		}
        DepthModification();
    }

    public void DepthModification()//対象の奥行きを修正
    {
        Vector3 newPosition = transform.position;
        newPosition.z = MapCoordinate.FromVector2(transform.position).Depth() - 1.5f;
        transform.position = newPosition;
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
		InitializeOnAwake ();
    }

	protected void InitializeOnAwake(){
		rb2d = gameObject.GetComponent<Rigidbody2D>();

		//追加分0311
		sr = gameObject.GetComponent<SpriteRenderer>();
		//<どっちに動いているか,対応するSprite>
		stateDic.Add (directions.Up, Sprite_Up);
		stateDic.Add (directions.UpLeft, Sprite_UpLeft);
		stateDic.Add (directions.UpRight, Sprite_UpRight);
		stateDic.Add (directions.Left, Sprite_Left);
		stateDic.Add (directions.Right, Sprite_Right);
		stateDic.Add (directions.Down, Sprite_Down);
		stateDic.Add (directions.DownLeft, Sprite_DownLeft);
		stateDic.Add (directions.DownRight, Sprite_DownRight);
		state = directions.Down;
	}
	private void Start(){
		InitializeOnStart ();
	}

	protected void InitializeOnStart(){
		sr.sprite = stateDic [state];
		Debug.Log (state);
	}

	//0311追加分
	//Vector2から動いてる方向を取得
	void GetState(Vector2 direction){
		var x = direction.x;
		var y = direction.y;
		state = directions.Down;
		if (y > 0) {
			if (x == 0) {
				state = directions.Up;
			} else if (x < 0) {
				state = directions.UpLeft;
			} else if (x > 0) {
				state = directions.UpRight;
			}
		} else if (y == 0) {
			if (x < 0) {
				state = directions.Left;
			} else if (x > 0) {
				state = directions.Right;
			} else if (x == 0) {
				return;
			}
		} else if (y < 0) {
			if (x == 0) {
				state = directions.Down;
			} else if (x < 0) {
				state = directions.DownLeft;
			} else if (x > 0) {
				state = directions.DownRight;
			}
		}
	}
}
