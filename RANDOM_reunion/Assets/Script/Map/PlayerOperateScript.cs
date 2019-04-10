using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;


public class PlayerOperateScript : MovableObjectScript {

	void Awake(){
		InitializeOnAwake ();
	}

	void Start(){
		InitializeOnStart ();
	}
	// Update is called once per frame
	void Update () {
        MapCoordinate c;
		if (IsArrowInput(out c)) {
			Move(c);
		}
	}

	private bool IsArrowInput(out MapCoordinate c){
        c = new MapCoordinate(0, 0);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            c += new MapCoordinate(1, 1);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            c -= new MapCoordinate(1, 1);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            c += new MapCoordinate(1, -1);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            c -= new MapCoordinate(1, -1);
        }
        if (c != new MapCoordinate(0, 0))
            return true;
        else
            return false;
	}
}
