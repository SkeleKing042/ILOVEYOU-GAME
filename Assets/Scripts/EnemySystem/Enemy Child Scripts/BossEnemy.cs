using ILOVEYOU.Player;
using ILOVEYOU.ProjectileSystem;
using ILOVEYOU.UI;
using UnityEngine;

namespace ILOVEYOU
{
    namespace EnemySystem
    {
        public class BossEnemy : Enemy
        {
            public static BossEnemy[] Instances = new BossEnemy[2];

            BulletPattern m_pattern;
            [SerializeField] private float m_lungeTime = .3f;
            [SerializeField] private float m_lungeSpeed = 20f;
            [SerializeField] private BulletPatternObject[] m_bossPatterns;

            private float m_lungeCooldown;
            private float m_tempSpeed = 0f;
            private float m_maxSpeed = 0f;
            private float m_maxHealth = 0f;

            private bool m_charging = false;

            public override void Initialize(Transform target)
            {
                m_lungeCooldown = m_lungeTime;
                base.Initialize(target);
                m_pattern.AddTarget(m_playerTransform);

                m_maxSpeed = m_speed;
                m_maxHealth = m_health;


                if (Instances[m_playerTransform.GetComponent<PlayerManager>().GetPlayerID])
                {
                    Instances[m_playerTransform.GetComponent<PlayerManager>().GetPlayerID].m_health = 
                        Mathf.Clamp(Instances[m_playerTransform.GetComponent<PlayerManager>().GetPlayerID].m_health + 200, 0, m_maxHealth);
                    BossBar.Instances[m_playerTransform.GetComponent<PlayerManager>().GetPlayerID].UpdateHealthBar(Instances[m_playerTransform.GetComponent<PlayerManager>().GetPlayerID].m_health);
                    Destroy(gameObject);
                }
                else
                {
                    Instances[m_playerTransform.GetComponent<PlayerManager>().GetPlayerID] = this;
                }

                BossBar.Instances[m_playerTransform.GetComponent<PlayerManager>().GetPlayerID].InitializeHealthBar("Legendary Big Boss", m_maxHealth);
            }


            void Awake()
            {
                m_pattern = GetComponent<BulletPattern>();
            }

            // Update is called once per frame
            void Update()
            {
                //BossBar.Instances[m_playerTransform.GetComponent<PlayerManager>().GetPlayerID].UpdateHealthBar(m_health);

                //this is simple movement logic, subsequent enemy scripts can be as simple or as complex as they want
                if (Vector3.Distance(transform.position, m_playerTransform.position) < m_distanceCondition)
                {
                    DoNearAction();
                }
                else
                {
                    m_speed = m_maxSpeed;
                    MoveToTarget();
                }
                
            }

            public override void DoNearAction()
            {
                if (m_tempSpeed >= 0)
                {
                    m_rigidBody.MovePosition(m_rigidBody.position + (m_tempSpeed * Time.deltaTime * transform.forward));
                    m_tempSpeed -= Time.deltaTime * (m_lungeSpeed / 2);
                    m_rigidBody.velocity = Vector3.zero;
                }
                else if (m_charging)
                {
                    //gets relative position between the player and enemy
                    Vector3 relativePos = m_playerTransform.position - transform.position;
                    //looks at the player (removing x, and z rotation)
                    Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    rotation = Quaternion.Euler(0f, Mathf.LerpAngle(transform.rotation.eulerAngles.y, rotation.eulerAngles.y, Time.deltaTime * m_turnSpeed), 0f);

                    transform.rotation = rotation;

                    m_lungeCooldown -= Time.deltaTime;
                    if (m_lungeCooldown <= 0f)
                    {
                        m_tempSpeed = m_lungeSpeed;
                        m_lungeCooldown = m_lungeTime;
                        //disengages charging sequence
                        m_pattern.ChangePattern(m_bossPatterns[1]);
                        m_charging = false;
                    }

                    m_pattern.PatternUpdate();
                }
                //lunging
                else if (Vector3.Distance(transform.position, m_playerTransform.position) < m_distanceCondition / 2f && !m_charging)
                {
                    //engages charging sequence
                    m_pattern.ChangePattern(m_bossPatterns[0]);
                    m_charging = true;
                }
                else
                {
                    //gets relative position between the player and enemy
                    Vector3 relativePos = m_playerTransform.position - transform.position;
                    //looks at the player (removing x, and z rotation)
                    Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
                    transform.rotation = rotation;

                    //shooting
                    m_pattern.PatternUpdate();
                    m_speed = m_maxSpeed / 2f;
                    MoveToTarget();
                }
                
            }

            private void OnDrawGizmos()
            {
                Gizmos.DrawWireSphere(transform.position, m_distanceCondition);
                //Gizmos.DrawWireSphere(transform.position, 24 / 1.5f);
                Gizmos.DrawWireSphere(transform.position, m_distanceCondition / 2f);
            }

            public override void TakeDamage(float damage)
            {
                BossBar.Instances[m_playerTransform.GetComponent<PlayerManager>().GetPlayerID].UpdateHealthBar(m_health);
                base.TakeDamage(damage);
            }

            private void OnDestroy()
            {
                BossBar.Instances[m_playerTransform.GetComponent<PlayerManager>().GetPlayerID].UpdateHealthBar(m_health);
            }
        }
    }

    
}


