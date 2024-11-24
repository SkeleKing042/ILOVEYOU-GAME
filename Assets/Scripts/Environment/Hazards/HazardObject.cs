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
            private bool m_isActive = false;
            private bool m_triggerOnce = false;
            private bool m_triggered = false;
            [SerializeField] private HazardTypes m_hazardType; 
            [SerializeField] private List<string> m_targetTags;
            [SerializeField] private UnityEvent m_effectOnActive;
            [SerializeField] private UnityEvent<GameObject> m_effectsToApplyOnEnter;
            [SerializeField] private UnityEvent<GameObject> m_effectsToApplyOnStay;
            [SerializeField] private UnityEvent<GameObject> m_effectsToApplyOnExit;
            [SerializeField] private UnityEvent m_effectOnDeactive;

            public HazardTypes HazardType() { return m_hazardType; }
            public void SetTrigger() { m_triggerOnce = true; }

            #region Toggling
            /// <summary>
            /// Enables all the triggers on this object, then disables them after the given time.
            /// </summary>
            /// <param name="time"></param>
            public void EnableHazard(float time)
            {
                if (m_triggered) return;

                m_triggered = m_triggerOnce;

                EnableHazard();
                CancelInvoke();
                Invoke(nameof(DisableHazard), time);
            }
            /// <summary>
            /// Enables all the triggers on this object.
            /// </summary>
            public void EnableHazard()
            {
                //possible TODO: add what ive done to other enable hazard to this

                if (m_isActive == false)
                {
                    Debug.Log($"Enabling {name} hazard");
                    m_isActive = true;
                    m_effectOnActive.Invoke();
                }
            }
            public void DisableHazard() { DisableHazard(false); }
            /// <summary>
            /// Disables all the trigger on this object.
            /// </summary>
            public void DisableHazard(bool force = false)
            {
                if (m_isActive == true || force)
                {
                    Debug.Log($"Disabling {name} hazard");
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
                Debug.Log($"{go} is causing a collision event with {gameObject}.");
            }
            #endregion
        }
    }
}