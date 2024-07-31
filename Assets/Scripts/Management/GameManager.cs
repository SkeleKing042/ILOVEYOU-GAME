using UnityEngine;
using UnityEngine.InputSystem;

namespace ILOVEYOU
{
    using Cards;
    using Player;
    namespace Management
    {

        public class GameManager : MonoBehaviour
        {
            //Management
            private PlayerManager[] m_playMen = new PlayerManager[2];
            public bool ReadyForPlay { get { return m_playMen[0] != null && m_playMen[1] != null; } }
            private CardManager m_cardMan;
            [Header("Tasks & Cards")]
            [Tooltip("The maximum number of tasks a player can have.")]
            [SerializeField] private int m_maxTaskCount;
            [Tooltip("The number of cards shown to the player.\nPLEASE KEEP AT 3")]
            [SerializeField] private int m_numberOfCardsToGive = 3;
            [Header("Difficulty")]
            [SerializeField] private float m_difficultySpeed;
            [SerializeField] private float m_timePerStage;
            [SerializeField] private int m_difficultyCap;
            private float m_timer;
            public int GetDifficulty { get { return (int)(m_timer / m_timePerStage); } }
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
                m_playMen[index] = input.GetComponent<PlayerManager>();
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
                if (ReadyForPlay)
                {
                    for (int i = 0; i < m_playMen.Length; i++)
                    {
                        if (m_playMen[i].NumberOfTasks < m_maxTaskCount)
                        {
                            //Change for random generation
                            Debug.Log($"Giving player {i + 1} a task.");
                            m_playMen[i].AddTask(TaskType.Time, 10);
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
                    }
                    if(GetDifficulty < m_difficultyCap)
                    m_timer += Time.deltaTime * m_difficultySpeed;
                }
                Debug.Log($"Current difficulty {GetDifficulty}.");
            }
        }
    }
}