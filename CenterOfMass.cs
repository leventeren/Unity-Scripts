using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CenterOfMass : MonoBehaviour
{
	public Vector3 _localCenterOfMass;

	private void Awake()
	{
		SetCenterOfMass();
		Destroy(this);
	}

	void SetCenterOfMass()
	{
		gameObject.GetComponent<Rigidbody>().centerOfMass = _localCenterOfMass;
	}

	private void OnDrawGizmosSelected()
	{
		Vector3 worldCenterOfMass = transform.TransformPoint(_localCenterOfMass);
		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(transform.TransformPoint(_localCenterOfMass), 0.1f);
		Gizmos.DrawLine(worldCenterOfMass + Vector3.up, worldCenterOfMass - Vector3.up);
		Gizmos.DrawLine(worldCenterOfMass + Vector3.forward, worldCenterOfMass - Vector3.forward);
		Gizmos.DrawLine(worldCenterOfMass + Vector3.right, worldCenterOfMass - Vector3.right);
	}
}
