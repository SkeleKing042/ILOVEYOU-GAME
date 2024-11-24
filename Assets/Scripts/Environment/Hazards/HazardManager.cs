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
            Water,
            Something
        }

        public class HazardManager : MonoBehaviour
        {
            [SerializeField] private Transform m_hazardContainer;
            private HazardObject[] m_levelHazards;
            private List<HazardObject> m_tempLevelHazards = new();
            [SerializeField] private UnityEvent m_onGlobalHazardEnable;
            [SerializeField] private UnityEvent m_onGlobalHazardDisable;
            // Start is called before the first frame update
            public bool Startup()
            {
                Debug.Log($"Starting {this}.");
                //Get all the hazards in the level.
                m_levelHazards = m_hazardContainer.GetComponentsInChildren<HazardObject>();


                if (m_levelHazards.Length == 0)
                {
                    Debug.LogWarning($"{this} was unable to find any hazard objects.");
                }
                foreach(var hazard in m_levelHazards)
                {
                    hazard.DisableHazard(true);
                }

                //passed
                Debug.Log($"{this} started successfully.");
                return true;
            }
            /// <summary>
            /// Enables all hazards assigned to hazard manager
            /// </summary>
            public void EnableAllHazards()
            {
                Debug.Log("Triggering hazards.");
                foreach(HazardObject hazardObject in m_levelHazards)
                {
                    hazardObject.EnableHazard();
                }
                foreach (HazardObject hazardObject in m_tempLevelHazards)
                {
                    hazardObject.EnableHazard();
                }
                m_onGlobalHazardEnable.Invoke();
            }
            /// <summary>
            /// enables all hazards assigned to hazard manager for set amount of time
            /// </summary>
            /// <param name="time">Amount of time hazards are active for</param>
            public void EnableAllHazards(float time)
            {
                Debug.Log($"Triggering hazards for {time} seconds.");
                foreach (HazardObject hazardObject in m_levelHazards)
                {
                    hazardObject.EnableHazard(time);
                }
                foreach (HazardObject hazardObject in m_tempLevelHazards)
                {
                    hazardObject.EnableHazard(time);
                }
                m_onGlobalHazardEnable.Invoke();
            }
            /// <summary>
            /// Disables all hazards assigned to hazard manager
            /// </summary>
            public void DisableAllHazards()
            {
                Debug.Log("Disabling hazards.");
                foreach (HazardObject hazardObject in m_levelHazards)
                {
                    hazardObject.DisableHazard();
                }
                foreach (HazardObject hazardObject in m_tempLevelHazards)
                {
                    hazardObject.DisableHazard();
                }
                m_onGlobalHazardDisable.Invoke();
            }
            /// <summary>
            /// Enables all hazards of type assigned to hazard manager
            /// </summary>
            /// <param name="type">Type if hazard to enable</param>
            public void EnableTypeHazards(HazardTypes type)
            {
                Debug.Log($"Triggering {type} hazards.");
                foreach (HazardObject hazardObject in m_levelHazards)
                {
                    if (type == hazardObject.HazardType()) hazardObject.EnableHazard();
                }
                foreach (HazardObject hazardObject in m_tempLevelHazards)
                {
                    if (type == hazardObject.HazardType()) hazardObject.EnableHazard();
                }
                //m_onGlobalHazardEnable.Invoke();
            }
            public void EnableTypeHazards(HazardTypes type, float time)
            {
                Debug.Log($"Triggering {type} hazards for {time} seconds."); ;
                foreach (HazardObject hazardObject in m_levelHazards)
                {
                    if (type == hazardObject.HazardType()) hazardObject.EnableHazard(time);
                }
                foreach (HazardObject hazardObject in m_tempLevelHazards)
                {
                    if (type == hazardObject.HazardType()) hazardObject.EnableHazard(time);
                }
                //m_onGlobalHazardEnable.Invoke();
            }
            /// <summary>
            /// Disables all hazards of type assigned to hazard manager
            /// </summary>
            /// <param name="type">Type to disable</param>
            public void DisableAllTypeHazards(HazardTypes type)
            {
                Debug.Log("Disabling hazards.");
                foreach (HazardObject hazardObject in m_levelHazards)
                {
                    if (type == hazardObject.HazardType()) hazardObject.DisableHazard();
                }
                foreach (HazardObject hazardObject in m_tempLevelHazards)
                {
                    if (type == hazardObject.HazardType()) hazardObject.DisableHazard();
                }
                //m_onGlobalHazardDisable.Invoke();
            }
            /// <summary>
            /// Adds a new hazard to the temp list
            /// </summary>
            public void AddHazard(HazardObject obj)
            {
                m_tempLevelHazards.Add(obj);
            }

            /// <summary>
            /// Adds a new hazard to the temp list
            /// </summary>
            public void AddHazard(HazardObject obj, float time)
            {
                m_tempLevelHazards.Add(obj);

                StartCoroutine(RemoveHazardTime(obj, time));
            }
            public IEnumerator RemoveHazardTime(HazardObject obj, float time)
            {
                yield return new WaitForSeconds(time);
                Destroy(obj.gameObject);
                yield return new WaitForEndOfFrame();
                CleanTempList();
            }
            
            public void RemoveHazard(HazardObject obj)
            {
                Destroy(obj.gameObject);
                CleanTempList();
            }

            public void RemoveAllHazards()
            {
                StopAllCoroutines();
                foreach(HazardObject obj in m_tempLevelHazards)
                {
                    Destroy(obj.gameObject);
                }
                CleanTempList();
            }

            /// <summary>
            /// Clears out empty values from the temp hazard list
            /// </summary>
            public void CleanTempList()
            {
                List<HazardObject> tempList = new();
                
                foreach(HazardObject hazard in m_tempLevelHazards)
                {
                    if (hazard) tempList.Add(hazard);
                }

                m_tempLevelHazards = tempList;
            }

        }
    }
}