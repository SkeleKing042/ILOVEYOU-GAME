using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace ProjectileSystem
    {
        public class PreciseHomingProjectile : Projectile
        {
            private float m_velocity;
            [SerializeField] private float m_checkRadius = 5f;
            [SerializeField] private LayerMask m_mask;

            // Update is called once per frame
            void FixedUpdate()
            {
                if (m_target)
                {
                    //gets relative position between the player and enemy
                    Vector3 relativePos = m_target.position - transform.position;

                    Quaternion rotationVelocity = Quaternion.LookRotation(relativePos, transform.up);

                    transform.rotation = Quaternion.Euler(Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.x, rotationVelocity.eulerAngles.x, Time.fixedDeltaTime * m_sideaccelValue),
                        Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.y, rotationVelocity.eulerAngles.y, Time.fixedDeltaTime * m_sideaccelValue),
                        Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, rotationVelocity.eulerAngles.z, Time.fixedDeltaTime * m_sideaccelValue));

                    

                    //apply speed based on direction of target
                    m_velocity += m_fwdaccelValue * Time.fixedDeltaTime;
                    //move bullet
                    transform.position += (m_speed + m_velocity) * Time.fixedDeltaTime * transform.forward;

                }
                //attempts to retarget if old one is lost
                else if (Physics.CheckSphere(transform.position, m_checkRadius, m_mask))
                {
                    Collider[] cols = Physics.OverlapSphere(transform.position, m_checkRadius, m_mask);
                    m_target = cols[0].transform;
                }
                //othewise just move with current velocity
                else
                {
                    transform.position += (m_speed + m_velocity) * Time.fixedDeltaTime * transform.forward;
                }


            }
            private void OnDrawGizmos()
            {
                Gizmos.DrawRay(transform.position, transform.forward);

                //shows whether the bullet is targeting something
                Gizmos.color = m_target ? Color.green : Color.red;
                //shows targeting radius
                Gizmos.DrawWireSphere(transform.position, m_checkRadius);
            }
        }

    }
}
