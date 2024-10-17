using ILOVEYOU.Management;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ILOVEYOU
{
    namespace UI
    {
        public class GameUI : MonoBehaviour
        {
            [SerializeField] private Color m_importantColour;

            [Header("In-Game Menu")]
            [SerializeField] private GameObject m_InGameSharedUI;
            [SerializeField] private TextMeshProUGUI m_timerText;

            [Header("Victory Menu")]
            [SerializeField] private GameObject m_winScreen;
            [SerializeField] private TextMeshProUGUI m_winText;
            [SerializeField] private Button m_restartButton;
            // Start is called before the first frame update
            public void Start()
            {
                ColorPref.Set("Important Color", m_importantColour);
            }
            public void DisplayWinScreen(int winnerID)
            {
                m_winScreen.SetActive(true);
                m_winText.text = $"Player {winnerID} wins!\nScore: {GameManager.GetScore.x} - {GameManager.GetScore.y}";
                EventSystem.current.SetSelectedGameObject(m_restartButton.gameObject);
            }
            public void UpdateTimer(float currentTime)
            {
                Color timeColour = Color.white - new Color(1-m_importantColour.r, 1-m_importantColour.g, 1-m_importantColour.b) * Mathf.Clamp(GameManager.Instance.PercentToMaxDiff, 0, 1);
                m_timerText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(timeColour)}>{(int)currentTime}</color>";
            }
        }
    }
}
