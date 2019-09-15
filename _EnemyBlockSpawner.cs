using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlockSpawner : MonoBehaviour
{
    [Header("Enemy Block Control")]
    //How long the delay between enemy placing blocks is
    public float delayBetweenSpawningEnemyBlocks;
    //The actual timer
    float enemyBlocksDelayTimer;
    //The box that will be spawned
    public GameObject enemyBox;
    //The game controller so we can get the list of current spawned blocks
    public GameController gameController;

    //User inputted ymin and ymax and zpos
    [Header("Height and Depth Cords")]
    public float minY;
    public float maxY;
    public float zPos;

    public float xLeftCord;
    public float xRightCord;

    public GameObject plank;

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        if (plank != null)
        {
            xLeftCord = plank.transform.position.x - plank.transform.localScale.x / 2;
            xRightCord = plank.transform.position.x + plank.transform.localScale.x / 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Spawn enemy blocks
        if (Time.time > enemyBlocksDelayTimer && gameController.playingGame)
        {
            //Pick an x position based on the smallest and largest values
            float xSpawnVal = Mathf.Round(Random.Range(xLeftCord + .5f, xRightCord - .5f));

            //We pick a random height for the block to spawn in based on ymin and ymax
            float ySpawnVal = Random.Range(minY, maxY);

            //Send where the enemy box is spawned and correlate to where the player has blocks spawned

            //spawn the box based on the random values and a predetermined zposition
            Instantiate(enemyBox, new Vector3(xSpawnVal, ySpawnVal, zPos), Quaternion.identity);

            //increase the delay timer for the next spawned block
            enemyBlocksDelayTimer = Time.time + delayBetweenSpawningEnemyBlocks;
        }
    }

}
