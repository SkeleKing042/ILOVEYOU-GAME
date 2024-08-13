using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Player
    {
        public class TaskManager : MonoBehaviour
        {
            private List<Task> m_tasks = new List<Task>();
            public int NumberOfTasks { get { return m_tasks.Count; } }
            [HideInInspector] public int TaskCompletionPoints;
            [Header("UI")]
            [SerializeField] private GameObject m_taskUIPrefab;
            public bool Startup()
            {
                TaskCompletionPoints = 0;
                return true;
            }
            /// <summary>
            /// Creates a new class to add to the list
            /// </summary>
            /// <param name="type"></param>
            /// <param name="cap"></param>
            /// <returns></returns>
            public int AddTask(TaskType type, float cap)
            {
                m_tasks.Add(new Task(type, cap));
                //GameObject taskUI = Instantiate(m_taskUIElementPrefab);
                //m_taskUIElements.Add(taskUI);
                //taskUI.transform.SetParent(m_taskUIContainer, false);
                _verifyTaskList();
                return m_tasks.Count - 1;
            }
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
                List<Task> completeTasks = new List<Task>();
                //Get a list of all the completed tasks
                foreach (Task task in m_tasks)
                {
                    if (task.IsComplete)
                        completeTasks.Add(task);
                }
                //Remove those completed tasks from the main list
                foreach (Task task in completeTasks)
                {
                    //Destroy(m_taskUIElements[m_tasks.IndexOf(task)]);
                    m_tasks.Remove(task);
                    TaskCompletionPoints++;
                }
                return true;
            }
            /// <summary>
            /// Returns all the indexes of tasks matching the given type
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public int[] GetMatchingTasks(TaskType type)
            {
                //Create a list of indexes
                List<int> indexes = new List<int>();
                //If the type of the current iteration matches the requested type...
                //...saved its index
                for (int i = 0; i < m_tasks.Count; i++)
                {
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
                UpdateTaskUI();
            }
            private void UpdateTaskUI()
            {
                for (int i = 0; i < m_tasks.Count; i++)
                {
                    Image tmp = m_taskUIElements[i].GetComponentInChildren<Image>();
                    if (tmp != null)
                    {
                    tmp.fillAmount = m_tasks[i].GetPercent;

                    }
                }
            }*/
        }
    }
}