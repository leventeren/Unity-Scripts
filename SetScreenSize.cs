using UnityEngine;
using System.Collections;
 
public class SetScreenSize : MonoBehaviour {
 
	// Use this for initialization
	void Start () {
        GameObject mainCamera = GameObject.Find("Main Camera");
 
        Camera.main.orthographicSize = (720 * (16f / 9f) / 2) / 100;
 
        Camera.main.aspect = 9f / 16f;
 
        float camHalfHeight = Camera.main.orthographicSize;
        float camHalfWidth = Camera.main.aspect * camHalfHeight;
 
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, camHalfHeight, mainCamera.transform.position.z);
 
        Vector3 topLeftPosition = new Vector3(-camHalfWidth, camHalfHeight, 0) + Camera.main.transform.position;
        print("Top Left : " + topLeftPosition);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
