using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToRedScript : MonoBehaviour {

	// Reference to game objects Sprite Renderer component
	SpriteRenderer rend;

	// Variable to hold value to fade down to.
	// Can be adjusted in inspector with slider
	[Range(0f, 1f)]
	public float fadeToRedAmount = 0f;

	// Variable to hold fading speed
	public float fadingSpeed = 0.05f;

	// Use this for initialization
	void Start () {

		// Getting Sprite Renderer component
		rend = GetComponent<SpriteRenderer> ();

		// Getting access to Color options
		Color c = rend.material.color;

		// Setting initial values for Green and Blue channels
		c.g = 1f;
		c.b = 1f;

		// Set sprite colors
		rend.material.color = c;
		
	}

	// Coroutine to slowly fade down to desireable color
	IEnumerator FadeToRed()
	{

		// Loop that runs from 1 down to desirable Red Channel Color amount
		for (float i = 1f; i >= fadeToRedAmount; i -= 0.05f)
		{
			// Getting access to Color options
			Color c = rend.material.color;

			// Setting values for Green and Blue channels
			c.g = i;
			c.b = i;

			// Set color to Sprite Renderer
			rend.material.color = c;

			// Pause to make color be changed slowly
			yield return new WaitForSeconds (fadingSpeed);
		}
	}


	// Method that starts fading coroutine when UI button is pressed
	public void StartFadeToRed()
	{		
		StartCoroutine ("FadeToRed");
	}
}
