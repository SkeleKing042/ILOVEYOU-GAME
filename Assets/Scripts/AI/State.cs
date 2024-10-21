using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU.AI
{
    public class State : MonoBehaviour
    {
        protected StateMachine m_machine;
        public virtual void StartState(StateMachine referenceObject)
        {
            m_machine = referenceObject;
        }
        public virtual void UpdateState()
        {

        }
        public virtual void EndState()
        {

        }
    }
}