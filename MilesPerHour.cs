public class MilesPerHour : MonoBehaviour
    {
        public float milesPerHour = 0f;

        [Space(20)]

        public float metersTravelledPerSecond;
        public float metersTravelledPerMinute;
        public float metersTravelledPerHour;

        // Got this value from a google search. Unity uses meters,
        // and one meter = 0.000621371f miles.
        private float metersToMiles = 0.000621371f;
        private Vector3 prevPosition = Vector3.zero;

        // Use this for initialization
        void Start()
        {
            prevPosition = transform.position;
        }

        private void FixedUpdate()
        {
            Vector3 v = transform.position - prevPosition;

            float distance = v.magnitude;
            if (distance < 0.0002f)
                distance = 0f;

            // Get the meters travelled per second.
            metersTravelledPerSecond = distance / Time.deltaTime;

            // Meters per minute. There are 60 seconds in one minute!
            metersTravelledPerMinute = metersTravelledPerSecond * 60f;

            // Meters per hour. There are 60 minutes in one hour!
            metersTravelledPerHour = metersTravelledPerMinute * 60f;

            // Finally, multiply are speed - which is in meters - by our conversion variable to get the miles per hour.
            milesPerHour = metersTravelledPerHour * metersToMiles;

            prevPosition = transform.position;
        }

        
    }
