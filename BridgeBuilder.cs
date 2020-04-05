using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Construct a bridge as a tracked object moves across it.
 * Created as a part of World of Zero
 * See it in action here: https://youtu.be/GaQBLD7bGCM
 */
public class BridgeBuilder : MonoBehaviour
{
    public Transform trackedObject;

    public Transform startPoint;
    public Transform endPoint;

    public GameObject bridgePrefab;
    public float length = 1;
    public float width = 1;
    public int columnWidth = 1;

    private Vector3 forwardVector;
    private Vector3 lastPoint;

	// Use this for initialization
	void Start ()
	{
	    lastPoint = startPoint.position;
	    forwardVector = (endPoint.position - startPoint.position).normalized;
        
	    //for (var point = startPoint.position;
	    //    Vector3.Dot(forwardVector, endPoint.position - point) >= 0;
	    //    point += forwardVector * length)
	    //{
	    //    Instantiate(bridgePrefab, point, Quaternion.LookRotation(forwardVector));
	    //}
	}
	
	// Update is called once per frame
	void Update () {
	    //if (Vector3.Dot(forwardVector, endPoint.position - lastPoint) >= 0)
	    //{
	    //    Destroy(this);
	    //}

	    var point = lastPoint - forwardVector;
	    var endToPoint = point - endPoint.position;
	    var playerToPoint = point - trackedObject.position;

	    if (Vector3.Dot(endToPoint, playerToPoint) > 0)
	    {
            Instantiate(bridgePrefab, lastPoint, Quaternion.LookRotation(forwardVector));
            lastPoint += forwardVector * length;
	    }
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPoint.position, 0.25f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endPoint.position, 0.25f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(startPoint.position, endPoint.position);
    }
}
