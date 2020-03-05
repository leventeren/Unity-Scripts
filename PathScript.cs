using System.Collections.Generic;
 using UnityEngine;
 
 public class PathScript : MonoBehaviour
 {
     public LineRenderer line;
     private List<Vector3> points = new List<Vector3>();
 
     // Use this for initialization
     public void start(LineRenderer lines)
     {
         points.Clear();
         line = lines;
         GameObject caret = null;
         caret = new GameObject("Lines");
 
         Vector3 left, right; // A position to the left of the current line
 
         // For all but the last point
         for (var i = 0; i < line.positionCount - 1; i++)
         {
             caret.transform.position = line.GetPosition(i);
             caret.transform.LookAt(line.GetPosition(i + 1));
             right = caret.transform.position + transform.right * line.startWidth / 2;
             left = caret.transform.position - transform.right * line.startWidth / 2;
             points.Add(left);
             points.Add(right);
         }
 
         // Last point looks backwards and reverses
         caret.transform.position = line.GetPosition(line.positionCount - 1);
         caret.transform.LookAt(line.GetPosition(line.positionCount - 2));
         right = caret.transform.position + transform.right * line.startWidth / 2;
         left = caret.transform.position - transform.right * line.startWidth / 2;
         points.Add(left);
         points.Add(right);
         Destroy(caret);
         DrawMesh();
     }
 
     private void DrawMesh()
     {
         Vector3[] verticies = new Vector3[points.Count];
 
         for (int i = 0; i < verticies.Length; i++)
         {
             verticies[i] = points[i];
         }
 
         int[] triangles = new int[((points.Count / 2) - 1) * 6];
 
         //Works on linear patterns tn = bn+c
         int position = 6;
         for (int i = 0; i < (triangles.Length / 6); i++)
         {
             triangles[i * position] = 2 * i;
             triangles[i * position + 3] = 2 * i;
 
             triangles[i * position + 1] = 2 * i + 3;
             triangles[i * position + 4] = (2 * i + 3) - 1;
 
             triangles[i * position + 2] = 2 * i + 1;
             triangles[i * position + 5] = (2 * i + 1) + 2;
         }
 
 
         Mesh mesh = GetComponent<MeshFilter>().mesh;
         mesh.Clear();
         mesh.vertices = verticies;
         mesh.triangles = triangles;
         mesh.RecalculateNormals();
     }
 }
