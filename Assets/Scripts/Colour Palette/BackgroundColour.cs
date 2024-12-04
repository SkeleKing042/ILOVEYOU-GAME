using ILOVEYOU.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ILOVEYOU.Colour{
    public class BackgroundColour : MonoBehaviour
    {
        [SerializeField] private bool m_background = true;


        void Start()
        {
            SetColour();
        }

        public void SetColour()
        {
            string key = (m_background) ? "Background Color0" : "Important Color0";


            if (GetComponent<Image>()) GetComponent<Image>().color = ColorPref.Get(key);
            else GetComponent<RawImage>().color = ColorPref.Get(key);
        }
    }
}


