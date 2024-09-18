using ILOVEYOU.ProjectileSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Cards
    {
        public class CopyPasteClone : MonoBehaviour
        {

            private Transform m_target;
            private Rigidbody m_rb;
            private BulletPatternObject m_bulletPattern;
            private BulletPattern m_pattern;

            // Start is called before the first frame update
            void Start()
            {


            }

            public void Initialize(Transform target, BulletPatternObject bulletPattern)
            {
                m_rb = GetComponent<Rigidbody>();
                m_pattern = GetComponent<BulletPattern>();

                m_target = target;
                m_bulletPattern = bulletPattern;

                m_pattern.ChangePattern(m_bulletPattern);
            }

            // Update is called once per frame
            void FixedUpdate()
            {
                m_rb.position = Vector3.MoveTowards(m_rb.position, m_target.position, 11f * Time.fixedDeltaTime);

                m_rb.velocity = Vector3.zero;

                m_pattern.PatternUpdate();
            }
        }

    }
}

