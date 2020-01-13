using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
    {

        public Transform pos1;
        public Transform pos2;
        public Transform movingEnemy;
        public float delayBetweenTransform = 1;
        public float movingEnemyMovingTime = 1;
        // Use this for initialization
        void Start()
        {
            movingEnemy.position = pos1.position;
            StartCoroutine(MoveEnemy());
        }

        private IEnumerator MoveEnemy()
        {
            yield return null;
            while (gameObject.activeInHierarchy)
            {
                Vector3 targetPos = GetTargetPos();
                float distance = (targetPos - movingEnemy.position).magnitude;
                float movedDistance = 0;
                while (movedDistance < distance)
                {
                    Vector3 d = (targetPos - transform.position).normalized * distance / movingEnemyMovingTime * Time.fixedDeltaTime;
                    movingEnemy.position += d;
                    movedDistance += d.magnitude;
                    yield return null;
                }
                yield return new WaitForSeconds(delayBetweenTransform);
            }
        }

        private Vector3 GetTargetPos()
        {
            if ((movingEnemy.position - pos1.position).magnitude < (pos2.position - pos1.position).magnitude / 2)
            {
                return pos2.position;
            }
            else
            {
                return pos1.position;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
