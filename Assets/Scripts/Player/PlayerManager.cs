using ILOVEYOU.Cards;
using ILOVEYOU.EnemySystem;
using ILOVEYOU.Environment;
using ILOVEYOU.Management;
using ILOVEYOU.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace ILOVEYOU
{
    namespace Player
    {
        [RequireComponent(typeof(TaskManager))]
        [RequireComponent(typeof(EnemySpawner))]
        [RequireComponent(typeof(PlayerControls))]
        public class PlayerManager : MonoBehaviour
        {
            //player
            private PlayerControls m_playerControls;
            public PlayerControls GetControls { get { return m_playerControls; } }
            private uint m_playerID;
            public uint GetPlayerID { get { return m_playerID; } }
            private LevelManager m_levelManager;
            private bool m_paused = false;
            //public bool Paused { get { return m_paused; } }
            public LevelManager GetLevelManager { get { return m_levelManager; } }
            //tasks
            private TaskManager m_taskMan;
            public TaskManager GetTaskManager { get { return m_taskMan; } }
            //cards
            private DisruptCard[] m_cardsHeld;
            public bool CardsInHand { get { return m_cardsHeld.Length > 0; } }
            //ui
            [SerializeField] private PlayerUI m_playerUI;
            public PlayerUI GetUI => m_playerUI;

            [Header("Event - sounds and visuals")]
            [SerializeField] private UnityEvent m_onGetCards;
            [SerializeField] private UnityEvent m_onDiscardHand;
            [SerializeField] private UnityEvent<bool> m_onCardSelected;
            [SerializeField] private UnityEvent m_onBlind;
            [SerializeField] private UnityEvent m_onUnblind;
            [SerializeField] public UnityEvent OnVictory;
            public bool Startup(LevelManager manager, uint index)
            {
                Debug.Log($"Starting {this}.");
                //clear cards
                m_cardsHeld = new DisruptCard[0];
                //set id
                m_playerID = index;
                gameObject.name = $"Player {index}";
                //save manager
                m_levelManager = manager;
                //camera setup
                float plyrCount = ControllerManager.Instance.NumberOfActivePlayers;
                float spacing = 1 / plyrCount;
                //set camera scale on screen space
                Camera cam = GetComponentInChildren<Camera>();
                    cam.rect = new(spacing * index, 0, spacing, 1);
                //dodgy??
                cam.GetUniversalAdditionalCameraData().cameraStack[0].rect = cam.rect;

                Debug.Log($"Getting task manager.");
                m_taskMan = GetComponent<TaskManager>();
                if (!m_taskMan.Startup())
                {
                    Debug.LogError($"{m_taskMan} failed startup, aborting...");
                    Destroy(gameObject);
                    return false;
                }

                Debug.Log($"Getting player controls");
                m_playerControls = GetComponent<PlayerControls>();
                if (!m_playerControls.Startup())
                {
                    Debug.LogError($"{m_playerControls} failed startup, aborting...");
                    Destroy(gameObject);
                    return false;
                }

                //UI setup
                //flip hud - needs tweaking
                if (m_playerID % 2 == 1)
                {
                    GetComponent<Animator>().SetBool("Flip", true);
                }

                if (!m_playerUI.Startup((int)m_playerID))
                {
                    Debug.LogError($"{m_playerUI} failed startup, aborting...");
                    Destroy(gameObject);
                    return false;
                }

                Debug.Log($"{this} started successfully");
                return true;
            }
            #region Card Management
            /// <summary>
            /// Save a given hand as the cards held by this player
            /// </summary>
            /// <param name="cards"></param>
            public void CollectHand(DisruptCard[] cards)
            {
                Debug.Log("Hand dealt, setting up cards.");
                CancelInvoke();
                m_onGetCards.Invoke();
                //Copy the given array to this hand
                m_cardsHeld = new DisruptCard[cards.Length];
                cards.CopyTo(m_cardsHeld, 0);

                m_playerUI.GetCardDisplay.DisplayCards(m_cardsHeld);

                //To stop stockpiling, delete the cards after a set time
                Invoke("_autoSelectCard", GameSettings.Current.GetCardTimeOut);
            }
            private void _autoSelectCard()
            {
                if(CardsInHand)
                _executeSelectedCard(1);
            }
            /// <summary>
            /// Destroys the player's hand card objects
            /// </summary>
            public void DiscardHand()
            {
                if (!CardsInHand)
                    return;

                m_cardsHeld = new DisruptCard[0];
                CancelInvoke();
                m_onDiscardHand.Invoke();
                Debug.Log($"Player {m_playerID} discards hand.");
            }
            /// <summary>
            /// Takes a player's input to select a card
            /// </summary>
            /// <param name="value"></param>
            public void OnSelectCard(InputValue value)
            {
                //If the hand is empty, don't continue
                if (!CardsInHand || m_paused)
                    return;

                //Get the vector of the face buttons
                Vector2 selection = value.Get<Vector2>();
                Debug.Log($"The inputed value {selection}");
                //This index will be used to choose a card
                int index = -1;
                switch (selection)
                {
                    //Top card / Y
                    case Vector2 v when v.Equals(new Vector2(0.0f, 1.0f)):
                        index = 1;
                        break;
                    //Left card / X
                    case Vector2 v when v.Equals(new Vector2(-1.0f, 0.0f)):
                        index = 0;
                        break;
                    //Right card / B
                    case Vector2 v when v.Equals(new Vector2(1.0f, 0.0f)):
                        index = 2;
                        break;
                }
                if(index == -1)
                {
                    Debug.LogWarning("Unabled to select card with given input.");
                    return;
                }
                m_onCardSelected.Invoke(m_cardsHeld[index].DoesEffectSelf);
                _executeSelectedCard(index);
            }
            private void _executeSelectedCard(int value)
            {
                if (!CardsInHand)
                    return;
                //Trigger the effects of the chosen card if a valid input was given.
                if (value > -1)
                {
                    string s = m_cardsHeld[value].name.Remove(m_cardsHeld[value].name.Length - 7); //name with (Clone) removed
                    List<int> chars = new();
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (char.IsUpper(s[i]))
                            chars.Add(i);
                    }
                    chars.Remove(0);
                    for (int i = 0; i < chars.Count; i++)
                    {
                        chars[i] += i;
                    }
                    foreach (int pos in chars)
                    {
                        s = s.Insert(pos, " ");
                    }
                    m_playerUI.GetLog.LogInput($"{s} selected, triggering events.");

                    m_cardsHeld[value].ExecuteEvents(this);
                    m_playerUI.GetCardDisplay.SelectCard(value);
                    DiscardHand();
                }
            }
            #endregion
            public void TriggerBlindness(int count)
            {
                //CancelInvoke();
                m_playerUI.GetBlindBox.StartPopUps(count);
                m_onBlind.Invoke();
                m_playerUI.GetLog.LogInput($"Reciving packet... running program \"areaSingles.exe\"");
                //Invoke("_disableBlindness", m_time);
            }

            public void Pause(bool pause)
            {
                m_paused = pause;
                m_playerControls.enabled = !pause;
            }
        }
    }
}
