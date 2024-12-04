using ILOVEYOU.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ILOVEYOU
{
    namespace UI
    {

        public class TaskDisplay : MonoBehaviour
        {
            private Task m_taskRef;
            [Header("UI Elements")]
            [SerializeField] private Image[] m_iconDisplays;
            [SerializeField] private TextMeshProUGUI m_descriptionBox;
            [Header("Task information")]
            [SerializeField] private Sprite[] m_taskIcons;
            [SerializeField] private string[] m_taskDescriptions;
            // Start is called before the first frame update
            public void SetTask(ref Task task, int playerID)
            {
                Color impC = ColorPref.Get("Important Color" + playerID);

                m_iconDisplays[1].color = impC;
                m_descriptionBox.color = impC;
                m_iconDisplays[0].color = new Color(impC.r,impC.g,impC.b,0) / 2.0f;
                m_iconDisplays[0].color += new Color(0,0,0,impC.a);

                m_taskRef = task;
                m_descriptionBox.text = m_taskDescriptions[(int)m_taskRef.GetTaskType];
                m_iconDisplays[0].sprite = m_iconDisplays[1].sprite = m_taskIcons[(int)m_taskRef.GetTaskType];
                GetComponent<Animator>().SetBool("Show", true);
                //gameObject.SetActive(true);
            }

            // Update is called once per frame
            void Update()
            {
                if (m_taskRef != null && !m_taskRef.IsComplete)
                    m_iconDisplays[1].fillAmount = m_taskRef.GetPercent;
                else
                    //gameObject.SetActive(false);
                    GetComponent<Animator>().SetBool("Show", false);
            }
        }
    }
}