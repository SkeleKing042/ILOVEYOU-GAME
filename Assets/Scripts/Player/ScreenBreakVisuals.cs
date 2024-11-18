using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU.UI
{
    public class ScreenBreakVisuals : MonoBehaviour
    {
        [SerializeField] private PlayerControls m_controlRef;
        [SerializeField] private float m_threashold = 0.5f;
        private float m_lastPercent;
        // Start is called before the first frame update
        void Start()
        {
            if (!m_controlRef)
            {
                Debug.LogError("No player controls reference set, plz fix boss");
                Destroy(this);
            }

            //disable all child objects
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        void FixedUpdate()
        {
            if (m_lastPercent != m_controlRef.GetHealthPercent)
            {
                if (m_controlRef.GetHealthPercent <= m_threashold)
                {
                    for (int i = 0; i < Mathf.RoundToInt(transform.childCount * (1 - m_controlRef.GetHealthPercent / m_threashold)); i++)
                    {
                        GameObject childT = transform.GetChild(i).gameObject;
                        if (!childT.activeSelf)
                        {
                            childT.SetActive(true);
                        }
                    }
                }
                m_lastPercent = m_controlRef.GetHealthPercent;
            }
        }
    }
}