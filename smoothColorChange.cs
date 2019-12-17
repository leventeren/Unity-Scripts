using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gamename
{
    public class smoothColorChange : MonoBehaviour
    {
        public float timeLeft;
        public Color colorTarget;

        private Renderer renderOBJ;

        void Start()
        {
            renderOBJ = GetComponent<Renderer>();
        }

        void Update()
        {
            if (timeLeft <= Time.deltaTime)
            {
                renderOBJ.material.color = colorTarget;
                timeLeft = 0f;
            }
            else
            {
                renderOBJ.material.color = Color.Lerp(renderOBJ.material.color, colorTarget, Time.deltaTime / timeLeft);
                timeLeft -= Time.deltaTime;
            }
        }
    }
}
