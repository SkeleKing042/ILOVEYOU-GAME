using ILOVEYOU.Hazards;
using ILOVEYOU.Management;
using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Cards
    {

        public class HazardCard : MonoBehaviour
        {
            [SerializeField] private float m_time;
            [SerializeField] private HazardTypes[] m_hazardType;
            public void ExecuteEvents(object[] data)
            {
                PlayerManager player = (PlayerManager)data[1];
                GameManager gm = (GameManager)data[0];

                if (m_hazardType.Length == 0) gm.GetOtherPlayer(player).GetLevelManager.GetComponent<HazardManager>().EnableAllHazards(m_time);
                else foreach(HazardTypes type in m_hazardType) gm.GetOtherPlayer(player).GetLevelManager.GetComponent<HazardManager>().EnableTypeHazards(type, m_time);
            }
        }
    }
}