using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitCamera : MonoBehaviour {

    public Camera targetCamera;

    void Fit()
    {
        var posViewport = new Vector3(0.5f, 0.5f, targetCamera.farClipPlane - targetCamera.nearClipPlane);
        transform.position = targetCamera.ViewportToWorldPoint(posViewport);
        transform.rotation = targetCamera.transform.rotation;
        var size = 2f * targetCamera.orthographicSize;
        transform.localScale = new Vector3(size * targetCamera.aspect, size, 1f);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Fit();
	}
}
