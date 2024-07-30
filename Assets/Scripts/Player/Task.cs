using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Player
    {
        public enum TaskType
        {
            Kills,
            Time,
            Unique
        };
        [System.Serializable]
        public struct Task
        {
            [SerializeField] private TaskType m_type;
            public TaskType GetTaskType { get { return m_type; } }

            [SerializeField] private float m_capValue;
            public float GetCapValue { get { return m_capValue; } }
            private float m_incValue;

            public bool IsComplete { get { return m_incValue >= m_capValue; } }

            public Task(TaskType type, float cap)
            {
                m_incValue = 0;
                m_type = type;
                m_capValue = cap;
            }
            /// <summary>
            /// Increments the value of this task
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool UpdateTask(float value)
            {
                m_incValue += value;
                return IsComplete;
            }
            /// <summary>
            /// Resets this task back to 0
            /// </summary>
            /// <returns></returns>
            public bool ResetTask()
            {
                m_incValue = 0;
                return IsComplete;
            }
        }
    }
}