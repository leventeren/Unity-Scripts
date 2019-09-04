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

//multi touch

private int finId1 = -1; //id finger for cancel touch event
private int finId2 = -1;

void Start() {
     Input.multiTouchEnabled = true; //enabled Multitouch
}

void Update() {
     //First check count of touch
     if (Input.touchCount > 0) {
          foreach (Touch touch in Input.touches) {
               //For left half screen
               if (touch.phase  == TouchPhase.Began && touch.position.x <= Screen.width && finId1 == -1) {
                    //Do something: start other function
                    finId1 = touch.fingerId; //store Id finger
               }
               //For right half screen
               if (touch.phase  == TouchPhase.Began && touch.position.x > Screen.width && finId2 == -1) {
                    //Do something
                    finId2 = touch.fingerId;
               }
               if (touch.phase == TouchPhase.Ended) { //correct end of touch
                    if(touch.fingerId == finId1) { //check id finger for end touch
                         finId1 = -1;
                    } else if(touch.fingerId == finId2) {
                         finId2 = -1;
                    }
               }
          }
     }
}
