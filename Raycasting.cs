using UnityEngine;

public class Raycasting : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject prefabToSpawn;

    [Header("Settings")]
    public float distance = 100f;

    public LayerMask collisionMask = -1;

    public float spawnRate = 5f;

    [Header("Debug")]
    public Color hitColor = Color.green;
    public Color noHitColor = Color.red;

    private float lastTimeSpawned;

    private void Update()
    {
        if(Time.time > lastTimeSpawned + 1/ spawnRate)
        {
            if(Input.GetMouseButton(0))
               PerformRaycast();
        }
    }

    private void PerformRaycast()
    {
        if(Camera.main == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool wasHit = Physics.Raycast(ray,out hit, distance, collisionMask);
        
        Color debugColor = wasHit ? hitColor : noHitColor;
        Vector3 rayEnd = wasHit ? hit.point : ray.direction * distance;

        Debug.DrawLine(Camera.main.transform.position, rayEnd, debugColor, 5f);

        if(wasHit && prefabToSpawn != null)
        {
            GameObject spawnedObject = Instantiate<GameObject>(prefabToSpawn, hit.point, Quaternion.LookRotation(hit.normal));
            lastTimeSpawned = Time.time;
        }
    }
}
