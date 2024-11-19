using ILOVEYOU.UI;
using UnityEngine;
using UnityEngine.UI;
using static ILOVEYOU.Cards.DisruptCard;

namespace ILOVEYOU.MainMenu
{
    public class CardColour : MonoBehaviour
    {
        private Color m_color;
        [SerializeField] private category m_cardType;
        private Image m_cardFace;

        void Awake()
        {
            m_cardFace = GetComponent<Image>();

            string key = $"{m_cardType} color";
            if (!PlayerPrefs.HasKey($"{key} R"))
            {
                ColorPref.Set(key, m_color);
            }
            else
            {
                m_color = ColorPref.Get(key);
            }

            m_cardFace.color = m_color;
        }
    }
}

