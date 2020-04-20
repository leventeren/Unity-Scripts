public class MapBuilderVertical : MonoBehaviour
    {

        public GameObject[] tilePrefabs;

        private Transform playerTransform;
        private float spawnY = 0.0f;
        private float tileLength = 10f;
        private int amnTilesOnScreen = 4;

        private void Awake()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            for (int i = 0; i < amnTilesOnScreen; i++)
            {
                SpawnTile();

            }

        }
        // Update is called once per frame
        private void Update()
        {
            if (playerTransform != null && playerTransform.position.y > (spawnY - amnTilesOnScreen * tileLength))
            {
                SpawnTile();
            }
        }
        private void SpawnTile(int prefabIndex = -1)
        {
            GameObject go;
            go = Instantiate(tilePrefabs[0]) as GameObject;
            go.transform.SetParent(transform);
            go.transform.position = Vector3.up * spawnY;
            spawnY += tileLength;
        }
    }
