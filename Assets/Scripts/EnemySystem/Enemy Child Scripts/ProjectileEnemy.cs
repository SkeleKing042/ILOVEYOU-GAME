using ILOVEYOU.ProjectileSystem;
using UnityEngine;
namespace ILOVEYOU
{
    namespace EnemySystem
    {
        public class ProjectileEnemy : Enemy
        {
            BulletPattern m_pattern;

            public override void Initialize(Transform target)
            {
                base.Initialize(target);
                m_pattern.AddTarget(m_playerTransform);
            }
            void Awake()
            {
                m_pattern = GetComponent<BulletPattern>();
          
            }

            public override void DoNearAction()
            {
                if (m_usingAIBrain)
                {
                    DisableAIBrain();
                }

                m_pattern.PatternUpdate();
                //gets relative position between the player and enemy
                Vector3 relativePos = m_playerTransform.position - transform.position;
                //looks at the player (removing x, and z rotation)
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
                transform.rotation = rotation;
            }
        }
    }
}


