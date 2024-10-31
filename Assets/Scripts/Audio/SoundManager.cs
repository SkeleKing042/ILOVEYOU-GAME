using System;
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
            [SerializeField][HideInInspector] private SoundTag m_tag = SoundTag.None;
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
            /// <summary>
            /// Gets group based on name
            /// </summary>
            public SoundGroup GetGroup(string name, out int pos)
            {
                SoundGroup group = null;
                pos = 0;

                for (int i = 0; i < m_sounds.Length; i++)
                {
                    if (m_sounds[i].GetName() == name)
                    {
                        pos = i;
                        group = m_sounds[i];
                    }

                    break;
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
            public int[] PreviousAudio { get { return m_prevAudio; } set { } }

        }

        [SerializeField] private SoundManagerData m_soundData = new();
        
        public void SetData(SoundManagerData data)
        {
            m_soundData = data;
        }

        #region Play Random Sound Loop
        /// <summary>
        /// plays random looping sound from group
        /// </summary>
        public void PlayRandomSoundLoop(int group)
        {
            GameObject obj = new("LoopObject: " + m_soundData.GetGroup(group).GetName());
            obj.transform.parent = transform;
            AudioSource loopingSource;
            int rando;

            loopingSource = obj.AddComponent<AudioSource>(); 
            obj.AddComponent<SoundID>().Initialize(m_soundData.Tag, -1); //adds id to object  

            rando = Random.Range(0, m_soundData.GetGroup(group).GetSounds().Length);

            if (m_soundData.ManualLoad)
            {
                _UnloadAudio();
                _LoadAudio(group, rando);
            }

            loopingSource.clip = m_soundData.GetGroup(group).GetSounds()[rando];
            loopingSource.volume = GetVolume() * m_soundData.GetGroup(group).GetSoundMult();
            loopingSource.loop = true;
            loopingSource.Play();
        }
        /// <summary>
        /// plays random looping sound from group
        /// </summary>
        /// <param name="soundID">ID will determine whether a sound will be overridden if a new one plays with the same ID. If an ID is -1 it will be ignored</param>
        public void PlayRandomSoundLoop(int group, int soundID)
        {
            
            AudioSource loopingSource;
            int rando;

            if (CheckID(soundID) > -1)
            {
                loopingSource = transform.GetChild(CheckID(soundID)).GetComponent<AudioSource>();

            }
            else
            {
                GameObject obj = new("LoopObject: " + m_soundData.GetGroup(group).GetName());
                obj.transform.parent = transform;

                loopingSource = obj.AddComponent<AudioSource>();
                obj.AddComponent<SoundID>().Initialize(m_soundData.Tag, soundID); //adds id to object  
            }
            

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
        /// plays random looping sound from group
        /// </summary>
        public void PlayRandomSoundLoop(string group)
        {
            GameObject obj = new("LoopObject: " + m_soundData.GetGroup(group).GetName());
            obj.transform.parent = transform;
            AudioSource loopingSource;
            int rando;

            loopingSource = obj.AddComponent<AudioSource>();
            obj.AddComponent<SoundID>().Initialize(m_soundData.Tag, -1); //adds id to object  


            rando = Random.Range(0, m_soundData.GetGroup(group).GetSounds().Length);

            if (m_soundData.ManualLoad)
            {
                _UnloadAudio();
                _LoadAudio(group, rando);
            }

            loopingSource.clip = m_soundData.GetGroup(group).GetSounds()[rando];
            loopingSource.volume = GetVolume() * m_soundData.GetGroup(group).GetSoundMult();
            loopingSource.loop = true;
            loopingSource.Play();
        }
        /// <summary>
        /// plays random looping sound from group
        /// </summary>
        /// <param name="soundID">ID will determine whether a sound will be overridden if a new one plays with the same ID. If an ID is -1 it will be ignored</param>
        public void PlayRandomSoundLoop(string group, int soundID)
        {
            
            AudioSource loopingSource;
            int rando;

            if (CheckID(soundID) > -1)
            {
                loopingSource = transform.GetChild(CheckID(soundID)).GetComponent<AudioSource>();

            }
            else
            {
                GameObject obj = new("LoopObject: " + m_soundData.GetGroup(group).GetName());
                obj.transform.parent = transform;

                loopingSource = obj.AddComponent<AudioSource>();
                obj.AddComponent<SoundID>().Initialize(m_soundData.Tag, soundID); //adds id to object  
            }


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
        #endregion
        #region Play Sound Loop
        /// <summary>
        /// plays selected looping sound from group
        /// </summary>
        public void PlaySoundLoop(int group, int sound)
        {
            GameObject obj = new("LoopObject: " + m_soundData.GetGroup(group).GetName());
            obj.transform.parent = transform;
            AudioSource loopingSource;

            

            loopingSource = obj.AddComponent<AudioSource>();
            obj.AddComponent<SoundID>().Initialize(m_soundData.Tag, -1); //adds id to object  

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
        /// plays selected looping sound from group
        /// </summary>
        /// <param name="soundID">ID will determine whether a sound will be overridden if a new one plays with the same ID. If an ID is -1 it will be ignored</param>
        public void PlaySoundLoop(int group, int sound, int soundID)
        {
            
            AudioSource loopingSource;

            if (CheckID(soundID) > -1)
            {
                loopingSource = transform.GetChild(CheckID(soundID)).GetComponent<AudioSource>();
            }
            else
            {
                GameObject obj = new("LoopObject: " + m_soundData.GetGroup(group).GetName());
                obj.transform.parent = transform;

                loopingSource = obj.AddComponent<AudioSource>();
                obj.AddComponent<SoundID>().Initialize(m_soundData.Tag, soundID); //adds id to object  
            }

            if (m_soundData.ManualLoad)
            {
                _UnloadAudio();
                _LoadAudio(group, sound);
            }

            loopingSource.Stop();
            loopingSource.clip = m_soundData.GetGroup(group).GetSounds()[sound];
            loopingSource.volume = GetVolume() * m_soundData.GetGroup(group).GetSoundMult();
            loopingSource.loop = true;
            loopingSource.Play();
        }
        /// <summary>
        /// plays selected looping sound from group
        /// </summary>
        public void PlaySoundLoop(string group, int sound)
        {
            GameObject obj = new("LoopObject: " + m_soundData.GetGroup(group).GetName());
            obj.transform.parent = transform;
            AudioSource loopingSource;



            loopingSource = obj.AddComponent<AudioSource>();
            obj.AddComponent<SoundID>().Initialize(m_soundData.Tag, -1); //adds id to object  

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
        /// plays selected looping sound from group
        /// </summary>
        /// <param name="soundID">ID will determine whether a sound will be overridden if a new one plays with the same ID. If an ID is -1 it will be ignored</param>
        public void PlaySoundLoop(string group, int sound, int soundID)
        {

            AudioSource loopingSource;

            if (CheckID(soundID) > -1)
            {
                loopingSource = transform.GetChild(CheckID(soundID)).GetComponent<AudioSource>();
            }
            else
            {
                GameObject obj = new("LoopObject: " + m_soundData.GetGroup(group).GetName());
                obj.transform.parent = transform;

                loopingSource = obj.AddComponent<AudioSource>();
                obj.AddComponent<SoundID>().Initialize(m_soundData.Tag, soundID); //adds id to object  
            }

            if (m_soundData.ManualLoad)
            {
                _UnloadAudio();
                _LoadAudio(group, sound);
            }

            loopingSource.Stop();
            loopingSource.clip = m_soundData.GetGroup(group).GetSounds()[sound];
            loopingSource.volume = GetVolume() * m_soundData.GetGroup(group).GetSoundMult();
            loopingSource.loop = true;
            loopingSource.Play();
        }
        #endregion
        #region Play Random Sound
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
        /// plays random sound from group as a one shot
        /// </summary>
        public void PlayRandomSound(string group)
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
        #endregion
        #region Play Sound
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
        /// <summary>
        /// plays selected sound from group as a one shot
        /// </summary>
        public void PlaySound(string group, int sound)
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
        #endregion

        public int CheckID(int ID)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i) == this) continue;

                if (transform.GetChild(i).GetComponent<SoundID>().ID == -1) continue;

                if (transform.GetChild(i).GetComponent<SoundID>().SoundTag == m_soundData.Tag && transform.GetChild(i).GetComponent<SoundID>().ID == ID)
                {
                    return i;
                }
            }

            return -1;
        }

        public float GetVolume()
        {
            return PlayerPrefs.GetFloat(Enum.GetName(typeof(SoundTag), m_soundData.Tag) + " Volume", 1f);
        }

        private void _LoadAudio(int group, int sound)
        {
            m_soundData.GetGroup(group).GetSounds()[sound].LoadAudioData();
            m_soundData.PreviousAudio = new int[2] {group, sound};

        }
        private void _LoadAudio(string group, int sound)
        {
            m_soundData.GetGroup(group, out int pos).GetSounds()[sound].LoadAudioData();
            m_soundData.PreviousAudio = new int[2] { pos, sound};

        }

        private void _UnloadAudio()
        {
            m_soundData.GetGroup(m_soundData.PreviousAudio[0]).GetSounds()[m_soundData.PreviousAudio[1]].UnloadAudioData();
        }
    }

}

