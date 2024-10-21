using UnityEngine;
using UnityEngine.AI;

namespace ILOVEYOU.AI
{
    public class UseBrain : EnemyState
    {
        private NavMeshAgent m_agent;
        public override void StartState(StateMachine referenceObject)
        {
            base.StartState(referenceObject);
            m_agent = m_machine.GetComponent<NavMeshAgent>();
            m_agent.SetDestination(m_enemyData.GetPlayerTransform.position);
        }

        public override void UpdateState()
        {
            if (!Physics.Raycast(m_enemyData.transform.position, (m_enemyData.GetPlayerTransform.position - m_enemyData.transform.position).normalized, Mathf.Infinity, m_enemyData.GetMask))
            {
                m_machine.ChangeState(new BLineState());
            }
        }
        public override void EndState()
        {
            m_agent.isStopped = true;
            base.EndState();
        }
    }
}
