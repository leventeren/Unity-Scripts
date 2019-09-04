void Update () {
     if (Input.touchCount > 0)
     {
         var touch = Input.GetTouch(0);
         if (touch.position.x < Screen.width/2)
         {
             Debug.Log ("Left click");
         }
         else if (touch.position.x > Screen.width/2)
         {
             Debug.Log ("Right click");
         }
     }
 }
