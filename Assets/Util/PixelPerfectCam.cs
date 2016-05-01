using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PixelPerfectCam : MonoBehaviour {
    // Use this for initialization
    private int screenH;
    Camera cam;
    void Start ()
    {
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Screen.height != screenH)
        {
            cam.orthographicSize = Screen.height / 4;
            screenH = Screen.height;
        }
    }
}
