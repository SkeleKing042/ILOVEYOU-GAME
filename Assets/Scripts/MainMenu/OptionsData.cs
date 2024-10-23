using UnityEngine;

namespace ILOVEYOU.MainMenu
{
    public class OptionsData : MonoBehaviour
    {
        public static OptionsData Instance;

        [SerializeField] private float m_volume;

        public float Volume { get { return m_volume; } }

        private void Awake()
        {

            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            if (!PlayerPrefs.HasKey("Volume")) PlayerPrefs.SetFloat("Volume", 1f);
            m_volume = PlayerPrefs.GetFloat("Volume");

            AudioListener.volume = Volume;
        }

        public void VolumeAdjust(float value)
        {
            m_volume = value;
            PlayerPrefs.SetFloat("Volume", value);
        }
    }

}

