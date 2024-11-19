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
        public class BuffCard : DisruptCard
        {
            [SerializeField] private int[] m_effectsToGive = new int[0];
            //[SerializeField] private float[] m_time = new float[0];
            [SerializeField] private int[] m_effectsToGiveSelf = new int[0];
            //[SerializeField] private float[] m_timeSelf = new float[0];

            //TODO: CUSTOM EDITOR SCRIPT THIS WILL BE SO FUN

            public override void ExecuteEvents(PlayerManager caller)
            {
                base.ExecuteEvents(caller);
                //gives effects to self
                for (int i = 0; i < m_effectsToGiveSelf.Length; i++)
                {
                    caller.GetComponent<BuffDataSystem>().GiveBuff(m_effectsToGiveSelf[i]);
                }
                foreach (PlayerManager target in GameManager.Instance.GetOtherPlayers(caller))
                {
                    //gives effects to enemy
                    for (int i = 0; i < m_effectsToGive.Length; i++)
                    {
                        target.GetComponent<BuffDataSystem>().GiveBuff(m_effectsToGive[i]);
                    }
                }
            }
        }
    }

}