using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRoadManager : MonoBehaviour
{

    public GameObject[] RoadPieces = new GameObject[2];
    public float RoadLength = 100; //length of roads
    public float RoadSpeed = 15f; //speed to scroll roads at    

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject road in RoadPieces)
        {
            Vector3 newRoadPos = road.transform.position;
            newRoadPos.z -= RoadSpeed * Time.deltaTime;
            if (newRoadPos.z < -RoadLength / 2)
            {
                newRoadPos.z += RoadLength;
            }
            road.transform.position = newRoadPos;
        }
    }
}
