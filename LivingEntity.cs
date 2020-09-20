using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ballistics
{
    public class LivingEntity : MonoBehaviour
    {
        /// <summary>
        /// maximum health
        /// </summary>
        public float StartHealth;

        /// <summary>
        /// health handler
        /// </summary>
        [HideInInspector]
        public float Health
        {
            get { return myHealth; }
            set
            {
                float before = myHealth;
                if (value > 0f && value <= StartHealth)
                {
                    myHealth = value;
                }
                else
                {
                    if (value > 0f)
                    {
                        myHealth = StartHealth;
                    }
                    else
                    {
                        myHealth = 0;
                        OnDeath();
                    }
                }
                OnHealthChanged(value - before);
            }
        }

        private float myHealth = 0;

        void Awake()
        {
            myHealth = StartHealth;
        }

        /// <summary>
        /// called when health is 0
        /// </summary>
        public virtual void OnDeath()
        { }

        /// <summary>
        /// called when the health value changed
        /// </summary>
        /// <param name="amount">value of the change</param>
        public virtual void OnHealthChanged(float amount)
        {

        }
    }
}
