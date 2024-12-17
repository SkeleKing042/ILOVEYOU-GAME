using UnityEngine;

namespace ILOVEYOU.MainMenu
{
    public class CreditsScroll : MonoBehaviour
    {
        //private GameObject m_creditsObject;

        //private void Start() => m_creditsObject = transform.GetChild(0).gameObject;

        public void Scroll(float scrollValue)
        {
            gameObject.transform.localPosition = new(gameObject.transform.localPosition.x,(40.26f * scrollValue));
        }
    }
}


