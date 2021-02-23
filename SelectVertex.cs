public int ClosestIndexToPoint(Raycast ray)
{
     RaycastHit hit;
     if(Raycast(ray.origin, ray.direction, out hit))
     {
         Mesh m = hit.transform.GetComponent<MeshFilter>().sharedMesh;
         int[] tri = new int[3] {
             m.triangles[hit.triangleIndex * 3 + 0],
             m.triangles[hit.triangleIndex * 3 + 1],
             m.triangles[hit.triangleIndex * 3 + 2]    
         }
   
         // loop through hit triangle and see which vertex is closest to the hit point
         Vector3 closestDistance = Vector3.Distance(m.vertices[tri[0]], hit.point);
         int closestVertexIndex = tri[0];
         for(int i = 0; i < tri.Length; i++)
         {
             Vector3 dist = Vector3.Distance(m.vertices[tri[i]], hit.point);
             if(dist < closestDistance)
             {
                  Vector3 closestDistance = dist;
                  closestVertexIndex = tri[i];
             }
         }
 
         // returns the index of the closest vertex to hit point.
         return closestVertexIndex;
     }
     else
          return -1;
}
