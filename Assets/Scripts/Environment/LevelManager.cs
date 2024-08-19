using ILOVEYOU.EnemySystem;
using ILOVEYOU.Hazards;
using ILOVEYOU.Management;
using ILOVEYOU.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ILOVEYOU
{
    namespace Environment
    {

        public class LevelManager : MonoBehaviour
        {
            private GameManager m_manager;
            public GameManager GetManager { get { return m_manager; } }
            [SerializeField] private bool m_debugging;
            private PlayerManager m_playMan;
            public bool hasPlayer { get { return m_playMan != null; } }
            public PlayerManager GetPlayer { get { return m_playMan; } }
            private EnemySpawner m_enSper;
            public EnemySpawner GetSpawner { get { return m_enSper; } }
            private HazardManager m_hazMan;
            [Header("Players")]
            [SerializeField] private Transform m_playerSpawn;

            /// <summary>
            /// Setup of scripts vars
            /// </summary>
            /// <param name="gm"></param>
            /// <returns></returns>
            public bool Startup(GameManager gm)
            {
                m_manager = gm;

                //Setup the hazard manager
                if (m_debugging) Debug.Log("Getting HazardManager");
                m_hazMan = GetComponent<HazardManager>();
                if (m_hazMan == null)
                {
                    Debug.LogError($"HazardManager not found, Aborting. Please add the CardManager script to {gameObject} and try again.");
                    Destroy(this);
                    return false;
                }
                if (m_debugging) Debug.Log("Starting HazardManager");
                if (!m_hazMan.Startup())
                {
                    Destroy(this);
                    return false;
                }
                return true;
            }
            /// <summary>
            /// Sets up the player and makes sure they have the correct components.
            /// </summary>
            /// <param name="index"></param>
            /// <param name="input"></param>
            /// <returns></returns>
            public bool ReadyPlayer(int index, PlayerInput input)
            {
                //get PlayerManager
                m_playMan = input.GetComponent<PlayerManager>();
                //Get and initialize EnemySpawner
                m_enSper = input.GetComponent<EnemySpawner>();
                m_enSper.Initialize(m_manager);

                //ensure the player has loaded correctly
                if (m_playMan == null)
                {
                    Debug.LogError($"Invalid player loaded, no PlayerManager found for player {index + 1}, Aborting.");
                    Destroy(this);
                    return false;
                }
                if (!m_playMan.Startup(this, index))
                {
                    Destroy(this);
                    return false;
                }
                if (m_debugging) Debug.Log($"Player {index + 1} has joined.");

                //move the player to the spawn point
                if (m_playerSpawn)
                    m_playMan.transform.SetPositionAndRotation(m_playerSpawn.position, Quaternion.identity);
                else
                    m_playMan.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                return true;
            }
            private void Update()
            {
                if (m_manager.isPlaying)
                {
                    //Giving cards & tasks cannot be done while the player has cards in their hand
                    m_manager.GivePlayerCards(m_playMan);
                    m_manager.GivePlayerTasks(m_playMan);
                    if (!m_playMan.CardsInHand)
                    {
                        //update any timer tasks
                        m_playMan.GetTaskManager.UpdateTimers(false);
                    }
                }
            }
        }
    }
}