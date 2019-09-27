public IEnumerator DoDeleteAll()
 {
     while (Holder.transform.childCount > 0)
     {
         var items = GameObject.FindGameObjectsWithTag("YOUR_TAG");
         foreach (var item in items)
         {
             Destroy(item );
         }
         yield return new WaitForSeconds(0.001f);
     }
 }
 
 ####################
 
 public static class TransformEx {
     public static Transform Clear(this Transform transform)
     {
         foreach (Transform child in transform) {
             GameObject.Destroy(child.gameObject);
         }
         return transform;
     }
 }
 
 #####################
 
 var children = new List<GameObject>();
 foreach (Transform child in transform) children.Add(child.gameObject);
 children.ForEach(child => Destroy(child));
