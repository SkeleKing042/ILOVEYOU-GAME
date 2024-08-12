using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Hazards
    {

        public class HazardManager : MonoBehaviour
        {
            [SerializeField] private bool m_debugging;
            private HazardObject[] m_levelHazards;
            // Start is called before the first frame update
            public bool Startup()
            {
                //Get all the hazards in the scene.
                m_levelHazards = FindObjectsOfType<HazardObject>();
                return true;
            }

            public void EnableHazards()
            {
                if (m_debugging) Debug.Log("Triggering hazards.");
                foreach(HazardObject hazardObject in m_levelHazards)
                {
                    hazardObject.EnableHazard();
                }
            }
            public void EnableHazards(float time)
            {
                if (m_debugging) Debug.Log($"Triggering hazards for {time} seconds.");
                foreach (HazardObject hazardObject in m_levelHazards)
                {
                    hazardObject.EnableHazard(time);
                }
            }
            public void DisableHazards()
            {
                if (m_debugging) Debug.Log("Disabling hazards.");
                foreach (HazardObject hazardObject in m_levelHazards)
                {
                    hazardObject.DisableHazard();
                }
            }
        }
    }
}