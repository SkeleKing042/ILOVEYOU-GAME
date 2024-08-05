using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

namespace ILOVEYOU
{
    namespace ProjectileSystem
    {
        public class Projectile : MonoBehaviour
        {
            protected float m_speed; // initial speed of bullet
            protected float m_fwdaccelValue; // foward acceleration factor of bullet
            protected float m_sideSpeed; // current sideways speed of the bullet
            protected float m_sideaccelValue; // sideways acceleration factor of the bullet
            protected Transform m_target; // target for homing bullets
            protected float m_lifeTime; // how long the projectile lasts in seconds
            protected bool m_isFriendly; // if the projectile has been shot by the player


            private void Update()
            {
                //bug: if initial m_speed is 0, any acceleration applied to the bulled makes it move all whacky
                //basic movement
                transform.position += m_speed * Time.deltaTime * transform.forward + m_sideSpeed * Time.deltaTime * transform.right;
                //accelerates speed linearly
                m_speed += m_fwdaccelValue * Time.deltaTime;
                //accelerates side speed linearly
                m_sideSpeed += m_sideaccelValue * Time.deltaTime;

            }

            public void InitializeProjectile(float speed, float accelValue, float sideaccelValue, Transform target, float lifeTime, bool isFriendly)
            {
                m_speed = speed;
                m_fwdaccelValue = accelValue;
                m_sideaccelValue = sideaccelValue;
                m_target = target;
                m_lifeTime = lifeTime;
                m_isFriendly = isFriendly;

                Destroy(gameObject, m_lifeTime);
            }

            private void OnTriggerEnter(Collider other)
            {
                
            }
        }
    }
}


