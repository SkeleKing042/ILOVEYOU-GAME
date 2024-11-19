using ILOVEYOU.Management;
using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Cards
    {

        public class CyberAttackCard : DisruptCard
        {
            [Tooltip("How long the effect will last for.")] [SerializeField] private float m_time = 1;
            public override void ExecuteEvents(PlayerManager caller)
            {
                base.ExecuteEvents(caller);
                foreach (PlayerManager target in GameManager.Instance.GetOtherPlayers(caller))
                    target.GetComponent<PlayerControls>().DisableShooting(m_time);
            }
        }
    }
}