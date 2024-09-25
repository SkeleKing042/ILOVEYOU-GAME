using ILOVEYOU.ProjectileSystem;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace EnemySystem
    {
        public class BossEnemy : Enemy
        {
            BulletPattern m_pattern;
            [SerializeField] private float m_lungeTime = .3f;
            [SerializeField] private float m_lungeSpeed = 20f;

            private float m_lungeCooldown;
            private float m_tempSpeed = 0f;

            public override void Initialize(Transform target)
            {
                m_lungeCooldown = m_lungeTime;
                base.Initialize(target);
                m_pattern.AddTarget(m_playerTransform);
            }


            void Awake()
            {
                m_pattern = GetComponent<BulletPattern>();
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

            public override void DoNearAction()
            {
                if (m_tempSpeed <= 0)
                {
                    m_rigidBody.MovePosition(m_rigidBody.position + (m_tempSpeed * Time.deltaTime * transform.forward));
                    m_tempSpeed -= Time.deltaTime * (m_lungeSpeed / 2);
                }
                else if (Vector3.Distance(transform.position, m_playerTransform.position) < 5f)
                {
                    //gets relative position between the player and enemy
                    Vector3 relativePos = m_playerTransform.position - transform.position;
                    //looks at the player (removing x, and z rotation)
                    Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    rotation = Quaternion.Euler(0f, Mathf.LerpAngle(transform.rotation.eulerAngles.y, rotation.eulerAngles.y, Time.deltaTime * m_turnSpeed), 0f);

                    transform.rotation = rotation;

                    m_lungeCooldown -= Time.deltaTime;
                    if (m_lungeCooldown <= 0f)
                    {
                        m_tempSpeed = m_lungeSpeed;
                        m_lungeCooldown = m_lungeTime;
                    }
                }
                else
                {
                    m_pattern.PatternUpdate();
                }
            }
        }
    }
}


