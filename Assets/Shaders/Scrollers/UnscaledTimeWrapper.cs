using UnityEngine;
using UnityEngine.UI;

namespace ILOVEYOU.Shader
{
    public class UnscaledTimeWrapper : MonoBehaviour
    {
        [SerializeField] private Material m_scanLineMaterial;
        
        private Material m_mat;

        // Start is called before the first frame update
        void Start()
        {
            Image image = GetComponent<Image>();
            image.material = new(m_scanLineMaterial);
            m_mat = image.material;
        }

        // Update is called once per frame
        void Update()
        {
            m_mat.SetFloat("_UnscaledTime", Time.unscaledTime);
        }

    }
}


