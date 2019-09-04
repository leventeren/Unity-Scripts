// If we are using mobile controls, turn left/right based on the tap side position on the screen
if (Application.isMobilePlatform)
{
    // If we have a steering wheel slider assigned, use it
    if (steeringWheel)
    {
        // If we press the mouse button, check our position relative to the screen center
        if (Input.GetMouseButton(0))
        {
            playerDirection = steeringWheel.value;
        }
        else // Otherwise, if we didn't press anything, don't rotate and straighten up
        {
            steeringWheel.value = playerDirection = 0;
        }

        steeringWheel.transform.Find("Wheel").eulerAngles = Vector3.forward * playerDirection * -100;
    }
    else if (Input.GetMouseButton(0)) // If we press the mouse button, check our position relative to the screen center
    {
        // If we are to the right of the screen, rotate to the right
        if (Input.mousePosition.x > Screen.width * 0.5f)
        {
            playerDirection = 1;
        }
        else // Othwerwise, rotate to the left
        {
            playerDirection = -1;
        }
    }
    else // Otherwise, if we didn't press anything, don't rotate and straighten up
    {
        playerDirection = 0;
    }
}
else // Otherwise, use gamepad/keyboard controls
{
    playerDirection = Input.GetAxis("Horizontal");
}

// Calculate the rotation direction
playerObject.Rotate(playerDirection);
