using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //for NavMeshAgent

[RequireComponent(typeof(NavMeshAgent))]
public class Bot : MonoBehaviour
{
    public float MinTime = 2;
    public float MaxTime = 5;
    public GameObject Ground = null;
    private NavMeshAgent nma = null;
    private Bounds bounds;

    private void Start()
    {
        nma = this.GetComponent<NavMeshAgent>();
        bounds = Ground.GetComponent<Renderer>().bounds;
    }
    private void Update()
    {
        if (nma.hasPath == false || nma.remainingDistance < 1.0f)
        {
            float wait = Random.Range(MinTime, MaxTime);
            this.GetComponent<Renderer>().material.color = Color.red;
            Invoke("PickRandomDestination", wait);
        }
    }
    private void PickRandomDestination()
    {
        float rx = Random.Range(bounds.min.x, bounds.max.x);
        float rz = Random.Range(bounds.min.z, bounds.max.z);
        Vector3 rpos = new Vector3(rx, this.transform.position.y, rz);
        nma.SetDestination(rpos);
        this.GetComponent<Renderer>().material.color = Color.green;
    }
}
