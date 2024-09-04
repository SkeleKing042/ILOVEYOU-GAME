using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace ProjectileSystem
    {
        public class HomingProjectile : Projectile
        {
            private Vector3 m_velocity;
            [SerializeField] private float m_checkRadius = 5f;
            [SerializeField] private LayerMask m_mask;

            void Awake()
            {
                StartCoroutine(_waitFrame());
            }

            //public override void InitializeProjectile(float speed, float accelValue, float sideaccelValue, Transform target, float damage, int pierce, float lifeTime, bool isFriendly)
            //{
            //    base.InitializeProjectile(speed, accelValue, sideaccelValue, target, damage, pierce, lifeTime, isFriendly);
            //}

            // Update is called once per frame
            void FixedUpdate()
            {
                if (m_target)
                {
                    //gets relative position between the player and enemy
                    Vector3 relativePos = m_target.position - transform.position;
                    //looks in the direction of current velocity (removing x, and z rotation)
                    Quaternion rotation = Quaternion.LookRotation(m_velocity, Vector3.up);
                    //apply speed based on direction of target
                    m_velocity += m_fwdaccelValue * Time.fixedDeltaTime * relativePos.normalized;
                    //move and rotate bullet
                    transform.position += m_velocity * Time.fixedDeltaTime;
                    transform.rotation = rotation;

                }
                //attempts to retarget if old one is lost
                else if (Physics.CheckSphere(transform.position, m_checkRadius, m_mask))
                {
                    Collider[] cols =  Physics.OverlapSphere(transform.position, m_checkRadius, m_mask);
                    m_target = cols[0].transform;
                }
                //othewise just move with current velocity
                else
                {
                    transform.position += m_velocity * Time.fixedDeltaTime;
                }

                
            }


            private IEnumerator _waitFrame()
            {
                //this is needed as it takes an update for the rotations to properly apply (could be the reason why this doesn't work in scene view)
                yield return new WaitForEndOfFrame();
                m_velocity = transform.forward * m_speed;
            }

            private void OnDrawGizmos()
            {
                //shows whether the bullet is targeting something
                Gizmos.color = m_target ? Color.green : Color.red;
                //shows targeting radius
                Gizmos.DrawWireSphere(transform.position, m_checkRadius);
            }
        }
    }
}