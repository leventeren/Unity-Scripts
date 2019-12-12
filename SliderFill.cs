using UnityEngine;
 using System.Collections;
 using UnityEngine.UI;
 
 [RequireComponent(typeof(Slider))]
 public class SliderFill : MonoBehaviour {
 
     public float fillSpeed = 1.0f;
 
     private Slider slider;
     private RectTransform fillRect;
     private float targetValue = 0f;
     private float curValue = 0f;
 
     void Awake () {
         slider = GetComponent<Slider>();
 
         //Adds a listener to the main slider and invokes a method when the value changes.
         slider.onValueChanged.AddListener (delegate {ValueChange ();});
 
         fillRect = slider.fillRect;
         targetValue = curValue = slider.value;
     }
 
     // Invoked when the value of the slider changes.
     public void ValueChange()
     {
         targetValue = slider.value;
     }
         
     // Update is called once per frame
     void Update () {
         curValue = Mathf.MoveTowards(curValue, targetValue, Time.deltaTime * fillSpeed);
 
         Vector2 fillAnchor = fillRect.anchorMax;
         fillAnchor.x = Mathf.Clamp01(curValue/slider.maxValue);
         fillRect.anchorMax = fillAnchor;
     }
 }
 
 
 
 /*
 public class Example : MonoBehaviour
 {
     public float thedamage;
     public float variable;
     public float actualvariable;
 
     private void Start()
     {
         // Both of theses floats have to be the same!
         variable = 5f;
         actualvariable = 5f;
 
         // Example damage
         thedamage = 1;
     }
 
     void DamageExample()
     {
         actualvariable -= thedamage;
     }
 
     private void Update()
     {
         variable = Mathf.Lerp(variable, actualvariable, 4 * Time.deltaTime);
         // And you can ajust the speed by changing the '4' number !
     }
 }
 */
