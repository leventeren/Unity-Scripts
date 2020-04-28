public List<GameObject> balloonList = new List<GameObject>();
#region FIND BALLOONS
foreach (GameObject balloonsFinded in GameObject.FindGameObjectsWithTag("Balloon"))
{
    if (balloonsFinded.GetComponent<Balloon>().balloonLive)
    {
        balloonList.Add(balloonsFinded);
    }
}            
#endregion

using System.Linq;
GameObject temp = list.Where(obj => obj.name == "Sword").SingleOrDefault();
   
Inventory item = inventario.Find(x=> x.name == komeslipapes); 
if(item == null){
    Debug.Log ("No!");
}
else{
    Debug.Log ("Yes!");
}

bool hasApple = inventario.Any(x => x.name == "Apple");

EnemyInfo enemyInList = enemyList.Find( x => x.enemy==target);

public List<GameObject> dropObject(GameObject gameObjectYouWantToDrop) {
  //if you want to find the gameObject itself, just replace FindIndex with Find
    Vector3 myLocation = new Vector3 (x,y,z);
    int indexOfYourGameObject = inventory.FindIndex(x => x.Equals(gameObjectYouWantToDrop));
    Instantiate (inventory[indexOfYourGameObject] , myLocation);
    inventory.RemoveAt(indexOfYourGameObject);
    return inventory;
}

int findIndex = GameManager.instance.balloonList.FindIndex(obj => obj.GetComponent<Balloon>().balloonNumber == balloonIndex);
Debug.Log("Findex: " + findIndex);
