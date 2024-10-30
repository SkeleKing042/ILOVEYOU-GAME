using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ILOVEYOU.Audio.SoundManager;

namespace ILOVEYOU.Audio
{
    public class SoundManagerInitializer : MonoBehaviour
    {
        [SerializeField] private SoundManagerData[] m_data;

        private void Awake()
        {
            //checks if audio managers have already been initialized, if it is already initialized destroy the object
            if (None != null) //<- none is always initialized first so I thought that would be good to check
            {
                Destroy(gameObject);
                return;
            }

            for (int i = 0; i < m_data.Length; i++)
            {
                //creates new soundmanager
                SoundManager soundManager = gameObject.AddComponent<SoundManager>();
                //sets the data of the soundmanager based on what has been created on the initializer
                soundManager.SetData(m_data[i]);
                //Makes each soundmanager static
                switch (i)
                {
                    case 0:
                        None = soundManager;
                        break;
                    case 1:
                        SFX = soundManager;
                        break;
                    case 2:
                        Music = soundManager;
                        break;
                    case 3:
                        SoundManager.UI = soundManager;
                        break;
                    case 4:
                        SoundManager.Environment = soundManager;
                        break;
                    default:
                        Debug.LogError("Something Has Gone Wrong with assigning the static field!");
                        break;
                }
            }

            DontDestroyOnLoad(this);

            //This scritp/object is just for setting up the audio managers, so the script will destroy itself one it has initialized
            Destroy(this);
        }

        private void Reset()
        {
            m_data = new SoundManagerData[Enum.GetNames(typeof(SoundTag)).Length];

            for (int i = 0; i < m_data.Length; i++)
            {
                m_data[i] = new();
                m_data[i].SetTag((SoundTag)i);
            }
        }
    }
}
