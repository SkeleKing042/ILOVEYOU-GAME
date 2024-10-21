using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ILOVEYOU.MainMenu
{
    public class VolumeSlider : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Slider>().value = OptionsData.Instance.Volume * GetComponent<Slider>().maxValue;
        }

        public void UpdateVolume(float value)
        {
            OptionsData.Instance.VolumeAdjust(value / GetComponent<Slider>().maxValue);
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(value / GetComponent<Slider>().maxValue * 100f) + "%";
            //Debug.Log(value / GetComponent<Slider>().maxValue);

            //AudioListener listener = GetComponent<AudioListener>();
            AudioListener.volume = value / GetComponent<Slider>().maxValue;
        }

    }
}


