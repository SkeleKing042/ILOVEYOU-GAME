using ILOVEYOU.Cards;
using ILOVEYOU.EnemySystem;
using ILOVEYOU.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ILOVEYOU.Management
{
    [CreateAssetMenu(fileName = "NewGameSettings", menuName = "ILOVEYOU Objects/New Game Settings")]
    public class GameSettingsSO : ScriptableObject
    {
        [Header("Difficulty")]
        [SerializeField] private float m_difficultyCap;
        public float GetDiffCap => m_difficultyCap;

        [Header("Tasks")]
        [Tooltip("The maximum number of tasks a player can have.")]
        [SerializeField] private int m_maxTaskCount;
        public int GetMaxTaskCount => m_maxTaskCount;
        [SerializeField] private Task[] m_taskList;
        public Task[] GetTasks => m_taskList;

        [Header("Cards")]
        [Tooltip("The number of cards shown to the player.\nPLEASE KEEP AT 3")]
        [SerializeField] private int m_numberOfCardToGive;
        public int GetNumberOfCardsToGive => m_numberOfCardToGive;
        [SerializeField] private float m_cardTimeOut;
        [Serializable]
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
                //Check if this card can be used with a boss while its active...
                if (BossEnemy.Instances[player.GetPlayerID] != null && !AllowWithBoss)
                {
                    //...if not, set the chance to 0
                    return CurrentChance = 0;
                }

                //Create an array for the values used to find the chance.
                float[] chances = new float[3];
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

                //Average all the chance values for the final result
                return CurrentChance = chances.Average();
            }
        }
        [Tooltip("RNG table for cards. The Chances get combined into an average.")]
        [SerializeField] private CardData[] m_cardData;

        [Header("Player")]
        [SerializeField] private float m_playerHealth;
        [SerializeField] private float m_iframes;
        [SerializeField] private float m_playerSpeed;
        [Header("Enemy")]
        [SerializeField] private EnemyPrefabs[] m_enemyGroups;
        [Serializable]
        public class EnemyPrefabs
        {
            [SerializeField] private string m_groupName;
            //[SerializeField][Tooltip("When difficulty/time is below this value, use this spawn group, values with 0 are ignored in the wave spawner"), Min(-1)] private Vector2 m_threshold;
            [Tooltip("When the enemy group spawns in. The X axis is the difficulty and Y is the likelyness of the enemy group spawning.")]
            [SerializeField] private AnimationCurve m_spawnRate;
            [SerializeField] private GameObject[] m_enemyPrefabs;

            public GameObject EnemyPrefab(int i) { return m_enemyPrefabs[i]; }
            public GameObject RandomEnemyPrefab() { return m_enemyPrefabs[UnityEngine.Random.Range(0, m_enemyPrefabs.Length)]; }
            public AnimationCurve Threshold() { return m_spawnRate; }

        }

        [SerializeField] private float m_spawnRangeMin;
        [SerializeField] private float m_spawnRangeMax;
        [SerializeField] private AnimationCurve m_spawnTime;
        [SerializeField] private AnimationCurve m_spawnCap;
        [Header("Color")]
        [SerializeField] private Color m_importantColor;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}