using TMPro;
using UnityEngine;

namespace ILOVEYOU.MainMenu
{
    public class CursorEffect : MonoBehaviour
    {
        [SerializeField] private float m_updateRate;
        [SerializeField] char m_cursorChar;
        private float m_time = 0;
        private bool m_enabled = false;
        private TextMeshProUGUI m_text;

        private void Awake()
        {
            m_text = GetComponent<TextMeshProUGUI>();
            EnableCursor();
        }

        private void Update()
        {
            if (m_enabled)
            {
                m_time += Time.deltaTime * m_updateRate;

                if (m_time > 1f)
                {

                    m_text.text = (m_text.text.ToCharArray()[^1] == m_cursorChar) ? m_text.text[..^1] : m_text.text + m_cursorChar;
                    m_time = 0;
                }
            }
        }

        public void EnableCursor()
        {
            if (m_text.text.ToCharArray().Length == 0) m_text.text += ' ';

            if (m_text.text.ToCharArray()[^1] != ' ')
            {
                m_text.text += ' ';
            }
            m_enabled = true;
        }

        public void DisableCursor()
        {
            if (m_text.text.Contains(m_cursorChar)) m_text.text.Remove(m_text.text.IndexOf(m_cursorChar));
            m_enabled = false;
        }
    }

}

