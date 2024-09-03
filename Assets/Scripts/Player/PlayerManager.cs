using ILOVEYOU.Cards;
using ILOVEYOU.EnemySystem;
using ILOVEYOU.Environment;
using ILOVEYOU.Management;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
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
            [SerializeField] private bool m_debugging;
            //player
            private PlayerControls m_playerControls;
            public PlayerControls GetControls { get { return m_playerControls; } }
            private int m_playerID;
            public int GetPlayerID { get { return m_playerID; } }
            private LevelManager m_levelManager;
            public LevelManager GetLevelManager { get { return m_levelManager; } }
            //tasks
            private TaskManager m_taskMan;
            public TaskManager GetTaskManager { get { return m_taskMan; } }
            //cards
            private DisruptCard[] m_cardsHeld;
            public bool CardsInHand { get { return m_cardsHeld.Length > 0; } }
            [SerializeField] private float m_cardTimeout;
            [SerializeField] private GameObject m_blindBox;
            //ui
            [SerializeField] private Transform m_cardDisplay;
            [SerializeField] private GameObject m_playerHud;
            [SerializeField] private Slider m_healthSlider;

            private EventLogUI m_eventLog;
            public EventLogUI GetLog { get { return m_eventLog; } }

            [Header("Event - sounds and visuals")]
            [SerializeField] private UnityEvent m_onGetCards;
            [SerializeField] private UnityEvent m_onDiscardHand;
            [SerializeField] private UnityEvent m_onCardSelected;
            [SerializeField] private UnityEvent m_onBlind;
            [SerializeField] private UnityEvent m_onUnblind;
            public bool Startup(LevelManager manager, int index)
            {
                if (m_debugging) Debug.Log($"Starting {this}.");
                m_cardsHeld = new DisruptCard[0];
                m_playerID = index;
                m_levelManager = manager;

                if (m_debugging) Debug.Log($"Getting task manager.");
                m_taskMan = GetComponent<TaskManager>();
                if (!m_taskMan.Startup())
                {
                    Debug.LogError($"{m_taskMan} failed startup, aborting...");
                    Destroy(gameObject);
                    return false;
                }


                if (m_debugging) Debug.Log($"Getting player controls");
                m_playerControls = GetComponent<PlayerControls>();
                if (!m_playerControls.Startup())
                {
                    Debug.LogError($"{m_playerControls} failed startup, aborting...");
                    Destroy(gameObject);
                    return false;
                }

                //ui setup
                if (m_playerID != 0) m_playerHud.transform.GetChild(0).localScale = new(-1, 1, 1);
                //m_healthSlider = m_playerHud.transform.GetChild(0).GetComponentInChildren<Slider>();
                m_blindBox.GetComponent<PopUps>().Initialize(m_playerControls);
                m_blindBox.SetActive(false);
                m_cardDisplay.parent.gameObject.SetActive(false);
                m_eventLog = GetComponent<EventLogUI>();
                m_playerControls.enabled = false;

                if (m_debugging) Debug.Log($"{this} started successfully");
                return true;
            }
            #region Card Management
            /// <summary>
            /// Save a given hand as the cards held by this player
            /// </summary>
            /// <param name="cards"></param>
            public void CollectHand(DisruptCard[] cards)
            {
                if(m_debugging) Debug.Log("Hand dealt, setting up cards.");
                CancelInvoke();
                m_onGetCards.Invoke();
                //Copy the given array to this hand
                m_cardsHeld = new DisruptCard[cards.Length];
                cards.CopyTo(m_cardsHeld, 0);

                //Set up each card
                foreach(DisruptCard card in m_cardsHeld)
                {
                    //puts the card in the card display
                    card.transform.SetParent(m_cardDisplay, false);
                    card.transform.localScale = Vector3.one;
                    //enables the display
                    m_cardDisplay.parent.gameObject.SetActive(true);
                    if(m_debugging) Debug.Log("Readying discard function to card.");
                    card.m_playerHandToDiscard.AddListener(delegate { DiscardHand(); });
                }
                //To stop stockpiling, delete the cards after a set time
                Invoke("DiscardHand", m_cardTimeout);
            }
            /// <summary>
            /// Destroys the player's hand card objects
            /// </summary>
            public void DiscardHand()
            {
                if (!CardsInHand)
                    return;

                m_eventLog.LogInput($"<i><#888888>Discarding hand.</color></i>");
                foreach (DisruptCard card in m_cardsHeld)
                {
                    Destroy(card.gameObject);
                }
                m_cardsHeld = new DisruptCard[0];
                m_onDiscardHand.Invoke();
                m_cardDisplay.parent.gameObject.SetActive(false);
            }
            /// <summary>
            /// Takes a player's input to select a card
            /// </summary>
            /// <param name="value"></param>
            public void OnSelectCard(InputValue value)
            {
                //If the hand is empty, don't continue
                if (!CardsInHand)
                    return;

                //Get the vector of the face buttons
                Vector2 selection = value.Get<Vector2>();
                if(m_debugging) Debug.Log($"The inputed value {selection}");
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

                //Trigger the effects of the chosen card if a valid input was given.
                if(index > -1)
                {
                    string s = m_cardsHeld[index].name.Remove(m_cardsHeld[index].name.Length - 7); //name with (Clone) removed
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
                    m_eventLog.LogInput($"{s} selected, triggering events.");
                    
                    m_cardsHeld[index].Trigger(GameManager.Instance, this);
                        m_onCardSelected.Invoke();
                }
            }
            #endregion
            public void TriggerBlindness(int count)
            {
                //CancelInvoke();
                m_blindBox.SetActive(true);
                m_blindBox.GetComponent<PopUps>().StartPopUps(count);
                m_onBlind.Invoke();
                m_eventLog.LogInput($"Reciving packet... running program \"areaSingles.exe\"");
                //Invoke("_disableBlindness", m_time);
            }
            //private void _disableBlindness()
            //{
            //    m_blindBox.SetActive(false);
            //    m_onUnblind.Invoke();
            //}
            public void UpdateHealthBar(float value)
            {
                m_healthSlider.value = value;
            }
        }
    }
}
