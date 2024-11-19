using UnityEngine;
using UnityEngine.UI;

namespace ILOVEYOU.MainMenu
{
    public class CodexScroll : MonoBehaviour
    {
        private VerticalLayoutGroup m_layout;
        // Start is called before the first frame update
        void Start()
        {
            m_layout = GetComponent<VerticalLayoutGroup>();

        }
        /// <summary>
        /// scrolls based on the increment value
        /// </summary>
        public void Scroll(float i)
        {
            transform.localPosition = new(0, (m_layout.minHeight / transform.childCount) * i);
        }
    }

}

