using UnityEngine;
using ILOVEYOU.Cards;
using ILOVEYOU.Environment;
using ILOVEYOU.EnemySystem;
using ILOVEYOU.Player;
using System.Collections;
using UnityEngine.Events;
using UnityEditor;
using System.Collections.Generic;
using ILOVEYOU.UI;
using ILOVEYOU.MainMenu;

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
            public static Vector2 GetScore { get { return m_score; } }
            public static void ResetScore() { m_score = Vector2.zero; }
            ///////////////////////////////////////////////////////////////////////////////////////////////////
            [Header("Settings")]
            [SerializeField] private GameSettings m_settings;
            [SerializeField] private bool m_devMode;
            [SerializeField] private float m_roundStartCountdown;
            [Header("References")]
            [SerializeField] private ControllerManager m_controllerManagerPrefab;
            [SerializeField] private LevelManager m_levelTemplate;
            private List<LevelManager> m_levelManagers = new();
            private CardManager m_cardMan;

            //Game info
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
            public bool isPlaying { get { return enabled; } }
            //Difficulty
            private float m_timer;
            private float m_spawnTimer = 0;
            public int GetCurrentDifficulty { get { return (int)m_timer; } }
            public float PercentToMaxDiff { get { return (float)GetCurrentDifficulty / (float)GameSettings.Current.GetDiffCap; } }

            [Header("UI")]
            [SerializeField] private GameUI m_gameUI;

            [Header("Events - mostly for visuals and sounds")]
            [SerializeField] private UnityEvent m_onGameStart;
            [SerializeField] private UnityEvent m_onStartError;
            [SerializeField] private UnityEvent m_onGameEnd;
            [SerializeField] private UnityEvent m_onTaskAssignment;
            private void Awake()
            {
                m_settings.Assign();
                //check for the input manager
                if (!ControllerManager.Instance)
                {
                    Debug.Log("Instancing controller manager");
                    Instantiate(m_controllerManagerPrefab);
                }
                if(!m_devMode)
                BeginSetup();
            }
            public void BeginSetup()
            {
                Time.timeScale = 1f;
                //Singleton setup
                Instance = this;
                if(!GameSettings.Current)
                {
                    Debug.LogError("No settings loaded, aborting!");
                    Destroy(gameObject);
                    return;
                }

                //Make sure that the other management scripts work
                Debug.Log("Game manager starting.");


                //Set card manager
                Debug.Log("Getting CardManager");
                m_cardMan = GetComponent<CardManager>();
                if (!m_cardMan.Startup())
                {
                    Debug.LogError($"{m_cardMan} has failed, aborting...");
                    Destroy(this);
                    return;
                }

                Debug.Log("Attempting to start the game.");
                GameObject[] players = ControllerManager.Instance.JoinPlayers();

                //Boss data setup
                BossBar.Instances = new BossBar[players.Length];
                BossEnemy.Instances = new BossEnemy[players.Length];

                for (int i = 0; i < players.Length; i++)
                {
                    //camera setup
                    //spawn a new level
                    Debug.Log("Initializing level");
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
                }

                //Boss setup
                m_onGameStart.Invoke();

                //passed
                Debug.Log($"Game started successfully!\nStarting game in {m_roundStartCountdown}.");
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
                    //give players the first task in the list to start with
                    player.GetPlayer.GetTaskManager.AddTask(GameSettings.Current.GetTasks[0]);
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
            public PlayerManager GetPlayer(int index)
            {
                return m_levelManagers[index].GetPlayer;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="player"></param>
            /// <returns>the opposite player</returns>
            public PlayerManager[] GetOtherPlayers(PlayerManager player)
            {
                List<PlayerManager> players = new();
                foreach(LevelManager level in m_levelManagers)
                {
                    if (level.GetPlayer != player)
                    {
                        players.Add(level.GetPlayer);
                    }
                }

                return players.ToArray();
            }
            /// <summary>
            /// function that does the setup for when a player loses
            /// </summary>
            /// <param name="player">player manager of the losing player</param>
            public void PlayerDeath(PlayerManager player)
            {

                player.GetComponent<Animator>().SetTrigger("Death");
                player.GetControls.GetPlayerAnimator.SetTrigger("Death");

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
                
                //disables player movement and enemy spawner
                foreach (var levelPlayer in m_levelManagers)
                {
                    levelPlayer.GetSpawner.DisableAllEnemies();
                    levelPlayer.GetSpawner.enabled = false;
                    levelPlayer.GetPlayer.GetControls.Zero();
                    levelPlayer.GetPlayer.GetControls.enabled = false;
                }
                

                enabled = false;

                //does a cool animation
                StartCoroutine(_CoolSlowMo(playerNum));
                m_onGameEnd.Invoke();
            }
            /// <summary>
            /// does a hitstop looking animation for emphasis before showing the win screen
            /// </summary>
            private IEnumerator _CoolSlowMo(int playerNum)
            {
                Time.timeScale = .05f;

                yield return new WaitForSecondsRealtime(3f);

                //increases the time scale until it reaches 1
                while (Time.timeScale != 1f)
                {
                    Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1f, Time.unscaledDeltaTime / 2f);
                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForSecondsRealtime(8f);

                Time.timeScale = 0f;
                m_gameUI.DisplayWinScreen(playerNum + 1);

                yield return null;
            }
            public void RestartScene()
            {
                SceneLoader.Instance.RestartScene();
            }
            /// <summary>
            /// loads the set scene
            /// </summary>
            public void LoadScene(string sceneName)
            {
                SceneLoader.Instance.LoadScene(sceneName);
            }

            /// <summary>
            /// reloads the current scene
            /// </summary>
            public void LoadScene(int scene)
            {
                SceneLoader.Instance.LoadScene(scene);
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
                    m_spawnTimer = GameSettings.Current.GetSpawnTime.Evaluate(m_timer / GameSettings.Current.GetDiffCap);
                }
                else
                {
                    m_spawnTimer -= 1f * Time.deltaTime;
                }

                m_timer += Time.deltaTime;
                m_gameUI.UpdateTimer(m_timer);
                //Debug.Log($"Current difficulty {GetDifficulty}.");
            }
            public void GivePlayerCards(PlayerManager player)
            {
                //Giving cards
                if (player.GetTaskManager.TaskCompletionPoints > 0 && !player.CardsInHand)
                {
                    //hand out cards to the player
                    Debug.Log($"Player {player.GetPlayerID} has completed a task, dealing cards.");
                    player.GetTaskManager.TaskCompletionPoints--;
                    player.CollectHand(m_cardMan.DispenseCards(GameSettings.Current.GetNumberOfCardsToGive, player).ToArray());
                }
            }
            public void GivePlayerTasks(PlayerManager player)
            {
                //Giving tasks
                if (!player.CardsInHand && player.GetTaskManager.NumberOfTasks < GameSettings.Current.GetMaxTaskCount)
                {
                    //Change for random generation
                    Debug.Log($"Giving player {player.GetPlayerID} a task.");
                    int rnd = 0;
                    for (int c = 100; c > 0; c--)
                    {
                        rnd = Random.Range(1, GameSettings.Current.GetTasks.Length);
                        //Check for no tasks of the same type
                        if (player.GetTaskManager.GetMatchingTasks(GameSettings.Current.GetTasks[rnd].GetTaskType).Length == 0)
                        {
                            break;
                        }
                    }
                    player.GetTaskManager.AddTask(GameSettings.Current.GetTasks[rnd]);
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