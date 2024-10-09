using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ILOVEYOU.Management;
using ILOVEYOU.Player;
using UnityEngine;
using UnityEngine.Events;

namespace ILOVEYOU
{
    namespace Cards
    {
        public class CardManager : MonoBehaviour
        {
            [SerializeField] private bool m_debugging;
            [System.Serializable]
            [Tooltip("RNG table for cards. The Chances get combined into an average.")]
            private class CardData
            {
                public DisruptCard DisruptCard;
                public AnimationCurve ChanceOverTime;
                public AnimationCurve ChanceOverEnemyCount;
                //public AnimationCurve ChanceOverHealth;
                //public bool AllowWithBoss = true;
                [HideInInspector] public float CurrentChance;

                public void GenerateChance(PlayerManager player)
                {
                    float[] chances = new float[2]; 
                    chances[0] = ChanceOverTime.Evaluate(GameManager.Instance.PercentToMaxDiff);
                    chances[1] = ChanceOverEnemyCount.Evaluate(player.GetLevelManager.GetSpawner.PercentToMaxEnemies);
                    //chances[2] = player.GetControls.
                    CurrentChance = chances.Average();
                }
            }
            [SerializeField] private CardData[] m_cardData;
            [SerializeField] private UnityEvent m_onDispenseCard;

            /// <summary>
            /// Sets up this script
            /// </summary>
            /// <returns></returns>
            public bool Startup()
            {
                if (m_debugging) Debug.Log($"Starting {this}.");
                //Check the cards for issues
                foreach (CardData card in m_cardData)
                {
                    //possible missing parts
                    if (card.DisruptCard.GetComponents(typeof(Component)).Length < 3)
                    {
                        if(m_debugging) Debug.LogWarning($"{card} might be missing an effect. Please make sure there is a script attached to the same object as the \"DisruptCardBase\" script, and that it has a function called \"ExecuteEvents\"");
                    }
                }

                //passed
                if(m_debugging) Debug.Log($"{this} started successfully.");
                return true;
            }
            public List<DisruptCard> DispenseCards(int count, PlayerManager player)
            {
                //Sum the chances of all the cards
                float chanceSum = 0;
                foreach(CardData card in m_cardData)
                {
                    card.GenerateChance(player);
                    chanceSum += card.CurrentChance;
                }
                //Clamp the number of possible cards
                Mathf.Clamp(count, 1, m_cardData.Length - 1);
                //Make a new array with the requested amount of cards
                List<DisruptCard> cards = new List<DisruptCard>();
                List<int> selectedCards = new List<int>();
                //Iterate though each card in the array
                for (int c = 0; c < count; c++)
                {
                    //Assign it a random card
                    int rndCard = -1;
                    for(int i = 100; i > 0; i--)
                    {
                        float rndChance = -1;
                        rndCard = Random.Range(0, m_cardData.Length);
                         rndChance = Random.Range(0.0f, 1.0f);
                        if (!selectedCards.Contains(rndCard) && m_cardData[rndCard].CurrentChance >= rndChance) break;
                    }
                    selectedCards.Add(rndCard);
                    cards.Add(Instantiate(m_cardData[rndCard].DisruptCard));
                }
                m_onDispenseCard.Invoke();
                //Return the array
                return cards;
            }
        }
    }
}