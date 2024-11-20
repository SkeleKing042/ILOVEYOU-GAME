using ILOVEYOU.ProjectileSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Cards
    {
        [RequireComponent(typeof(BulletPattern))]
        [RequireComponent(typeof(Rigidbody))]
        public class CopyPasteClone : MonoBehaviour
        {

            private float m_moveSpeed = 11f;

            private Transform m_target;
            private Rigidbody m_rb;
            private BulletPatternObject m_bulletPattern;
            private BulletPattern m_pattern;
            private Animator m_animator;
            private Animator m_playerAnimator; //animator on the player

            private Vector3 m_lastLocation;

            public void Initialize(Transform target, BulletPatternObject bulletPattern, Animator playerAnimator)
            {
                //get components
                m_rb = GetComponent<Rigidbody>();
                m_pattern = GetComponent<BulletPattern>();
                m_animator = GetComponentInChildren<Animator>();
                m_playerAnimator = playerAnimator;
                //point the clone to where it needs to go and give it a bullet pattern
                m_target = target;
                m_bulletPattern = bulletPattern;

                m_pattern.ChangePattern(m_bulletPattern);
            }

            // Update is called once per frame
            void Update()
            {
                m_animator.SetBool("Shooting", m_playerAnimator.GetBool("Shooting"));

                if (m_playerAnimator.GetBool("Shooting")) m_pattern.PatternUpdate();
            }

            private void FixedUpdate()
            {
                Vector3 velocity = (transform.position - m_lastLocation);

                Vector3 dir = velocity.normalized * (velocity.magnitude / Time.fixedDeltaTime / m_moveSpeed);

                //this is just a copy from the UpdateAnimator function from PlayerControls

                //Converts movement into angle
                float moveAngle = Mathf.Rad2Deg * Mathf.Atan2(dir.x, dir.z);
                //gets quaternions to convert to vectors
                Quaternion moveQ = Quaternion.Euler(0f, moveAngle, 0f);
                Quaternion shotQ = transform.rotation;
                //sets required animation variables
                m_animator.SetFloat("moveX", (moveQ * Quaternion.Inverse(shotQ) * Vector3.forward * dir.magnitude).x);
                m_animator.SetFloat("moveZ", (moveQ * Quaternion.Inverse(shotQ) * Vector3.forward * dir.magnitude).z);

                m_rb.position = Vector3.MoveTowards(m_rb.position, m_target.position, m_moveSpeed * Time.fixedDeltaTime);

                m_rb.velocity = Vector3.zero;

                m_lastLocation = transform.position;
            }
        }

    }
}

