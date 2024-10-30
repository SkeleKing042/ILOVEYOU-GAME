using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ILOVEYOU.Audio
{
    public enum SoundTag
    {
        None,
        SFX,
        Music,
        UI,
        Environment,
    }


    public class SoundManager : MonoBehaviour
    {
        public static SoundManager None;
        public static SoundManager SFX;
        public static SoundManager Music;
        public static SoundManager UI;
        public static SoundManager Environment;

        [Serializable]
        public class SoundManagerData
        {
            [Serializable]
            public class SoundGroup
            {
                [SerializeField] private string m_name;
                [SerializeField][Range(0f, 1f)] private float m_soundMult = 1f;
                [SerializeField] private AudioClip[] m_clips;

                public AudioClip[] GetSounds() { return m_clips; }
                public string GetName() { return m_name; }
                public float GetSoundMult() { return m_soundMult; }
            }

            [SerializeField] private SoundGroup[] m_sounds;
            [SerializeField] [HideInInspector] private SoundTag m_tag = SoundTag.None;
            [Tooltip("Sounds in this group are manually loaded and unloaded in when playing. Mainly targeted for music.")][SerializeField] private bool m_manualLoad = false;
            private int[] m_prevAudio = new int[2]; //only relevent to manual loading
            
            /// <summary>
            /// gets group at i index
            /// </summary>
            public SoundGroup GetGroup(int i)  
            { 
                return m_sounds[i];
            }
            /// <summary>
            /// Gets group based on name
            /// </summary>
            public SoundGroup GetGroup(string name)
            {
                SoundGroup group = null;

                for (int i = 0; i < m_sounds.Length; i++)
                {
                    if (m_sounds[i].GetName() == name)
                    {
                        group = m_sounds[i];
                    }
                }

                if (group == null) { Debug.LogWarning("Sound Group of Name: \"" + name + "\" Not found!"); }

                return group;
            }

            public bool ManualLoad { get { return m_manualLoad; } }

            public void SetTag(SoundTag tag)
            {
                m_tag = tag;
            }

            public SoundTag Tag
            {
                get { return m_tag; }
            }

        }

        [SerializeField] private SoundManagerData m_soundData = new();
        
        public void SetData(SoundManagerData data)
        {
            m_soundData = data;
        }


        /// <summary>
        /// plays random looping sound from group
        /// </summary>
        public void PlayRandomSoundLoop(int group)
        {
            AudioSource loopingSource;
            int rando;

            //if there is a loop already playing on this object, override it
            if (gameObject.GetComponent<AudioSource>())
            {
                loopingSource = gameObject.GetComponent<AudioSource>();

                rando = Random.Range(0, m_soundData.GetGroup(group).GetSounds().Length);

                if (m_soundData.ManualLoad) 
                { 
                    _UnloadAudio();
                    _LoadAudio(group, rando);
                }

                loopingSource.Stop();
                loopingSource.clip = m_soundData.GetGroup(group).GetSounds()[rando];
                loopingSource.volume = GetVolume() * m_soundData.GetGroup(group).GetSoundMult();
                loopingSource.Play();

                return;
            }

            loopingSource = gameObject.AddComponent<AudioSource>(); 

            rando = Random.Range(0, m_soundData.GetGroup(group).GetSounds().Length);

            if (m_soundData.ManualLoad)
            {
                _UnloadAudio();
                _LoadAudio(group, rando);
            }

            loopingSource.Stop();
            loopingSource.clip = m_soundData.GetGroup(group).GetSounds()[rando];
            loopingSource.volume = GetVolume() * m_soundData.GetGroup(group).GetSoundMult();
            loopingSource.loop = true;
            loopingSource.Play();
        }
        /// <summary>
        /// plays selected looping sound from group
        /// </summary>
        public void PlaySoundLoop(int group, int sound)
        {
            AudioSource loopingSource;

            //if there is a loop already playing on this object, override it
            if (gameObject.GetComponent<AudioSource>())
            {
                loopingSource = gameObject.GetComponent<AudioSource>();

                if (m_soundData.ManualLoad)
                {
                    _UnloadAudio();
                    _LoadAudio(group, sound);
                }

                loopingSource.Stop();
                loopingSource.clip = m_soundData.GetGroup(group).GetSounds()[sound];
                loopingSource.volume = GetVolume() * m_soundData.GetGroup(group).GetSoundMult();
                loopingSource.Play();

                return;
            }

            loopingSource = gameObject.AddComponent<AudioSource>();

            if (m_soundData.ManualLoad)
            {
                _UnloadAudio();
                _LoadAudio(group, sound);
            }

            loopingSource.clip = m_soundData.GetGroup(group).GetSounds()[sound];
            loopingSource.volume = GetVolume() * m_soundData.GetGroup(group).GetSoundMult();
            loopingSource.loop = true;

            loopingSource.Play(); 
        }

        /// <summary>
        /// plays random sound from group as a one shot
        /// </summary>
        public void PlayRandomSound(int group)
        {
            AudioSource oneShotSource = new GameObject("OneShotObject: " + m_soundData.GetGroup(group).GetName()).AddComponent<AudioSource>(); //woahhh oneshot reference!!!

            int rando = Random.Range(0, m_soundData.GetGroup(group).GetSounds().Length);

            if (m_soundData.ManualLoad)
            {
                _UnloadAudio();
                _LoadAudio(group, rando);
            }

            oneShotSource.PlayOneShot(m_soundData.GetGroup(group).GetSounds()[rando], GetVolume() * m_soundData.GetGroup(group).GetSoundMult()); //play it right now
            Destroy(oneShotSource.gameObject, m_soundData.GetGroup(group).GetSounds()[rando].length);
        }
        /// <summary>
        /// plays selected sound from group as a one shot
        /// </summary>
        public void PlaySound(int group, int sound)
        {
            AudioSource oneShotSource = new GameObject("OneShotObject: " + m_soundData.GetGroup(group).GetName()).AddComponent<AudioSource>(); //woahhh oneshot reference!!!

            if (m_soundData.ManualLoad)
            {
                _UnloadAudio();
                _LoadAudio(group, sound);
            }

            oneShotSource.PlayOneShot(m_soundData.GetGroup(group).GetSounds()[sound], GetVolume() * m_soundData.GetGroup(group).GetSoundMult()); //play it right now
            Destroy(oneShotSource.gameObject, m_soundData.GetGroup(group).GetSounds()[sound].length);
        }

        public float GetVolume()
        {
            return PlayerPrefs.GetFloat(Enum.GetName(typeof(SoundTag), m_soundData.Tag) + " Volume", 1f);
        }

        private void _LoadAudio(int group, int sound)
        {
            m_soundData.GetGroup(group).GetSounds()[sound].LoadAudioData();
            //m_prevAudio = new int[2] {group, sound};

        }

        private void _UnloadAudio()
        {
            //m_soundData[m_prevAudio[0]].GetSounds()[m_prevAudio[1]].UnloadAudioData();
        }
    }

}

