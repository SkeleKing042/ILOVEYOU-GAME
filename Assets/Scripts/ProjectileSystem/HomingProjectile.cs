using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace ProjectileSystem
    {
        public class HomingProjectile : Projectile
        {
            // Start is called before the first frame update
            void Start()
            {

            }

            // Update is called once per frame
            void Update()
            {
                if (m_target)
                {
                    //gets relative position between the player and enemy
                    Vector3 relativePos = m_target.position - transform.position;
                    //looks at the player (removing x, and z rotation)
                    Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    rotation = Quaternion.Euler(0f, Mathf.LerpAngle(transform.rotation.eulerAngles.y, rotation.eulerAngles.y, Time.deltaTime * 3f), 0f);
                    //moves and rotates the enemy
                    transform.SetPositionAndRotation(transform.position + (m_speed * Time.deltaTime * transform.forward), rotation);
                }
                else
                {
                    transform.position += m_speed * Time.deltaTime * transform.forward;
                }


            }
        }
    }
}