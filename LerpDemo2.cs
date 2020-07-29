private Vector3 Pos1;
private Vector3 Pos2;
    
gameobject.transform.localPosition = Vector3.Lerp(gameobject.transform.localPosition, Pos1, Time.deltaTime / 0.2f);



private bool CheckIfColliderAbove()
{
    //returns true if anything collides with a 1 unit long raycast drawn from player's current position (ultimately checks if something solid is above him)
    return Physics.Raycast(transform.position, Vector3.up, 1);
}
