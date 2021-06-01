void findNearEnemy()
{
    fenemy = GameObject.FindGameObjectsWithTag("EnemyAi");
    Vector3 shortestDistaceToEnemy = new Vector3(1000, 1000, 1000);
    for (int i = 0; i < fenemy.Length; i++)
    {
        Vector3 distanceToEnemy = transform.position - fenemy[i].transform.position;
        if (distanceToEnemy.magnitude < shortestDistaceToEnemy.magnitude)
        {
            shortestDistaceToEnemy = transform.position - fenemy[i].transform.position;
            enemy = fenemy[i];
        }
    }
}
