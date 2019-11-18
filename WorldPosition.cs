public Vector4[] playerWorldPosArray = new Vector4[20];

void Awake()
        {
            // initialize the global array of player world positions
            for (var i = 0; i < playerWorldPosArray.Length; i++)
            {
                playerWorldPosArray[i] = new Vector4(
                    transform.position.x,
                    transform.position.y,
                    transform.position.z,
                    1);
            }
            StartCoroutine(UpdateWorldPos());
        }
        IEnumerator UpdateWorldPos()
        {
            while (true)
            {
                // get the most recent player world position
                var wp = new Vector4(
                    transform.position.x,
                    transform.position.y,
                    transform.position.z,
                    1);
                // dequeue the last position from the global position array
                PWShiftRight();
                // enqueue the new position on to the global position array
                PWPush(wp);
                // set our position array as a global shader variable -
                // this way, other shaders can use it for free!
                Shader.SetGlobalVectorArray("_PlayerWorldPos", playerWorldPosArray);
                yield return new WaitForEndOfFrame();
            }
        }
        void PWShiftRight()
        {
            for (var i = playerWorldPosArray.Length - 1; i >= 1; i--)
            {
                playerWorldPosArray[i] = playerWorldPosArray[i - 1];
            }
        }
        void PWPush(Vector4 v) { playerWorldPosArray[0] = v; }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // This draws the player position array in the Unity editor
            Gizmos.color = Color.cyan - (Color.black * .5f);
            for (var i = 0; i < playerWorldPosArray.Length; i++)
            {
                var pos = new Vector3(playerWorldPosArray[i].x,
                    playerWorldPosArray[i].y,
                    playerWorldPosArray[i].z);
                Gizmos.DrawSphere(pos, .05f);
            }
        }
#endif





And the vertex shader code.

#define P_LENGTH 20
uniform float4 _PlayerWorldPos[P_LENGTH];
uniform float _DisplacementStrength;
uniform float _Radius;
uniform float _SquishThreshold;
float getPlayerObjectDistance(int pIndex) {
    float2 pPos = mul(unity_WorldToObject, _PlayerWorldPos[pIndex]).xz;
    return length(pPos);
}
void vert(inout appdata_full v) {
    // check if this object is within a certain distance of the player
    float d = getPlayerObjectDistance(0);
    if (d > (_Radius * P_LENGTH * .3)) {
        // ignore this vertex if the player is too far away
        return;
    }
    // check how many player positions are within range of this object
    int numPFramesContained = 0;
    if (d < _Radius) numPFramesContained++;
    for (int i = 1; i < P_LENGTH; i++) {
        if(getPlayerObjectDistance(i) < _Radius) numPFramesContained++;
    }
    float originalVertY = v.vertex.y;
    // displace this vertex down a certain intensity based on how long the player has been near this object
    float intensity = (numPFramesContained / (float)P_LENGTH);
    // we transform intensity here with a cubic function for the fast-in, slow-out effect
    intensity -= 1.;
    intensity = 1. + (intensity*intensity*intensity);
    v.vertex.y -= intensity * _DisplacementStrength * .5 * saturate(v.vertex.y);
    if (originalVertY > v.vertex.y) {
        v.vertex.y = max(_SquishThreshold, v.vertex.y);
    }
}
