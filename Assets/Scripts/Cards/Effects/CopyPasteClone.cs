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

                Vector3 velocity = (transform.position - m_lastLocation);
                Debug.Log(velocity.normalized);

                m_animator.SetBool("Shooting", m_playerAnimator.GetBool("Shooting"));

                m_rb.position = Vector3.MoveTowards(m_rb.position, m_target.position, 11f * Time.deltaTime);

                m_rb.velocity = Vector3.zero;

                if (m_playerAnimator.GetBool("Shooting")) m_pattern.PatternUpdate();

                m_lastLocation = transform.position;
            }
        }

    }
}

