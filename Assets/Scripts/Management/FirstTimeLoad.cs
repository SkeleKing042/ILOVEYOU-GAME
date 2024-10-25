using ILOVEYOU.UI;
using UnityEngine;

namespace ILOVEYOU.Management
{
    public class FirstTimeLoad : MonoBehaviour
    {
        [Header("Defaults")]
        public float m_volume;
        public Color m_important;
        [Tooltip("Buff, Debuff, Hazard, Summon")]
        public Color[] m_cardColors = new Color[4];
        // Start is called before the first frame update
        void Start()
        {
            //First time the game has been loaded.
            if(!PlayerPrefs.HasKey("IsFirstLoad") || PlayerPrefs.GetInt("IsFirstLoad") == 1)
            {
                _firstLoad();
                PlayerPrefs.SetInt("IsFirstLoad", 0);
            } 
       }
       private void _firstLoad()
       {
            Debug.Log("Loading default settings");
            PlayerPrefs.SetFloat("Volume", m_volume);
            ColorPref.Set("Important Color", m_important);
            ColorPref.Set("Buff color", m_cardColors[0]);
            ColorPref.Set("Debuff color", m_cardColors[1]);
            ColorPref.Set("Hazard color", m_cardColors[2]);
            ColorPref.Set("Summon color", m_cardColors[3]);
       }
    }
}