using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ILOVEYOU
{
    namespace Management
    {

        public class Controller : MonoBehaviour
        {
            //The devices on the controller
            private uint m_id;
            public uint ID { get { return m_id; } set { m_id = value; } }
            private InputDevice[] m_devices;
            public InputDevice[] GetDevice => m_devices;
            private GameObject m_assignedObject;
            public bool IsAssigned { get { return m_assignedObject != null; } }
            private void Awake()
            {
                //Get all the devices from the input
                m_devices = GetComponent<PlayerInput>().devices.ToArray();
            }
            public bool AssignObject(GameObject obj)
            {
                //an object using this controller has been added
                if (!IsAssigned)
                {
                    m_assignedObject = obj;
                    return true;
                }
                return false;
            }
            /// <summary>
            /// Stuff to do when the controller is connected.
            /// </summary>
            public void OnJoin()
            {

            }
            /// <summary>
            /// Remove this object on leave function
            /// </summary>
            public void OnLeave()
            {
                if (!IsAssigned)
                    Destroy(gameObject);
            }
        }
    }
}