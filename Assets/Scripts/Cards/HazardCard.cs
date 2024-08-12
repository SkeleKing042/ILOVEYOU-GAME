using ILOVEYOU.Hazards;
using ILOVEYOU.Management;
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
                GameManager gm = (GameManager)data[0];
                gm.GetComponent<HazardManager>().EnableHazards(m_time);
            }
        }
    }
}