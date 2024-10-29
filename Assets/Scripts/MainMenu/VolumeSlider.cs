using ILOVEYOU.Audio;
using ILOVEYOU.Management;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace ILOVEYOU.MainMenu
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private SoundTag m_soundTag;

        void Start()
        {
            GetComponent<Slider>().SetValueWithoutNotify(OptionsData.Instance.Volume[(int)m_soundTag] * GetComponent<Slider>().maxValue);
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(GetComponent<Slider>().value / GetComponent<Slider>().maxValue * 100f) + "%";
        }

        public void UpdateVolume(float value)
        {
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(value / GetComponent<Slider>().maxValue * 100f) + "%";
            OptionsData.Instance.VolumeAdjust(value / GetComponent<Slider>().maxValue, (int)m_soundTag);
            
        }

    }
}


