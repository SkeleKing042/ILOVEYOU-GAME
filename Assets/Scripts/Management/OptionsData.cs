using UnityEngine;
using ILOVEYOU.Audio;
using System;
using static UnityEngine.Rendering.DebugUI;

namespace ILOVEYOU.Management
{
    public class OptionsData : MonoBehaviour
    {
        public static OptionsData Instance;

        [SerializeField] private float[] m_volume;


        public float[] Volume { get { return m_volume; } }

        private void Awake()
        {

            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            m_volume = new float[Enum.GetNames(typeof(SoundTag)).Length];

            if (!PlayerPrefs.HasKey("Master Volume")) PlayerPrefs.SetFloat("Master Volume", 1f);
            m_volume[0] = PlayerPrefs.GetFloat("Master Volume");

            AudioListener.volume = m_volume[0];

            for (int i = 1; i < Enum.GetNames(typeof(SoundTag)).Length; i++)
            {
                if (!PlayerPrefs.HasKey(Enum.GetNames(typeof(SoundTag))[i] + " Volume")) PlayerPrefs.SetFloat(Enum.GetNames(typeof(SoundTag))[i] + " Volume", 1f);
                m_volume[i] = PlayerPrefs.GetFloat(Enum.GetNames(typeof(SoundTag))[i ] + " Volume");
            }
        }

        public void VolumeAdjust(float value, int volumeTag)
        {
            m_volume[volumeTag] = value;

            if (volumeTag == 0)
            {
                PlayerPrefs.SetFloat("Master Volume", value);
                AudioListener.volume = m_volume[0];
            }
            else
            {
                PlayerPrefs.SetFloat(Enum.GetNames(typeof(SoundTag))[volumeTag] + " Volume", value);
                AdjustAllSound(volumeTag);
            }
        }
        /// <summary>
        /// Finds all audio sources and adjusts volume based on tag
        /// </summary>
        public void AdjustAllSound(int volumeTag)
        {
            foreach(AudioSource src in FindObjectsOfType<AudioSource>())
            {
                if (src.GetComponent<SoundID>())
                {
                    if((int)src.GetComponent<SoundID>().SoundTag == volumeTag)
                    {
                        float indMult = 1f;

                        indMult = src.GetComponent<SoundID>().SoundTag switch
                        {
                            SoundTag.None =>
                                indMult = SoundManager.None.GetData().GetGroup(src.GetComponent<SoundID>().Group).GetSoundMult(),
                            SoundTag.SFX =>
                                indMult = SoundManager.SFX.GetData().GetGroup(src.GetComponent<SoundID>().Group).GetSoundMult(),
                            SoundTag.Music =>
                                indMult = SoundManager.Music.GetData().GetGroup(src.GetComponent<SoundID>().Group).GetSoundMult(),
                            SoundTag.UI =>
                                indMult = SoundManager.UI.GetData().GetGroup(src.GetComponent<SoundID>().Group).GetSoundMult(),
                            SoundTag.Environment =>
                                indMult = SoundManager.Environment.GetData().GetGroup(src.GetComponent<SoundID>().Group).GetSoundMult(),
                            _ => throw new NotImplementedException(),
                        };

                        src.volume = PlayerPrefs.GetFloat(Enum.GetNames(typeof(SoundTag))[volumeTag] + " Volume", 1f) * indMult;
                    }
                }
            }
        }
    }

}

