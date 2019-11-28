public int groupCount = 1;
public float spawnRadius = 1f;
    
usage: CreateEnemiesAroundPoint(groupCount, gameObject.transform.position, spawnRadius);

public void CreateEnemiesAroundPoint(int num, Vector3 point, float radius)
    {
        for (int i = 0; i < num; i++)
        {

            /* Distance around the circle */
            var radians = 2 * Mathf.PI / num * i;

            /* Get the vector direction */
            var vertrical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector3(horizontal, 0, vertrical);

            /* Get the spawn position */
            var spawnPos = point + spawnDir * radius; // Radius is just the distance away from the point

            /* Now spawn */
            var enemy = Instantiate(runnerMan, spawnPos, Quaternion.Euler(0f, 90f, 0f)) as GameObject;
            enemy.transform.SetParent(gameObject.transform);
            
            /* Rotate the enemy to face towards player */
            //enemy.transform.LookAt(point);

            /* Adjust height */
            //enemy.transform.Translate(new Vector3(0, enemy.transform.localScale.y / 2, 0));
        }
    }
