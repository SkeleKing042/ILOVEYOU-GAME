using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Shader
    {
        public class DamageBlink : MonoBehaviour
        {
            [SerializeField] private Material m_blinkMat;
            [SerializeField] private float m_activeTime;

            List<Renderer> m_renderers = new List<Renderer>();
            [SerializeField] private List<Renderer> m_rendererBlacklist = new();

            private void Awake()
            {
                Renderer ren = GetComponent<Renderer>();
                if (ren)
                    m_renderers.Add(ren);

                Renderer[] rens = GetComponentsInChildren<Renderer>();
                foreach(var render in rens)
                    if(!m_rendererBlacklist.Contains(render))
                        m_renderers.Add(render);
            }
            public void StartBlink()
            {
                foreach (Renderer renderer in m_renderers)
                {
                    List<Material> mats = new();
                    for (int i = 0; i < renderer.materials.Length; i++)
                    {
                        mats.Add(renderer.materials[i]);
                    }
                    mats.Add(m_blinkMat);

                    renderer.materials = new Material[mats.Count];
                    renderer.materials = mats.ToArray();

                }
                Invoke("EndBlink", m_activeTime);
            }
            public void EndBlink()
            {
                foreach (Renderer renderer in m_renderers)
                {
                    List<Material> mats = new();
                    mats.AddRange(renderer.materials);

                    for (int i = 0; i < renderer.materials.Length; i++)
                    {
                        if (mats[i].shader == m_blinkMat.shader)
                        {
                            mats.RemoveAt(i);
                            break;
                        }
                    }
                    renderer.materials = new Material[mats.Count];
                    renderer.materials = mats.ToArray();
                }
            }
        }
    }
}