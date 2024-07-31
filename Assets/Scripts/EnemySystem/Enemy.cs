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
            [SerializeField] protected float m_damage = 1f;
            [SerializeField] protected float m_health = 1f;
            [SerializeField] protected float m_distanceCondition = 1f;
            protected Transform m_playerTransform; //temp serialization

            public void Initialize(Transform target)
            {
                m_playerTransform = target;

                //Potential TODO: add a "modifier" value that is dependent on current difficulty/time that influences the base values
            }


            // Update is called once per frame
            void Update()
            {
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
                Vector3 relativePos = m_playerTransform.position - transform.position;

                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);

                transform.SetPositionAndRotation(transform.position + (m_speed * Time.deltaTime * transform.forward), rotation);

            }

            public virtual void DoNearAction()
            {
                Debug.Log("xoxoxoxoxo");
            }


        }
    }
}
