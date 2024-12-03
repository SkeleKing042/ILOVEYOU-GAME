using ILOVEYOU.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ILOVEYOU.Colour{
    public class BackgroundColour : MonoBehaviour
    {
        void Start()
        {
            SetColour();
        }

        public void SetColour()
        {
            if (GetComponent<Image>()) GetComponent<Image>().color = ColorPref.Get("Background Color0");
            else GetComponent<RawImage>().color = ColorPref.Get("Background Color0");
        }
    }
}


