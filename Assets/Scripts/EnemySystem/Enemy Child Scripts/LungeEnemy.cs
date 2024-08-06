using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace EnemySystem
    {
        public class LungeEnemy : Enemy
        {
            [SerializeField] private float m_lungeCooldown = 5f;
            [SerializeField] private float m_tempSpeed = 0f;

            // Start is called before the first frame update
            void Start()
            {

            }

            // Update is called once per frame
            void Update()
            {
                //this is simple movement logic, subsequent enemy scripts can be as simple or as complex as they want
                if (Vector3.Distance(transform.position, m_playerTransform.position) < m_distanceCondition && m_tempSpeed <= 0)
                {
                    DoNearAction();
                }
                else if (m_tempSpeed <= 0)
                {
                    MoveToTarget();
                }
                else
                {
                    transform.position += m_tempSpeed * Time.deltaTime * transform.forward;
                    m_tempSpeed -= Time.deltaTime * m_speed * 2f;
                }
            }

            public override void DoNearAction()
            {
                //gets relative position between the player and enemy
                Vector3 relativePos = m_playerTransform.position - transform.position;
                //looks at the player (removing x, and z rotation)
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                rotation = Quaternion.Euler(0f, Mathf.LerpAngle(transform.rotation.eulerAngles.y, rotation.eulerAngles.y, Time.deltaTime), 0f);

                transform.rotation = rotation;

                m_lungeCooldown -= Time.deltaTime;
                if(m_lungeCooldown <= 0f)
                {
                    m_tempSpeed = m_speed * 3f;
                    m_lungeCooldown = 3f;
                }
            }
        }
    }
}


