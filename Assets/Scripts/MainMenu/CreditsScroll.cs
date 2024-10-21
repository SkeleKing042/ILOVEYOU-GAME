using UnityEngine;

namespace ILOVEYOU.MainMenu
{
    public class CreditsScroll : MonoBehaviour
    {
        private GameObject m_creditsObject;

        private void Start() => m_creditsObject = transform.GetChild(0).gameObject;

        public void Scroll(float scrollValue)
        {
            m_creditsObject.transform.localPosition = new(m_creditsObject.transform.localPosition.x, -602 + (401 * scrollValue) + 201);
        }
    }
}


