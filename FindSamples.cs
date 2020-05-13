void Start()
{
    foreach (Transform t in transform)
    {
        if (t.name == "GATE")
        {
            floorGatesCollider = t.gameObject;
            //floorGates.Add(t.gameObject);
        }
    }
}
