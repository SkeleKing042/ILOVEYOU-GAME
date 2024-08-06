using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Hazards
    {

        public class HazardObject : MonoBehaviour
        {
            private Collider[] m_effectArea;
            private bool m_isActive = false;
            // Start is called before the first frame update
            void Awake()
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
            }
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
                }
                ToggleBoxes();
            }
            /// <summary>
            /// Disables all the trigger on this object.
            /// </summary>
            public void DisableHazard()
            {
                if (m_isActive == true)
                {
                    m_isActive = false;
                }
                ToggleBoxes();
            }
        }
    }
}