using ILOVEYOU.UI;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

namespace ILOVEYOU
{
    namespace Player
    {
        public class TaskManager : MonoBehaviour
        {
            [SerializeField] private bool m_debugging;
            private PlayerManager m_player;
            private Task[] m_tasks = new Task[10];
            private int m_tasksCompleted;
            private float m_taskWaitTime;

           //[SerializeField] private string[] m_taskDescriptions = new string[5];

            public int NumberOfTasks
            {
                get
                {
                    int i = 0;
                    foreach (Task task in m_tasks)
                        if (task.GetTaskType != TaskType.Invalid)
                            i++;
                    return i;
                }
            }
            [SerializeField] private uint m_taskLimit = 10;
            [HideInInspector] public int TaskCompletionPoints;
            [Header("UI")]
            //this should have a slider/image
            /*[SerializeField] private Image m_taskUIPrefab;
            private Image[] m_taskBars = new Image[10];
            [SerializeField] private Transform m_taskUIContainer;
            [SerializeField] private TextMeshProUGUI m_iconDisplay;
            [SerializeField] private Color m_iconColor;*/
            [SerializeField] private TaskDisplay m_taskDisplay;
            public bool Startup()
            {
                if(m_debugging) Debug.Log($"Starting {this}");

                m_player = GetComponent<PlayerManager>();
                TaskCompletionPoints = 0;
                m_tasks = new Task[m_taskLimit];
                for (int i = 0; i < m_tasks.Length; i++)
                {
                    m_tasks[i] = new(TaskType.Invalid, 0);
                }
                //m_taskBars = new Image[m_taskLimit];

                if (m_debugging) Debug.Log($"{this} started successfully.");
                return true;
            }
            /// <summary>
            /// Creates a new class to add to the list
            /// </summary>
            /// <param name="type"></param>
            /// <param name="cap"></param>
            /// <returns>The index of the task</returns>
            public int AddTask(TaskType type, float cap)
            {
                //Find an empty slot in the array
                for (int i = 0; i < m_tasks.Length; i++)
                {
                    if (m_tasks[i] == null || m_tasks[i].GetTaskType == TaskType.Invalid)
                    {
                        //Fill the slot with a new task
                        m_tasks[i] = new(type, cap);

                        //Set up ui element
                        if (m_taskDisplay) m_taskDisplay.SetTask(ref m_tasks[i]);

                        if (m_tasks[i].GetTaskType == TaskType.Area)
                        {
                            m_player.GetLevelManager.StartControlPoint(m_tasks[i]);
                        }
                        if (m_tasks[i].GetTaskType == TaskType.Sequence)
                        {
                            m_player.GetLevelManager.StartSequence(m_tasks[i]);
                        }

                        m_player.GetLog.LogInput($"<color=\"green\"><sprite=\"iconSheet\" index={(int)m_tasks[i].GetTaskType} color=#00FF00>{m_tasks[i].GetTaskType}</color> task assigned to task list.");

                        m_taskWaitTime = 0;
                        _verifyTaskList();
                        //Return the index of the new task
                        return i;
                    }
                }
                //No empty spaces exist in the array and the task cannot be added.
                return -1;
            }
            /// <summary>
            /// Creates a new class to add to the list
            /// </summary>
            /// <param name="task"></param>
            /// <returns>The index of the task</returns>
            public int AddTask(Task task)
            {
                return AddTask(task.GetTaskType, task.GetCapValue);
            }
            /// <summary>
            /// Checks if any tasks are complete and removes them from the list
            /// </summary>
            /// <returns></returns>
            private bool _verifyTaskList()
            {
                //Find any completed tasks...
                for (int i = 0; i < m_tasks.Length; i++)
                {
                    if (m_tasks[i].GetTaskType == TaskType.Invalid)
                        continue;
                    if (m_tasks[i].IsComplete)
                    {
                        m_player.GetLog.LogInput($"{m_tasks[i].GetTaskType} task complete. Rewarding cards.");
                        //..clear the task in that slot
                        m_tasks[i] = new(TaskType.Invalid, 0);

                        //Give the player a point that will get exchanged for cards later
                        TaskCompletionPoints++;
                        DataExporter.DataExport.GetValue($"Player {m_player.GetPlayerID + 1} task time", m_tasksCompleted) = m_taskWaitTime;
                        m_tasksCompleted++;
                    }
                }
                return true;
            }
            public Task GetTask(int index)
            {
                return m_tasks[index];
            }
            /// <summary>
            /// Returns all the indexes of tasks matching the given type
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public int[] GetMatchingTasks(TaskType type)
            {
                //Create a list of indexes
                List<int> indexes = new();
                //If the type of the current iteration matches the requested type...
                //...saved its index
                for (int i = 0; i < m_tasks.Length; i++)
                {
                    if (m_tasks[i].GetTaskType == TaskType.Invalid)
                        continue;
                    if (m_tasks[i].GetTaskType == type)
                    {
                        indexes.Add(i);
                    }
                }
                return indexes.ToArray();
            }
            /// <summary>
            /// Updates the value of a task at a given index
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool UpdateTask(int index, float value)
            {
                bool x = m_tasks[index].UpdateTask(value);
                _verifyTaskList();
                return x;
            }
            /// <summary>
            /// Resets the task at the given index
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public bool ResetTask(int index)
            {
                bool x = m_tasks[index].ResetTask();
                _verifyTaskList();
                return x;
            }
            /// <summary>
            /// Updates any kill type tasks
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool UpdateKillTrackers(float value)
            {
                foreach (Task task in m_tasks)
                {
                    if (task.GetTaskType == TaskType.Invalid)
                        continue;
                    if (task.GetTaskType == TaskType.Kills)
                        task.UpdateTask(value);
                }
                _verifyTaskList();
                return true;
            }

            /// <summary>
            /// Updates any time type tasks
            /// </summary>
            /// <param name="doReset"></param>
            /// <returns></returns>
            public bool UpdateTimers(bool doReset)
            {
                foreach (Task task in m_tasks)
                {
                    if (task.GetTaskType == TaskType.Invalid)
                        continue;
                    if (task.GetTaskType == TaskType.Time)
                    {
                        if (doReset)
                            task.ResetTask();

                        else
                            task.UpdateTask(Time.deltaTime);
                    }
                }
                _verifyTaskList();
                return true;
            }
            /*private void Update()
            {
                _updateTaskUI();
            }*/
            /*private void _updateTaskUI()
            {
                for (int i = 0; i < m_tasks.Length; i++)
                {
                    if (m_taskBars[i] != null)
                        m_taskBars[i].fillAmount = m_tasks[i].GetPercent;
                }
            }*/
        }
    }
}