using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ILOVEYOU.UI
{
    public class EndScreenRandomText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_victoryText;
        [TextArea]
        [SerializeField] private string[] m_victoryStrings;
        [SerializeField] private TextMeshProUGUI m_lossText;
        [TextArea]
        [SerializeField] private string[] m_lossStrings;

        public void ApplyRandomVictory()
        {
            if(m_victoryStrings.Length > 0)
                m_victoryText.text = m_victoryStrings[Random.Range(0, m_victoryStrings.Length)];
        }
        public void ApplyRandomLoss()
        {
            if(m_lossStrings.Length > 0)
                m_lossText.text = m_lossStrings[Random.Range(0, m_lossStrings.Length)];
        }
    }
}