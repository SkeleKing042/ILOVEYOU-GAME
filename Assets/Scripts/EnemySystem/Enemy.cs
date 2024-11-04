using ILOVEYOU.Player;
using UnityEngine;
using ILOVEYOU.Shader;
using UnityEngine.AI;
using System.Collections;
namespace ILOVEYOU
{
    namespace EnemySystem
    {
        public class Enemy : MonoBehaviour
        {
            [SerializeField] protected float m_damage = 1f;
            [SerializeField] protected float m_health = 1f;
            [SerializeField] protected float m_deathTimeout = 10f;
            [SerializeField] protected float m_distanceCondition = 1f;
            protected bool m_stunned = false;
            [SerializeField] protected float m_stunnedRecoveryTime = 1f;
            protected bool m_isDead = false;

            [SerializeField] protected LayerMask m_obscureMask;
            protected bool m_canSeePlayer { get { return !Physics.Raycast(transform.position, (m_playerTransform.position - transform.position).normalized, (m_playerTransform.position - transform.position).magnitude, m_obscureMask); } }
            [SerializeField] protected bool m_ignoreSight;

            [Header("Despawning offscreen")]
            [Tooltip("The time this enemy can spend offscreen before despawning")]
            [SerializeField] protected float m_offscrenDespawnTime = 5f;
            protected float m_timeSpentOffscreen;
            [Tooltip("The amount this enemy can be offscreen with starting the countdown - generally for larger enemies that may be seen despawning. (0 is the far left/bottom of the screen and 1 is the far right/top")]
            [SerializeField] protected Vector2 m_screenSizeBuffer = Vector2.zero;
            [Tooltip("When this enemy gets despawned, should another enemy get spawned in.")]
            [SerializeField] protected bool m_shouldRespawn = false;

            [Header("Player Tracking")]
            protected Transform m_playerTransform;
            protected Rigidbody m_rigidBody;
            protected NavMeshAgent m_agent;
            protected bool m_usingAIBrain = false;
            [SerializeField] private Vector2 m_repathTimes = new Vector2(0.2f, 0.5f);
            protected Animator m_anim;

            private DamageBlink m_blinkScript;
            //this is used for the enemy hurtbox script
            public float GetDamage() { return m_damage; }

            public virtual void Initialize(Transform target)
            {
                m_rigidBody = GetComponent<Rigidbody>();
                m_playerTransform = target;
                m_blinkScript = GetComponent<DamageBlink>();
                m_agent = GetComponent<NavMeshAgent>();
                m_anim = GetComponentInChildren<Animator>();

                //gets relative position between the player and enemy
                Vector3 relativePos = m_playerTransform.position - transform.position;
                //looks at the player (removing x, and z rotation)
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
                //sets rotation
                transform.rotation = rotation;

                //Potential TODO: add a "modifier" value that is dependent on current difficulty/time that influences the base values
            }


            // Update is called once per frame
            protected virtual void Update()
            {
                if (!m_stunned)
                {
                    //this is simple movement logic, subsequent enemy scripts can be as simple or as complex as they want
                    if (Vector3.Distance(transform.position, m_playerTransform.position) < m_distanceCondition && (m_canSeePlayer || m_ignoreSight))
                    {
                        DoNearAction();
                    }
                    else
                    {
                        if ((m_agent.destination - transform.position).magnitude <= 1 && m_usingAIBrain)
                        {
                            m_agent.SetDestination(m_playerTransform.position);
                        }
                        else if (!m_usingAIBrain)
                        {
                            EnableAIBrain();
                        }
                        /*                    if (m_canSeePlayer)
                                            {
                                                MoveToTarget();
                                            }*/
                    }
                }

                Vector3 positionInViewport = m_playerTransform.GetComponentInChildren<Camera>().WorldToViewportPoint(transform.position);
                if (positionInViewport.x < 0 - m_screenSizeBuffer.x || positionInViewport.x > 1 + m_screenSizeBuffer.x || positionInViewport.y < 0 - m_screenSizeBuffer.y || positionInViewport.y > 1 + m_screenSizeBuffer.y)
                {
                    //this is offscreen - start countdown
                    m_timeSpentOffscreen += Time.deltaTime;
                    //been offscreen too long - despawn object
                    if (m_timeSpentOffscreen >= m_offscrenDespawnTime)
                    {
                        Debug.Log($"Enemy spent too long offscreen - despawning.");
                        DisableAIBrain();
                        Destroy(gameObject);
                    }
                }
                else if (m_timeSpentOffscreen > 0)
                    m_timeSpentOffscreen = 0;
            }
            public void GetStunned()
            {
                DisableAIBrain();
                m_rigidBody.constraints = RigidbodyConstraints.None;
                m_rigidBody.useGravity = true;
                m_stunned = true;
            }
            private void OnCollisionEnter(Collision collision)
            {
                if (m_stunned && collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
                {
                    Invoke("StunRecovery", m_stunnedRecoveryTime);
                }
            }
            public void StunRecovery()
            {
                m_stunned = false;
                m_rigidBody.useGravity = false;
                m_rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
                m_rigidBody.constraints = RigidbodyConstraints.FreezePositionY;
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                EnableAIBrain();
            }

            //public virtual void MoveToTarget()
            //{
            //    //gets relative position between the player and enemy
            //    Vector3 relativePos = m_playerTransform.position - transform.position;
            //    //looks at the player (removing x, and z rotation)
            //    Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            //    rotation = Quaternion.Euler(0f, Mathf.LerpAngle(transform.rotation.eulerAngles.y,rotation.eulerAngles.y, Time.deltaTime * m_turnSpeed), 0f);
            //    //moves and rotates the enemy
            //    //transform.SetPositionAndRotation(transform.position + (m_speed * Time.deltaTime * transform.forward), rotation);
            //    m_rigidBody.MoveRotation(rotation);
            //    m_rigidBody.MovePosition(m_rigidBody.position + (m_speed *Time.deltaTime * transform.forward));
            //    //m_rigidBody.velocity = (m_speed * transform.forward);
            //}
            public virtual void EnableAIBrain()
            {
                m_usingAIBrain = true;
                m_rigidBody.isKinematic = true;
                m_agent.enabled = true;
                StartCoroutine(Repath());
            }
            protected void DisableAIBrain()
            {
                m_agent.enabled = false;
                m_rigidBody.isKinematic = false;
                m_usingAIBrain = false;
            }
            protected IEnumerator Repath()
            {
                while (m_usingAIBrain)
                {
                    if (!m_agent.SetDestination(m_playerTransform.position))
                    {
                        Debug.LogWarning($"Agent failed to path. DEATH.");
                        Destroy(gameObject);
                        break;
                    }
                    yield return new WaitForSeconds(Random.Range(m_repathTimes.x, m_repathTimes.y));
                }
            }

            public virtual void DoNearAction()
            {
                
            }

            public virtual void TakeDamage(float damage)
            {
                m_blinkScript.StartBlink();
                m_health -= damage;
                //death
                if (m_health <= 0)
                {
                    m_isDead = true;
                    enabled = false;
                    m_agent.enabled = false;
                    StopAllCoroutines();
                    m_playerTransform.GetComponent<PlayerManager>().GetTaskManager.UpdateKillTrackers(1);
                    m_rigidBody.mass /= 2f;
                    /*foreach(Collider col in GetComponentsInChildren<Collider>())
                    {
                        col.enabled = false;
                    }*/
                    m_anim?.SetTrigger("Death");
                    Destroy(gameObject, m_deathTimeout);
                }
            }

            
        }
    }
}
