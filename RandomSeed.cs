Update(){
  Random.seed = 12345;
  Debug.Log(Random.Range(0,10)); //returns 6 every time
  Debug.Log(Random.Range(0,10)); //returns 2 every time
}
