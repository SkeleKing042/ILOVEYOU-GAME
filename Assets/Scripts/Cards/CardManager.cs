using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Cards
    {
        public class CardManager : MonoBehaviour
        {
            [SerializeField] private DisruptCard[] m_disruptCards;

            public bool Startup()
            {
                foreach(DisruptCard card in m_disruptCards)
                {
                    if (card.GetComponents(typeof(Component)).Length < 3)
                    {
                        Debug.LogWarning($"This card might be missing an effect. Please make sure there is a script attached to the same object as the \"DisruptCardBase\" script, and that it has a function called \"ExecuteEvents\"");
                    }
                }
                Debug.Log("CardManager started successfully.");
                return true;
            }
            public List<DisruptCard> DispenseCards(int count)
            {
                //Clamp the number of possible cards
                Mathf.Clamp(count, 1, m_disruptCards.Length - 1);
                //Make a new array with the requested amount of cards
                List<DisruptCard> cards = new List<DisruptCard>();

                //Iterate though each card in the array
                for (int i = 0; i < count; i++)
                {
                    //Assign it a random card
                    cards.Add(Instantiate(m_disruptCards[Random.Range(0, m_disruptCards.Length)]));
                }
                //Return the array
                return cards;
            }
        }
    }
}