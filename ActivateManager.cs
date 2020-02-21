using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class ActivateManager : MonoBehaviour
    {
        GameObject[] array;
        public float distance;

        // Update is called once per frame
        void Update()
        {
            GetInactiveInRadius();
        }

        
        void GetInactiveInRadius()
        {
            foreach (GameObject obj in array)
            {
                if (obj) //destroyed?
                {
                    if (Vector3.Distance(transform.position, obj.transform.position) < distance)
                    {
                        if (obj) //destroyed?
                        {
                            obj.SetActive(true);
                        }
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color32(183,0,0,32);
            Gizmos.DrawSphere(transform.position, distance);
        }
    }
