using ILOVEYOU.Audio;
using ILOVEYOU.Management;
using ILOVEYOU.ProjectileSystem;
using ILOVEYOU.Shader;
using ILOVEYOU.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace ILOVEYOU
{
    namespace Player
    {
        [RequireComponent(typeof(Rigidbody))]
        [RequireComponent(typeof(Collider))]
        public class PlayerControls : MonoBehaviour
        {            
            private PlayerManager m_plaMa;
            private Rigidbody m_rb;

            [Header("General")]
            [SerializeField] private float m_maxHealth = 10f;
            private float m_health;
            public float GetHealthPercent => m_health / m_maxHealth;
            private float m_iframesCurrent;
            [SerializeField] private Animator m_anim; //animator should be located on the player model
            public Animator GetPlayerAnimator { get { return m_anim; } }
            [SerializeField] private Renderer[] m_characterRenderers;

            [Header("Movement")]
            private float m_moveSpeed;
            private Vector3 m_moveDir;

            [Header("Shooting")]
            [SerializeField] private float m_damage;
            [SerializeField] private float m_fireRate;
            private Vector3 m_aimDir;
            private bool m_allowShooting;
            private float m_aimMagnitude { get { return m_aimDir.magnitude; } }
            [SerializeField, Range(0f, 1f)] private float m_aimDeadZone;

            //private GameObject m_playerModel;
            private Transform m_facingObject;
            //private GameObject m_patternObject; //gameobject that holds the bullet pattern script
            private BulletPattern m_pattern;

            [Header("Raycasting")]
            [SerializeField] private Vector3 m_boxCastSize;
            [SerializeField] LayerMask m_mask;
            private Collider m_Collider;
            private RaycastHit m_Hit;
            private bool m_hitDetect;

            [Header("Events - for sounds and visuals")]
            [SerializeField] private UnityEvent m_onTakeDamage;
            [SerializeField] private UnityEvent m_onDeath;
            [SerializeField] private UnityEvent m_onShootingDisabled;
            [SerializeField] private UnityEvent m_onShootingEnabled;

            [SerializeField] private ContextBox m_contextBox;
            public ContextBox GetContextBox => m_contextBox;
            private void Awake()
            {
                m_facingObject = transform.GetChild(0);
                m_pattern = m_facingObject.GetComponent<BulletPattern>();
                m_pattern.ChangePattern(GameSettings.Current.GetPlayerShootingPattern);
                m_Collider = GetComponent<Collider>();
                m_plaMa = GetComponent<PlayerManager>();
                m_rb = GetComponent<Rigidbody>();
                m_maxHealth = GameSettings.Current.GetPlayerHealth;
                m_health = m_maxHealth;
                m_moveSpeed = GameSettings.Current.GetPlayerSpeed;
                UpdateHealthBar();
                m_allowShooting = true;
            }
            public bool Startup()
            {
                Debug.Log($"There's nothing to start in {this}. It was all done in Awake.");
                return true;
            }

            public void SetMaterial(Material[] materials)
            {
                m_characterRenderers[0].material = materials[0];
                m_characterRenderers[1].material = materials[1];
            }

            /// <summary>
            /// Changes a stat
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool ChangeStat(int index, float value)
            {
                switch (index)
                {
                    case 0:
                        m_moveSpeed += value;
                        break;
                    case 1:
                        m_maxHealth += value;
                        break;
                    case 2:
                        m_pattern.AddDamage(value);
                        break;
                    case 3:
                        m_pattern.AddFireSpeed(value);
                        break;
                }
                return true;
            }

            public void ChangeWeapon(BulletPatternObject obj)
            {
                m_pattern.ChangePattern(obj);
            }
            public void Update()
            {
                Color tmp_color = Color.blue;
                if (m_aimMagnitude >= m_aimDeadZone && m_allowShooting)
                {
                    Debug.Log($"{gameObject} is firing");;

                    //raycasts in front of the player for a target for the bullets
                    m_hitDetect = Physics.BoxCast(m_Collider.bounds.center, m_boxCastSize * 0.5f, m_facingObject.transform.forward, out m_Hit, m_facingObject.transform.rotation, 200f, m_mask);

                    //adds target to pattern script
                    if (m_hitDetect) m_pattern.AddTarget(m_Hit.collider.transform);
                    else m_pattern.AddTarget(null);

                    //updates cooldowns
                    m_pattern.PatternUpdate();

                    //update animator
                    m_anim.SetBool("Shooting", true);
                }
                else
                {
                    //update animator
                    m_anim.SetBool("Shooting", false);
                }
                m_iframesCurrent = Mathf.Clamp(m_iframesCurrent - Time.deltaTime, 0f, GameSettings.Current.GetiFrameDuration);
                Debug.DrawRay(transform.position, 5 * m_aimMagnitude * m_aimDir, tmp_color);
                //Apply direction to the player object
                m_rb.velocity = m_moveDir * m_moveSpeed;
            }
            public void OnMove(InputValue value)
            {
                if (!enabled) return;

                //Get the direction from the given input
                m_moveDir = value.Get<Vector2>();
                m_moveDir = new Vector3(m_moveDir.x, 0, m_moveDir.y);
                //Debug.Log($"Moving {gameObject} by {m_moveDir}.");

                UpdateAnimator();
            }
            public void OnFire(InputValue value)
            {
                if (!enabled) return;

                //Get the direction of the right stick
                m_aimDir = value.Get<Vector2>();
                if (m_aimDir == Vector3.zero) return;
                //Apply it to the x & z axis
                m_aimDir = new Vector3(m_aimDir.x, 0, m_aimDir.y);
                //Debug.Log($"{gameObject} is aiming towards {m_aimDir}.");

                //find and apply rotation to the pattern object
                Vector3 relativePos = (transform.position + m_aimDir) - transform.position;
                //gets the required rotation for the shooting
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                m_facingObject.transform.rotation = rotation;
                //m_playerModel.transform.rotation = rotation;

                UpdateAnimator();
            }

            /// <summary>
            /// updates the player animator for movement
            /// </summary>
            private void UpdateAnimator()
            {
                //Converts movement into angle
                float moveAngle = Mathf.Rad2Deg * Mathf.Atan2(m_moveDir.x, m_moveDir.z);
                //gets quaternions to convert to vectors
                Quaternion moveQ = Quaternion.Euler(0f, moveAngle, 0f);
                Quaternion shotQ = m_facingObject.rotation;
                //sets required animation variables
                m_anim.SetFloat("moveX", (moveQ * Quaternion.Inverse(shotQ) * Vector3.forward * m_moveDir.magnitude).x);
                m_anim.SetFloat("moveZ", (moveQ * Quaternion.Inverse(shotQ) * Vector3.forward * m_moveDir.magnitude).z);
            }

            public void OnContextButton(InputValue value)
            {
                //Debug.Log("HEwwo!!!!");
                m_contextBox.GetAction?.Invoke();
            }

            public void OnJoin(InputValue value)
            {
                if (!enabled) return;

                GameManager.Instance.PauseGame();
                //Debug.Log("Paused!");
            }
            /// <summary>
            /// zeros out player movement
            /// </summary>
            public void Zero()
            {
                m_moveDir = Vector3.zero;
                m_rb.velocity = Vector3.zero;
                m_aimDir = Vector3.zero;
                m_anim.SetFloat("moveX", 0f);
                m_anim.SetFloat("moveZ", 0f);
                m_anim.SetBool("Shooting", false);
            }
            public void HealDamage(float value, bool doOverheal = false)
            {
                Debug.Log($"Healing {gameObject.name} for {value} points");
                if (doOverheal)
                {
                    m_health += value;
                }
                else
                {
                    m_health = Mathf.Clamp(m_health + value, 0, m_maxHealth);
                }
                UpdateHealthBar();
            }
            /// <summary>
            /// makes the player take the damage oh noooo this is bad
            /// </summary>
            public void TakeDamage(float damage)
            {
                if (m_iframesCurrent > 0) return;
                m_health -= damage;
                m_iframesCurrent = GameSettings.Current.GetiFrameDuration;
                UpdateHealthBar();
                //reset all timed tasks when damaged
                m_plaMa.GetTaskManager.UpdateTimers(true);
                if (m_health <= 0)
                {
                    GameManager.Instance.PlayerDeath(m_plaMa);
                    m_onDeath.Invoke();
                }
                else
                {
                    m_onTakeDamage.Invoke();
                }
                
            }

            public void UpdateHealthBar()
            {
                float current = m_health / m_maxHealth;
                m_plaMa.GetUI.UpdateHealthBar(current);
            }

            public void PlaySound(string name)
            {
                SoundManager.SFX.PlayRandomSound(name);
            }

            private void OnDrawGizmos()
            {
#if UNITY_EDITOR
                if(m_facingObject)
                {
                    Gizmos.color = Color.red;

                    //Check if there has been a hit yet
                    if (m_hitDetect)
                    {
                        //Draw a Ray forward from GameObject toward the hit
                        Gizmos.DrawRay(m_facingObject.transform.position, m_facingObject.transform.forward * m_Hit.distance);
                        //Draw a cube that extends to where the hit exists
                        Gizmos.DrawWireCube(m_facingObject.transform.position + m_facingObject.transform.forward * m_Hit.distance, m_boxCastSize);
                    }
                    //If there hasn't been a hit yet, draw the ray at the maximum distance
                    else
                    {
                        //Draw a Ray forward from GameObject toward the maximum distance
                        Gizmos.DrawRay(m_facingObject.transform.position, m_facingObject.transform.forward * 200f);
                        //Draw a cube at the maximum distance
                        Gizmos.DrawWireCube(m_facingObject.transform.position + m_facingObject.transform.forward * 200f, m_boxCastSize);
                    }
                }
#endif
            }
            public void DisableShooting(float time)
            {
                m_allowShooting = false;
                m_plaMa.GetUI.GetLog.LogInput($"<color=\"red\">Debugger disabled.</color> Rebooting in {time} seconds");
                m_onShootingDisabled.Invoke();
            }
            public void ReenableShooting()
            {
                m_allowShooting = true;
                m_plaMa.GetUI.GetLog.LogInput($"Debugger rebooted, please enjoy your free trial!");
                m_onShootingEnabled.Invoke();
            }

        }
    }
}
