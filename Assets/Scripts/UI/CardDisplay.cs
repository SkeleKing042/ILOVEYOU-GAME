using ILOVEYOU.Cards;
using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.VirtualTexturing;

namespace ILOVEYOU
{
    namespace UI
    {

        public class CardDisplay : MonoBehaviour
        {
            [SerializeField] private Transform[] m_displayPoints;
            private DisruptCard[] m_cards = new DisruptCard[0];
            [SerializeField] private GameObject m_background;
            [SerializeField] private PlayerManager m_player;

            [Header("Events")]
            [SerializeField] private UnityEvent m_onGetCards;
            [SerializeField] private UnityEvent<string, float> m_onSelectCards;
            [SerializeField] private UnityEvent<string, float> m_onDiscard;
            public void DisplayCards(DisruptCard[] cards)
            {
                if (m_cards.Length > 0)
                {
                    foreach(var card in m_cards)
                    {
                        if (card)
                            Destroy(card.gameObject);
                    }
                    m_cards = new DisruptCard[0];
                }
                gameObject.SetActive(true);
                m_background.GetComponent<Animator>().SetBool("Show", true);
                m_onGetCards.Invoke();
                m_cards = new DisruptCard[cards.Length];
                m_cards = cards;

                //Set up each card
                for(int i = 0; i < m_cards.Length; i++)
                {
                    //puts the card in the card display
                    m_cards[i].transform.SetParent(m_displayPoints[i], false);
                    m_cards[i].transform.SetAsFirstSibling();
                    m_cards[i].transform.localScale = Vector3.one;
                }
            }
            public void SelectCard(int index)
            {
                m_onSelectCards.Invoke("SelectedCard", index);
            }
            public void DiscardHand()
            {
                if (!m_player.CardsInHand)
                {
                    Debug.Log("Discarding card visuals");
                    foreach (var card in m_cards)
                    {
                        if (card)
                            Destroy(card.gameObject);
                    }
                    gameObject.SetActive(false);
                    m_background.GetComponent<Animator>().SetBool("Show", false);
                }
            }
        }
    }
}