using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* this script follow the points of the line renderer attached to it
* drag & drop the line renderer component to the field, or use GetComponent at start
* disable script to stop from other script component
*/

public class LineFollower : MonoBehaviour {

   /// <summary>
   /// Current main speed 
   /// </summary>
   public float speed;

   /// <summary>
   /// The line that this follow.
   /// </summary>
   public LineRenderer lineToFollow;

   /// <summary>
   /// So, we have to stop after the first lap?
   /// </summary>
   public bool justOnce = true;

   /// <summary>
   /// Internal variable, is the first lap completed?
   /// </summary>
   bool completed = false;

   /// <summary>
   /// Follow a smooth path.
   /// </summary>
   public bool smooth = false;

   /// <summary>
   /// the number of iterations that split each curve
   /// </summary>
   public int iterations = 10;
   public float radius = 0;

   /// <summary>
   /// The points of the line
   /// </summary>
   Vector3 [] wayPoints;

   /// <summary>
   /// The Current Point
   /// </summary>
   int currentPoint = 0;


   // Use this for initialization
   void OnEnable () {
       Vector3 [] temp = new Vector3[500];
       int total = 0;
       if (lineToFollow != null){
           //get the verts from LineRenderer is a headache, warning, not far of 500 verts, or you'll need to increase the numbre above
           total = lineToFollow.GetPositions(temp);
           wayPoints = new Vector3[total];
           for(int i = 0; i< total; i++)
               wayPoints[i] = temp[i];
       }
       completed = false;
   }

   void Start(){
       Vector3 [] temp = new Vector3[500];
       int total = 0;
       if (lineToFollow != null){
           total = lineToFollow.GetPositions(temp);
           wayPoints = new Vector3[total];
           for(int i = 0; i< total; i++)
               wayPoints[i] = temp[i];
       }
       completed = false;
   }

   // Update is called once per frame
   void Update () {
       if (completed){
           return;
           this.enabled = false;
       }
       if(0 < wayPoints.Length){
           if (smooth)
               FollowSmooth ();
           else {
               FollowClumsy ();
           }
       }
   }


   // Small methods to make job easy to read


   // about the array of points

   /// <summary>
   /// Prevoius the specified Index, the points.Length must be > 0.
   /// </summary>
   /// <param name="Index">index on array points.</param>
   Vector3 Prevoius(int index){
       if (0 == index) {
           return wayPoints [wayPoints.Length - 1];
       } else {
           return wayPoints [index - 1];
       }
   }

   /// <summary>
   /// Current at the specified Index.
   /// </summary>
   /// <param name="Index">index on array points.</param>
   Vector3 Current(int index){
       return wayPoints [index];
   }


   /// <summary>
   /// Next of the specified index.
   /// </summary>
   /// <param name="Index">index on array points.</param>
   Vector3 Next(int index){
       if (wayPoints.Length == index+1) {
           return wayPoints [0];
       } else {
           return wayPoints [index + 1];
       }
   }

   void IncreaseIndex(){
       currentPoint ++;
       if (currentPoint == wayPoints.Length) {
           if (justOnce)
               completed = true;
           else
               currentPoint = 0;
       }
   }



   //about 3d geometry

   /// <summary>
   /// Non smooth following
   /// </summary>
   void FollowClumsy(){
       transform.LookAt (Current (currentPoint));
       transform.position = Vector3.MoveTowards (transform.position, Current (currentPoint), speed*Time.deltaTime);
       //if is close to the waypoint, pass to the next, if is the last, stop following
       if ((transform.position-Current (currentPoint)).sqrMagnitude < (speed*Time.deltaTime) * (speed*Time.deltaTime) ) {
           IncreaseIndex ();
       }
   }

   int i = 1;


   //the function try, just try, to apply the quadratic beizer algorithm, but thos is based on number of subdivisions, not by speed, so, 
   //the speed varies, usually on closed trurns so, to minimize it I put the splits dependig of speed, but, still is a problem

   void FollowSmooth(){
       Vector3 anchor1 = Vector3.Lerp (Prevoius (currentPoint), Current (currentPoint), .5f);
       Vector3 anchor2 = Vector3.Lerp (Current (currentPoint), Next (currentPoint), .5f);

       if (i < iterations) {
           float currentProgress = (1f / (float)iterations) * (float)i;
           transform.LookAt (Vector3.Lerp (anchor1, Current (currentPoint), currentProgress));
           transform.position = Vector3.Lerp (Vector3.Lerp (anchor1, Current (currentPoint), currentProgress), Vector3.Lerp (Current (currentPoint), anchor2, currentProgress), currentProgress);
           i++;
       } else {
           i = 1;
           IncreaseIndex ();
           Vector3 absisa = Vector3.Lerp (Vector3.Lerp (anchor1, Current (currentPoint), .5f), Vector3.Lerp (Current (currentPoint), anchor2, .5f), .5f);
           float it = (((anchor1-absisa).magnitude + (anchor2 - absisa).magnitude)/(speed*Time.deltaTime));
           iterations = (int)it;
        }
   }

   /// <summary>
   /// you can also split the vertexs of the LineRenderer, and you know how to assign it, with setvertex
   /// </summary>
   /// <returns>The vertex.</returns>
   /// <param name="numSplit">Number split.</param>


   Vector3[] SplitVertex(int numSplit){
       Vector3[] ret = new Vector3[numSplit*wayPoints.Length];
       for(int oldPoint = 0; oldPoint <wayPoints.Length; oldPoint++) {
           Vector3 anchor1 = Vector3.Lerp (Prevoius (oldPoint), Current (oldPoint), .5f);
           Vector3 anchor2 = Vector3.Lerp (Current (oldPoint), Next (oldPoint), .5f);

           for (int j = 1; j < numSplit; j++) {
               float currentProgress = (1f / (float)iterations) * (float)i;
               ret[oldPoint*numSplit + j] = Vector3.Lerp (Vector3.Lerp (anchor1, Current (oldPoint), currentProgress), Vector3.Lerp (Current (oldPoint), anchor2, currentProgress), currentProgress);
           }
           IncreaseIndex ();
       }
       return ret;
   }

}
