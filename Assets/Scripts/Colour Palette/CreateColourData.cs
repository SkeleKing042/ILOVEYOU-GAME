using ILOVEYOU.UI;
using UnityEngine;

namespace ILOVEYOU.Colour
{
    public class CreateColourData : MonoBehaviour
    {

        [SerializeField] private Color[] m_default = new Color[6];

        // Start is called before the first frame update
        void Start()
        {
            if(!PlayerPrefs.HasKey("Important Color0 R"))
            {
                ColorPref.Set("Important Color0", m_default[0]);
                ColorPref.Set("Important Color1", m_default[0]);

                ColorPref.Set("Buff color0", m_default[1]);
                ColorPref.Set("Buff color1", m_default[1]);

                ColorPref.Set("Debuff color0", m_default[2]);
                ColorPref.Set("Debuff color1", m_default[2]);

                ColorPref.Set("Hazard color0", m_default[3]);
                ColorPref.Set("Hazard color1", m_default[3]);

                ColorPref.Set("Summon color0", m_default[4]);
                ColorPref.Set("Summon color1", m_default[4]);

                ColorPref.Set("Background Color0", m_default[5]);
                ColorPref.Set("Background Color1", m_default[5]);
            }
        }


    }
}

