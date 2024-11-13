using System.Collections.Generic;
using ILOVEYOU.EnemySystem;
using ILOVEYOU.Management;
using ILOVEYOU.Player;
using UnityEngine;
using UnityEngine.Events;

namespace ILOVEYOU
{
    namespace Cards
    {
        [System.Serializable]
        public class CardData
        {
            public DisruptCard DisruptCard;
            [Tooltip("The chance of a card dropping over the round time")]
            public AnimationCurve ChanceOverTime;
            [Tooltip("The chance of a card dropping over the enemy count of the calling object.\nUnseen AI will take the average of all other players' enemy counts instead.")]
            public AnimationCurve ChanceOverEnemyCount;
            [Tooltip("The chacne of a card dropping over the health differene of the calling object to the other players.\nUnseen AI will just use the average health of all players instead.")]
            public AnimationCurve ChanceOverHealthDelta;
            [Tooltip("If this card can drop when other player have a boss.")]
            public bool AllowWithBoss = true;
            public float CurrentChance { get; private set; }

            public float GenerateChance(PlayerManager player)
            {
                //Get all the other players
                PlayerManager[] others = GameManager.Instance.GetOtherPlayers(player);

                //fails if any other player has the boss active.
                if(!AllowWithBoss)
                foreach(var p in others)
                {
                    if (BossEnemy.Instances[p.GetPlayerID] != null && BossEnemy.Instances[p.GetPlayerID].GetCurrentHealth > 0)
                    {
                        return CurrentChance = 0;
                    }
                }

                float[] chances = new float[3];

                //Get the current difficulty
                chances[0] = ChanceOverTime.Evaluate(GameManager.Instance.PercentToMaxDiff);
                
                //Get the current enemy count
                if(player != null)
                {
                    chances[1] = ChanceOverEnemyCount.Evaluate(player.GetLevelManager.GetSpawner.PercentToMaxEnemies);
                }
                //If there is no player manager, get the average enemy count of the other player.
                else
                {
                    float sum = 0;
                    foreach(var p in others)
                    {
                        sum += p.GetLevelManager.GetSpawner.PercentToMaxEnemies;
                    }
                    chances[1] = sum / others.Length;
                }

                //Get the average health for all other players
                float healthSum = 0;
                foreach(var p in others)
                {
                    healthSum += p.GetControls.GetHealthPercent;
                }
                healthSum /= others.Length;

                //Compare this health average to the caller's current health.
                if(player != null)
                {
                    chances[2] = ChanceOverHealthDelta.Evaluate(Mathf.Clamp(healthSum - player.GetControls.GetHealthPercent, 0, 1));
                }
                //If there is no caller, just use the average health.
                else
                {
                    chances[2] = ChanceOverHealthDelta.Evaluate(healthSum);
                }

                //Mult all the chance values for the final result
                float result = 1;
                foreach(var num in chances)
                {
                    result *= num;
                    if (result <= 0) break;
                }
                return CurrentChance = result;
            }
        }

        public class CardManager : MonoBehaviour
        {
            [SerializeField] private UnityEvent m_onDispenseCard;

            /// <summary>
            /// Sets up this script
            /// </summary>
            /// <returns></returns>
            public bool Startup()
            {
                Debug.Log($"Starting {this}.");
                //Check the cards for issues
                foreach (CardData card in GameSettings.Current.GetCardData)
                {
                    //possible missing parts
                    if (card.DisruptCard.GetComponents(typeof(Component)).Length < 3)
                    {
                        Debug.LogWarning($"{card} might be missing an effect. Please make sure there is a script attached to the same object as the \"DisruptCardBase\" script, and that it has a function called \"ExecuteEvents\"");
                    }
                }

                //passed
                Debug.Log($"{this} started successfully.");
                return true;
            }
            public DisruptCard[] DispenseCards(int count, PlayerManager player)
            {
                //Clamp the number of possible cards
                Mathf.Clamp(count, 1, GameSettings.Current.GetCardData.Length - 1);

                //Make a new array for the requested cards
                List<DisruptCard> selectedCards = new();
                //Update the chances of the cards dropping
                UpdateChances(GameSettings.Current.GetCardData, player);
                //Get enough cards
                for (int c = 0; c < count; c++)
                {
                    /*int rndCard = -1;
                        float rndChance = -1;
                        rndCard = Random.Range(0, GameSettings.Current.GetCardData.Length);
                         rndChance = Random.Range(0.0f, 1.0f);
                        if (!selectedCards.Contains(rndCard) && GameSettings.Current.GetCardData[rndCard].CurrentChance >= rndChance) break;
                    selectedCards.Add(rndCard);
                    */
                    //Get a random card
                    for (int i = 100; i > 0; i--)
                    {
                        DisruptCard selected = GetRandomCard(GameSettings.Current.GetCardData);
                        if (!selectedCards.Contains(selected))
                        {
                            selectedCards.Add(selected);
                            break;
                        }
                    }
                }
                for (int i = 0; i < 100; i++)
                {
                    if (selectedCards.Count >= count)
                        break;

                    int rnd = Random.Range(0, GameSettings.Current.GetCardData.Length);
                    selectedCards.Add(GameSettings.Current.GetCardData[rnd].DisruptCard);
                }

                DisruptCard[] cards = new DisruptCard[selectedCards.Count];
                //Return the array
                for(int i = 0; i < selectedCards.Count; i++)
                {
                    //Instance each card
                    cards[i] = Instantiate(selectedCards[i]);
                }
                m_onDispenseCard.Invoke();
                return cards;
            }
            static public CardData[] UpdateChances(CardData[] array, PlayerManager player = null)
            {
                foreach (var data in array)
                {
                    data.GenerateChance(player);
                }
                return array;
            }
            static public DisruptCard GetRandomCard(CardData[] array, int attempts = 100)
            {
                for (int i = attempts; i > 0; i--)
                {
                    float rndChance = Random.Range(0.00f, 1.00f);
                    int rndCard = Random.Range(0, array.Length);
                    Debug.Log($"Pulled card at index {rndCard}");
                    if (array[rndCard].CurrentChance >= rndChance)
                        return array[rndCard].DisruptCard;
                }
                return array[0].DisruptCard;
            }
        }
    }
}