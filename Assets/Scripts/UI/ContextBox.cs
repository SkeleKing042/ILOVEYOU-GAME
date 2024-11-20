using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ILOVEYOU.UI
{
    public class ContextBox : MonoBehaviour
    {
        [Header("Context Button - Delegate void stuff")]
        [SerializeField] TextMeshProUGUI m_contextText;
        public delegate void ContextPress();
        private ContextPress m_contextPress;
        public ContextPress GetAction => m_contextPress;
        private int m_contextPriority; //current priority of the current context action
        /// <summary>
        /// Sets the context press function to whatever function is put in the ContextPress
        /// </summary>
        /// <param name="context">function for the context button to attatch to</param>
        /// <param name="priority">what priority this action has, the higher the priority, the less likely it will be overwritten</param>
        /// <param name="contextText">what display for the context text</param>
        public void SetContext(ContextPress context, int priority, string contextText)
        {
            if (priority > m_contextPriority && (context != m_contextPress || context == null)) { gameObject.SetActive(true); m_contextPress = context; m_contextText.text = contextText; }
        }
        //possible TODO: think about perhaps having multiple context actions at once
        /// <summary>
        /// removes the set context provided and resets values
        /// </summary>
        /// <param name="context">function to remove</param>
        public void RemoveSetContext(ContextPress context)
        {
            m_contextPress -= context;
            m_contextPriority = 0;
            m_contextText.text = "";
            gameObject.SetActive(false);
        }
        //might not need this one
        /// <summary>
        /// removes the all contexts provided and resets values
        /// </summary>
        public void RemoveAllContext()
        {
            m_contextPress = null;
            m_contextPriority = 0;
            m_contextText.text = "";
            gameObject.SetActive(false);
        }
    }
}