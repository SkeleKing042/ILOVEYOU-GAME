using ILOVEYOU.Cards;
using ILOVEYOU.EnemySystem;
using ILOVEYOU.Player;
using ILOVEYOU.UI;
using UnityEngine;

namespace ILOVEYOU.Management
{
    [CreateAssetMenu(fileName = "NewGameSettings", menuName = "ILOVEYOU Objects/New Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public static GameSettings Current { get; private set; }
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
        public float GetCardTimeOut => m_cardTimeOut;
        [Tooltip("RNG table for cards. The Chances get combined into an average.")]
        [SerializeField] private CardData[] m_cardData;
        public CardData[] GetCardData => m_cardData;

        [Header("Player")]
        [SerializeField] private float m_playerHealth;
        public float GetPlayerHealth => m_playerHealth;
        [SerializeField] private float m_iframes;
        public float GetiFrameDuration => m_iframes;
        [SerializeField] private float m_playerSpeed;
        public float GetPlayerSpeed => m_playerSpeed;
        [SerializeField] private float m_knockbackWindow = 0.1f;
        public float GetKnockbackWindow => m_knockbackWindow;
        [SerializeField] private Vector2 m_knockbackStrength;
        public Vector2 GetKnockbackStrength => m_knockbackStrength;
        [SerializeField] private float m_knockbackRadius;
        public float GetKnockbackRadius => m_knockbackRadius;
        [Header("Enemy")]
        [SerializeField] private EnemyPrefabs[] m_enemyGroups;
        public EnemyPrefabs[] GetEnemyGroups => m_enemyGroups;

        [SerializeField] private float m_spawnRangeMin;
        public float GetSpawnRangeMin => m_spawnRangeMin;
        [SerializeField] private float m_spawnRangeMax;
        public float GetSpawnRangeMax => m_spawnRangeMax;
        public Vector2 GetSpawnRange => new Vector2(m_spawnRangeMin, m_spawnRangeMax);
        [SerializeField] private AnimationCurve m_spawnTime;
        public AnimationCurve GetSpawnTime => m_spawnTime;
        [SerializeField] private AnimationCurve m_spawnCap;
        public AnimationCurve GetSpawnCap => m_spawnCap;

        [System.Serializable]
        public struct ColorPrefType
        {
            public string m_key;
            public Color m_color;

            public void setup()
            {
                Debug.Log($"Initializing color with key {m_key} and value {m_color.ToString()}");
                ColorPref.Set(m_key, m_color);
            }
        }
        [Header("Color")]
        [SerializeField] private ColorPrefType[] m_prefColors;
        public void Assign()
        {
            Debug.Log("Assigning new settings.");
            Current = this;
        }
        public void InitalizePrefs()
        {
            foreach(var color in m_prefColors)
            {
                color.setup();
            }
        }
    }
}