using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace weaponMaster
{
    public class HandMove : MonoBehaviour
    {

        public Transform targetRight;
        public Transform targetLeft;
        public Transform lookTarget;
        Animator m_anim;
        [Range(0, 1)]
        public float weight = 1.0f;
        public float speed;

        public enum Part
        {
            RightHand,
            LeftHand,
            TwoHand,
            RightFoot,
            LeftFoot,
            TwoFoot
        }
        public Part part;

        void Start()
        {
            m_anim = GetComponent<Animator>();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (part == Part.RightHand) {
                m_anim.SetIKPosition(AvatarIKGoal.RightHand, targetRight.position);
                m_anim.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);

                m_anim.SetLookAtPosition(lookTarget.position);
                m_anim.SetLookAtWeight(weight);
            }
            else if (part == Part.LeftHand)
            {
                m_anim.SetIKPosition(AvatarIKGoal.LeftHand, targetLeft.position);
                m_anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);

                m_anim.SetLookAtPosition(lookTarget.position);
                m_anim.SetLookAtWeight(weight);
            }
            else if (part == Part.TwoHand)
            {
                m_anim.SetIKPosition(AvatarIKGoal.RightHand, targetRight.position);
                m_anim.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
                m_anim.SetIKPosition(AvatarIKGoal.LeftHand, targetLeft.position);
                m_anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);

                m_anim.SetLookAtPosition(lookTarget.position);
                m_anim.SetLookAtWeight(weight);
            }           
            
        }
    }
}
