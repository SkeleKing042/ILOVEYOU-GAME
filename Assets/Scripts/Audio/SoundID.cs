using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU.Audio
{
    public class SoundID : MonoBehaviour
    {
        private SoundTag m_soundTag;
        private int m_id;
        
        public void Initialize(SoundTag soundTag, int id)
        {
            m_soundTag = soundTag;
            m_id = id;
        }

        public SoundTag SoundTag { get { return m_soundTag; } }
        public int ID { get { return m_id; } }
    }

}

