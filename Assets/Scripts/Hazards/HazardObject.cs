using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ILOVEYOU
{
    namespace Hazards
    {

        public class HazardObject : MonoBehaviour
        {
            [SerializeField] private bool m_debugging;
            private bool m_isActive = false;
            [SerializeField] private List<string> m_targetTags;
            [SerializeField] private UnityEvent m_effectOnActive;
            [SerializeField] private UnityEvent<GameObject> m_effectsToApplyOnEnter;
            [SerializeField] private UnityEvent<GameObject> m_effectsToApplyOnStay;
            [SerializeField] private UnityEvent<GameObject> m_effectsToApplyOnExit;
            [SerializeField] private UnityEvent m_effectOnDeactive;
      /*      void Awake()
            {
                //Get all the colliders on this object
                Collider[] allColliders = GetComponents<Collider>();
                //Make an empty array with the same length
                Collider[] justTriggers = new Collider[allColliders.Length];
                int index = 0;
                //Go through each collider...
                foreach(Collider c in allColliders)
                {
                    //...if that collider is a trigger...
                    if (c.isTrigger)
                    {
                        //...save that collider to the triggers array
                        justTriggers[index] = c;
                        index++;
                    }
                }
                //Set the main collider array to the length of the number of triggers found
                m_effectArea = new Collider[index];
                //Put all the triggers in the main array.
                for(int i = 0; i < m_effectArea.Length; i++)
                {
                    m_effectArea[i] = justTriggers[i];
                }
                ToggleBoxes();
            }
            private void ToggleBoxes()
            {
                foreach (Collider box in m_effectArea)
                {
                    box.enabled = m_isActive;
                }
            }*/
            #region Toggling
            /// <summary>
            /// Enables all the triggers on this object, then disables them after the given time.
            /// </summary>
            /// <param name="time"></param>
            public void EnableHazard(float time)
            {
                EnableHazard();
                Invoke("DisableHazard", time);
            }
            /// <summary>
            /// Enables all the triggers on this object.
            /// </summary>
            public void EnableHazard()
            {
                if (m_isActive == false)
                {
                    m_isActive = true;
                    m_effectOnActive.Invoke();
                }
            }
            /// <summary>
            /// Disables all the trigger on this object.
            /// </summary>
            public void DisableHazard()
            {
                if (m_isActive == true)
                {
                    m_isActive = false;
                    m_effectOnDeactive.Invoke();
                }
            }
            #endregion
            #region CollisionEvents
            private void OnTriggerEnter(Collider other)
            {
                //If this object isn't enabled, don't do anything
                if(!m_isActive)
                    return;

                //If the tag of the colliding object is in our list...
                if(m_targetTags.Contains(other.tag))
                {
                    //..carry out the effects on it.
                    m_effectsToApplyOnEnter.Invoke(other.gameObject);
                }
            }
            private void OnTriggerStay(Collider other)
            {
                //If this object isn't enabled, don't do anything
                if(!m_isActive)
                    return;

                //If the tag of the colliding object is in our list...
                if(m_targetTags.Contains(other.tag))
                {
                    //..carry out the effects on it.
                    m_effectsToApplyOnStay.Invoke(other.gameObject);
                }
            }
            private void OnTriggerExit(Collider other)
            {
                //If this object isn't enabled, don't do anything
                if(!m_isActive)
                    return;

                //If the tag of the colliding object is in our list...
                if(m_targetTags.Contains(other.tag))
                {
                    //..carry out the effects on it.
                    m_effectsToApplyOnExit.Invoke(other.gameObject);
                }
            }
            public void LogObject(GameObject go)
            {
                if(m_debugging) Debug.Log($"{go} is causing a collision event with {gameObject}.");
            }
            #endregion
        }
    }
}