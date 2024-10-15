using ILOVEYOU.Cards;
using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

namespace ILOVEYOU
{
    namespace UI
    {
        //ui
        public class PlayerUI : MonoBehaviour
        {
            [SerializeField] private bool m_debugging;
            [Header("Card UI")]
            [SerializeField] private PopUps m_blindBox;
            [SerializeField] private CardDisplay m_cardDisplay;
            public CardDisplay GetCardDisplay => m_cardDisplay;
            public PopUps GetBlindBox => m_blindBox;
            [Header("World Space")]
            [SerializeField] private PointerArrow m_pointer;
            public PointerArrow GetPointer { get { return m_pointer; } }
            [Header("HUD elements")]
            [SerializeField] private Transform m_mirroredUIContainer;
            [SerializeField] private Slider m_healthSlider;
            [SerializeField] private Image m_healthFill;
            [SerializeField] private EventLogUI m_eventLog;
            public EventLogUI GetLog { get { return m_eventLog; } }
            [Header("Health Flash settings")]
            [SerializeField] private float m_flashTick;
            [SerializeField] private float m_flashReductionRate;
            [SerializeField] private float m_sizeBurst;
            [SerializeField] private float m_sizeReductionRate;
            private bool m_eStop;

            // Start is called before the first frame update
            public bool Startup(int id)
            {
                if (m_debugging) Debug.Log($"Starting player UI");

                if (m_debugging) Debug.Log("Setting up point tracker");
                if (m_pointer != null)
                {
                    m_pointer.gameObject.SetActive(false);
                }

                //UI setup
                //flip hud if odd - needs tweaking
                if (id % 2 == 1)
                {
                    m_mirroredUIContainer.localScale = new(-1, 1, 1);
                }
                m_blindBox.Initialize();
                m_cardDisplay.gameObject.SetActive(false);

                //bosshud setup
                transform.GetComponentInChildren<BossBar>().Initialize(id);

                if (m_debugging) Debug.Log($"Player UI started successfully!");
                return true;
            }
            public void UpdateHealthBar(float value)
            {
                m_eStop = true;
                StartCoroutine(FadeHealthBar());
                m_healthSlider.value = value;
            }
            private IEnumerator FadeHealthBar()
            {
                m_eStop = false;

                Color current = m_healthFill.color;
                m_healthFill.color = Color.white;
                Color flipped = new Color(1 - current.r, 1 - current.g, 1 - current.b, 0);

                float currentPosition = m_healthFill.rectTransform.offsetMin.x;
                m_healthFill.rectTransform.offsetMin += new Vector2(m_sizeBurst, 0);
                while (true)
                {
                    if(m_eStop)
                    {
                        m_healthFill.color = current;
                        m_healthFill.rectTransform.offsetMin = new Vector2(currentPosition, m_healthFill.rectTransform.offsetMin.y);
                        break;
                    }
                    bool doBreak = true;
                    if (m_healthFill.color != current)
                    {
                        doBreak = false;
                        m_healthFill.color -= flipped * m_flashReductionRate;
                    }
                    if (m_healthFill.rectTransform.offsetMin.x != currentPosition)
                    {
                        doBreak = false;
                        m_healthFill.rectTransform.offsetMin -= new Vector2(m_sizeReductionRate, 0);
                    }
                    if (doBreak)
                    {
                        break;
                    }
                    yield return new WaitForSeconds(m_flashTick);
                }
            }
        }
    }
}