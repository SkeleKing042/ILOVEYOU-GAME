using ILOVEYOU.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    using Player;
    namespace Cards
    {

        public class BlindCard : MonoBehaviour
        {
            [SerializeField] private float m_time = 3;
            public void ExecuteEvents(object[] data)
            {
                GameManager manager = (GameManager)data[0];
                PlayerManager player = (PlayerManager)data[1];
                PlayerManager target = manager.GetOtherPlayer(player);

                target.TriggerBlindness(m_time);
            }
        }
    }
}