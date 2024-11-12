using ILOVEYOU.EnemySystem;
using ILOVEYOU.Hazards;
using ILOVEYOU.Management;
using ILOVEYOU.Player;
//using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ILOVEYOU
{
    namespace Environment
    {
        [RequireComponent(typeof(HazardManager))]
        public class LevelManager : MonoBehaviour
        {
            //private GameManager m_manager;
            //public GameManager GetManager { get { return m_manager; } }
            [SerializeField] private bool m_debugging;
            private PlayerManager m_playMan;
            private ParticleSpawner m_parSper;
            public bool hasPlayer { get { return m_playMan != null; } }
            public PlayerManager GetPlayer { get { return m_playMan; } }
            public ParticleSpawner GetParticleSpawner { get { return m_parSper; } }
            private EnemySpawner m_enSper;
            public EnemySpawner GetSpawner { get { return m_enSper; } }
            private HazardManager m_hazMan;

            [Header("Players")]
            [SerializeField] private Transform m_playerSpawn;
            [Header("Control points")]
            [SerializeField] private List<AreaControlPoint> m_controlPoints;
            [Header("Sequences")]
            [SerializeField] private List<Sequence> m_sequences;

            /// <summary>
            /// Setup of scripts vars
            /// </summary>
            /// <param name="gm"></param>
            /// <returns></returns>
            public bool Startup(GameObject player, uint index)
            {
                if (m_debugging) Debug.Log($"Starting {this}.");

                //setup name and position
                gameObject.name = $"Level {index}";
                gameObject.transform.position = new Vector3(500 * (index), 0, 0);
                //build navmesh
                GetComponent<NavMeshSurface>().BuildNavMesh();

                //player setup
                if (m_debugging) Debug.Log("Initalizing player.");
                m_playMan = player.GetComponent<PlayerManager>();
                if (!player.GetComponent<PlayerManager>().Startup(this, index))
                {
                    Debug.LogError($"{player} failed startup, aborting...");
                    Destroy(this);
                    return false;
                }
                //move the player to the spawn point
                if (m_playerSpawn)
                    m_playMan.transform.SetPositionAndRotation(m_playerSpawn.position, Quaternion.identity);
                else
                    m_playMan.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                m_playMan.transform.SetParent(transform);

                player.GetComponent<PlayerControls>().GetPlayerAnimator.SetInteger("Player", (int)index);

                //Setup the hazard manager
                if (m_debugging) Debug.Log("Getting HazardManager");
                m_hazMan = GetComponent<HazardManager>();
                if (!m_hazMan.Startup())
                {
                    //failure - impossible
                    Debug.LogError($"{m_hazMan} has failed startup, aborting...");
                    Destroy(this);
                    return false;
                }

                //Get particle spawner
                m_parSper = GetComponent<ParticleSpawner>();

                //Get EnemySpawner
                if (m_debugging) Debug.Log("Getting enemy spawnner.");
                m_enSper = m_playMan.GetComponent<EnemySpawner>();
                if (!m_enSper.Initialize())
                {
                    Debug.LogError($"{m_enSper} failed startup, aborting...");
                    Destroy(this);
                    return false;
                }

                //passed
                if (m_debugging) Debug.Log($"{this} started successfully.");
                return true;
            }
            /// <summary>
            /// Sets up the player and makes sure they have the correct components.
            /// </summary>
            /// <param name="index"></param>
            /// <param name="input"></param>
            /// <returns></returns>
            /*public bool ReadyPlayer(int index, PlayerInput input)
            {
                if (m_debugging) Debug.Log($"A player has joined, begin preperation.");

                if (m_debugging) Debug.Log($"Getting player manager.");
                //get PlayerManager
                m_playMan = input.GetComponent<PlayerManager>();

                //ensure the player has loaded correctly
                if (m_playMan == null)
                {
                    Debug.LogError($"Invalid player loaded, no PlayerManager found for player {index + 1}, Aborting.");
                    Destroy(this);
                    return false;
                }
                if (!m_playMan.Startup(this, index))
                {
                    Debug.LogError($"{m_playMan} failed startup, aborting...");
                    Destroy(this);
                    return false;
                }


                if (m_debugging) Debug.Log("Getting enemy spawnner.");
                //Get EnemySpawner
                m_enSper = input.GetComponent<EnemySpawner>();

                //check if found and run startup
                if (m_enSper == null)
                {
                    Debug.LogError($"No enemy spawner found for player {index + 1}, Aborting.");
                    Destroy(this);
                    return false;
                }
                if (!m_enSper.Initialize(m_manager))
                {
                    Debug.LogError($"{m_enSper} failed startup, aborting...");
                    Destroy(this);
                    return false;
                }

                if (m_debugging) Debug.Log("Getting point tracker.");
                m_pointTracker = GetComponentInChildren<PointFollower>();

                if (m_debugging) Debug.Log("Setting up point tracker");
                if(m_pointTracker == null)
                {
                    Debug.LogWarning("AI tracker not found.");
                }
                else if (m_pointTracker.Init(m_playMan.transform))
                {
                    Debug.LogWarning("Point tracker failed to initialize correctly");
                }
                m_playMan.GetComponentInChildren<PointerArrow>().Target = m_pointTracker.transform;

                if (m_debugging) Debug.Log($"Player {index + 1} has joined.");

                m_playMan.GetTaskManager.AddTask(new(TaskType.Area, 5));

                //move the player to the spawn point
                if (m_playerSpawn)
                    m_playMan.transform.SetPositionAndRotation(m_playerSpawn.position, Quaternion.identity);
                else
                    m_playMan.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                return true;
            }*/
            private void Update()
            {
                if (GameManager.Instance.isPlaying)
                {
                    //Giving cards & tasks cannot be done while the player has cards in their hand
                    GameManager.Instance.GivePlayerCards(m_playMan);
                    GameManager.Instance.GivePlayerTasks(m_playMan);
                    if (!m_playMan.CardsInHand)
                    {
                        //update any timer tasks
                        m_playMan.GetTaskManager.UpdateTimers(false);
                    }
                }
            }
            /// <summary>
            /// Starts a control point
            /// </summary>
            /// <param name="task"></param>
            /// <returns></returns>
            public bool StartControlPoint(Task task)
            {
                int rnd = Random.Range(0, m_controlPoints.Count);

                if (m_controlPoints[rnd].Init(task))
                {
                    m_playMan.GetUI.GetPointer.GeneratePath(m_controlPoints[rnd].transform);
                    m_controlPoints[rnd].AttachFunctionToStarted(() => m_playMan.GetUI.GetPointer.gameObject.SetActive(false));
                    m_controlPoints[rnd].AttachFunctionToStopped(() => m_playMan.GetUI.GetPointer.gameObject.SetActive(true));
                    return true;
                }
                else
                    return false;
            }
            /// <summary>
            /// Starts a sequence
            /// </summary>
            /// <param name="task"></param>
            /// <returns></returns>
            public bool StartSequence(Task task)
            {
                int rnd = Random.Range(0, m_sequences.Count);

                return m_sequences[rnd].Init(task);
            }
        }
    }
}