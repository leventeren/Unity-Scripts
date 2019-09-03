using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public float followSpeed;

	public Vector3 offset;
	private Vector3 refVelocity;

	public Transform target;

	void FixedUpdate() {

		Vector3 desiredPos = target.position + offset;
		Vector3 smoothedPos = Vector3.SmoothDamp(
			
			transform.position, desiredPos, ref refVelocity, 
			followSpeed * Time.deltaTime
		
		);

		transform.position = smoothedPos;

	}

}
