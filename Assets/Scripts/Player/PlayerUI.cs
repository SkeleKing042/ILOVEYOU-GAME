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
            [Header("Card UI")]
            [SerializeField] private PopUps m_blindBox;
            public PopUps GetBlindBox => m_blindBox;
            [SerializeField] private CardDisplay m_cardDisplay;
            public CardDisplay GetCardDisplay => m_cardDisplay;
            [SerializeField] private CardVignette m_cardVin;
            public CardVignette GetCardVin => m_cardVin;
            [Header("World Space")]
            [SerializeField] private PointerArrow m_pointer;
            public PointerArrow GetPointer { get { return m_pointer; } }
            [Header("HUD elements")]
            [SerializeField] private Transform[] m_mirroredUIElements;
            [SerializeField] private EventLogUI m_eventLog;
            public EventLogUI GetLog { get { return m_eventLog; } }
            [Header("Health")]
            [SerializeField] private Slider m_healthSlider;
            [SerializeField] private Image m_healthFill;
            private Color m_initialHealthBarColor;
            private float m_initialHealthSize = -1;
            [SerializeField] private float m_flashTick;
            [SerializeField] private float m_flashReductionRate;
            [SerializeField] private float m_sizeBurst;
            [SerializeField] private float m_sizeReductionRate;

            // Start is called before the first frame update
            public bool Startup(int id)
            {
                Debug.Log($"Starting player UI");

                Debug.Log("Setting up point tracker");
                if (m_pointer != null)
                {
                    m_pointer.gameObject.SetActive(false);
                }

                //UI setup
                //flip hud if odd - needs tweaking
                if (id % 2 == 1)
                {
                    foreach (var element in m_mirroredUIElements)
                        element.localScale = new(-1, 1, 1);
                }
                m_blindBox.Initialize();
                m_cardDisplay.gameObject.SetActive(false);

                //bosshud setup
                transform.GetComponentInChildren<BossBar>().Initialize(id);

                Debug.Log($"Player UI started successfully!");
                return true;
            }
            public void UpdateHealthBar(float value, Color flashColor)
            {
                StopAllCoroutines();
                if(m_initialHealthSize == -1)
                {
                    m_initialHealthBarColor = m_healthFill.color;
                    m_initialHealthSize = m_healthFill.rectTransform.offsetMin.x;
                }
                StartCoroutine(FadeHealthBar(flashColor));
                m_healthSlider.value = value;
            }
            private IEnumerator FadeHealthBar(Color flashColor)
            {
                m_healthFill.color = flashColor;
                //Color flipped = new Color(1 - m_initialHealthBarColor.r, 1 - m_initialHealthBarColor.g, 1 - m_initialHealthBarColor.b, 0);

                m_healthFill.rectTransform.offsetMin = new Vector2(m_sizeBurst + m_initialHealthSize, 0);
                int iteration = 0;
                while (true)
                {
                    /*if(m_eStop)
                    {
                        m_healthFill.color = originalColour;
                        m_healthFill.rectTransform.offsetMin = new Vector2(m_initialHealthSize, m_healthFill.rectTransform.offsetMin.y);
                        break;
                    }*/
                    bool doBreak = true;
                    if (m_healthFill.color != m_initialHealthBarColor)
                    {
                        doBreak = false;
                        Vector3 fillColor = new(m_healthFill.color.r, m_healthFill.color.g, m_healthFill.color.b);
                        Vector3 targetColor = new(m_initialHealthBarColor.r, m_initialHealthBarColor.g, m_initialHealthBarColor.b);
                        Vector3 lerpedVec = Vector3.Lerp(fillColor, targetColor, iteration * m_flashTick);
                        Color LerpedColor = new(lerpedVec.x, lerpedVec.y, lerpedVec.z);
                        m_healthFill.color = LerpedColor;
                    }
                    if (m_healthFill.rectTransform.offsetMin.x != m_initialHealthSize)
                    {
                        doBreak = false;
                        m_healthFill.rectTransform.offsetMin -= new Vector2(m_sizeReductionRate, 0);
                    }
                    if (doBreak)
                    {
                        break;
                    }
                    iteration++;
                    yield return new WaitForSeconds(m_flashTick);
                }
            }
        }
    }
}