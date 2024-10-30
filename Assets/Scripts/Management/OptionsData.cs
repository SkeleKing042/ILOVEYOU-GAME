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
                if (src.GetComponent<SoundManager>())
                {
                    //if((int)src.GetComponent<SoundManager>().Tag == volumeTag)
                    //{
                    //    src.volume = PlayerPrefs.GetFloat(Enum.GetNames(typeof(SoundTag))[volumeTag] + " Volume", 1f);
                    //}
                }
            }
        }
    }

}

