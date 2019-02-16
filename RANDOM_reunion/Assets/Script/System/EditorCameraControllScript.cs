using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCameraControllScript : MonoBehaviour {
    public Vector3 MousePosition;
    // Use this for initialization
    void Start () {
 

}

// Update is called once per frame
void Update () {
        MousePosition = Input.mousePosition;
        Debug.Log(MousePosition);
    }
}
