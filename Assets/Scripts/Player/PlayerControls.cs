using ILOVEYOU.Management;
using ILOVEYOU.ProjectileSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ILOVEYOU
{
    namespace Player
    {
        public class PlayerControls : MonoBehaviour
        {
            [SerializeField] private bool m_debugging;
            private PlayerManager m_plaMa;
            [Header("General")]
            [SerializeField] private float m_MaxHealth = 10f;
            private float m_health;
            [SerializeField] private float m_iframesTotal = 1f; //this is in seconds
            private float m_iframesCurrent;
            [Header("Movement")]
            [SerializeField] private float m_moveSpeed;
            private Vector3 m_moveDir;

            [Header("Shooting")]
            [SerializeField] private float m_damage;
            [SerializeField] private float m_fireRate;
            private Vector3 m_aimDir;
            private float m_aimMagnitude { get { return m_aimDir.magnitude; } }
            [SerializeField, Range(0f, 1f)] private float m_aimDeadZone;

            private GameObject m_patternObject; //gameobject that holds the bullet pattern script
            private BulletPattern m_pattern;

            [Header("Raycasting")]
            [SerializeField] private Vector3 m_boxCastSize;
            [SerializeField] LayerMask m_mask;
            private Collider m_Collider;
            private RaycastHit m_Hit;
            private bool m_hitDetect;

            private void Awake()
            {
                m_patternObject = transform.GetChild(2).gameObject; //this is the empty gameobject with the pattern script object
                m_pattern = m_patternObject.GetComponent<BulletPattern>();
                m_Collider = GetComponent<Collider>();
                m_plaMa = GetComponent<PlayerManager>();
                m_health = m_MaxHealth;
                UpdateHealthBar();
            }


            /// <summary>
            /// Changes a stat
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool ChangeState(int index, float value)
            {
                switch (index)
                {
                    case 0:
                        m_moveSpeed += value;
                        break;
                    case 1:
                        m_pattern.AddDamage(value);
                        break;
                    case 2:
                        m_pattern.AddFireSpeed(value);
                        break;
                }
                return true;
            }
            public void Update()
            {

                Color tmp_color = Color.blue;
                if (m_aimMagnitude >= m_aimDeadZone)
                {
                    if (m_debugging) Debug.Log($"{gameObject} is firing");
                    if (m_debugging) tmp_color = Color.red;

                    //raycasts in front of the player for a target for the bullets
                    m_hitDetect = Physics.BoxCast(m_Collider.bounds.center, m_boxCastSize * 0.5f, m_patternObject.transform.forward, out m_Hit, m_patternObject.transform.rotation, 200f, m_mask);

                    //adds target to pattern script
                    if (m_hitDetect) m_pattern.AddTarget(m_Hit.collider.transform);
                    else m_pattern.AddTarget(null);

                    //updates cooldowns
                    m_pattern.PatternUpdate();

                }
                m_iframesCurrent = Mathf.Clamp(m_iframesCurrent - Time.deltaTime, 0f, m_iframesTotal);
                if (m_debugging) Debug.DrawRay(transform.position, m_aimDir * m_aimMagnitude * 5, tmp_color);
            }
            public void FixedUpdate()
            {
                //Apply direction to the player object
                transform.Translate(m_moveDir * m_moveSpeed * Time.fixedDeltaTime);
            }
            public void OnMove(InputValue value)
            {
                //Get the direction from the given input
                m_moveDir = value.Get<Vector2>();
                m_moveDir = new Vector3(m_moveDir.x, 0, m_moveDir.y);
                if (m_debugging) Debug.Log($"Moving {gameObject} by {m_moveDir}.");
            }
            public void OnFire(InputValue value)
            {
                //Get the direction of the right stick
                m_aimDir = value.Get<Vector2>();
                //Apply it to the x & z axis
                m_aimDir = new Vector3(m_aimDir.x, 0, m_aimDir.y);
                if (m_debugging) Debug.Log($"{gameObject} is aiming towards {m_aimDir}.");

                //find and apply rotation to the pattern object
                Vector3 relativePos = (transform.position + m_aimDir) - transform.position;
                //gets the required rotation for the shooting
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                m_patternObject.transform.rotation = rotation;
            }
            /// <summary>
            /// makes the player take the damage oh noooo this is bad
            /// </summary>
            public void TakeDamage(float damage)
            {
                if (m_iframesCurrent > 0) return;
                m_health -= damage;
                m_iframesCurrent = m_iframesTotal;
                UpdateHealthBar();
                if (m_health <= 0) m_plaMa.GetGameManager().PlayerDeath(m_plaMa);
                
            }

            public void UpdateHealthBar()
            {
                float current = m_health / m_MaxHealth;
                m_plaMa.UpdateHealthBar(current);
            }

            private void OnDrawGizmos()
            {
                if (m_debugging)
                {
                    Gizmos.color = Color.red;

                    //Check if there has been a hit yet
                    if (m_hitDetect)
                    {
                        //Draw a Ray forward from GameObject toward the hit
                        Gizmos.DrawRay(m_patternObject.transform.position, m_patternObject.transform.forward * m_Hit.distance);
                        //Draw a cube that extends to where the hit exists
                        Gizmos.DrawWireCube(m_patternObject.transform.position + m_patternObject.transform.forward * m_Hit.distance, m_boxCastSize);
                    }
                    //If there hasn't been a hit yet, draw the ray at the maximum distance
                    else
                    {
                        //Draw a Ray forward from GameObject toward the maximum distance
                        Gizmos.DrawRay(m_patternObject.transform.position, m_patternObject.transform.forward * 200f);
                        //Draw a cube at the maximum distance
                        Gizmos.DrawWireCube(m_patternObject.transform.position + m_patternObject.transform.forward * 200f, m_boxCastSize);
                    }
                }
            }
        }
    }
}