using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPasteClone : MonoBehaviour
{

    private Transform m_target;
    private Rigidbody m_rb;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void Initialize(Transform target)
    {
        m_target = target;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_rb.position = Vector3.MoveTowards(m_rb.position, m_target.position, 10f * Time.fixedDeltaTime);

        m_rb.velocity = Vector3.zero;
    }
}
