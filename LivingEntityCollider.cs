using UnityEngine;
using System.Collections;

namespace Ballistics
{
    public class LivingEntityCollider : MonoBehaviour
    {
        //set (for example) to 0.75f when LivingEntity has armor
        public float DamageMultiplier = 1;

        //when LivingEntityCollider is child of a GameObject with LivingEntity 
        //ParentLE can be set automatically through LivingEntity-Inspector (Button)
        public LivingEntity ParentLivingEntity;
    }
}
