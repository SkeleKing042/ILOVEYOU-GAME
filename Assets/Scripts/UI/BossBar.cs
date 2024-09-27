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

                _InitializeCustomText();
            }

            public void InitializeHealthBar(string text, float maxHealth)
            {
                CancelInvoke();
                StopAllCoroutines();
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = GenerateName();

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


            private void _InitializeCustomText()
            {
                if (!Directory.Exists($".\\CustomNames"))
                {
                    Directory.CreateDirectory($".\\CustomNames");
                }

                if (!File.Exists($".\\CustomNames\\names.txt"))
                {
                    TextWriter tw = new StreamWriter($".\\CustomNames\\names.txt", false);
                    tw.WriteLine("Put Names Here: (Line Break to seperate, this line isn't read)"); ;
                    tw.Close();
                }

                if (!File.Exists($".\\CustomNames\\titles.txt"))
                {
                    TextWriter tw = new StreamWriter($".\\CustomNames\\titles.txt", false);
                    tw.WriteLine("Put Titles Here: (Line Break to seperate, this line isn't read)"); ;
                    tw.Close();
                }

                if (!File.Exists($".\\CustomNames\\adjectives.txt"))
                {
                    TextWriter tw = new StreamWriter($".\\CustomNames\\adjectives.txt", false);
                    tw.WriteLine("Put Adjectives Here: (Line Break to seperate, this line isn't read)"); ;
                    tw.Close();
                }

                if (!File.Exists($".\\CustomNames\\places.txt"))
                {
                    TextWriter tw = new StreamWriter($".\\CustomNames\\places.txt", false);
                    tw.WriteLine("Put Places Here: (Line Break to seperate, this line isn't read)"); ;
                    tw.Close();
                }
            }

            private string GenerateName()
            {
                string[] names = File.ReadAllLines($".\\CustomNames\\names.txt");
                string[] titles = File.ReadAllLines($".\\CustomNames\\titles.txt");
                string[] adjectives = File.ReadAllLines($".\\CustomNames\\adjectives.txt");
                string[] places = File.ReadAllLines($".\\CustomNames\\places.txt");

                return Random.Range(0, 7) switch
                {
                    //NAME
                    0 => names[Random.Range(1, names.Length)],
                    //ADJCECTIVE NAME
                    1 => adjectives[Random.Range(1, adjectives.Length)] + " " + names[Random.Range(1, names.Length)],
                    //TITLE NAME
                    2 => titles[Random.Range(1, titles.Length)] + " " + names[Random.Range(1, names.Length)],
                    //TITLE NAME the ADJECTIVE
                    3 => titles[Random.Range(1, titles.Length)] + " " + names[Random.Range(1, names.Length)] + " the " + adjectives[Random.Range(1, adjectives.Length)],
                    //NAME of PLACE
                    4 => names[Random.Range(1, names.Length)] + " of " + places[Random.Range(1, places.Length)],
                    //TITLE NAME of PLACE
                    5 => titles[Random.Range(1, titles.Length)] + " " + names[Random.Range(1, names.Length)] + " of " + places[Random.Range(1, places.Length)],
                    //NAME THE ADJECTIVE
                    6 => names[Random.Range(1, names.Length)] + " the " + adjectives[Random.Range(1, adjectives.Length)],
                    _ => "ERROR",
                };
            }
        }
    }

}


