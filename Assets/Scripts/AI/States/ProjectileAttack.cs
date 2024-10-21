using ILOVEYOU.ProjectileSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU.AI
{
    public class ProjectileAttack : EnemyState
    {
        private BulletPattern m_pattern;
        public override void StartState(StateMachine referenceObject)
        {
            base.StartState(referenceObject);
            m_pattern = m_machine.GetComponent<BulletPattern>();
            m_pattern.AddTarget(m_enemyData.GetPlayerTransform);
        }
        public override void UpdateState()
        {
            m_pattern.PatternUpdate();
            //gets relative position between the player and enemy
            Vector3 relativePos = m_enemyData.GetPlayerTransform.position - transform.position;
            //looks at the player (removing x, and z rotation)
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
            transform.rotation = rotation;

            if (!m_enemyData.IsPlayerWithinRange)
            {
                m_machine.ChangeState(new BLineState());
            }
        }
    }
}
