using ILOVEYOU.EnemySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU.AI
{
    public class EnemyState : State
    {
        protected Enemy m_enemyData;
        public override void StartState(StateMachine referenceObject)
        {
            base.StartState(referenceObject);
            m_enemyData = m_machine.GetComponent<Enemy>();
        }
    }
}
