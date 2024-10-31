using ILOVEYOU.Audio;
using System.Collections;
using UnityEngine;

namespace ILOVEYOU.MainMenu
{
    public class MainMenuAudio : MonoBehaviour
    {

        public static MainMenuAudio Instance;

        private AudioSource[] m_audioSource = new AudioSource[2];
        [SerializeField] private AudioClip[] m_clips;
        [SerializeField] private bool m_skip = false;

        public bool IsPlaying { get { return m_audioSource[1].isPlaying; } }

        // Start is called before the first frame update
        void Awake()
        {

            if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
            else { Destroy(gameObject); }

            m_audioSource[0] = gameObject.AddComponent<AudioSource>();
            m_audioSource[0].clip = m_clips[0];
            m_audioSource[0].volume = SoundManager.Environment.GetVolume();

            m_audioSource[1] = gameObject.AddComponent<AudioSource>();
            m_audioSource[1].clip = m_clips[1];
            m_audioSource[1].volume = SoundManager.Environment.GetVolume();
            m_audioSource[1].loop = true;

            if (m_skip)
            {
                m_audioSource[1].Play();
            }

        }

        public IEnumerator AudioSequence()
        {
            yield return new WaitForSeconds(1f);
            m_audioSource[0].Play();

            yield return new WaitForSeconds(15.20f);

            m_audioSource[1].Play();
        }

        public void Beep()
        {
            AudioSource temp = gameObject.AddComponent<AudioSource>();
            temp.PlayOneShot(m_clips[2], SoundManager.Environment.GetVolume());
            Destroy(temp, m_clips[2].length);
        }

        public void Skip()
        {
            Instance = null;
            Destroy(gameObject);

        }
    }

}

