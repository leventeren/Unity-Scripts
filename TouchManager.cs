void update()
{
    if (Input.GetMouseButton(0))
    {
        Vector3 mousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 mousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

        Vector3 MousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
        Vector3 MousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

        Debug.DrawRay(MousePosN, MousePosF - MousePosN, Color.green);

    }
}
