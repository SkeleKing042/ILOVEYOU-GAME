using UnityEngine;
using UnityEngine.InputSystem;
using ILOVEYOU.Cards;
using ILOVEYOU.EnemySystem;
using ILOVEYOU.Hazards;
using ILOVEYOU.Player;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace ILOVEYOU
{

    namespace Management
    {

        public class GameManager : MonoBehaviour
        {
            [SerializeField] private bool m_debugging;
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
            [SerializeField] private Task[] m_taskList;

            [Header("Difficulty")]
            [SerializeField] private float m_timePerStage;
            [SerializeField] private int m_difficultyCap;
            private float m_timer;
            private float[] m_spawnTimer = new float[] { 5f, 5f };
            [SerializeField] private float[] m_spawnTime = new float[] { 5f, 5f };
            public int GetDifficulty { get { return (int)Mathf.Clamp(m_timer / m_timePerStage, 0, m_difficultyCap); } }

            [Header("UI")]
            [SerializeField] private bool m_useUI = true;
            [SerializeField] private GameObject m_mainMenuUI;
            [SerializeField] private GameObject m_InGameSharedUI;
            [SerializeField] private TextMeshProUGUI m_timerText;
            [SerializeField] private GameObject m_winScreen;

            [Header("Events - mostly for visuals and sounds")]
            [SerializeField] private UnityEvent m_onGameStart;
            [SerializeField] private UnityEvent m_onStartError;
            [SerializeField] private UnityEvent m_onGameEnd;
            [SerializeField] private UnityEvent m_onTaskAssignment;
            private void Awake()
            {
                Time.timeScale = 1f;
                //Make sure that the other management scripts work
                if (m_debugging) Debug.Log("Game starting.");
                if (m_debugging) Debug.Log("Getting CardManager");
                m_cardMan = GetComponent<CardManager>();
                if (m_cardMan == null)
                {
                    if (m_debugging) Debug.LogError($"CardManager not found, Aborting. Please add the CardManager script to {gameObject} and try again.");
                    Destroy(this);
                    return;
                }
                if (m_debugging) Debug.Log("Starting CardManager");
                if (!m_cardMan.Startup())
                {
                    Destroy(this);
                    return;
                }

                if (m_debugging) Debug.Log("Getting HazardManager");
                m_hazMan = GetComponent<HazardManager>();
                if (m_hazMan == null)
                {
                    Debug.LogError($"HazardManager not found, Aborting. Please add the CardManager script to {gameObject} and try again.");
                    Destroy(this);
                    return;
                }
                if (m_debugging) Debug.Log("Starting HazardManager");
                if (!m_hazMan.Startup())
                {
                    Destroy(this);
                    return;
                }
            }
            public void OnPlayerJoined(PlayerInput input)
            {
                if (input.currentControlScheme != "Gamepad")
                {
                    input.DeactivateInput();
                    Destroy(input);
                    return;
                }
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
                if (!m_playMen[index].Startup(this, index))
                {
                    Destroy(this);
                    return false;
                }
                if (m_debugging) Debug.Log($"Player {index + 1} has joined.");
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
            /// <summary>
            /// function that does the setup for when a player loses
            /// </summary>
            /// <param name="player">player manager of the losing player</param>
            public void PlayerDeath(PlayerManager player)
            {
                m_winScreen.SetActive(true);

                //winning player
                int playerNum = (player == m_playMen[0]) ? 1 : 0;

                m_winScreen.GetComponentInChildren<TextMeshProUGUI>().text = $"Player {playerNum + 1} wins!";
                EventSystem.current.SetSelectedGameObject(m_winScreen.transform.GetChild(2).gameObject);

                StartCoroutine(_coolSlowMo());
                m_onGameEnd.Invoke();
            }

            private IEnumerator _coolSlowMo()
            {
                while (Time.timeScale != 0)
                {
                    Time.timeScale = Mathf.MoveTowards(Time.timeScale, 0f, Time.unscaledDeltaTime);
                    yield return new WaitForEndOfFrame();
                }
                yield return null;
            }
            /// <summary>
            /// reloads the current scene
            /// </summary>
            public void RestartScene()
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            private void Update()
            {
                //Give players tasks
                if (m_isPlaying)
                {
                    for (int i = 0; i < m_playMen.Length; i++)
                    {
                        //Giving cards & tasks cannot be done while the player has cards in their hand
                        //Giving cards
                        if (m_playMen[i].GetTaskManager.TaskCompletionPoints > 0 && !m_playMen[i].CardsInHand)
                        {
                            //hand out cards to the player
                            if (m_debugging) Debug.Log($"Player {i + 1} has completed a task, dealing cards.");
                            m_playMen[i].GetTaskManager.TaskCompletionPoints--;
                            m_playMen[i].CollectHand(m_cardMan.DispenseCards(m_numberOfCardsToGive).ToArray());
                        }
                        //Giving tasks
                        if (m_playMen[i].GetTaskManager.NumberOfTasks < m_maxTaskCount && !m_playMen[i].CardsInHand)
                        {
                            //Change for random generation
                            if (m_debugging) Debug.Log($"Giving player {i + 1} a task.");
                            int rnd = 0;
                            for (int c = 100; c > 0; c--)
                            {
                                rnd = Random.Range(0, m_taskList.Length);
                                //Check for no tasks of the same type
                                if (m_playMen[i].GetTaskManager.GetMatchingTasks(m_taskList[rnd].GetTaskType).Length == 0)
                                {
                                    break;
                                }
                            }
                            m_playMen[i].GetTaskManager.AddTask(m_taskList[rnd]);
                            m_onTaskAssignment.Invoke();
                        }
                        if (!m_playMen[i].CardsInHand)
                        {
                            //update any timer tasks
                            m_playMen[i].GetTaskManager.UpdateTimers(false);
                        }

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
                    m_timer += Time.deltaTime;

                    if (m_useUI)
                        m_timerText.text = ((int)m_timer).ToString();

                    //Debug.Log($"Current difficulty {GetDifficulty}.");
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
                        m_onGameStart.Invoke();
                    }
                }
                else
                {
                    if (m_debugging) Debug.Log("There aren't enough players");
                    m_onStartError.Invoke();
                }
            }
        }
    }
}