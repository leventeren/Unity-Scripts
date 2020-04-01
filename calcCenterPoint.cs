public void calcCenterPoint(GameObject gobj)
{
    int i = 0;
    centerPoint += player.GetComponent<PlayerController>().firstActivePiece.transform.position;

    foreach (GameObject obj in gobj.GetComponent<PlayerController>().activePieces)
    {
        if (obj)
        {
            centerPoint += obj.transform.position;
            i++;
        }
    }

    centerPoint /= i+1;

    if (debugGOBJ)
    {
        debugGOBJ.transform.position = centerPoint;
    }
}
