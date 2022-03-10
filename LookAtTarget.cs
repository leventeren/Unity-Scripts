using System.Collections;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform Target;
    public float Speed = 1f;

    private Coroutine LookCoroutine;

    public void StartRotating()
    {
        if (LookCoroutine != null)
        {
            StopCoroutine(LookCoroutine);
        }

        LookCoroutine = StartCoroutine(LookAt());
    }

    private IEnumerator LookAt()
    {
        Quaternion lookRotation = Quaternion.LookRotation(Target.position - transform.position);

        float time = 0;

        Quaternion initialRotation = transform.rotation;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, lookRotation, time);

            time += Time.deltaTime * Speed;

            yield return null;
        }
    }

    // You can remove this, it is only for the demo.
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 30), "Look At"))
        {
            StartRotating();
        }
    }
}
