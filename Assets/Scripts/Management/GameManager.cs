using UnityEngine;
using UnityEngine.InputSystem;
using ILOVEYOU.Cards;
using ILOVEYOU.Environment;
using ILOVEYOU.EnemySystem;
using ILOVEYOU.Hazards;
using ILOVEYOU.Player;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

namespace ILOVEYOU
{

    namespace Management
    {
        [RequireComponent(typeof(CardManager))]
        public class GameManager : MonoBehaviour
        {
            public static GameManager Instance { get; private set; }

            [SerializeField] private bool m_debugging;
            [SerializeField] private LevelManager m_levelTemplate;
            //Other managers
            private List<LevelManager> m_levelManagers = new();
            private CardManager m_cardMan;
            public int NumberOfPlayers { get
                {
                    int count = 0;
                    foreach(LevelManager manager in m_levelManagers)
                    {
                        if (manager.hasPlayer)
                            count++;
                    }
                    return count;
                } }

            [HideInInspector] public bool isPlaying;

            [Header("Tasks & Cards")]
            [Tooltip("The maximum number of tasks a player can have.")]
            [SerializeField] private int m_maxTaskCount;
            [Tooltip("The number of cards shown to the player.\nPLEASE KEEP AT 3")]
            [SerializeField] private int m_numberOfCardsToGive = 3;
            [SerializeField] private Task[] m_taskList;
            public Task[] GetTasks { get { return m_taskList; } }

            [Header("Difficulty")]
            [SerializeField] private float m_timePerStage;
            [SerializeField] private int m_difficultyCap;
            private float m_timer;
            private float m_spawnTimer = 5f;
            [SerializeField] private float m_spawnTime = 5f;
            public int GetDifficulty { get { return (int)Mathf.Clamp(m_timer / m_timePerStage, 0, m_difficultyCap); } }

            [Header("UI")]
            [SerializeField] private bool m_useUI = true;
            [SerializeField] private GameObject m_mainMenuUI;
            [SerializeField] private GameObject m_InGameSharedUI;
            [SerializeField] private TextMeshProUGUI m_timerText;
            [SerializeField] private GameObject m_winScreen;

            [Header("Main Menu")]
            [SerializeField] private TextMeshProUGUI m_reporterTextBox;
            [SerializeField] private Button m_startButton;

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

                //card manager
                if (m_debugging) Debug.Log("Getting CardManager");
                m_cardMan = GetComponent<CardManager>();
                if (!m_cardMan.Startup())
                {
                    Debug.LogError($"{m_cardMan} has failed, aborting...");
                    Destroy(this);
                    return;
                }

                //passed
                if (m_debugging) Debug.Log("Game started successfully! Yippee!!");
            }
            public void OnPlayerJoined(PlayerInput input)
            {
                if (m_debugging) Debug.Log("Initializing level");
                LevelManager newLevel = Instantiate(m_levelTemplate);
                m_levelManagers.Add(newLevel);


                int index = m_levelManagers.Count - 1;
                /*//too small
                if (m_levelManagers.Length < 2)
                {
                    Debug.LogError($"Not enough level managers found. Please ensure that there are at least 2. Currently there are {m_levelManagers.Length}.");
                    Destroy(this);
                    return;
                foreach (LevelManager lMan in m_levelManagers)
                {
                }
                }*/
                //start managers
                if (!m_levelManagers[index].Startup(this, input, index))
                {
                    //manager failed
                    Debug.LogError($"{m_levelManagers} has failed, aborting...");
                    Destroy(this);
                    return;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="player"></param>
            /// <returns>the opposite player</returns>
            public PlayerManager GetOtherPlayer(PlayerManager player)
            {
                if (player == m_levelManagers[0].GetPlayer)
                {
                    return m_levelManagers[1].GetPlayer;
                }
                else
                {
                    return m_levelManagers[0].GetPlayer;
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
                int playerNum = (player == m_levelManagers[0].GetPlayer) ? 1 : 0;

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
                if (isPlaying)
                {
                    //update spawn timer
                    if (m_spawnTimer <= 0)
                    {
                        m_levelManagers[0].GetSpawner.SpawnEnemyWave();
                        m_levelManagers[1].GetSpawner.SpawnEnemyWave();
                        m_spawnTimer = m_spawnTime; //Possible TODO: add formula to scale spawn time with difficulty
                    }
                    else
                    {
                        m_spawnTimer -= 1f * Time.deltaTime;
                    }

                    m_timer += Time.deltaTime;

                    if (m_useUI)
                        m_timerText.text = ((int)m_timer).ToString();

                    //Debug.Log($"Current difficulty {GetDifficulty}.");
                }
                else
                {
                    string displayText = "Connect contorollers.\n";
                    if (NumberOfPlayers >= 2)
                    {
                        m_startButton.interactable = true;
                        displayText = "";
                    }
                    if(m_useUI) m_reporterTextBox.text = $"{displayText}{NumberOfPlayers} player(s) connected.";
                }
            }
            public void AttemptStartGame()
            {
                if (NumberOfPlayers >= 2)
                {
                    isPlaying = true;
                    if (m_useUI)
                    {
                        m_mainMenuUI.SetActive(false);
                        m_InGameSharedUI.SetActive(true);
                        foreach(LevelManager manager in m_levelManagers)
                        {
                            manager.GetPlayer.GetControls.enabled = true;
                        }
                        m_onGameStart.Invoke();
                    }
                }
                else
                {
                    if (m_debugging) Debug.Log("There aren't enough players");
                    m_onStartError.Invoke();
                }
            }
            public void GivePlayerCards(PlayerManager player)
            {
                //Giving cards
                if (player.GetTaskManager.TaskCompletionPoints > 0 && !player.CardsInHand)
                {
                    //hand out cards to the player
                    if (m_debugging) Debug.Log($"Player {player.GetPlayerID} has completed a task, dealing cards.");
                    player.GetTaskManager.TaskCompletionPoints--;
                    player.CollectHand(m_cardMan.DispenseCards(m_numberOfCardsToGive).ToArray());
                }
            }
            public void GivePlayerTasks(PlayerManager player)
            {
                //Giving tasks
                if (!player.CardsInHand && player.GetTaskManager.NumberOfTasks < m_maxTaskCount)
                {
                    //Change for random generation
                    if (m_debugging) Debug.Log($"Giving player {player.GetPlayerID} a task.");
                    int rnd = 0;
                    for (int c = 100; c > 0; c--)
                    {
                        rnd = Random.Range(0, m_taskList.Length);
                        //Check for no tasks of the same type
                        if (player.GetTaskManager.GetMatchingTasks(m_taskList[rnd].GetTaskType).Length == 0)
                        {
                            break;
                        }
                    }
                    player.GetTaskManager.AddTask(m_taskList[rnd]);
                    m_onTaskAssignment.Invoke();
                }
            }

            public void QuitApp()
            {
#if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
#else
                Application.Quit();
#endif
            }
        }
    }
}