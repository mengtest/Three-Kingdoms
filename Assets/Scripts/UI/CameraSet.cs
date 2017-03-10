using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    public float Width = 1280.0f;
    public float Height = 720.0f;
    
	// Update is called once per frame
	void Update () {
	    if (Screen.width/Screen.height >= Width/Height)
	    {
	        GetComponent<Camera>().orthographicSize = Height/Screen.height;
	    }
	    else
	    {
	        GetComponent<Camera>().orthographicSize = Width/Screen.width;
	    }
	}
}
