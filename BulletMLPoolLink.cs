public class BulletPoolScript : MonoBehaviour
{
	void Start()
	{
		var bulletManager = FindObjectOfType<BulletManagerScript>();
		if (bulletManager != null)
		{
			bulletManager.OnBulletSpawned += OnBulletSpawned;
			bulletManager.OnBulletDestroyed += OnBulletDestroyed;
		}
	}
	
	BulletScript OnBulletSpawned(BulletObject bullet, string bulletName)
	{
		// Get a GameObject from the pool
		// bulletName and bullet can help you identify the bullet you want
		// TODO: Your pool here
		GameObject go = MyPool.Get();

		// Make sure this GameObject has a BulletScript and return it
		// BulletScript can be added on the fly, there is no special parameter to pass

		BulletScript bulletScript = go.GetComponent<BulletScript>();
		if(bulletScript == null) 
		{
			bulletScript = go.AddComponent<BulletScript>();
		}

		return bulletScript;
	}

	void OnBulletDestroyed(GameObject bullet)
	{
		// Recycle your GameObject
		// 1/ If you need the label, you can retrieve it this way
		BulletScript bulletScript = bullet.GetComponent<BulletScript>();
		if (bulletScript != null)
		{
			BulletPool pool = null;
			string bulletName = bulletScript.Bullet.Label.ToLower();

      // TODO: Your pool here
			MyPool.Recycle(bulletScript);
		}
		
		// 2/ Otherwise you have a direct reference to the bullet's GameObject
		// TODO: Your pool here
		MyPool.Recycle(bullet);
	}
}
