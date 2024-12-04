using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ILOVEYOU.UI
{
    public class CodeScroll : MonoBehaviour
    {
        [SerializeField] private Vector2 m_speed;

        private RawImage m_img;


        private void Start()
        {
            m_img = GetComponent<RawImage>();
        }

        // Update is called once per frame
        void Update()
        {
            m_img.uvRect = new Rect(m_img.uvRect.x + (m_speed.x * Time.deltaTime), m_img.uvRect.y + (m_speed.y * Time.deltaTime), m_img.uvRect.width, m_img.uvRect.height);
        }
    }

}

