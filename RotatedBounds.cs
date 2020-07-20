using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// HOW TO CALCULATE A NON-AXIS ALIGNED BOUNDING BOX FOR AN OBJECT.
// This data could be used for something like collision calculations.
// Takes into consideration both local scaling and rotation.
public class RotatedBounds : MonoBehaviour
{
    Vector3[] sourcePoints = new Vector3[8];
    Vector3[] points = new Vector3[8];

    MeshRenderer meshRenderer;


    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }


    void Update()
    {
        // Store original rotation
        Quaternion originalRotation = transform.rotation;

        // Reset rotation
        transform.rotation = Quaternion.identity;

        // Get object bounds from unrotated object
        Bounds bounds = meshRenderer.bounds;

        // Get the unrotated points
        sourcePoints[0] = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z) - transform.position; // Bot left near
        sourcePoints[1] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z) - transform.position; // Bot right near
        sourcePoints[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z) - transform.position; // Top left near
        sourcePoints[3] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z) - transform.position; // Top right near
        sourcePoints[4] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z) - transform.position; // Bot left far
        sourcePoints[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z) - transform.position; // Bot right far
        sourcePoints[6] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z) - transform.position; // Top left far
        sourcePoints[7] = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z) - transform.position; // Top right far

        // Restore rotation
        transform.rotation = originalRotation;

        // Apply scaling
        for (int s = 0; s < sourcePoints.Length; s++)
        {
            sourcePoints[s] = new Vector3(sourcePoints[s].x / transform.localScale.x,
                                          sourcePoints[s].y / transform.localScale.y,
                                          sourcePoints[s].z / transform.localScale.z);
        }

        // Transform points from local to world space
        for (int t = 0; t < points.Length; t++)
        {
            points[t] = transform.TransformPoint(sourcePoints[t]);
        }
    }


    // Visualize in Editor viewport
    void OnDrawGizmos()
    {
        if (points.Length == 0)
            return;

        Color c = Color.green;

        // near quad
        Debug.DrawLine (points[0], points[1], c); // Bot left near to bot right near
        Debug.DrawLine (points[2], points[3], c); // Top left near to top right near
        Debug.DrawLine (points[0], points[2], c); // Bot left near to top left near
        Debug.DrawLine (points[1], points[3], c); // Bot right near to top right near
        // far quad
        Debug.DrawLine (points[4], points[5], c); // Bot left far to bot right far
        Debug.DrawLine (points[6], points[7], c); // Top left far to top right far
        Debug.DrawLine (points[4], points[6], c); // Bot left far to top left far
        Debug.DrawLine (points[5], points[7], c); // Bot right far to top right far
        // 4 lines connecting the quads
        Debug.DrawLine (points[0], points[4], c); // Bot left near to bot left far
        Debug.DrawLine (points[1], points[5], c); // Bot right near to bot right far
        Debug.DrawLine (points[2], points[6], c); // Top left near to top left far
        Debug.DrawLine (points[3], points[7], c); // Top right near to top right far
    }

}
