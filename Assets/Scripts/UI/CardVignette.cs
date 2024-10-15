using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CardVignette : MonoBehaviour
{
    [SerializeField] private float m_fadeInRate;
    [SerializeField] private float m_fadeOutRate;
    private bool m_fadingOut;
    private Image m_image;
    // Start is called before the first frame update
    void Start()
    {
        m_image = GetComponent<Image>();
    }

    public void FlashIn(Color wantedColor)
    {
        wantedColor.a = 0.0001f;
        m_image.color = wantedColor;

        StartCoroutine(FadeVisual());
    }

    private IEnumerator FadeVisual()
    {
        m_fadingOut = false;
        while (true)
        {
            if(!m_fadingOut)
            {
                m_image.color += new Color(0, 0, 0, m_fadeInRate);
            }
            else
            {
                m_image.color -= new Color(0, 0, 0, m_fadeOutRate);
            }

            if(m_image.color.a >= 1)
            {
                m_fadingOut = true;
            }
            if (m_image.color.a <= 0)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
