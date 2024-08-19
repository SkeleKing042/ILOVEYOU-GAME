using Palmmedia.ReportGenerator.Core;
using System.Collections;
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
            private bool m_isOrdered;
            [SerializeField] private List<Transform> m_points;
            private bool[] m_flags;

            [SerializeField] private UnityEvent m_onSequenceStart;
            [SerializeField] private UnityEvent m_onSequenceProgressed;
            [SerializeField] private UnityEvent m_onSequenceProgressFail;
            [SerializeField] private UnityEvent m_onSequenceReset;
            [SerializeField] private UnityEvent m_onSequenceEnd;

            public float Progress
            {
                get
                {
                    int trues = 0;
                    foreach(bool flag in m_flags)
                    {
                        if (flag) trues++;
                    }
                    return trues / m_flags.Length;
                }
            }
            public bool isComplete { get { return Progress == 1; } }
            public bool Init(bool isOrdered)//, UnityEvent[] events)
            {
                //setPoints.CopyTo(m_points, 0);
                m_flags = new bool[m_points.Count];
                m_isOrdered = isOrdered;

                //events
                //m_onSequenceStart = events[0];
                //m_onSequenceProgressed = events[1];
                //m_onSequenceProgressFail = events[2];
                //m_onSequenceReset = events[3];
                //m_onSequenceEnd = events[4];

                m_onSequenceStart.Invoke();
                return true;
            }
            public bool AttemptProgress(Transform sender, bool isSafe = true)
            {
                //unordered
                if(!m_isOrdered)
                {
                    if (m_points.Contains(sender))
                    {
                        m_flags[m_points.IndexOf(sender)] = true;
                        return true;
                    }
                    return false;
                }
                //ordered
                if (m_isOrdered)
                {
                    //get the index of the sender
                    int index = m_points.IndexOf(sender);

                    //if this isn't the first object
                    if (index > 0)
                        //if the previous flag is false
                        if (m_flags[index - 1] == false)
                        {
                            //if this call was unsafe
                            if(!isSafe)
                                ResetFlags();

                            //this call was made out of order
                            return false;
                        }

                    //this must be the next flase
                    m_flags[index] = true;
                    return true;
                }
                return false;
            }
            public void ResetFlags()
            {
                //reset all the flags
                for (int i = 0; i < m_flags.Length; i++)
                {
                    m_flags[i] = false;
                }
            }
        }
    }
}