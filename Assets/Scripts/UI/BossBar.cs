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
            public static BossBar[] Instances = new BossBar[1];
            private bool m_active = false;
            private float m_timer = 0f;
            private float m_lagDamage = 0f;

            [SerializeField] private TextMeshProUGUI m_nameBox;
            [SerializeField] private TextMeshProUGUI m_damageNumBox;

            [SerializeField] private Slider m_lag;
            [SerializeField] private Slider m_current;

            private void Update()
            {
                //if the boss bar is currently active
                if (m_active)
                {
                    //decrease timer (delay before the lag values get reset) 
                    m_timer -= Time.deltaTime;
                    //if no damage has been delt in a amount of time
                    if (m_timer <= 0f)
                    {
                        m_lagDamage = 0f;
                        m_damageNumBox.gameObject.SetActive(false); //hides damage number
                        m_lag.value = Mathf.MoveTowards(m_lag.value, m_current.value, Time.deltaTime * m_current.maxValue / 2f); //moves the timer
                    }
                }
            }
            //sets instance
            public void Initialize(int index)
            {
                Instances[index] = this;

                if(index == 0) _InitializeCustomText();

                gameObject.SetActive(false);
            }

            public void InitializeHealthBar(float maxHealth)
            {
                //stops any potentially already running functions
                CancelInvoke();
                StopAllCoroutines();
                //activates object and generates name
                gameObject.SetActive(true);
                //transform.GetChild(0).gameObject.SetActive(true);
                if (!m_active) m_nameBox.text = GenerateName();
                //resets values
                m_lag.maxValue = maxHealth;
                m_lag.value = 0f;
                m_current.maxValue = maxHealth;
                m_current.value = 0f;
                //plays the bar filling animation
                StartCoroutine(_StartAnim());
            }

            /// <summary>
            /// Increases the bar until it is full
            /// </summary>
            private IEnumerator _StartAnim()
            {
                while (m_current.value != m_current.maxValue)
                {
                    m_current.value = Mathf.MoveTowards(m_current.value, m_current.maxValue, Time.deltaTime * m_current.maxValue);

                    yield return new WaitForEndOfFrame();
                }

                m_lag.value = m_lag.maxValue;

                m_active = true;

                yield return null;
            }

            public void UpdateHealthBar(float value)
            {
                //stops conflicts with the starting animation
                if (m_active)
                {
                    //shows the darksouls damage number and increases its damage
                    m_lagDamage += m_current.value - value;
                    m_damageNumBox.gameObject.SetActive(true);
                    m_damageNumBox.text = "" + m_lagDamage; //lazy i know
                    //resets timer and decreases health bar
                    m_timer = 0.3f;
                    m_current.value = value;

                    //closes health bar if killed
                    if (value <= 0f)
                    {
                        /*Invoke(nameof(*/
                        _CloseHealthBar();//), 0.2f);
                    }
                }
            }
            /// <summary>
            /// hides hud and disables parts of script
            /// </summary>
            private void _CloseHealthBar()
            {
                m_active = false;
                gameObject.SetActive(false);
            }

            /// <summary>
            /// creates CustomName text files and folders if none is found  
            /// </summary>
            private void _InitializeCustomText()
            {
                if (!Directory.Exists($".\\CustomNames"))
                {
                    Directory.CreateDirectory($".\\CustomNames");
                }

                if (!File.Exists($".\\CustomNames\\names.txt"))
                {
                    TextWriter tw = new StreamWriter($".\\CustomNames\\names.txt", false);
                    tw.WriteLine("Put Names Here: (Line Break to separate, this line isn't read)"); ;
                    tw.Close();
                }

                if (!File.Exists($".\\CustomNames\\titles.txt"))
                {
                    TextWriter tw = new StreamWriter($".\\CustomNames\\titles.txt", false);
                    tw.WriteLine("Put Titles Here: (Line Break to separate, this line isn't read)"); ;
                    tw.Close();
                }

                if (!File.Exists($".\\CustomNames\\adjectives.txt"))
                {
                    TextWriter tw = new StreamWriter($".\\CustomNames\\adjectives.txt", false);
                    tw.WriteLine("Put Adjectives Here: (Line Break to separate, this line isn't read)"); ;
                    tw.Close();
                }

                if (!File.Exists($".\\CustomNames\\places.txt"))
                {
                    TextWriter tw = new StreamWriter($".\\CustomNames\\places.txt", false);
                    tw.WriteLine("Put Places Here: (Line Break to separate, this line isn't read)"); ;
                    tw.Close();
                }
            }

            private string GenerateName()
            {
                //gets names from CustomNames folder
                string[] names = File.ReadAllLines($".\\CustomNames\\names.txt");
                string[] titles = File.ReadAllLines($".\\CustomNames\\titles.txt");
                string[] adjectives = File.ReadAllLines($".\\CustomNames\\adjectives.txt");
                string[] places = File.ReadAllLines($".\\CustomNames\\places.txt");

                //if for some reason this fails at any point it will just default to BIG BOSS
                try
                {
                    //selects a random name to generate from a selection of 7 combinations
                    string final = Random.Range(0, 7) switch
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

                    return final;
                }
                catch
                {
                    return "BIG BOSS";
                }

            }
        }
    }

}


