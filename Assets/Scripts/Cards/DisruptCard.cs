using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ILOVEYOU
{
    namespace Cards
    {
        public class DisruptCard : MonoBehaviour
        {
            [HideInInspector]
            public UnityEvent m_playerHandToDiscard;
            public void Trigger()
            {
                //This function is called by a button on click event, a script with this function should be attached to the same gameobject as this script
                SendMessage("ExecuteEvents");
                m_playerHandToDiscard.Invoke();
            }
        }
    }
}