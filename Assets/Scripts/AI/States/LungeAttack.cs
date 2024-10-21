using ILOVEYOU.EnemySystem;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;

namespace ILOVEYOU.AI
{
    public class LungeAttack : EnemyState
    {
        private LungeEnemy m_lungeSettings;
        private float m_cooldown = 0;
        private float m_tempSpeed;
        private Rigidbody m_rigidbody;
        public override void StartState(StateMachine referenceObject)
        {
            base.StartState(referenceObject);
            m_lungeSettings = m_machine.GetComponent<LungeEnemy>();
            m_cooldown = m_lungeSettings.GetLungeTime;
            m_rigidbody = m_machine.GetComponent<Rigidbody>();
        }
        public override void UpdateState()
        {
            if (m_tempSpeed <= 0)
            {

                //gets relative position between the player and enemy
                Vector3 relativePos = m_enemyData.GetPlayerTransform.position - transform.position;
                //looks at the player (removing x, and z rotation)
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                rotation = Quaternion.Euler(0f, Mathf.LerpAngle(transform.rotation.eulerAngles.y, rotation.eulerAngles.y, Time.deltaTime * m_enemyData.GetTurnSpeed), 0f);

                transform.rotation = rotation;

                m_cooldown -= Time.deltaTime;
                if (m_cooldown <= 0f)
                {
                    m_tempSpeed = m_lungeSettings.GetLungeSpeed;
                    m_cooldown = m_lungeSettings.GetLungeTime;
                }
            }
            else
            {
                //transform.position += m_tempSpeed * Time.deltaTime * transform.forward;
                if (Physics.BoxCast(transform.position, new Vector3(.45f, .25f, .25f), transform.forward, transform.rotation, .3f, m_lungeSettings.GetWall)) m_tempSpeed = 0f;

                m_rigidbody.MovePosition(m_rigidbody.position + (m_tempSpeed * Time.deltaTime * transform.forward));
                m_tempSpeed -= Time.deltaTime * (m_lungeSettings.GetLungeSpeed / 2);
            }

            if (!m_enemyData.IsPlayerWithinRange)
            {
                m_machine.ChangeState(new BLineState());
            }
        }
    }
}