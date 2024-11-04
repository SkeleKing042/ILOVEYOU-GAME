using ILOVEYOU.EnemySystem;
using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
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
            protected float m_damage; // damage of the bullet
            protected int m_pierce; // how many time a bullet can go through a target
            protected float m_lifeTime; // how long the projectile lasts in seconds
            protected bool m_isFriendly; // if the projectile has been shot by the player (might not need this)


            private void FixedUpdate()
            {
                //bug: if initial m_speed is 0, any acceleration applied to the bullet makes it move all whacky
                //basic movement
                transform.position += m_speed * Time.fixedDeltaTime * transform.forward + m_sideSpeed * Time.fixedDeltaTime * transform.right;
                //accelerates speed linearly
                m_speed += m_fwdaccelValue * Time.fixedDeltaTime;
                //accelerates side speed linearly
                m_sideSpeed += m_sideaccelValue * Time.fixedDeltaTime;

            }

            public virtual void InitializeProjectile(float speed, float accelValue, float sideaccelValue, Transform target, float damage, int pierce, float lifeTime, bool isFriendly)
            {
                m_speed = speed;
                m_fwdaccelValue = accelValue;
                m_sideaccelValue = sideaccelValue;
                m_target = target;
                m_damage = damage;
                m_pierce = pierce;
                m_lifeTime = lifeTime;
                m_isFriendly = isFriendly;

                //if the bullet being fired is friendly put it on friendly layer, if hostile put it on hostile layer
                gameObject.layer = (m_isFriendly) ? 8 : 9;

                Destroy(gameObject, m_lifeTime);
            }

            protected virtual void OnTriggerEnter(Collider other)
            {
                //if bullet collided with enemy
                if (other.gameObject.GetComponent<Enemy>())
                {
                    //try to damage the other object - don't to anything if damages fails to be dealt.
                    if (other.gameObject.GetComponent<Enemy>().TakeDamage(m_damage))
                    {
                        m_pierce--;
                        if (m_pierce <= 0) Destroy(gameObject);
                    }
                    return;
                }
                //if bullet collided with player
                if (other.gameObject.GetComponent<PlayerControls>())
                {
                    //Debug.Log("Enemy hit player with projectile for " + m_damage + " damage!");

                    //insert player damage script
                    other.gameObject.GetComponent<PlayerControls>().TakeDamage(m_damage);

                    m_pierce--;
                    if (m_pierce <= 0) Destroy(gameObject);
                }
            }
        }
    }
}


