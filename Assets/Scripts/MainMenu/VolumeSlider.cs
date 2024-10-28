using ILOVEYOU.Audio;
using ILOVEYOU.Management;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ILOVEYOU.MainMenu
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private SoundTag m_soundTag;

        void Start()
        {
            GetComponent<Slider>().value = OptionsData.Instance.Volume[(int)m_soundTag] * GetComponent<Slider>().maxValue;
        }

        public void UpdateVolume(float value)
        {
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(value / GetComponent<Slider>().maxValue * 100f) + "%";
            OptionsData.Instance.VolumeAdjust(value / GetComponent<Slider>().maxValue, (int)m_soundTag);
            
            //Debug.Log(value / GetComponent<Slider>().maxValue);

            //AudioListener listener = GetComponent<AudioListener>();
            //AudioListener.volume = value / GetComponent<Slider>().maxValue;
        }

    }
}


