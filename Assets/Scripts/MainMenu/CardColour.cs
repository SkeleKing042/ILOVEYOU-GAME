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
            if (!PlayerPrefs.HasKey($"{key + 0} R")) //the + 0 is just to specify player 1's colours
            {
                ColorPref.Set(key + 0, m_color);
            }
            else
            {
                m_color = ColorPref.Get(key + 0);
            }

            m_cardFace.color = m_color;
        }
    }
}

