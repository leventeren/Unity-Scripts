public class Enlarger : MonoBehaviour {
 
     public GameObject Player;
 
     void OnTriggerEnter(Collider other)
     {
         print("Collision detected with trigger object " + other.name);
         PlayerComponent playerComponent = other.gameObject.GetComponent<PlayerComponent> ();
         
         //checking if collided with player
         if (playerComponent) {
             StartCoroutine(ScaleOverTime(1));
         }
     }
     
     IEnumerator ScaleOverTime(float time)
     {
         Vector3 originalScale = Player.transform.localScale;
         Vector3 destinationScale = new Vector3(2.0f, 2.0f, 2.0f);
         
         float currentTime = 0.0f;
         
         do
         {
             Player.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
             currentTime += Time.deltaTime;
             yield return null;
         } while (currentTime <= time);
         
         Destroy(gameObject);
     }
 }
