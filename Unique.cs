private int maxNumbers = 20;
private List<int> uniqueNumbers;
private List<int> finishedList;
private List<int> finishedNumbers;

void Start(){
  uniqueNumber = new List<int>();
  finishedList = new List<int>();
}

public void GenerateRandomList(){
  for(int i = 0; i < maxNumbers; i++){
     uniqueNumbers.Add(i);
  }
  for(int i = 0; i< maxNumbers; i ++){
    int ranNum = uniqueNumbers[Random.Range(0,uniqueNumbers.Count)];
    finishedNumbers.Add(ranNum);
    uniqueNumbers.Remove(ranNum)
  }
}
