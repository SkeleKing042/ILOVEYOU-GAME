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
            //[SerializeField] private bool m_targetSelf;
            [SerializeField] private int[] m_effectsToGive = new int[0];
            [SerializeField] private int[] m_effectsToGiveSelf = new int[0];

            public void ExecuteEvents(object[] data)
            {
                //get required data
                GameManager manager = (GameManager)data[0];
                PlayerManager player = (PlayerManager)data[1];
                PlayerManager target = manager.GetOtherPlayer(player);

                //gives effects to self
                for (int i = 0; i < m_effectsToGiveSelf.Length; i++)
                {
                    player.GetComponent<BuffDataSystem>().GiveBuff(m_effectsToGiveSelf[i]);
                }
                //gives effects to enemy
                for (int i = 0; i < m_effectsToGive.Length; i++)
                {
                    target.GetComponent<BuffDataSystem>().GiveBuff(m_effectsToGive[i]);
                }


                //else target.GetComponent<BuffDataSystem>().GiveBuff(m_effectsToGive);
            }
        }
    }

}