using ILOVEYOU.Cards;
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
            private DisruptCard[] m_cards;

            [Header("Events")]
            [SerializeField] private UnityEvent m_onGetCards;
            [SerializeField] private UnityEvent<string, float> m_onSelectCards;
            [SerializeField] private UnityEvent<string, float> m_onDiscard;
            public void DisplayCards(DisruptCard[] cards)
            {
                gameObject.SetActive(true);
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
                m_onDiscard.Invoke("SelectedCard", -1);
                foreach(var card in m_cards)
                {
                    Destroy(card.gameObject);
                }
                m_cards = new DisruptCard[0];
                gameObject.SetActive(false);
            }
        }
    }
}