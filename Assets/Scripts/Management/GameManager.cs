using UnityEngine;
using UnityEngine.InputSystem;

namespace ILOVEYOU
{
    using Cards;
    using ILOVEYOU.EnemySystem;
    using ILOVEYOU.Hazards;
    using Player;
    using TMPro;

    namespace Management
    {

        public class GameManager : MonoBehaviour
        {
            //Other managers
            private PlayerManager[] m_playMen = new PlayerManager[2];
            private EnemySpawner[] m_EnSper = new EnemySpawner[2];
            private HazardManager m_hazMan;
            private CardManager m_cardMan;

            [Header("Players")]
            [SerializeField] private Transform[] m_playerSpawns = new Transform[2];
            public bool ReadyForPlay { get { return m_playMen[0] != null && m_playMen[1] != null; } }
            private bool m_isPlaying;

            [Header("Tasks & Cards")]
            [Tooltip("The maximum number of tasks a player can have.")]
            [SerializeField] private int m_maxTaskCount;
            [Tooltip("The number of cards shown to the player.\nPLEASE KEEP AT 3")]
            [SerializeField] private int m_numberOfCardsToGive = 3;

            [Header("Difficulty")]
            [SerializeField] private float m_timePerStage;
            [SerializeField] private int m_difficultyCap;
            private float m_timer;
            private float[] m_spawnTimer = new float[] { 5f, 5f };
            [SerializeField] private float[] m_spawnTime = new float[] {5f, 5f};
            public int GetDifficulty { get { return (int)(m_timer / m_timePerStage); } }

            [Header("UI")]
            [SerializeField] private bool m_useUI = true;
            [SerializeField] private GameObject m_mainMenuUI;
            [SerializeField] private GameObject m_InGameSharedUI;
            [SerializeField] private TextMeshProUGUI m_timerText;
            private void Awake()
            {
                //Make sure that the other management scripts work
                Debug.Log("Game starting.");
                Debug.Log("Getting CardManager");
                m_cardMan = GetComponent<CardManager>();
                if (m_cardMan == null)
                {
                    Debug.LogError($"CardManager not found, Aborting. Please add the CardManager script to {gameObject} and try again.");
                    Destroy(this);
                    return;
                }
                Debug.Log("Starting CardManager");
                if (!m_cardMan.Startup())
                {
                    Destroy(this);
                    return;
                }

                Debug.Log("Getting HazardManager");
                m_hazMan = GetComponent<HazardManager>();
                if (m_hazMan == null)
                {
                    Debug.LogError($"HazardManager not found, Aborting. Please add the CardManager script to {gameObject} and try again.");
                    Destroy(this);
                    return;
                }
                Debug.Log("Starting HazardManager");
                if (!m_hazMan.Startup())
                {
                    Destroy(this);
                    return;
                }
            }
            public void OnPlayerJoined(PlayerInput input)
            {
                if (m_playMen[0] == null)
                {
                    ReadyPlayer(0, input);
                }
                else
                {
                    ReadyPlayer(1, input);
                }
            }
            /// <summary>
            /// Sets up the player and makes sure they have the correct components.
            /// </summary>
            /// <param name="index"></param>
            /// <param name="input"></param>
            /// <returns></returns>
            private bool ReadyPlayer(int index, PlayerInput input)
            {
                //get PlayerManager
                m_playMen[index] = input.GetComponent<PlayerManager>();
                //Get and initialize EnemySpawner
                m_EnSper[index] = input.GetComponent<EnemySpawner>();
                m_EnSper[index].Initialize(this);

                if (m_playMen[index] == null)
                {
                    Debug.LogError($"Invalid player loaded, no PlayerManager found for player {index + 1}, Aborting.");
                    Destroy(this);
                    return false;
                }
                if (!m_playMen[index].Startup())
                {
                    Destroy(this);
                    return false;
                }
                Debug.Log($"Player {index + 1} has joined.");
                if (m_playerSpawns[index])
                    m_playMen[index].transform.SetPositionAndRotation(m_playerSpawns[index].position, Quaternion.identity);
                else
                    m_playMen[index].transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                return true;
            }
            public PlayerManager GetPlayer(int index)
            {
                return m_playMen[index];
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="player"></param>
            /// <returns>the opposite player</returns>
            public PlayerManager GetOtherPlayer(PlayerManager player)
            {
                if (player == m_playMen[0])
                {
                    return m_playMen[1];
                }
                else
                {
                    return m_playMen[0];
                }
            }

            private void Update()
            {
                //Give players tasks
                if (m_isPlaying)
                {
                    for (int i = 0; i < m_playMen.Length; i++)
                    {
                        if (m_playMen[i].NumberOfTasks < m_maxTaskCount)
                        {
                            //Change for random generation
                            Debug.Log($"Giving player {i + 1} a task.");
                        }
                        if (m_playMen[i].TaskCompletionPoints > 0 && !m_playMen[i].CardsInHand)
                        {
                            //hand out cards to the player
                            Debug.Log($"Player {i + 1} has completed a task, dealing cards.");
                            m_playMen[i].TaskCompletionPoints--;
                            m_playMen[i].CollectHand(m_cardMan.DispenseCards(m_numberOfCardsToGive).ToArray());
                        }
                        //update any timer tasks
                        m_playMen[i].UpdateTimers(false);

                        //update spawn timer
                        if (m_spawnTimer[i] <= 0)
                        {
                            m_EnSper[i].SpawnEnemyWave();
                            m_spawnTimer[i] = m_spawnTime[i]; //Possible TODO: add formula to scale spawn time with difficulty
                        }
                        else
                        {
                            m_spawnTimer[i] -= 1f * Time.deltaTime;
                        }
                    }
                    if(GetDifficulty < m_difficultyCap)
                    m_timer += Time.deltaTime;

                    if(m_useUI)
                        m_timerText.text = ((int)m_timer).ToString();

                Debug.Log($"Current difficulty {GetDifficulty}.");
                }
            }
            public void AttemptStartGame()
            {
                if (ReadyForPlay)
                {
                    m_isPlaying = true;
                    if (m_useUI)
                    {
                        m_mainMenuUI.SetActive(false);
                        m_InGameSharedUI.SetActive(true);
                    }
                }
                else
                {
                    Debug.Log("There aren't enough players");
                }
            }
        }
    }
}