using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ILOVEYOU
{
    using ILOVEYOU.UI;
    using Management;
    using UnityEngine.UI;

    namespace Cards
    {
        public class DisruptCard : MonoBehaviour
        {
            public enum category
            {
                Buff,
                Debuff,
                Summon,
                Hazard  
            };
            [SerializeField] private category m_cardType;
            private Color m_color;
            [SerializeField] private bool m_effectSelf;
            [SerializeField] private Image m_cardFace;
            void Awake()
            {
                string key = $"{m_cardType} color";
                if(!PlayerPrefs.HasKey($"{key} R"))
                {
                    ColorPref.Set(key, m_color);
                }
                else
                {
                    m_color = ColorPref.Get(key);
                }

                m_cardFace.color = m_color;
            }
            public virtual void ExecuteEvents(PlayerManager caller)
            {
                //This function is called by a button on click event, a script with this function should be attached to the same gameobject as this script
                switch (m_effectSelf)
                {
                    case true:
                        caller.GetUI.GetCardVin.FlashIn(m_color);
                        break;
                    case false:
                        foreach(PlayerManager others in GameManager.Instance.GetOtherPlayers(caller))
                        {
                            others.GetUI.GetCardVin.FlashIn(m_color);
                        }
                        break;
                }
            }
        }
    }
}