using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPaste : MonoBehaviour
{
    [SerializeField] private GameObject m_targetObject;
    [SerializeField] private GameObject m_cloneObj;

    [SerializeField] private float m_radius;

    private List<GameObject[]> m_clones = new();
    private List<GameObject> m_cloneTargets = new();


    private void Start()
    {
            
    }

    private void Update()
    {
        m_cloneTargets.Add(new GameObject());
        Debug.Break();

        if (m_cloneTargets.Count > 0)
        {
            for (int i = 0; i < m_cloneTargets.Count; i++)
            {
                float angle = (i / m_cloneTargets.Count) * Mathf.PI * 2f;

                m_cloneTargets[i].transform.localPosition = new(transform.position.x + (Mathf.Cos(angle) * m_radius), 0f,
                        transform.position.z + (Mathf.Sin(angle) * m_radius));
            }
        }
    }



    public void CreateClones(int count)
    {
        GameObject[] clones = new GameObject[count];

        for (int i =0; i < count; i++)
        {
            
        }
    }

    public void RemoveOldestClones()
    {

    }
}
