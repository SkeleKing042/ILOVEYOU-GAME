using ILOVEYOU.EnemySystem;
using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Hazards
    {
        public class SimpleDamage : MonoBehaviour
        {
            [SerializeField] private float m_damage;
            [SerializeField] private float m_rate;
            private float m_time;
            public void DealDamage(GameObject target)
            {
                if (m_time <= 0)
                {
                    Debug.Log($"Dealing damage to {target.name}");
                    PlayerControls player = target.GetComponent<PlayerControls>();

                    if (player)
                        player.TakeDamage(m_damage);

                    m_time = m_rate;
                }
            }
            private void Update()
            {
                if(m_time > 0)
                {
                    m_time -= m_rate * Time.deltaTime;
                }
            }
        }
    }
}