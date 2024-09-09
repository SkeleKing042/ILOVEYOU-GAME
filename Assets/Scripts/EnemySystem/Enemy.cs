using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
namespace ILOVEYOU
{
    namespace EnemySystem
    {
        public class Enemy : MonoBehaviour
        {
            [SerializeField] protected float m_speed = 1f;
            [SerializeField] protected float m_turnSpeed = 3f;
            [SerializeField] protected float m_damage = 1f;
            [SerializeField] protected float m_health = 1f;
            [SerializeField] protected float m_distanceCondition = 1f;
            
            protected Transform m_playerTransform;
            protected Rigidbody m_rigidBody;

            private DamageBlink m_blinkScript;
            //this is used for the enemy hurtbox script
            public float GetDamage() { return m_damage; }

            public virtual void Initialize(Transform target)
            {
                m_rigidBody = GetComponent<Rigidbody>();
                m_playerTransform = target;
                m_blinkScript = GetComponent<DamageBlink>();

                //Potential TODO: add a "modifier" value that is dependent on current difficulty/time that influences the base values
            }


            // Update is called once per frame
            void Update()
            {
                //this is simple movement logic, subsequent enemy scripts can be as simple or as complex as they want
                if (Vector3.Distance(transform.position, m_playerTransform.position) < m_distanceCondition)
                {
                    DoNearAction();
                }
                else
                {
                    MoveToTarget();
                }

            }
            
            public virtual void MoveToTarget()
            {
                //gets relative position between the player and enemy
                Vector3 relativePos = m_playerTransform.position - transform.position;
                //looks at the player (removing x, and z rotation)
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                rotation = Quaternion.Euler(0f, Mathf.LerpAngle(transform.rotation.eulerAngles.y,rotation.eulerAngles.y, Time.deltaTime * m_turnSpeed), 0f);
                //moves and rotates the enemy
                //transform.SetPositionAndRotation(transform.position + (m_speed * Time.deltaTime * transform.forward), rotation);
                m_rigidBody.MoveRotation(rotation);
                m_rigidBody.MovePosition(m_rigidBody.position + (m_speed *Time.deltaTime * transform.forward));
                //m_rigidBody.velocity = (m_speed * transform.forward);
            }

            public virtual void DoNearAction()
            {
                //Debug.Log("xoxoxoxoxo");
            }

            public void TakeDamage(float damage)
            {
                m_blinkScript.StartBlink();
                m_health -= damage;

                if (m_health <= 0)
                {
                    m_playerTransform.GetComponent<PlayerManager>().GetTaskManager.UpdateKillTrackers(1);
                    Destroy(gameObject);
                }
            }

            
        }
    }
}
