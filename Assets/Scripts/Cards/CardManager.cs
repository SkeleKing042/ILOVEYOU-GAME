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
            public AnimationCurve ChanceOverTime;
            public AnimationCurve ChanceOverEnemyCount;
            public AnimationCurve ChanceOverHealthDelta;
            public bool AllowWithBoss = true;
            [HideInInspector] public float CurrentChance;

            public float GenerateChance(PlayerManager player)
            {
                float[] chances = new float[3];
                //Chances if called from player
                if (player)
                {
                    //Check if this card can be used with a boss while its active...
                    if (BossEnemy.Instances[player.GetPlayerID] != null && !AllowWithBoss)
                    {
                        //...if not, set the chance to 0
                        return CurrentChance = 0;
                    }

                    //Create an array for the values used to find the chance.
                    //Get the game time compared to max diffculty
                    chances[0] = ChanceOverTime.Evaluate(GameManager.Instance.PercentToMaxDiff);
                    //Get the percent of enemies on this players side
                    chances[1] = ChanceOverEnemyCount.Evaluate(player.GetLevelManager.GetSpawner.PercentToMaxEnemies);
                    //Get the health difference between this and the other player.
                    float averageHealth = 0;
                    //Average the other players' health values
                    PlayerManager[] others = GameManager.Instance.GetOtherPlayers(player);
                    for (int i = 0; i < others.Length; i++)
                    {
                        averageHealth += others[i].GetControls.GetHealthPercent;
                    }
                    averageHealth /= others.Length;
                    chances[2] = ChanceOverHealthDelta.Evaluate(Mathf.Clamp(averageHealth - player.GetControls.GetHealthPercent, 0, 1));
                }
                else
                {
                    PlayerManager[] others = GameManager.Instance.GetOtherPlayers(null);
                    foreach(var other in others)
                    {
                        if (BossEnemy.Instances[other.GetPlayerID] != null && !AllowWithBoss)
                        {
                            return CurrentChance = 0;
                        }
                    }
                    chances[0] = ChanceOverTime.Evaluate(GameManager.Instance.PercentToMaxDiff);
                    chances[1] = 1;
                    chances[2] = 1;
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
            static public DisruptCard GetRandomCard(CardData[] array, int attempts = 100)
            {
                foreach(var data in array)
                {
                    data.GenerateChance(null);
                }
                for (int i = attempts; i > 0; i--)
                {

                    float rndChance = Random.Range(0.00f, 1.00f);
                    int rndCard = Random.Range(0, array.Length);
                    if (array[rndCard].CurrentChance >= rndChance)
                        return array[rndCard].DisruptCard;
                }
                return array[0].DisruptCard;
            }
        }
    }
}