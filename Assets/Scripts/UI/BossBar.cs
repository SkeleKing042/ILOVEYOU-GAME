using ILOVEYOU.EnemySystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ILOVEYOU
{
    namespace UI 
    {
        public class BossBar : MonoBehaviour
        {
            public static BossBar[] Instances = new BossBar[2];
            private int m_index = 0;
            private bool m_active = false;
            private float m_timer = 0f;

            [SerializeField] private Slider m_lag;
            [SerializeField] private Slider m_current;

            private void Update()
            {
                if (m_active)
                {
                    m_timer -= Time.deltaTime;

                    if (m_timer <= 0f)
                    {
                        m_lag.value = Mathf.MoveTowards(m_lag.value, m_current.value, Time.deltaTime * m_current.maxValue / 2f);
                    }
                }
            }

            public void Initialize(int index)
            {
                Instances[index] = this;

                if (!File.Exists($".\\names.txt"))
                {
                    //create default config
                    TextWriter tw = new StreamWriter($".\\names.txt", false);
                    tw.WriteLine("Put Names Here:");;
                    tw.Close();
                }
            }

            public void InitializeHealthBar(string text, float maxHealth)
            {
                CancelInvoke();
                StopAllCoroutines();
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = text;

                m_lag.maxValue = maxHealth;
                m_lag.value = 0f;
                m_current.maxValue = maxHealth;
                m_current.value = 0f;

                //m_active = true;
                StartCoroutine(_StartAnim());
            }


            private IEnumerator _StartAnim()
            {
                while (m_current.value != m_current.maxValue)
                {
                    m_current.value = Mathf.MoveTowards(m_current.value, m_current.maxValue, Time.deltaTime * m_current.maxValue);
                    //m_current.value = Mathf.Round(m_current.value);

                    yield return new WaitForEndOfFrame();
                }

                m_lag.value = m_lag.maxValue;

                m_active = true;

                yield return null;
            }

            public void UpdateHealthBar(float value)
            {
                if (m_active)
                {
                    m_timer = 0.3f;
                    m_current.value = value;

                    if (value <= 0f)
                    {
                        Invoke(nameof(_CloseHealthBar), 0.5f);
                    }
                }
            }

            private void _CloseHealthBar()
            {
                m_active = false;
                transform.GetChild(0).gameObject.SetActive(false);
            }


            private string GenerateName()
            {
                StreamReader test = new StreamReader("");

                switch(Random.Range(0, 2)) 
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                }

                return "ERROR";
            }
        }
    }

}


