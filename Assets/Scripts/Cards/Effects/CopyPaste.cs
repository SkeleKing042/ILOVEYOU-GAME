using ILOVEYOU.Player;
using ILOVEYOU.ProjectileSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Cards
    {
        public class CopyPaste : MonoBehaviour
        {
            [SerializeField] private GameObject m_targetObject;
            [SerializeField] private GameObject m_cloneObj;

            [SerializeField] private float m_radius;

            private List<GameObject[]> m_clones = new();
            private List<GameObject> m_cloneTargets = new();

            private void Update()
            {

                if (m_cloneTargets.Count > 0)
                {
                    for (int i = 0; i < m_cloneTargets.Count; i++)
                    {
                        float angle = ((float)i / (float)m_cloneTargets.Count) * Mathf.PI * 2f;

                        m_cloneTargets[i].transform.localPosition = new((Mathf.Cos(angle) * m_radius), 0f,
                                (Mathf.Sin(angle) * m_radius));
                    }

                    for (int i = 0; i < m_clones.Count; i++)
                    {
                        foreach (GameObject clone in m_clones[i])
                        {
                            clone.transform.rotation = m_targetObject.transform.rotation;


                        }
                    }
                }
            }



            public void CreateClones(int count)
            {
                GameObject[] clones = new GameObject[count];

                for (int i = 0; i < count; i++)
                {
                    //create target object
                    GameObject Target = new GameObject();
                    Target.transform.parent = m_targetObject.transform;
                    m_cloneTargets.Add(Target);

                    //create clone
                    GameObject clone = Instantiate(m_cloneObj);
                    clone.transform.position = transform.position;
                    clone.GetComponent<CopyPasteClone>().Initialize(Target.transform, m_targetObject.GetComponent<BulletPattern>().GetBulletPatternObject);

                    clones[i] = clone;
                }

                m_clones.Add(clones);
            }

            public void RemoveOldestClones()
            {
                for (int i = 0; i < m_clones[0].Length; i++)
                {
                    Destroy(m_clones[0][i]);
                    Destroy(m_cloneTargets[i]);

                }

                m_cloneTargets.RemoveRange(0, m_clones[0].Length);
                m_clones.RemoveAt(0);
            }

            public void RemoveAllClones()
            {
                for (int i = 0; i < m_clones.Count; i++)
                {
                    for (int j = 0; j < m_clones[0].Length; j++)
                    {
                        Destroy(m_clones[0][j]);
                        Destroy(m_cloneTargets[j]);

                    }
                    m_cloneTargets.RemoveRange(0, m_clones[0].Length);
                    m_clones.RemoveAt(0);

                }
            }
        }
    }
}

