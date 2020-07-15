using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 20f;
    public float speedBorder = 5f;

    Vector3 mMouseUpPos;
    Vector3 clickPos;
    Vector3 ballPos;

    LineRenderer lr;
    Rigidbody rb;
    [SerializeField] AnimationCurve ac;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        SwipeAndMove();
    }

    private void SwipeAndMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ballPos = gameObject.transform.position;

            lr.enabled = true;
            lr.positionCount = 2;
            lr.SetPosition(0, gameObject.transform.position);
            lr.useWorldSpace = true;
            lr.widthCurve = ac;
            lr.numCapVertices = 10;
            //lr.SetWidth(0, 10);
        }

        if (Input.GetMouseButton(0))
        {
            rb.isKinematic = true;
            clickPos = Vector3ForceCreator();
            clickPos = CalculateReflection(clickPos, new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z));

            var x = Vector3.Distance(gameObject.transform.position, clickPos);

            if (x > speedBorder)
            {
                var k = speedBorder / (x - speedBorder);
                var x1 = (gameObject.transform.position.x + (k * clickPos.x)) / (1 + k);
                var z1 = (gameObject.transform.position.z + (k * clickPos.z)) / (1 + k);
                Vector3 newPos = new Vector3(x1, 1, z1);
                lr.SetPosition(1, newPos);
            }
            else
            {
                lr.SetPosition(1, clickPos);
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            rb.isKinematic = false;
            mMouseUpPos = Vector3ForceCreator();
            var direction = ballPos - mMouseUpPos;

            direction = Vector3.ClampMagnitude(direction, speedBorder);
            GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.VelocityChange);
            lr.enabled = false;

        }

    }

    private Vector3 Vector3ForceCreator()
    {
        Vector3 clickPosition = new Vector3();
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        if (Physics.Raycast(castPoint, out RaycastHit hit, Mathf.Infinity))
        {
            clickPosition = hit.point;
        }

        return clickPosition;
    }

    private Vector3 CalculateReflection(Vector3 originalVec, Vector3 reflectionPoint)
    {
        Vector3 resultVec = new Vector3((2 * reflectionPoint.x) - originalVec.x, 1, (2 * reflectionPoint.z) - originalVec.z);
        return resultVec;
    }

}
