public void GroupResize (int Size, ref GameObject[] Group)
 {
         
     GameObject[] temp = new GameObject[Size];
     for (int c = 1; c < Mathf.Min(Size, Group.Length); c++ ) {
         temp [c] = Group [c];
     }
     Group = temp;
 }
         
 void Start ()
 {
     GameObject[] Test = new GameObject[5];
     Debug.Log (Test.Length);
     GroupResize (15, ref Test);
     Debug.Log (Test.Length);
 }
