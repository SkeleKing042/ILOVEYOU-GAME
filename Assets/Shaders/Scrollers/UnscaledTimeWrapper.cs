using UnityEngine;
using UnityEngine.UI;

namespace ILOVEYOU.Shader
{
    public class UnscaledTimeWrapper : MonoBehaviour
    {
        private Material m_mat;

        // Start is called before the first frame update
        void Start()
        {
            m_mat = GetComponent<Image>().material;
        }

        // Update is called once per frame
        void Update()
        {
            m_mat.SetFloat("_UnscaledTime", Time.unscaledTime);
        }
    }
}


