using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////
/// Author: Nicolas Granese.
/// To contact the author, please use the electronic address nicolasgranese65@gmail.com
//////

/// <summary>
/// This script takes the shortest distance from several raycasts.
/// </summary>

public class Proximity_sensor : MonoBehaviour
{
	public Transform[] sensors;
	public RaycastHit[] hitINFO;
	public LayerMask rayMask;
	public float maxRayDistance = 5f;
	private readonly float min = 1.5f, max = 4.0f;

	private float maxDist = Mathf.Infinity;
	public float factor;

	private int distIndex = 0;
	public int actualIndex;

    // Update is called once per frame
    void Update()
    {
        ProximitySensor();
    }

	void ProximitySensor()
	{
		hitINFO = new RaycastHit[sensors.Length];
		for (int n = 0; n < sensors.Length; n++)
		{
			
			if (Physics.Raycast(sensors[n].transform.position, sensors[n].transform.right, out hitINFO[n], maxRayDistance, rayMask)) // Uses local rotation
			{
				Debug.DrawRay(sensors[n].transform.position, sensors[n].transform.right * hitINFO[n].distance, Color.red);

				// Gets the shortest hit distance.
				if (hitINFO[n].distance < maxDist)
				{
					maxDist = hitINFO[n].distance;
					distIndex = n;
				}
				else
				{
					distIndex = sensors.Length + 1;
					maxDist = hitINFO[n].distance;
				}
			}
			else
			{
				Debug.DrawRay(sensors[n].position, sensors[n].right * maxRayDistance, Color.green);

				maxDist = Mathf.Infinity;
			}
			if (distIndex != (sensors.Length + 1))
			{
				// Generates the interpolation factor.
				actualIndex = distIndex;
				factor = Mathf.InverseLerp(max, min, hitINFO[actualIndex].distance);

				// Command line debug.
				string debug = string.Format("Shortest Hit Index: {0}  | Distance: {1}  |  Interpolation: {2}", actualIndex, hitINFO[actualIndex].distance, factor);
				print(debug);
			}
		}
	}
}
