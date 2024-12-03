using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ILOVEYOU
{
    using ILOVEYOU.Audio;
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
            private int m_playerID = 0;
            [SerializeField] private bool m_effectSelf;
            public bool DoesEffectSelf => m_effectSelf;
            [SerializeField] private Image m_cardFace;
            [SerializeField] private GameObject[] m_particleEffects;
            [SerializeField] private GameObject[] m_selfParticleEffects;
            [SerializeField][Tooltip("putting in nothing will play nothing")] private string m_soundToPlay = "";
            [SerializeField] [Tooltip("At the moment, this is purely for sound. Make sure these don't conflict with existing cards")] private int m_cardID;
            [SerializeField] private bool m_loop;
            void Awake()
            {
                //SetupColours();
            }
            public void SetPlayerID(int id)
            {
                m_playerID = id;
            }
            public void SetupColours()
            {
                string key = $"{m_cardType} color" + m_playerID;
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
            public virtual void ExecuteEvents(PlayerManager caller)
            {
                SoundManager.SFX.PlayRandomSound("CardSelect");

                if (m_soundToPlay != "")
                {
                    if (m_loop) SoundManager.SFX.PlayRandomSoundLoop(m_soundToPlay, m_cardID);
                    else SoundManager.SFX.PlayRandomSound(m_soundToPlay);
                }

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
                            if(m_particleEffects.Length > 0)
                            {
                                ParticleSpawner.SpawnParticles(m_particleEffects, others.transform);
                            }
                        }
                        break;
                }

                if(m_selfParticleEffects.Length > 0 && caller != null)
                    ParticleSpawner.SpawnParticles(m_selfParticleEffects, caller.transform);
            }
        }
    }
}