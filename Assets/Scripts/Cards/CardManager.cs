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
                    chances[1] = ChanceOverEnemyCount.Evaluate(sum / others.Length);
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

                //passed
                Debug.Log($"{this} started successfully.");
                return true;
            }
            public DisruptCard[] DispenseCards(int count, PlayerManager player)
            {
                Debug.Log("Gathering cards");
                //Clamp the number of possible cards
                Mathf.Clamp(count, 1, GameSettings.Current.GetCardData.Length - 1);

                //Update the chances of the cards dropping
                UpdateChances(GameSettings.Current.GetCardData, player);

                //Make a new array for the requested cards
                List<DisruptCard> cardInsts = new();
                DisruptCard[] cards = GetRandomCard(GameSettings.Current.GetCardData, count);
                foreach (var card in cards)
                {
                    cardInsts.Add(Instantiate(card));
                }
                foreach(var card in cardInsts)
                {
                    card.SetPlayerID((int)player.GetPlayerID);
                    card.SetupColours();
                }
                m_onDispenseCard.Invoke();
                return cardInsts.ToArray();
            }
            static public CardData[] UpdateChances(CardData[] array, PlayerManager player = null)
            {
                Debug.Log($"Updating card array of length {array.Length}");
                foreach (var data in array)
                {
                    data.GenerateChance(player);
                }
                return array;
            }
            static public DisruptCard[] GetRandomCard(CardData[] array, int returnCount = 1, int attempts = 100)
            {
                //Array size check
                if(returnCount < 1) { Debug.Log("Return count set too low!"); returnCount = 1; }

                //Make array
                List<DisruptCard> returnedCards = new();
                for (int c = 0; c < returnCount; c++)
                {
                    //Generate rng
                    float rndChance = 0;
                    if (attempts > 0)
                    {
                        rndChance = Random.Range(0.00f, 1.00f);
                    }
                    Debug.Log($"Rolled rnd of {rndChance * 100}%");
                    int rndOffset = Random.Range(0, array.Length);
                    //Go through until card found
                    for(int i = 0; i < array.Length; i++)
                    {
                        if(i + rndOffset >= array.Length)
                        {
                            rndOffset -= array.Length - 1;
                        }

                        if (!GameSettings.Current.DoAllowDoubleUps && returnedCards.Contains(array[i + rndOffset].DisruptCard))
                        {
                            Debug.Log($"Double up, skipping");
                            continue;
                        }
                        if (array[i + rndOffset].CurrentChance >= rndChance)
                        {
                            Debug.Log($"{array[i + rndOffset].DisruptCard} has beaten the odds of {rndChance * 100}%");
                            returnedCards.Add(array[i + rndOffset].DisruptCard);
                            break;
                        }
                        else if (i == array.Length - 1)
                        {
                            Debug.Log($"Failed to get card with chance of {rndChance * 100}%, trying again.");
                            c--;
                        }
                    }
                    attempts--;

                }
                return returnedCards.ToArray();
            }
        }
    }
}