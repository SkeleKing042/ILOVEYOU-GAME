using ILOVEYOU.Management;
using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Cards
    {

        public class CyberAttackCard : MonoBehaviour
        {
            [SerializeField] private float m_time = 1;
            public void ExecuteEvents(object[] data)
            {
                GameManager manager = (GameManager)data[0];
                PlayerManager player = (PlayerManager)data[1];
                PlayerManager target = manager.GetOtherPlayer(player);
                target.GetComponent<PlayerControls>().TempDisableShooting(m_time);
            }
        }
    }
}