using ILOVEYOU.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    using Player;
    namespace Cards
    {

        public class BlindCard : DisruptCard
        {
            //[Tooltip("How long the effect will last for.")] [SerializeField] private float m_time = 3;
            [SerializeField] private int m_popupAmount;
            public override void ExecuteEvents(PlayerManager caller)
            {
                base.ExecuteEvents(caller);
                foreach(PlayerManager target in GameManager.Instance.GetOtherPlayers(caller))
                {
                    target.TriggerBlindness(m_popupAmount);
                }
            }
        }
    }
}