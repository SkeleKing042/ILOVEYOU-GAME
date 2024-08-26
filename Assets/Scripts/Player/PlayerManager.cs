using ILOVEYOU.Cards;
using ILOVEYOU.Environment;
using ILOVEYOU.Management;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ILOVEYOU
{
    namespace Player
    {
        [RequireComponent(typeof(TaskManager))]
        public class PlayerManager : MonoBehaviour
        {
            [SerializeField] private bool m_debugging;
            //player
            private PlayerControls m_playerControls;
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
            private Slider m_healthSlider;

            [Header("Event - sounds and visuals")]
            [SerializeField] private UnityEvent m_onGetCards;
            [SerializeField] private UnityEvent m_onDiscardHand;
            [SerializeField] private UnityEvent m_onCardSelected;
            [SerializeField] private UnityEvent m_onBlind;
            [SerializeField] private UnityEvent m_onUnblind;
            public bool Startup(LevelManager manager, int playerNum)
            {
                //Reset variables
                m_taskMan = GetComponent<TaskManager>();
                m_playerControls = GetComponent<PlayerControls>();
                if (!m_taskMan)
                {
                    if (m_debugging) Debug.Log("Task manager not found! Please fix boss");
                    return false;
                }
                if (!m_taskMan.Startup())
                {
                    return false;
                }

                m_cardsHeld = new DisruptCard[0];
                m_levelManager = manager;
                m_playerID = playerNum;

                //ui setup
                if (m_playerID != 0) m_playerHud.transform.GetChild(0).localScale = new(-1, 1, 1);
                m_healthSlider = m_playerHud.transform.GetChild(0).GetComponentInChildren<Slider>();
                m_blindBox.GetComponent<PopUps>().Initialize(m_playerControls);
                m_blindBox.SetActive(false);
                m_cardDisplay.parent.gameObject.SetActive(false);

                if (m_debugging) Debug.Log("PlayerManager started successfully");
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
                foreach(DisruptCard card in m_cardsHeld)
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
                    m_cardsHeld[index].Trigger(m_levelManager.GetManager, this);
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
