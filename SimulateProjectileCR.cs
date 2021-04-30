public float gravity = 60f;
public float heightOfShot = 12f;

IEnumerator SimulateProjectileCR(Transform projectile, Vector3 startPosition, Vector3 endPosition)
        {
            projectile.position = startPosition;
            float arcAmount = 8f;
            Vector3 newVel = new Vector3();
            // Find the direction vector without the y-component
            Vector3 direction = new Vector3(endPosition.x, 0f, endPosition.z) - new Vector3(startPosition.x, 0f, startPosition.z);
            // Find the distance between the two points (without the y-component)
            float range = direction.magnitude;

            // Find unit direction of motion without the y component
            Vector3 unitDirection = direction.normalized;
            // Find the max height

            float maxYPos = startPosition.y + heightOfShot;

            // if it has, switch the height to match a 45 degree launch angle
            if (range / 2f > maxYPos)
                maxYPos = range / arcAmount;
            //fix bug when shooting on tower
            if (maxYPos - startPosition.y <= 0)
            {
                maxYPos = startPosition.y + 2f;
            }
            //fix bug caused if we can't shoot higher than target
            if (maxYPos - endPosition.y <= 0)
            {
                maxYPos = endPosition.y + 2f;
            }
            // find the initial velocity in y direction
            newVel.y = Mathf.Sqrt(-2.0f * -gravity * (maxYPos - startPosition.y));
            // find the total time by adding up the parts of the trajectory
            // time to reach the max
            float timeToMax = Mathf.Sqrt(-2.0f * (maxYPos - startPosition.y) / -gravity);
            // time to return to y-target
            float timeToTargetY = Mathf.Sqrt(-2.0f * (maxYPos - endPosition.y) / -gravity);
            // add them up to find the total flight time
            float totalFlightTime = timeToMax + timeToTargetY;
            // find the magnitude of the initial velocity in the xz direction
            float horizontalVelocityMagnitude = range / totalFlightTime;
            // use the unit direction to find the x and z components of initial velocity
            newVel.x = horizontalVelocityMagnitude * unitDirection.x;
            newVel.z = horizontalVelocityMagnitude * unitDirection.z;

            float elapse_time = 0;
            while (elapse_time < totalFlightTime)
            {
                projectile.Translate(newVel.x * Time.deltaTime, (newVel.y - (gravity * elapse_time)) * Time.deltaTime, newVel.z * Time.deltaTime);
                elapse_time += Time.deltaTime;
                yield return null;
            }


        }
