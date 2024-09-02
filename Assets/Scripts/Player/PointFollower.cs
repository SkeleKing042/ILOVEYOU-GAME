using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ILOVEYOU
{
    namespace Player
    {

        [RequireComponent(typeof(NavMeshAgent))]
        public class PointFollower : MonoBehaviour
        {
            //AI
            private NavMeshAgent m_agent;
            private Vector3 m_targetDestination;

            //Player
            private Transform m_playerObject;

            //Raycasts
            private bool m_viewOfPlayerBlocked;
            private Vector3 m_distanceToPlayer;
            private bool m_playerSeen;
            private Vector3 m_playerDistToTarget;
            private bool m_playerSeesTarget;

            //Settings
            [SerializeField] private LayerMask m_obscureMask;
            [SerializeField] private float m_playerMinDistance;

            // Start is called before the first frame update
            public bool Init(Transform player)
            {
                m_agent = GetComponent<NavMeshAgent>();
                m_playerObject = player;
                if (m_playerObject == null)
                {
                    Debug.LogError("Player not found");
                    Destroy(gameObject);
                    return false;
                }
                return true;
            }

            // Update is called once per frame
            void Update()
            {
                if (!m_playerObject)
                    return;

                //player
                m_distanceToPlayer = m_playerObject.position - transform.position;
                RaycastHit hit;
                m_viewOfPlayerBlocked = Physics.Raycast(transform.position, m_distanceToPlayer.normalized, out hit, m_distanceToPlayer.magnitude, m_obscureMask);

                m_playerDistToTarget = m_targetDestination - m_playerObject.position;
                m_playerSeesTarget = !Physics.Raycast(m_playerObject.position, m_playerDistToTarget.normalized, m_playerDistToTarget.magnitude, m_obscureMask);
                //goto player when can't see them
                if (m_viewOfPlayerBlocked)
                {
                    Debug.Log($"Player view blocked by {hit.transform}");

                    m_agent.isStopped = false;
                    m_agent.SetDestination(m_playerObject.position);
                    m_playerSeen = false;
                }
                else if (!m_playerSeen)
                {
                    Debug.Log("Player back within view!");
                    m_agent.isStopped = true;
                    m_playerSeen = true;
                }

                float playerDist = (m_targetDestination - m_playerObject.position).magnitude;
                float thisDist = (m_targetDestination - transform.position).magnitude;
                if (m_playerSeesTarget || /*(thisDist > playerDist && !m_viewOfPlayerBlocked) ||*/ m_distanceToPlayer.magnitude <= m_playerMinDistance)
                {
                    _returnToTarget();
                }
            }
            private void _returnToTarget()
            {
                m_agent.isStopped = false;
                m_agent.SetDestination(m_targetDestination);
            }

            public void OnTriggerEnter(Collider other)
            {
                //once close enough run back to the target
                if (other.transform == m_playerObject)
                {
                    Debug.Log("Player is close, return to target");
                    _returnToTarget();
                }
            }
            public void SetDestination(Transform destination)
            {
                gameObject.SetActive(true);
                m_targetDestination = destination.position;
                m_agent.Warp(m_targetDestination);
            }
            public void SetDestination(Vector3 position)
            {
                gameObject.SetActive(true);
                m_targetDestination = position;
                m_agent.Warp(m_targetDestination);
            }

            public void DisableTracker()
            {
                gameObject.SetActive(false);
                //m_agent.isStopped = true;
            }

#if UNITY_EDITOR
            private void OnDrawGizmos()
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(m_targetDestination, new(1, 1, 1));

                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(transform.position, m_playerMinDistance);

                if (m_playerObject != null)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireCube(m_playerObject.position, new(1, 1, 1));

                    Gizmos.color = m_viewOfPlayerBlocked ? Color.red : Color.green;
                    Gizmos.DrawRay(transform.position, (m_playerObject.position - transform.position).normalized * m_distanceToPlayer.magnitude);

                    Gizmos.color = m_playerSeesTarget ? Color.red : Color.green;
                    Gizmos.DrawRay(m_playerObject.position, (m_targetDestination - m_playerObject.position).normalized * m_playerDistToTarget.magnitude);
                }
            }
#endif
        }
    }
}