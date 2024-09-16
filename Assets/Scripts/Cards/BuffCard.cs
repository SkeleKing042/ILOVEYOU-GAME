using ILOVEYOU.BuffSystem;
using ILOVEYOU.Management;
using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ILOVEYOU 
{ 
    namespace Cards
    {
        public class BuffCard : MonoBehaviour
        {
            [SerializeField] private bool m_targetSelf;
            [SerializeField] private int m_effectToGive;

            public void ExecuteEvents(object[] data)
            {
                //get required data
                GameManager manager = (GameManager)data[0];
                PlayerManager player = (PlayerManager)data[1];
                PlayerManager target = manager.GetOtherPlayer(player);

                if (m_targetSelf) player.GetComponent<BuffDataSystem>().GiveBuff(m_effectToGive);
                else target.GetComponent<BuffDataSystem>().GiveBuff(m_effectToGive);
            }
        }
    }

}