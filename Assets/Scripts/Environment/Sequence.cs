using ILOVEYOU.Player;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace ILOVEYOU
{
    namespace Environment
    {
        public class Sequence : MonoBehaviour
        {
            [Tooltip("Whether or not the sequence progresses reguardless of point order.")]
            [SerializeField] private bool m_isOrdered;
            [Tooltip("Whether or not the progress should be reset on a fail.")]
            [SerializeField] private bool m_isSafe;
            [Tooltip("Whether or not already tripped flags should be ignored when checking the sequence.")]
            [SerializeField] private bool m_IgnoreTripped;
            [SerializeField] private List<Transform> m_points;
            private bool[] m_flags;

            private Task m_taskReference;

            [Header("Events")]
            [SerializeField] private UnityEvent m_onSequenceStart;
            [SerializeField] private UnityEvent m_onSequenceProgressed;
            [SerializeField] private UnityEvent m_onSequenceProgressFail;
            [SerializeField] private UnityEvent m_onSequenceReset;
            [SerializeField] private UnityEvent m_onSequenceEnd;

            //Checks the number of flags set to true then divides it by the number of total flags
            public float Progress
            {
                get
                {
                    float trues = 0;
                    foreach(bool flag in m_flags)
                    {
                        if (flag) trues++;
                    }
                    return trues / m_flags.Length;
                }
            }
            public bool IsComplete { get { return Progress == 1; } }
            public bool Init(Task task)
            {
                //Set the task ref
                m_taskReference = task;

                //Reset flags
                m_flags = new bool[m_points.Count];

                //Make sure each point has a sequence point component
                foreach(Transform segment in m_points)
                {
                    //Get the sequence point component
                    SequencePoint point = segment.GetComponent<SequencePoint>();
                    //If there isn't one...
                    if(!point)
                    {
                        //...add it
                        point = segment.AddComponent<SequencePoint>();
                    }
                    //Attempt to initialise the point
                    if (!point.Init(AttemptProgress))
                    {
                        Debug.LogWarning($"One or more points in {this} failed to be initialised correctly.");
                        return false;
                    }
                }

                //Sequence has been started, trigger events
                m_onSequenceStart.Invoke();
                return true;
            }
            /// <summary>
            /// Checks the flags and sent object to see if this sequence should be updated
            /// </summary>
            /// <param name="sender"></param>
            /// <returns>True if progress increased</returns>
            public bool AttemptProgress(Transform sender)
            {
                //Get the sent transform's index in the point list
                int index = m_points.IndexOf(sender);
                
                //If its in the list
                if (index >= 0)
                {
                    //begin sequence checks

                    if (!m_isOrdered)
                    {
                        //If already set, ask what should happen
                        if (m_flags[index])
                            return _alreadyTripped();
                        else //enable the matching flag
                        {
                            m_flags[index] = true;
                            //update progress
                            _progressSuccess();
                            return true;
                        }
                    }
                    else //if unordered
                    {
                        //If already set, ask what should happen
                        if (m_flags[index])
                            return _alreadyTripped();
                        else //enable the matching flag
                        {
                            //if this isn't the first object in the list...
                            if (index > 0)
                                //...and the previous flag is false...
                                if (m_flags[index - 1] == false)
                                    //The sequence has failed to be progressed
                                    return _progressFailed();

                            //This must be the next flag in the sequence
                            m_flags[index] = true;
                            _progressSuccess();
                            return true;
                        }
                    }
                }
                else
                {
                    //object not in sequence
                    return _progressFailed();
                }
            }
            /// <summary>
            /// Sets all the flags in sequence to false
            /// </summary>
            public void ResetFlags()
            {
                //reset all the flags
                for (int i = 0; i < m_flags.Length; i++)
                {
                    m_flags[i] = false;
                }
                //Trigger events and update task progress
                m_onSequenceReset.Invoke();
                m_taskReference.SetValue(Progress);
            }
            /// <summary>
            /// Handles what happens when a flag is already set to true
            /// </summary>
            /// <returns></returns>
            private bool _alreadyTripped()
            {
                //Skip if we're not bothering with them
                if (m_IgnoreTripped)
                    return true;
                else //count it as a fail
                    return _progressFailed();
            }
            /// <summary>
            /// Handles what happens when progress has been made
            /// </summary>
            /// <returns></returns>
            private bool _progressSuccess()
            {
                //Trigger events and update task progress
                m_onSequenceProgressed.Invoke();
                m_taskReference.SetValue(Progress);
                SequenceComplete();
                return true;
            }
            /// <summary>
            /// Handles what happens when the sequence is progressed incorrectly
            /// </summary>
            /// <returns></returns>
            private bool _progressFailed()
            {
                if (!m_isSafe) //safety disabled - force a reset
                    ResetFlags();
                else //just fail normally
                    m_onSequenceProgressFail.Invoke();

                //Update task progress
                m_taskReference.SetValue(Progress);
                return false;
            }
            public bool SequenceComplete()
            {
                if (IsComplete)
                {
                    m_onSequenceEnd.Invoke();
                    return true;
                }
                return false;
            }
        }
    }
}