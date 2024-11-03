using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Audio
    {

        [RequireComponent(typeof(AudioSource))]
        public class RadoPitchShift : MonoBehaviour
        {
            [SerializeField] private Vector2 m_minMax;
            private AudioSource m_source;
            // Start is called before the first frame update
            void Start()
            {
                m_source = GetComponent<AudioSource>();
            }

            public void Play()
            {
                float rnd = Random.Range(m_minMax.x, m_minMax.y);
                m_source.pitch = rnd;
                m_source.Play();
            }
        }
    }
}