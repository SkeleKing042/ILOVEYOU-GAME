using ILOVEYOU.Management;
using System;
using System.Collections;
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

            [Header("Announcement Settings")]
            [SerializeField] private GameObject m_announcementBox;
            [SerializeField] private float m_announcementDuration;

            [Header("In-Game Menu")]
            [SerializeField] private GameObject m_InGameSharedUI;
            [SerializeField] private TextMeshProUGUI m_timerText;

            [Header("Victory Menu")]
            [SerializeField] private GameObject m_winScreen;
            [SerializeField] private TextMeshProUGUI m_winText;
            [SerializeField] private Button m_restartButton;

            //[TextArea][SerializeField] private string m_onePlayerText;
            //[TextArea][SerializeField] private string m_twoPlayerText;


            // Start is called before the first frame update
            public void Start()
            {
                m_importantColor = ColorPref.Get("Important Color");
            }
            public void DisplayWinScreen(int winnerID)
            {
                m_winScreen.SetActive(true);
                //m_winText.text = $"Player {winnerID} wins!\nScore: {GameManager.GetScore.x} - {GameManager.GetScore.y}";
                EventSystem.current.SetSelectedGameObject(m_restartButton.gameObject);

                //chooses what to display based on how many players there are 
                if (GameManager.Instance.NumberOfPlayers == 1)
                {
                    m_winText.text = $"You survived for {GameManager.Instance.GetCurrentDifficulty} seconds!";
                }
                else
                {
                    m_winText.text = $"Player {winnerID} wins!\nScore: {GameManager.GetScore.x} - {GameManager.GetScore.y}";
                }
            }
            public void UpdateTimer(float currentTime)
            {
                Color timeColour = Color.white - new Color(1- m_importantColor.r, 1- m_importantColor.g, 1- m_importantColor.b) * Mathf.Clamp(GameManager.Instance.PercentToMaxDiff, 0, 1);
                m_timerText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(timeColour)}>{(int)currentTime}</color>";
            }

            public IEnumerator PlayAnnouncement()
            {
                if (m_announcementBox && GameSettings.Current.GetAnouncement != "")
                {
                    m_announcementBox.GetComponentInChildren<TextMeshProUGUI>().text = $"{GameSettings.Current.GetAnouncement}";
                    m_announcementBox.SetActive(true);
                    yield return new WaitForSecondsRealtime(m_announcementDuration);
                    m_announcementBox.SetActive(false);
                }
            }
        }
    }
}
