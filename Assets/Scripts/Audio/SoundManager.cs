using UnityEngine;

namespace ILOVEYOU.Audio
{
    public class SoundManager : MonoBehaviour
    {

        [System.Serializable]
        public class SoundList
        {
            [SerializeField] private string m_name;
            [SerializeField] private AudioClip[] m_clips;

            public AudioClip[] GetSounds() { return m_clips; }
            public string GetName() { return m_name; }

        }

        [SerializeField] private SoundList[] m_sounds;

        /// <summary>
        /// plays random sound from group
        /// </summary>
        public void PlayRandomSound(int group)
        {
            AudioSource oneShotSource = new GameObject("OneShotObject: " + m_sounds[group].GetName()).AddComponent<AudioSource>(); //woahhh oneshot reference!!!

            int rando = Random.Range(0, m_sounds[group].GetSounds().Length);

            oneShotSource.PlayOneShot(m_sounds[group].GetSounds()[rando]); //play it right now
            Destroy(oneShotSource.gameObject, m_sounds[group].GetSounds()[rando].length);
        }
    }

}

