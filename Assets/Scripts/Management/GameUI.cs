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
            private Color m_importantColor;
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
                m_importantColor = ColorPref.Get("Important Color");
            }
            public void DisplayWinScreen(int winnerID)
            {
                m_winScreen.SetActive(true);
                m_winText.text = $"Player {winnerID} wins!\nScore: {GameManager.GetScore.x} - {GameManager.GetScore.y}";
                EventSystem.current.SetSelectedGameObject(m_restartButton.gameObject);
            }
            public void UpdateTimer(float currentTime)
            {
                Color timeColour = Color.white - new Color(1- m_importantColor.r, 1- m_importantColor.g, 1- m_importantColor.b) * Mathf.Clamp(GameManager.Instance.PercentToMaxDiff, 0, 1);
                m_timerText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(timeColour)}>{(int)currentTime}</color>";
            }
        }
    }
}
