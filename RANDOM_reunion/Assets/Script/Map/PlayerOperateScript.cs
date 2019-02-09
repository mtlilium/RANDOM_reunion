using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;


public class PlayerOperateScript : MovableObjectScript {

	[DataMember]
	Vector2 tmp;

	void Start(){
		tmp = new Vector2 (0,0);
	}
	// Update is called once per frame
	void Update () {
		if (IsInput()) {
			Move (tmp);
		}
	}


	private bool IsInput(){
		tmp = new Vector2 (0,0);
		if (!Input.anyKeyDown) {
			return false;
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			tmp += new Vector2 (1, 1);
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			tmp += new Vector2 (-1, -1);
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			tmp += new Vector2 (1, 0);
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			tmp += new Vector2 (0, 1);
		} 
		return true;
	}

}
