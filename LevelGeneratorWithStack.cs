/* LevelManager.cs */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class LevelManager : MonoBehaviour
    {
        public int _levelSize = 10;
        public Transform _levelParent;
        public GameObject currentTile;

        [Header("Finish Tile")]
        public GameObject tileFinishPrefab;

        [Header("Tiles")]
        public GameObject[] tilePrefabs;

        public int counter = 0; //for debug
        public GameObject tmp;

        public Stack<GameObject> getLeftTiles { get; set; } = new Stack<GameObject>();
        public Stack<GameObject> getTopTiles { get; set; } = new Stack<GameObject>();

        private static LevelManager instance;
        public static LevelManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<LevelManager>();
                }
                return instance;
            }
        }

        void Start()
        {
            createTiles(_levelSize);

            for (int i = 0; i < _levelSize; i++)
            {
                spawnTile();
            }
            
            createFinishTile();
        }

        public void spawnTile()
        {
            if (getLeftTiles.Count == 0 || getTopTiles.Count == 0)
            {
                createTiles(_levelSize);
            }
            int randomIndex = Random.Range(0, tilePrefabs.Length);
            if (randomIndex == 0)
            {
                tmp = getLeftTiles.Pop();
                tmp.SetActive(true);
                tmp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position;
                currentTile = tmp;
            }
            else if (randomIndex == 1)
            {
                tmp = getTopTiles.Pop();
                tmp.SetActive(true);
                tmp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position;
                currentTile = tmp;
            }

            Debug.Log(tmp.name);            
        }

        public void createFinishTile()
        {
            GameObject goFinish = Instantiate(tileFinishPrefab);
            goFinish.name = "FinishTile";
            goFinish.SetActive(true);
            goFinish.transform.SetParent(_levelParent);
            goFinish.transform.position = currentTile.transform.GetChild(0).transform.GetChild(1).position;
        }

        public void createTiles(int count)
        {
            for (int i = 0; i < count; i++)
            {
                
                GameObject go = Instantiate(tilePrefabs[0]);
                go.name = "LeftTile";
                go.SetActive(false);
                getLeftTiles.Push(go);
                go.transform.SetParent(_levelParent);

                go = Instantiate(tilePrefabs[1]);
                go.name = "TopTile";
                go.SetActive(false);
                getTopTiles.Push(go);
                go.transform.SetParent(_levelParent);
                
            }
        }

        
    }
}

/* TileItem.cs */
/* Need collider -> add level parts */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class TileItem : MonoBehaviour
    {
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("left: " + LevelManager.Instance.getLeftTiles.Count);
                Debug.Log("top: " + LevelManager.Instance.getTopTiles.Count);
                LevelManager.Instance.spawnTile();
                //LevelManager.Instance.getLeftTiles.Push(gameObject);
                //LevelManager.Instance.getTopTiles.Push(gameObject);

                switch (gameObject.name)
                {
                    case "LeftTile":
                        gameObject.SetActive(false);
                        LevelManager.Instance.getLeftTiles.Push(gameObject);
                        break;
                    case "TopTile":
                        gameObject.SetActive(false);
                        LevelManager.Instance.getTopTiles.Push(gameObject);
                        break;
                }
            }
        }
    }
}
