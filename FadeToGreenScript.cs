﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToGreenScript : MonoBehaviour {


	// Reference to game objects Sprite Renderer component
	SpriteRenderer rend;

	// Variable to hold value to fade down to.
	// Can be adjusted in inspector with slider
	[Range(0f, 1f)]
	public float fadeToGreenAmount = 0f;

	// Variable to hold fading speed
	public float fadingSpeed = 0.05f;

	// Use this for initialization
	void Start () {

		// Getting Sprite Renderer component
		rend = GetComponent<SpriteRenderer> ();

		// Getting access to Color options
		Color c = rend.material.color;

		// Setting initial values for Red and Blue channels
		c.r = 1f;
		c.b = 1f;

		// Set sprite colors
		rend.material.color = c;

	}

	// Coroutine to slowly fade down to desireable color
	IEnumerator FadeToGreen()
	{

		// Loop that runs from 1 down to desirable Green Channel Color amount
		for (float i = 1f; i >= fadeToGreenAmount; i -= 0.05f)
		{

			// Getting access to Color options
			Color c = rend.material.color;

			// Setting values for Red and Blue channels
			c.r = i;
			c.b = i;

			// Set color to Sprite Renderer
			rend.material.color = c;

			// Pause to make color be changed slowly
			yield return new WaitForSeconds (fadingSpeed);
		}
	}

	// Method that starts fading coroutine when UI button is pressed
	public void StartFadeToGreen()
	{		
		StartCoroutine ("FadeToGreen");
	}
}
