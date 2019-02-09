using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;


public class PlayerOperateScript : MovableObjectScript {

	void Start(){

	}
	// Update is called once per frame
	void Update () {
        MapCoordinate c;
		if (IsInput(out c)) {
			Move(c);
		}
	}


	private bool IsInput(out MapCoordinate c){
        c = new MapCoordinate(0, 0);
        if (!(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            return false;
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			c += new MapCoordinate(0, 1);
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            c -= new MapCoordinate(0, 1);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            c += new MapCoordinate(1, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
			c -= new MapCoordinate(1, 0);
		} 
		return true;
	}

}
