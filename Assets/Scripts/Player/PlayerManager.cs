using ILOVEYOU.Cards;
using ILOVEYOU.Management;
using UnityEngine;
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
            private int m_playerID;
            public int GetPlayerID { get { return m_playerID; } }
            private GameManager m_manager;
            public GameManager GetGameManager { get { return m_manager; } }
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
            public bool Startup(GameManager manager, int playerNum)
            {
                //Reset variables
                m_taskMan = GetComponent<TaskManager>();
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
                m_manager = manager;
                m_playerID = playerNum;

                //ui setup
                if (m_playerID != 0) m_playerHud.transform.GetChild(0).localScale = new(-1, 1, 1);
                m_healthSlider = m_playerHud.transform.GetChild(0).GetComponentInChildren<Slider>();
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

                //Copy the given array to this hand
                m_cardsHeld = new DisruptCard[cards.Length];
                cards.CopyTo(m_cardsHeld, 0);

                //Set up each card
                foreach(DisruptCard card in m_cardsHeld)
                {
                    card.transform.SetParent(m_cardDisplay, false);
                    card.transform.localScale = Vector3.one;
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
                if(index > - 1)
                m_cardsHeld[index].Trigger(m_manager, this);
            }
            #endregion
            public void TriggerBlindness(float m_time)
            {
                CancelInvoke();
                m_blindBox.SetActive(true);
                Invoke("_disableBlindness", m_time);
            }
            private void _disableBlindness()
            {
                m_blindBox.SetActive(false);
            }
            public void UpdateHealthBar(float value)
            {
                m_healthSlider.value = value;
            }
        }
    }
}
