using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Environment
    {
        public class SequencePoint : MonoBehaviour
        {
            public delegate bool Callback(Transform sender);
            private Callback m_sequenceHead;

            /// <summary>
            /// Point initialisation
            /// </summary>
            /// <param name="method"></param>
            /// <returns>True if successfully started</returns>
            public bool Init(Callback method)
            {
                //Save the given method to the delegate
                m_sequenceHead = method;
                //Check for trigger boxes
                if(!GetComponent<Collider>().isTrigger)
                {
                    Debug.LogWarning($"No trigger colliders are attached to {this} and will not update the sequence. Please add on or check the \"Is Trigger\" bool to true.");
                    return false;
                }
                return true;
            }
            /// <summary>
            /// Fires if it detects the player
            /// </summary>
            /// <param name="other"></param>
            private void OnTriggerEnter(Collider other)
            {
                if(other.tag == "Player")
                {
                    m_sequenceHead(transform);
                }
            }
        }
    }
}