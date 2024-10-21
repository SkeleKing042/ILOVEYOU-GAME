using ILOVEYOU.EnemySystem;
using UnityEngine;

namespace ILOVEYOU.AI
{
    public class BLineState : EnemyState
    {
        public override void StartState(StateMachine referenceObject)
        {
            base.StartState(referenceObject);
            m_enemyData.GetRigidbody.isKinematic = false;
        }
        public override void UpdateState()
        {
            if (Physics.Raycast(m_enemyData.transform.position, (m_enemyData.GetPlayerTransform.position - m_enemyData.transform.position).normalized, Mathf.Infinity, m_enemyData.GetMask))
            {
                m_machine.ChangeState(new UseBrain());
                return;
            }
            if(m_enemyData.IsPlayerWithinRange)
            {
                m_machine.ChangeState(m_enemyData.GetNearState);
                return;
            }
            //gets relative position between the player and enemy
            Vector3 relativePos = m_enemyData.GetPlayerTransform.position - m_machine.transform.position;
            //looks at the player (removing x, and z rotation)
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            rotation = Quaternion.Euler(0f, Mathf.LerpAngle(m_machine.transform.rotation.eulerAngles.y, rotation.eulerAngles.y, Time.deltaTime * m_enemyData.GetTurnSpeed), 0f);
            //moves and rotates the enemy
            //transform.SetPositionAndRotation(transform.position + (m_speed * Time.deltaTime * transform.forward), rotation);
            m_enemyData.GetRigidbody.MoveRotation(rotation);
            m_enemyData.GetRigidbody.MovePosition(m_machine.transform.position + (m_enemyData.GetSpeed * Time.deltaTime * m_machine.transform.forward));
            //m_rigidBody.velocity = (m_speed * transform.forward);
        }
        public override void EndState()
        {
            base.EndState();
            m_enemyData.GetRigidbody.isKinematic = true;
        }
    }
}