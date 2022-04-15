using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameOfName
{
    public class ScreenSize : MonoBehaviour
    {
        public float aspectRatio;
        public float w = 9;
        public float h = 16;
        public float dFactor = 1;
        void Start()
        {
            aspectRatio = Camera.main.aspect;
            Camera.main.fieldOfView = (((60 * (w / h)) / dFactor) / aspectRatio);
        }
    }
}
