using UnityEngine;
using System.Collections;

public class HoverCam : MonoBehaviour {
//	The object to follow.
	public Transform target;

//	Spring parameters for hovering, following and "whiskers" (forces to push
//	camera away from surrounding walls).
	public LinearSpring hoverSpring, followSpring, whiskerSpring;
	
	Vector3 xz = Vector3.right + Vector3.forward;
	
	
	void Start () {
	//	Set hover spring to counteract gravity.
		hoverSpring.baseForce = -Physics.gravity.magnitude * rigidbody.mass;
	}
	
	
	void FixedUpdate () {
		RaycastHit hit;
		
		Vector3 camPos = transform.position;
		Vector3 targetPos = target.position;
		
	//	Hovering.
		Vector3 hoverForce = Vector3.zero;
		
		if (Physics.Raycast(camPos, Physics.gravity, out hit)) {
			hoverForce = hoverSpring.Reaction(camPos, hit.point, rigidbody.velocity);
		}
		
	//	Following.
		Vector3 relVelocity = rigidbody.velocity - target.rigidbody.velocity;
		Vector3 followForce = followSpring.Reaction(camPos, targetPos, relVelocity);
		
	//	Whiskers.
		Vector3 whiskerForce = Vector3.zero;
		
		if (Physics.Raycast(camPos, transform.up, out hit)) {	
			whiskerForce += whiskerSpring.Reaction(camPos, hit.point, rigidbody.velocity);
		}
		
		if (Physics.Raycast(camPos, transform.right, out hit)) {	
			whiskerForce += whiskerSpring.Reaction(camPos, hit.point, rigidbody.velocity);
		}
		
		if (Physics.Raycast(camPos, -transform.right, out hit)) {	
			whiskerForce += whiskerSpring.Reaction(camPos, hit.point, rigidbody.velocity);
		}
		
		rigidbody.AddForce(hoverForce + followForce + whiskerForce);
		
	//	Lines to visualise where the whiskers extend to. Comment out for efficiency
	//	when not debugging.
	//	/*
		Debug.DrawLine(camPos, camPos + transform.TransformDirection(-Vector3.right * whiskerSpring.idealDistance));
		Debug.DrawLine(camPos, camPos + transform.TransformDirection(Vector3.right * whiskerSpring.idealDistance));
		Debug.DrawLine(camPos, camPos + transform.TransformDirection(Vector3.up * whiskerSpring.idealDistance));
	/*	*/
	
	//	Azimuth.
		Vector3 groundVec = Vector3.Scale(targetPos - camPos, xz);
		transform.rotation = Quaternion.LookRotation(groundVec);
	
	//	Elevation.
		Camera cam = Camera.main;
		
		float groundDist = groundVec.magnitude;
		float heightDiff = camPos.y - targetPos.y;
		float elevAngle = Mathf.Atan(heightDiff / groundDist) * Mathf.Rad2Deg;
		cam.transform.localRotation = Quaternion.AngleAxis(elevAngle, Vector3.right);
	}
}




/* spring.cs file */

using UnityEngine;
using System.Collections;

public enum LinearSpringMode { CompressExtend, CompressOnly, ExtendOnly }

//	LinearSpring is used to calculate the force that must be applied to a
//	rigidbody to keep it an ideal distance from a target object.

//	The parameters are:-

//	Base force: spring pushes with this force regardless of spring extension - this
//	is mainly useful for anti-gravity springs.

//	Reaction force: how much force is applied per unit extension.

//	Ideal distance: the distance at which no push/pull is applied.

//	Damping: reduction of force per unit velocity in spring's direction of movement.

//	Mode: spring can be set to register only compression, only extension or both
//	(the default is both).

[System.Serializable]
public class LinearSpring {

	public float baseForce, reactionForce, idealDistance, damping;
	public LinearSpringMode mode;
	
	
	public LinearSpring(	float baseForce,
							float reactionForce,
							float idealDistance,
							float damping,
							LinearSpringMode mode
						) {
		this.baseForce = baseForce;
		this.reactionForce = reactionForce;
		this.idealDistance = idealDistance;
		this.damping = damping;
		this.mode = mode;
	}
	
	
	public LinearSpring(	float baseForce,
							float reactionForce,
							float idealDistance,
							float damping
						) {
		this.baseForce = baseForce;
		this.reactionForce = reactionForce;
		this.idealDistance = idealDistance;
		this.damping = damping;
		this.mode = LinearSpringMode.CompressExtend;
	}
	
	
	public LinearSpring(	float reactionForce,
							float idealDistance,
							float damping
						) {
		this.baseForce = 0f;
		this.reactionForce = reactionForce;
		this.idealDistance = idealDistance;
		this.damping = damping;
		this.mode = LinearSpringMode.CompressExtend;
	}
	
//	The reaction is the force that the object at point a gets from the spring.
//	Just negate the result if you need a symmetrical reaction from the object
//	at b. Relative velocity is velocity of a minus velocity of b.
	public Vector3 Reaction(Vector3 a, Vector3 b, Vector3 relVelocity) {
		Vector3 springVec = b - a;
		float dist = springVec.magnitude;
		float tension = dist - idealDistance;
		
		if (	((mode == LinearSpringMode.CompressOnly) && (tension > 0)) ||
				((mode == LinearSpringMode.ExtendOnly) && (tension < 0)) ) {
			return Vector3.zero;
		}
		
		Vector3 normSpring = springVec / dist;
		float damp = Vector3.Dot(relVelocity, normSpring) * damping;
		return normSpring * (baseForce + tension * reactionForce - damp);
	}
}
