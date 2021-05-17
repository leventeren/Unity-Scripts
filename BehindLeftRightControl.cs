var directionToEnemy = transform.position - player.transform.position;
var projectionOnRight = Vector3.Dot(directionToEnemy, player.transform.right);
if (projectionOnRight < 0)
{
    Debug.Log("LEFT");

}
else if (projectionOnRight > 0)
{
    Debug.Log("RIGHT");
}
