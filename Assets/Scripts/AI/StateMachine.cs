using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU.AI
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private State m_currentState;
        // Start is called before the first frame update
        public void Initialize()
        {
            if (m_currentState == null)
            {
                Debug.LogError($"State machine instance {gameObject.name} has no default state, please fix!");
            }
            else
                m_currentState.StartState(this);
        }
        public void ChangeState(State targetState, bool reset = false)
        {
            if (m_currentState.GetType() == targetState.GetType() && !reset)
                return;

            m_currentState.EndState();
            m_currentState = targetState;
            m_currentState.StartState(this);
        }

        // Update is called once per frame
        void Update()
        {
            m_currentState.UpdateState();
        }
    }
}