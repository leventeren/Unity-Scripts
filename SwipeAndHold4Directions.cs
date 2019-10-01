using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeAndHold4Directions : MonoBehaviour
{
    //declare variables for later
    public float fingerInitialPositionX;
    public float fingerMovedPositionX;
    public float fingerHeldPositionX;
    public float fingerEndPositionX;

    public float fingerInitialPositionY;
    public float fingerMovedPositionY;
    public float fingerHeldPositionY;
    public float fingerEndPositionY;

    public bool swipeUpOn;
    public bool swipeDownOn;
    public bool swipeRightOn;
    public bool swipeLeftOn;
	// Use this for initialization
	void Start ()
    {
        //initialy set all booleans to false
        swipeUpOn = false;
        swipeDownOn = false;
        swipeRightOn = false;
        swipeLeftOn = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        foreach (Touch FingerTouch in Input.touches) //get touches
        {
            //do things when touch has just begun
            if (FingerTouch.phase == TouchPhase.Began)
            {
                fingerInitialPositionX = FingerTouch.position.x; //get initial X position of touch
                fingerInitialPositionY = FingerTouch.position.y; //get initial Y position of touch
                Debug.Log("Touch initiated");
            }

            //do things as soon as finger is moved (and make it not repeat theinformation every frame)
            else if (FingerTouch.phase == TouchPhase.Moved) //do things as soon as finger is moved
            {
                fingerMovedPositionX = FingerTouch.position.x; //get the new X position of touch
                fingerMovedPositionY = FingerTouch.position.y; //get the new Y position of touch

                //first case - finger is moved right, movement is predominantly horizontal (x axis)
                if (fingerMovedPositionX > fingerInitialPositionX && Mathf.Abs(fingerMovedPositionX - fingerInitialPositionX) > Mathf.Abs(fingerMovedPositionY - fingerInitialPositionY))
                {
                    //swipe right
                    if (swipeRightOn == false && swipeLeftOn == false && swipeUpOn == false && swipeDownOn == false) //make it so you can't initiate a new swipe after one has already bin initiated
                    {
                        //initiate stuff on swipe right
                        swipeRightOn = true;
                        Debug.Log("Swipe right initiated");
                    }
                }
                //second case - finger is moved left, movement is predominantly horizontal (x axis)
                else if (fingerMovedPositionX < fingerInitialPositionX && Mathf.Abs(fingerMovedPositionX - fingerInitialPositionX) > Mathf.Abs(fingerMovedPositionY - fingerInitialPositionY))
                {
                    //swipe left
                    if (swipeRightOn == false && swipeLeftOn == false && swipeUpOn == false && swipeDownOn == false)
                    {
                        //initiate stuff on swipe left
                        swipeLeftOn = true;
                        Debug.Log("Swipe left initiated");
                    }
                }

                //third case - finger is moved up, movement predominantly vertical (y axis)
                else if (fingerMovedPositionY > fingerInitialPositionY && Mathf.Abs(fingerMovedPositionX - fingerInitialPositionX) < Mathf.Abs(fingerMovedPositionY - fingerInitialPositionY))
                {
                    //swipe up
                    if (swipeRightOn == false && swipeLeftOn == false && swipeUpOn == false && swipeDownOn == false)
                    {
                        //initiate stuff on swipe up
                        swipeUpOn = true;
                        Debug.Log("Swipe up initiated");
                    }
                }

                //fourth case - finger is moved down, movement predominantly vertical (y axis)
                else if (fingerMovedPositionY < fingerInitialPositionY && Mathf.Abs(fingerMovedPositionX - fingerInitialPositionX) < Mathf.Abs(fingerMovedPositionY - fingerInitialPositionY))
                {
                    //swipe down
                    if (swipeRightOn == false && swipeLeftOn == false && swipeUpOn == false && swipeDownOn == false)
                    {
                        //initiate stuff on swipe down
                        swipeDownOn = true;
                        Debug.Log("Swipe down initiated");
                    }
                }
            }

            //do things when touch has ended
            else if (FingerTouch.phase == TouchPhase.Ended)
            {
                fingerEndPositionX = FingerTouch.position.x; //get the X position at the end, you may not need it unless you make gestures such as right and then left
                fingerEndPositionY = FingerTouch.position.y; //get the Y position at the end, you may not need it unless you make gestures such as down and then up

                //now reset all booleans and do stuff at the end of all swipes - like despawning shields on release etc.
                if (swipeRightOn == true)
                {
                    swipeRightOn = false;
                    Debug.Log("Swipe right released");
                }
                else if (swipeLeftOn == true)
                {
                    swipeLeftOn = false;
                    Debug.Log("Swipe left released");
                }
                else if (swipeUpOn == true)
                {
                    swipeUpOn = false;
                    Debug.Log("Swipe up released");
                }
                else if (swipeDownOn == true)
                {
                    swipeDownOn = false;
                    Debug.Log("Swipe down released");
                }
            }

            //else statement which makes it so you can hold down a swipe and keep things activated etc.
            else
            {
                //get current position of touch
                fingerHeldPositionX = FingerTouch.position.x;
                fingerHeldPositionY = FingerTouch.position.y;

                if (swipeRightOn == true)
                {
                    //swipe right is held
                    Debug.Log("Swipe right is held");
                }
                else if (swipeLeftOn == true)
                {
                    //swipe left is held
                    Debug.Log("Swipe left is held");
                }
                else if (swipeUpOn == true)
                {
                    //swipe up is held
                    Debug.Log("Swipe up is held");
                }
                else if (swipeDownOn == true)
                {
                    //swipe down is held
                    Debug.Log("Swipe down is held");
                }
            }
        }
    }
}
