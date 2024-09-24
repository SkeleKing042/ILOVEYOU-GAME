using ILOVEYOU.Player;
using UnityEngine;
using UnityEngine.Events;

namespace ILOVEYOU
{
    namespace Environment
    {

        public class AreaControlPoint : MonoBehaviour
        {
            private Task m_taskReference;
            private bool m_taskVerified;

            [Header("Events")]
            [SerializeField] private UnityEvent m_onAreaCreated;
            [SerializeField] private UnityEvent m_onAreaStarted;
            [SerializeField] private UnityEvent m_onAreaProgressed;
            [SerializeField] private UnityEvent m_onAreaStopped;
            [SerializeField] private UnityEvent m_onAreaCompleted;
            /// <summary>
            /// Object initialisation
            /// </summary>
            /// <param name="task"></param>
            /// <returns></returns>
            public bool Init(Task task)
            {
                //Save the task ref number
                m_taskReference = task;
                //If the task hasn't been verified...
                if (!m_taskVerified)
                {
                    //..check the task type is an area type
                    if (m_taskReference.GetTaskType == TaskType.Area)
                    {
                        m_taskVerified = true;
                    }
                    else
                    {
                        Debug.LogError($"Task type mismatch! Given task index reference isn't of the Area type but has been given to {this}. {this} will now close.");
                        CloseArea();
                        return false;
                    }
                }

                m_taskVerified = false;
                m_onAreaCreated.Invoke();
                return true;
            }
            /// <summary>
            /// Destorys this area
            /// </summary>
            /// <param name="forceClose"></param>
            /// <returns></returns>
            public bool CloseArea()//bool forceClose)
            {
                /*if (!forceClose)
                {
                }*/
                    m_onAreaCompleted.Invoke();

                //gameObject.SetActive(false);

                return true;
            }

            private void OnTriggerEnter(Collider other)
            {
                if (other.tag != "Player")
                    return;

                m_onAreaStarted.Invoke();
            }
            private void OnTriggerStay(Collider other)
            {
                //Don't do anything if other isn't the player
                if (other.tag != "Player" || m_taskReference == null)
                    return;

                //Update the task
                bool isCompleted = m_taskReference.UpdateTask(Time.deltaTime);

                //If the task is completed, close this area
                if(isCompleted)
                {
                    CloseArea();
                    return;
                }

                m_onAreaProgressed.Invoke();
            }
            private void OnTriggerExit(Collider other)
            {
                //Don't do anything if other isn't the player
                if (other.tag != "Player")
                    return;

                m_onAreaStopped.Invoke();
            }
        }
    }
}