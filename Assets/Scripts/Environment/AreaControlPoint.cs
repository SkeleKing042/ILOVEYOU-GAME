using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace ILOVEYOU
{
    namespace Environment
    {

        public class AreaControlPoint : MonoBehaviour
        {
            private int m_taskReferenceNumber = -1;
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
            /// <param name="index"></param>
            /// <returns></returns>
            public bool Init(int index)
            {
                //Save the task ref number
                m_taskReferenceNumber = index;

                //If a task wasn't created, close
                if(m_taskReferenceNumber == -1)
                {
                    CloseArea(true);
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
            public bool CloseArea(bool forceClose)
            {
                if (!forceClose)
                {
                    m_onAreaCompleted.Invoke();
                }

                Destroy(gameObject);

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
                if (other.tag != "Player")
                    return;

                //If the task hasn't been verified...
                if (!m_taskVerified)
                {
                    //..check the task type is an area type
                    if(other.GetComponent<TaskManager>().GetTask(m_taskReferenceNumber).GetTaskType == TaskType.Area)
                    {
                        m_taskVerified = true;
                    }
                    else
                    {
                        Debug.LogError($"Task type mismatch! Given task index reference isn't of the Area type but has been given to {this}. {this} will now close.");
                        CloseArea(true);
                        return;
                    }
                }

                //Update the task
                bool isCompleted = other.GetComponent<TaskManager>().UpdateTask(m_taskReferenceNumber, Time.deltaTime);

                //If the task is completed, close this area
                if(isCompleted)
                {
                    CloseArea(false);
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