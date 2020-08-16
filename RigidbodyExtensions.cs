public static class RigidbodyExtensions
{
	public static void Merge(this Rigidbody a, Rigidbody b)
	{
		if (b == null)
		{
			return;
		}
		
		var mass = a.mass + b.mass;
		var velocity = a.velocity * (a.mass / mass) + b.velocity * (b.mass / mass);
		var angularVelocity = a.angularVelocity * (a.mass / mass) + b.angularVelocity * (b.mass / mass);

		a.mass = mass;
		a.velocity = velocity;
		a.angularVelocity = angularVelocity;
		a.ResetCenterOfMass();
	}
}
