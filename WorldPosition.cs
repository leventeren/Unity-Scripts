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
