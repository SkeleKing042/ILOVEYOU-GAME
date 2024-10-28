using UnityEngine;

namespace ILOVEYOU
{
    namespace EnemySystem
    {
        public class LungeEnemy : Enemy
        {
            [SerializeField] private float m_lungeTime = 5f;
            [SerializeField] private float m_lungeSpeed = 3f;
            [SerializeField] private LayerMask m_wallMask;

            private float m_lungeCooldown;
            private float m_tempSpeed = 0f;

            // Start is called before the first frame update
            public override void Initialize(Transform target)
            {
                m_lungeCooldown = m_lungeTime;
                base.Initialize(target);
            }

            // Update is called once per frame
            protected override void Update()
            {
                //this is simple movement logic, subsequent enemy scripts can be as simple or as complex as they want
                if (m_tempSpeed <= 0)
                {
                    m_anim.SetBool("Charging", false);
                    base.Update();
                }
                else
                {
                    //transform.position += m_tempSpeed * Time.deltaTime * transform.forward;
                    if (Physics.BoxCast(transform.position,new Vector3(.45f,.25f,.25f), transform.forward, transform.rotation, .3f, m_wallMask)) m_tempSpeed = 0f;

                    m_rigidBody.MovePosition(m_rigidBody.position + (m_tempSpeed * Time.deltaTime * transform.forward));
                    m_tempSpeed -= Time.deltaTime * (m_lungeSpeed / 2);

                    m_anim.SetFloat("TempSpeed", (m_tempSpeed / m_lungeSpeed) * 2f);
                }
            }

            public override void DoNearAction()
            {
                if (m_usingAIBrain)
                {
                    DisableAIBrain();
                }

                m_anim.SetBool("Charging", true);

                //gets relative position between the player and enemy
                Vector3 relativePos = m_playerTransform.position - transform.position;
                //looks at the player (removing x, and z rotation)
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                rotation = Quaternion.Euler(0f, Mathf.LerpAngle(transform.rotation.eulerAngles.y, rotation.eulerAngles.y, Time.deltaTime * m_agent.angularSpeed), 0f);

                transform.rotation = rotation;

                m_lungeCooldown -= Time.deltaTime;
                if(m_lungeCooldown <= 0f)
                {
                    m_tempSpeed = m_lungeSpeed;
                    m_lungeCooldown = m_lungeTime;
                    m_anim.SetFloat("TempSpeed", (m_tempSpeed / m_lungeSpeed) * 2f);
                }
            }
        }
    }
}


