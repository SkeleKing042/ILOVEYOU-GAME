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
            Invalid,
            Kills,
            Time,
            Area,
            Sequence
        };
        [System.Serializable]
        public class Task
        {
            [SerializeField] private TaskType m_type;
            public TaskType GetTaskType { get { return m_type; } }

            [SerializeField] private float m_capValue;
            public float GetCapValue { get { return m_capValue; } }
            private float m_incValue;
            public float GetCurrentValue {  get { return m_incValue; } }
            public float GetPercent { get { return m_incValue/m_capValue; } }

            public bool IsComplete
            {
                get
                {
                    if (m_type == TaskType.Invalid)
                        return false;
                    return m_incValue >= m_capValue;
                }
            }

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