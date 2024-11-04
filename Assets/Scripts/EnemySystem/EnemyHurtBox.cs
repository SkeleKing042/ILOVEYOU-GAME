using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace EnemySystem
    {
        [RequireComponent(typeof(Collider))]
        public class EnemyHurtBox : MonoBehaviour
        {
            private float m_damage;
            //private Enemy m_enemyScript;

            private void Awake()
            {
                m_damage = GetComponentInParent<Enemy>().GetSetDamage;
            }

            public void OnTriggerStay(Collider collision)
            {
                //if collided with player
                if (collision.gameObject.GetComponent<PlayerControls>())
                {
                    //Debug.Log("Player touched enemy! They took " + m_damage + " damage!");

                    collision.gameObject.GetComponent<PlayerControls>().TakeDamage(m_damage);

                    //damage player
                }
            }
        }
    }
}

