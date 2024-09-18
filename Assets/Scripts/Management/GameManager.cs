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
            //static stuff
            public static GameManager Instance { get; private set; }
            private static Vector2 m_score;
            public Vector2 GetScore { get { return m_score; } }
            public static void ResetScore() { m_score = Vector2.zero; }

            [SerializeField] private bool m_debugging;

            //Other managers
            [SerializeField] private LevelManager m_levelTemplate;
            private List<LevelManager> m_levelManagers = new();
            private CardManager m_cardMan;
            public int NumberOfPlayers
            {
                get
                {
                    int count = 0;
                    foreach (LevelManager manager in m_levelManagers)
                    {
                        if (manager.hasPlayer)
                            count++;
                    }
                    return count;
                }
            }

            //Game settings
            [Header("Settings")]
            [SerializeField] private float m_roundStartCountdown;
            public bool isPlaying { get { return enabled; } }
            //Game rules
            [Header("Difficulty")]
            //[SerializeField] private float m_timePerStage;
            [SerializeField] private int m_difficultyCap;
            private float m_timer;
            private float m_spawnTimer = 0;
            [SerializeField] private AnimationCurve m_spawnTime;
            public int GetDifficulty { get { return (int)m_timer; } }
            public float PercentToMaxDiff { get { return (float)GetDifficulty / (float)m_difficultyCap; } }

            [Header("Tasks & Cards")]
            [Tooltip("The maximum number of tasks a player can have.")]
            [SerializeField] private int m_maxTaskCount;
            [Tooltip("The number of cards shown to the player.\nPLEASE KEEP AT 3")]
            [SerializeField] private int m_numberOfCardsToGive = 3;
            [SerializeField] private Task[] m_taskList;
            public Task[] GetTasks { get { return m_taskList; } }

            [Header("UI")]
            [Header("Main Menu")]
            [SerializeField] private GameObject m_mainMenuUI;
            [SerializeField] private TextMeshProUGUI m_reporterTextBox;
            [SerializeField] private Button m_startButton;

            [Header("In-Game Menu")]
            [SerializeField] private GameObject m_InGameSharedUI;
            [SerializeField] private TextMeshProUGUI m_timerText;

            [Header("Victory Menu")]
            [SerializeField] private GameObject m_winScreen;
            [SerializeField] private TextMeshProUGUI m_winText;
            [SerializeField] private Button m_restartButton;

            [Header("Events - mostly for visuals and sounds")]
            [SerializeField] private UnityEvent m_onGameStart;
            [SerializeField] private UnityEvent m_onStartError;
            [SerializeField] private UnityEvent m_onGameEnd;
            [SerializeField] private UnityEvent m_onTaskAssignment;
            private void Awake()
            {
                if(!m_debugging)
                BeginSetup();
            }
            public void BeginSetup()
            {
                Time.timeScale = 1f;
                //Singleton setup
                Instance = this;

                //Make sure that the other management scripts work
                if (m_debugging) Debug.Log("Game manager starting.");

                //Set card manager
                if (m_debugging) Debug.Log("Getting CardManager");
                m_cardMan = GetComponent<CardManager>();
                if (!m_cardMan.Startup())
                {
                    Debug.LogError($"{m_cardMan} has failed, aborting...");
                    Destroy(this);
                    return;
                }

                if (m_debugging) Debug.Log("Attempting to start the game.");
                GameObject[] players = ControllerManager.Instance.JoinPlayers();
                for (int i = 0; i < players.Length; i++)
                {
                    //camera setup
                    //spawn a new level
                    if (m_debugging) Debug.Log("Initializing level");
                    LevelManager newLevel = Instantiate(m_levelTemplate);
                    //start level manager
                    if (!newLevel.Startup(players[i], (uint)i))
                    {
                        //manager failed
                        Debug.LogError($"{m_levelManagers[i]} has failed, aborting...");
                        Destroy(this);
                        return;
                    }
                    m_levelManagers.Add(newLevel);
                    //give players the first task in the list to start with
                    newLevel.GetPlayer.GetTaskManager.AddTask(m_taskList[0]);
                }
                m_onGameStart.Invoke();

                //passed
                if (m_debugging) Debug.Log($"Game started successfully!\nStarting game in {m_roundStartCountdown}.");
                if (!enabled)
                {
                    StartCoroutine(_startGame());
                }
            }
            private IEnumerator _startGame()
            {
                yield return new WaitForSecondsRealtime(m_roundStartCountdown);
                enabled = true;
                foreach(var player in m_levelManagers)
                {
                    player.GetPlayer.GetControls.enabled = true;
                }
            }
/*            public void AttemptStartGame()
            {
                if (NumberOfPlayers >= 2)
                {

                    if (m_debugging) Debug.Log("There's enough players, starting game.");
                    m_mainMenuUI.SetActive(false);
                    m_InGameSharedUI.SetActive(true);
                    foreach (LevelManager manager in m_levelManagers)
                    {
                        manager.GetPlayer.GetControls.enabled = true;
                    }
                }
                else
                {
                    if (m_debugging) Debug.Log("There aren't enough players");
                    m_onStartError.Invoke();
                }
            }*/
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
                switch (playerNum)
                {
                    case 0:
                        m_score.x++;
                        break;
                    case 1:
                        m_score.y++;
                        break;
                }
                m_winText.text = $"Player {playerNum + 1} wins!\nScore: {m_score.x} - {m_score.y}";
                EventSystem.current.SetSelectedGameObject(m_restartButton.gameObject);

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
            public void LoadScene(string sceneName)
            {
                SceneManager.LoadSceneAsync(sceneName);
            }

            private void Update()
            {
                //update spawn timer
                if (m_spawnTimer <= 0)
                {
                    foreach(var level in m_levelManagers)
                    {
                        level.GetSpawner.SpawnEnemyWave();
                    }
                    m_spawnTimer = m_spawnTime.Evaluate(m_timer / m_difficultyCap);
                }
                else
                {
                    m_spawnTimer -= 1f * Time.deltaTime;
                }

                m_timer += Time.deltaTime;
                Color timeColor = new(1.0f - Mathf.Clamp(PercentToMaxDiff, 0, 1), 1.0f, 1.0f - Mathf.Clamp(PercentToMaxDiff, 0, 1));
                m_timerText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(timeColor)}>{(int)m_timer}</color>";
                //Debug.Log($"Current difficulty {GetDifficulty}.");
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
                        rnd = Random.Range(1, m_taskList.Length);
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