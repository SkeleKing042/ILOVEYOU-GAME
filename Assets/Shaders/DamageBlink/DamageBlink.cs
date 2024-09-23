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

            List<MeshRenderer> m_renderers = new List<MeshRenderer>();

            private void Awake()
            {
                MeshRenderer ren = GetComponent<MeshRenderer>();
                if (ren)
                    m_renderers.Add(ren);

                MeshRenderer[] rens = GetComponentsInChildren<MeshRenderer>();
                if (rens.Length > 0)
                    m_renderers.AddRange(rens);
            }
            public void StartBlink()
            {
                foreach (MeshRenderer renderer in m_renderers)
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
                foreach (MeshRenderer renderer in m_renderers)
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