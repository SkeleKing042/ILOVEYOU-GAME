using UnityEngine;

namespace ILOVEYOU.Audio
{
    public class SoundID : MonoBehaviour
    {
        private SoundTag m_soundTag;
        private int m_id;
        private string m_group;
        
        public void Initialize(SoundTag soundTag, int id, string group)
        {
            m_soundTag = soundTag;
            m_id = id;
            m_group = group;
        }

        public SoundTag SoundTag { get { return m_soundTag; } }
        public int ID { get { return m_id; } }
        public string Group { get { return m_group; } }
    }

}

