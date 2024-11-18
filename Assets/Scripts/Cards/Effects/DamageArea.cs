using ILOVEYOU.EnemySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Cards
    {
        public class DamageArea : MonoBehaviour
        {
            [SerializeField] private float m_radius = 2f;
            [SerializeField] private float m_damage = 1f;
            [SerializeField] private float m_damageTime = 0.5f;
            private float m_time = 0f;
            private bool m_enable = false;
            private readonly float m_transSpeed = 6f;
            [SerializeField] private LayerMask m_mask;

            void Update()
            {
                //if enabled
                if (m_enable)
                {
                    //counts down
                    m_time -= Time.deltaTime;

                    if (m_time <= 0)
                    {
                        //refreshes timer
                        m_time = m_damageTime;
                        //checks around player
                        Collider[] col = Physics.OverlapSphere(transform.position, m_radius, m_mask);

                        for (int i = 0; i < col.Length; i++)
                        {
                            //damages enemies
                            col[i].GetComponent<Enemy>().TakeDamage(m_damage);
                        }
                    }
                    //lerps sphere size so it looks cool or something
                    //transform.GetChild(0).localScale = Vector3.Lerp(transform.GetChild(0).localScale, new(m_radius * 2f, m_radius * 2f, m_radius * 2f), Time.deltaTime * m_transSpeed);
                }
            }

            /// <summary>
            /// Enables the physics checks and shows the sphere
            /// </summary>
            public void Enable()
            {
                StopAllCoroutines();

                //transform.GetChild(0).gameObject.SetActive(true);
                m_enable = true;
            }
            /// <summary>
            /// Enables the physics checks and shows the sphere
            /// </summary>
            public void Enable(float size)
            {
                StopAllCoroutines();

                //transform.GetChild(0).gameObject.SetActive(true);
                m_radius = size;
                m_enable = true;
            }
            /// <summary>
            /// Enables the physics checks and shows the sphere
            /// </summary>
            public void Enable(float size, float damage)
            {
                StopAllCoroutines();

                //transform.GetChild(0).gameObject.SetActive(true);
                m_radius = size;
                m_damage = damage;
                m_enable = true;
            }
            /// <summary>
            /// disables the physics checks and hides the sphere
            /// </summary>
            public void Disable()
            {
                m_enable = false;
                //StartCoroutine(_SmoothTransition());
            }
            /*/// <summary>
            /// does a cool transition when disabling. Hides the visual effect when it reaches zero
            /// </summary>
            private IEnumerator _SmoothTransition()
            {
                while (transform.GetChild(0).localScale != Vector3.zero)
                {
                    transform.GetChild(0).localScale = Vector3.Lerp(transform.GetChild(0).localScale, Vector3.zero, Time.deltaTime * m_transSpeed);
                    if (Vector3.Distance(transform.GetChild(0).localScale, Vector2.zero) < 0.1f) transform.GetChild(0).localScale = Vector3.zero;

                    yield return new WaitForEndOfFrame();
                }

                transform.GetChild(0).gameObject.SetActive(false);

                yield return null;
            }*/

            private void OnDrawGizmosSelected()
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, m_radius);
            }
        }
    }
}