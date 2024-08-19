using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ILOVEYOU
{
    namespace Hazards
    {
        public enum HazardTypes
        {
            Overheat,
            Stinky,
            Smelly
        }

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

            public void EnableAllHazards()
            {
                if (m_debugging) Debug.Log("Triggering hazards.");
                foreach(HazardObject hazardObject in m_levelHazards)
                {
                    hazardObject.EnableHazard();
                }
                m_onGlobalHazardEnable.Invoke();
            }
            public void EnableAllHazards(float time)
            {
                if (m_debugging) Debug.Log($"Triggering hazards for {time} seconds.");
                foreach (HazardObject hazardObject in m_levelHazards)
                {
                    hazardObject.EnableHazard(time);
                }
                m_onGlobalHazardEnable.Invoke();
            }
            public void DisableAllHazards()
            {
                if (m_debugging) Debug.Log("Disabling hazards.");
                foreach (HazardObject hazardObject in m_levelHazards)
                {
                    hazardObject.DisableHazard();
                }
                m_onGlobalHazardDisable.Invoke();
            }

            public void EnableTypeHazards(HazardTypes type)
            {
                if (m_debugging) Debug.Log($"Triggering {type} hazards.");
                foreach (HazardObject hazardObject in m_levelHazards)
                {
                    if (type == hazardObject.HazardType()) hazardObject.EnableHazard();
                }
                //m_onGlobalHazardEnable.Invoke();
            }

            public void EnableTypeHazards(HazardTypes type, float time)
            {
                if (m_debugging) Debug.Log($"Triggering {type} hazards for {time} seconds."); ;
                foreach (HazardObject hazardObject in m_levelHazards)
                {
                    if (type == hazardObject.HazardType()) hazardObject.EnableHazard(time);
                }
                //m_onGlobalHazardEnable.Invoke();
            }
        }
    }
}