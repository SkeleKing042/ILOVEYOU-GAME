using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU.Audio
{
    [RequireComponent(typeof(SoundManager))]
    public class SoundManagerEmitter : MonoBehaviour
    {
        
        [SerializeField] private int m_group = 0;
        [SerializeField] private bool m_loop = true;
        [SerializeField] private bool m_changeAfterLoop = false;
        [SerializeField] private bool m_random = true;
        [SerializeField] private int m_soundToSelect = 0;

        // Start is called before the first frame update
        private void Start()
        {
            enabled = false;

            if (m_random)
            {
                if (m_loop) 
                { 
                    //GetComponent<SoundManager>().PlayRandomSoundLoop(m_group);
                    
                }
                //else GetComponent<SoundManager>().PlayRandomSound(m_group);
                
            }
            else
            {

                //if (m_loop) GetComponent<SoundManager>().PlaySoundLoop(m_group, m_soundToSelect);
                //else GetComponent<SoundManager>().PlaySound(m_group, m_soundToSelect);
            }

        }

        private void Update()
        {
            //debug shtuff ignore
            //if (Input.GetKeyDown(KeyCode.G))
            //{
            //    GetComponent<SoundManager>().PlayRandomSoundLoop(m_group);
            //}

            if (!GetComponent<AudioSource>().isPlaying)
            {

            }
        }
    }
}

