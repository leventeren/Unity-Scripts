public Transform someObject; //object that moves along parabola.
float objectT = 0; //timer for that object

public Transform Ta, Tb; //transforms that mark the start and end
public float h; //desired parabola height

Vector3 a, b; //Vector positions for start and end

void Update () {
    if ( Ta &&  Tb ) {
        a = Ta.position; //Get vectors from the transforms
        b = Tb.position;

        if ( someObject ) {
            //Shows how to animate something following a parabola
            objectT = Time.time % 1; //completes the parabola trip in one second
            someObject.position = SampleParabola( a, b, h, objectT );
        }
    }
}


void OnDrawGizmos () {

    //Draw the parabola by sample a few times
    Gizmos.color = Color.red;
    Gizmos.DrawLine( a, b );
    float count = 20;
    Vector3 lastP = a;
    for ( float i = 0; i < count + 1; i++ ) {
        Vector3 p = SampleParabola( a, b, h, i / count );
        Gizmos.color = i % 2 == 0 ? Color.blue : Color.green;
        Gizmos.DrawLine( lastP, p );
        lastP = p;
    }
}

#region Parabola sampling function
/// <summary>
/// Get position from a parabola defined by start and end, height, and time
/// </summary>
/// <param name='start'>
/// The start point of the parabola
/// </param>
/// <param name='end'>
/// The end point of the parabola
/// </param>
/// <param name='height'>
/// The height of the parabola at its maximum
/// </param>
/// <param name='t'>
/// Normalized time (0->1)
/// </param>S
Vector3 SampleParabola ( Vector3 start, Vector3 end, float height, float t ) {
    float parabolicT = t * 2 - 1;
    if ( Mathf.Abs( start.y - end.y ) < 0.1f ) {
        //start and end are roughly level, pretend they are - simpler solution with less steps
        Vector3 travelDirection = end - start;
        Vector3 result = start + t * travelDirection;
        result.y += ( -parabolicT * parabolicT + 1 ) * height;
        return result;
    } else {
        //start and end are not level, gets more complicated
        Vector3 travelDirection = end - start;
        Vector3 levelDirecteion = end - new Vector3( start.x, end.y, start.z );
        Vector3 right = Vector3.Cross( travelDirection, levelDirecteion );
        Vector3 up = Vector3.Cross( right, travelDirection );
        if ( end.y > start.y ) up = -up;
        Vector3 result = start + t * travelDirection;
        result += ( ( -parabolicT * parabolicT + 1 ) * height ) * up.normalized;
        return result;
    }
}


/* OPTIMIZE */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test5 : MonoBehaviour
{
    public Transform _moveableObject; // Objet to move on path
    public GameObject _trajectoryElement; // GO to draw in path of trajectory. GO having signle white arrow attached on it
    public Transform _startPosition; // Start position from where trajectory should start and mouse of finger position is the destination
    public float _height; //desired parabola height
    public int _numberOfElements = 10; // Number of elements should draw in path of parabola

    Vector3 a, b; //Vector positions for start and end
    List<GameObject> _trajectoryElementsContainer = new List<GameObject> ();

    void Start ()
    {
        // For now I have instantiated all the objects will be drawn on the path of parabola. Then did handle them later.
        // Please modify it according to your needs
        for (int i = 0; i < _numberOfElements; i++)
            _trajectoryElementsContainer.Add (Instantiate (_trajectoryElement) as GameObject);
    }

    void Update ()
    {
        if (Input.GetMouseButton (0) && _startPosition) {
            a = _startPosition.position; //Get vectors from the transforms
            a = new Vector3 (a.x, a.y, 0);
            b = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            b = new Vector3 (b.x, b.y, 0);

            float distributionTime = 0;
            for (float i = 1; i <= _numberOfElements; i++) {
                distributionTime++;
                Vector3 currentPosition = SampleParabola (a, b, _height, i / (float)_numberOfElements);
                _trajectoryElementsContainer [(int)i - 1].transform.position = new Vector3 (currentPosition.x, currentPosition.y, 0);

                Vector3 nextPosition = SampleParabola (a, b, _height, (i + 1) / (float)_numberOfElements);
                float angleInR = Mathf.Atan2 ((nextPosition.y - currentPosition.y), (nextPosition.x - currentPosition.x));
                _trajectoryElementsContainer [(int)i - 1].transform.eulerAngles = new Vector3 (0, 0, (Mathf.Rad2Deg * angleInR) - 90);
            }

            if (_moveableObject) {
                //Shows how to animate something following a parabola
                _moveableObject.position = SampleParabola (a, b, _height, Time.time % 1);
            }
        }
    }


    void OnDrawGizmos ()
    {

        //Draw the parabola by sample a few times
        Gizmos.color = Color.red;
        Gizmos.DrawLine (a, b);
        float count = 20;
        Vector3 lastP = a;
        for (float i = 0; i < count + 1; i++) {
            Vector3 p = SampleParabola (a, b, _height, i / count);
            Gizmos.color = i % 2 == 0 ? Color.blue : Color.green;
            Gizmos.DrawLine (lastP, p);
            lastP = p;
        }
    }

    #region Parabola sampling function
    /// <summary>
    /// Get position from a parabola defined by start and end, height, and time
    /// </summary>
    /// <param name='start'>
    /// The start point of the parabola
    /// </param>
    /// <param name='end'>
    /// The end point of the parabola
    /// </param>
    /// <param name='height'>
    /// The height of the parabola at its maximum
    /// </param>
    /// <param name='t'>
    /// Normalized time (0->1)
    /// </param>S
    Vector3 SampleParabola (Vector3 start, Vector3 end, float height, float t)
    {
        float parabolicT = t * 2 - 1;
        if (Mathf.Abs (start.y - end.y) < 0.1f) {
            //start and end are roughly level, pretend they are - simpler solution with less steps
            Vector3 travelDirection = end - start;
            Vector3 result = start + t * travelDirection;
            result.y += (-parabolicT * parabolicT + 1) * height;
            return result;
        } else {
            //start and end are not level, gets more complicated
            Vector3 travelDirection = end - start;
            Vector3 levelDirecteion = end - new Vector3 (start.x, end.y, start.z);
            Vector3 right = Vector3.Cross (travelDirection, levelDirecteion);
            Vector3 up = Vector3.Cross (right, travelDirection);
            if (end.y > start.y)
                up = -up;
            Vector3 result = start + t * travelDirection;
            result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
            return result;
        }
    }
    #endregion
}
