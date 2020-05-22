private List<GameObject> AllChilds(GameObject root)
{
    List<GameObject> result = new List<GameObject>();
    if (root.transform.childCount > 0)
    {
        foreach (Transform VARIABLE in root.transform)
        {
            Searcher(result,VARIABLE.gameObject);
        }
    }
    return result;
}

private void Searcher(List<GameObject> list,GameObject root)
{
    list.Add(root);
    if (root.transform.childCount > 0)
    {
        foreach (Transform VARIABLE in root.transform)
        {
            Searcher(list,VARIABLE.gameObject);
        }
    }
}



/* OTHER */
USE : gameobjet.GetAllChilds()
    
List<Transform> GetAllChilds(Transform _t)
 {
     List<Transform> ts = new List<Transform>();

     foreach (Transform t in _t)
     {
         ts.Add(t);
         if (t.childCount > 0)
             ts.AddRange(GetAllChilds(t));
     }

     return ts;
 }


/* EXTENSION */

public static class TransformExtension {
     public static List<Transform> GetAllChildren(this Transform parent, List<Transform> transformList = null)
      {
          if (transformList == null) transformList = new List<Transform>();
          
          foreach (Transform child in parent) {
              transformList.Add(child);
              child.GetAllChildren(transformList);
          }
          return transformList;
      }
 }
