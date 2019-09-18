using UnityEngine;
 using System.Collections;
 
 public class ScoreCounter : MonoBehaviour 
 {
     public float duration = 0.5f;
     int score = 0;
     
     void OnGUI () {
         GUIButtonCountTo (0);
         GUIButtonCountTo (5000);
         GUIButtonCountTo (20000);
         GUILayout.Label ("Current score is " + score);        
     }
 
     void GUIButtonCountTo (int target) {
         if (GUILayout.Button ("Count to " + target)) {
             StopCoroutine ("CountTo");
             StartCoroutine ("CountTo", target);
         }
     }
     
     IEnumerator CountTo (int target) {
         int start = score;
         for (float timer = 0; timer < duration; timer += Time.deltaTime) {
             float progress = timer / duration;
             score = (int)Mathf.Lerp (start, target, progress);
             yield return null;
         }
         score = target;
     }
 }


/*
public int partialScore;
private int scoreT = 0;
void Start () {
    partialScore = 3000; //example score
}
void Update () {        
    if(int.Parse(GetComponent<UILabel>().text) < partialScore){
        GetComponent<UILabel>().text = scoreT.ToString();
        scoreT = scoreT + 10; //Example Step
    }
}
*/
