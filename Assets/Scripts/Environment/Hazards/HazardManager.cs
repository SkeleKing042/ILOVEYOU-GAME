using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ILOVEYOU
{
    namespace Hazards
    {

        public class HazardManager : MonoBehaviour
        {
            [SerializeField] private bool m_debugging;
            private HazardObject[] m_levelHazards;
            [SerializeField] private UnityEvent m_onGlobalHazardEnable;
            [SerializeField] private UnityEvent m_onGlobalHazardDisable;
            // Start is called before the first frame update
            public bool Startup()
            {
                //Get all the hazards in the level.
                m_levelHazards = transform.parent.GetComponentsInChildren<HazardObject>();
                return true;
            }

            public void EnableHazards()
            {
                if (m_debugging) Debug.Log("Triggering hazards.");
                foreach(HazardObject hazardObject in m_levelHazards)
                {
                    hazardObject.EnableHazard();
                }
                m_onGlobalHazardEnable.Invoke();
            }
            public void EnableHazards(float time)
            {
                if (m_debugging) Debug.Log($"Triggering hazards for {time} seconds.");
                foreach (HazardObject hazardObject in m_levelHazards)
                {
                    hazardObject.EnableHazard(time);
                }
                m_onGlobalHazardEnable.Invoke();
            }
            public void DisableHazards()
            {
                if (m_debugging) Debug.Log("Disabling hazards.");
                foreach (HazardObject hazardObject in m_levelHazards)
                {
                    hazardObject.DisableHazard();
                }
                m_onGlobalHazardDisable.Invoke();
            }
        }
    }
}