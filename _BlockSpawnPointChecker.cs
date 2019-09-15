using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnPointChecker : MonoBehaviour
{
    public bool objectAlreadyAtSpawn = false;
    public float sideLengthOfCollider;
    BoxCollider checkerCollider;

    private void Start()
    {
        objectAlreadyAtSpawn = false;
        checkerCollider = GetComponent<BoxCollider>();
        checkerCollider.size = new Vector3(sideLengthOfCollider, sideLengthOfCollider, sideLengthOfCollider);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObject") || other.CompareTag("EnemyObject") || other.CompareTag("Environment"))
        {
            objectAlreadyAtSpawn = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerObject") || other.CompareTag("EnemyObject") || other.CompareTag("Environment"))
        {
            objectAlreadyAtSpawn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerObject") || other.CompareTag("EnemyObject") || other.CompareTag("Environment"))
        {
            objectAlreadyAtSpawn = false;
        }
    }

    private void Update()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = Camera.main.gameObject.transform.position.z;

        Vector3 spawnPos = Camera.main.ScreenToWorldPoint(pos);

        gameObject.transform.position = spawnPos;
    }

}
