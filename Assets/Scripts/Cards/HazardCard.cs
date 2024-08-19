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
            public void ExecuteEvents(object[] data)
            {
                PlayerManager player = (PlayerManager)data[1];
                GameManager gm = (GameManager)data[0];
                gm.GetOtherPlayer(player).GetLevelManager.GetComponent<HazardManager>().EnableHazards(m_time);
            }
        }
    }
}