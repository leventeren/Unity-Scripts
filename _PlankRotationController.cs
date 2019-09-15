using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankRotationController : MonoBehaviour
{
    [Header("Plank")]
    public GameObject plank;

    //Rotation Control
    [Header("Rotation Control")]
    //The plank where the player want to land the values
    //Basically how this works is how far to the left or right the plank can rotate to
    //The max rotation angle, this is the max angle that the plank can rotate to
    public float maxRightRotationAnglePlank;
    //The min rotation angle, this is the minimum angle the plank can rotate to
    public float maxLeftRotationAnglePlank;
    //How long before the plank picks a new rotation
    public int plankRotationDelay;
    //How fast the plank will move to the new rotation MAX
    public float maxRotationSpeedPlank;
    //the slowest the plank will move to its next rotation
    public float minRotationSpeedPlank;
    //I dont know what this does
    //public float rotationAdder;
    //The timer counting up to the next rotation
    float plankRotationTimer;
    //the next target rotation
    float plankRotationTarget;
    //the speed the plank will rotate to the next rotation
    float plankRotationSpeed;
    public GameController gameController;


    // Use this for initialization
    void Start()
    {
        plankRotationTimer = Time.time + plankRotationDelay;
        plank = gameObject;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate platform
        //if the current global time is longer than the plank rotation timer its time to rotate again
        if (Time.time > plankRotationTimer && gameController.playingGame)
        {
            plankRotationTarget = Mathf.Round(Random.Range(-maxLeftRotationAnglePlank, maxRightRotationAnglePlank));

            //No idea what this does
            //plankRotationTarget += (Mathf.Sign(plankRotationTarget) * rotationAdder);

            plankRotationSpeed = Random.Range(minRotationSpeedPlank, maxRotationSpeedPlank);
            plankRotationTimer = Time.time + plankRotationDelay;
        }

        //create the new value and move towards it
        float angle = Mathf.MoveTowardsAngle(plank.transform.eulerAngles.z, plankRotationTarget, plankRotationSpeed * Time.deltaTime);
        //set the transform of the plank
        plank.transform.eulerAngles = new Vector3(0, 0, angle);
        //rotate platform ends
    }
}
