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

        [System.Serializable]
        public class SoundList
        {
            [SerializeField] private string m_name;
            [SerializeField] [Range(0f,1f)] private float m_soundMult = 1f;
            [SerializeField] private AudioClip[] m_clips;

            public AudioClip[] GetSounds() { return m_clips; }
            public string GetName() { return m_name; }
            public float GetSoundMult() { return m_soundMult; }

        }

        [SerializeField] private SoundList[] m_sounds;
        [SerializeField] private SoundTag m_tag;
        public SoundTag Tag { get { return m_tag; } }
        
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

                rando = Random.Range(0, m_sounds[group].GetSounds().Length);

                loopingSource.Stop();
                loopingSource.clip = m_sounds[group].GetSounds()[rando];
                loopingSource.volume = GetVolume() * m_sounds[group].GetSoundMult();
                loopingSource.Play();

                return;
            }
            
            loopingSource = gameObject.AddComponent<AudioSource>(); 

            rando = Random.Range(0, m_sounds[group].GetSounds().Length);

            loopingSource.clip = m_sounds[group].GetSounds()[rando];
            loopingSource.volume = GetVolume() * m_sounds[group].GetSoundMult();
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

                loopingSource.Stop();
                loopingSource.clip = m_sounds[group].GetSounds()[sound];
                loopingSource.volume = GetVolume() * m_sounds[group].GetSoundMult();
                loopingSource.Play();

                return;
            }


            loopingSource = gameObject.AddComponent<AudioSource>();

            loopingSource.clip = m_sounds[group].GetSounds()[sound];
            loopingSource.volume = GetVolume() * m_sounds[group].GetSoundMult();
            loopingSource.loop = true;

            loopingSource.Play(); 
        }

        /// <summary>
        /// plays random sound from group as a one shot
        /// </summary>
        public void PlayRandomSound(int group)
        {
            AudioSource oneShotSource = new GameObject("OneShotObject: " + m_sounds[group].GetName()).AddComponent<AudioSource>(); //woahhh oneshot reference!!!

            int rando = Random.Range(0, m_sounds[group].GetSounds().Length);

            oneShotSource.PlayOneShot(m_sounds[group].GetSounds()[rando], GetVolume() * m_sounds[group].GetSoundMult()); //play it right now
            Destroy(oneShotSource.gameObject, m_sounds[group].GetSounds()[rando].length);
        }
        /// <summary>
        /// plays selected sound from group as a one shot
        /// </summary>
        public void PlaySound(int group, int sound)
        {
            AudioSource oneShotSource = new GameObject("OneShotObject: " + m_sounds[group].GetName()).AddComponent<AudioSource>(); //woahhh oneshot reference!!!

            oneShotSource.PlayOneShot(m_sounds[group].GetSounds()[sound], GetVolume() * m_sounds[group].GetSoundMult()); //play it right now
            Destroy(oneShotSource.gameObject, m_sounds[group].GetSounds()[sound].length);
        }

        public float GetVolume()
        {
            return PlayerPrefs.GetFloat(Enum.GetName(typeof(SoundTag), m_tag) + " Volume", 1f);
        }
    }

}

