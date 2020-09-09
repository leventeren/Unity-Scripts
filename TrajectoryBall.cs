using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject ball;
    public GameObject trajectoryDot;
    public float rotateSpeedX = 5f;
    public float rotateSpeedY = 5f;
    public float forceFactor;
    public int numberOfDots;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 initPos;
    private Rigidbody ballRb;
    private Vector3 forceAtPlayer;
    private GameObject[] trajectoryDots;

    private void Start()
    {
        initPos = Camera.main.ScreenToWorldPoint(gameObject.transform.position);
        trajectoryDots = new GameObject[numberOfDots];
        SpawnTrajectoryDots();
    }

    private void Update()
    {
        UserInput();
    }

    private void UserInput()
    {
        if (Input.GetMouseButtonDown(0))
        { //click
            SpawnBall();
            startPos = gameObject.transform.position;
            TrajectoryDotsActiveState(true);
        }

        if (Input.GetMouseButton(0))
        { //drag
            var mousePos = Input.mousePosition;
            mousePos.z = 10; // select distance = 10 units from the camera

            RotateCanonBall();

            endPos = Camera.main.ScreenToWorldPoint(mousePos) + new Vector3(0, 0, -10);
            endPos.y *= rotateSpeedY;
            endPos.x *= rotateSpeedX;
            forceAtPlayer = endPos - startPos;

            CalculateDotsPosition();
        }

        if (Input.GetMouseButtonUp(0))
        { //leave
            ballRb.useGravity = true;
            ballRb.velocity = new Vector3(-forceAtPlayer.x * forceFactor,
                                          -forceAtPlayer.y * forceFactor,
                                          -forceAtPlayer.z * forceFactor);

            TrajectoryDotsActiveState(false);
        }
    }

    private void SpawnBall()
    {
        GameObject instantiatedBall = Instantiate(ball, gameObject.transform.position, Quaternion.identity);
        ballRb = instantiatedBall.GetComponent<Rigidbody>();
    }

    private void RotateCanonBall()
    {
        //Get mouse position
        Vector3 mousePos = Input.mousePosition;

        //Adjust mouse z position
        mousePos.z = Camera.main.transform.position.y - transform.position.y;

        //Get a world position for the mouse
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        //Get the angle to rotate and rotate
        float angle = -Mathf.Atan2(transform.position.z - mouseWorldPos.z,
                                   (transform.position.x - mouseWorldPos.x) * rotateSpeedX) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle + 90, 0), 20 * Time.deltaTime);
    }

    #region Trajectory Dots

    private void SpawnTrajectoryDots()
    {
        for (int i = 0; i < numberOfDots; i++)
        {
            trajectoryDots[i] = Instantiate(trajectoryDot, gameObject.transform);
            (trajectoryDots[i] as GameObject).transform.parent = gameObject.transform;
        }
    }

    private void TrajectoryDotsActiveState(bool activeState)
    {
        for (int i = 0; i < numberOfDots; i++)
        {
            trajectoryDots[i].SetActive(activeState);
        }
    }

    private void CalculateDotsPosition()
    {
        for (int i = 0; i < numberOfDots; i++)
        {
            trajectoryDots[i].transform.position = CalculatePosition(i * 0.1f);
        }
    }

    private Vector3 CalculatePosition(float elapsedTime)
    {
        return gameObject.transform.position +
               new Vector3(-forceAtPlayer.x * forceFactor,
                           -forceAtPlayer.y * forceFactor,
                           -forceAtPlayer.z * forceFactor) * elapsedTime + 0.5f * Physics.gravity * elapsedTime * elapsedTime;
    }

    #endregion
}
