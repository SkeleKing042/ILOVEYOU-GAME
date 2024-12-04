using ILOVEYOU.Cards;
using ILOVEYOU.EnemySystem;
using ILOVEYOU.Player;
using ILOVEYOU.ProjectileSystem;
using ILOVEYOU.UI;
using System;
using UnityEngine;

namespace ILOVEYOU.Management
{
    [CreateAssetMenu(fileName = "NewGameSettings", menuName = "ILOVEYOU Objects/New Game Settings")]
    [Serializable]
    public class GameSettings : ScriptableObject
    {
        public static GameSettings Current { get; private set; }
        public string BuildVersion;
        public bool IsOutdated { get { return BuildVersion != Application.version; } }
        [SerializeField] private string[] m_announcement = new string[0];
        public string GetAnouncement
        {
            get
            {
                if (m_announcement.Length > 0)
                {
                    int rnd = UnityEngine.Random.Range(0, m_announcement.Length); return m_announcement[rnd];
                }
                return "";
            }
        }
        //[Header("Difficulty")]
        [SerializeField] private float m_difficultyCap = 60f;
        public float GetDiffCap => m_difficultyCap;
        [SerializeField] private int m_PlayerLimit = 0;
        public int GetPlayerLimit => m_PlayerLimit;

        //[Header("Tasks")]
        [Tooltip("The maximum number of tasks a player can have.")]
        [SerializeField] private int m_maxTaskCount = 1;
        public int GetMaxTaskCount => m_maxTaskCount;
        [SerializeField] private Task[] m_taskList = new Task[2];
        public Task[] GetTasks => m_taskList;
        [SerializeField] private bool m_tasksCanHeal;
        public bool CanTasksHeal => m_tasksCanHeal;

        //[Header("Cards")]
        [Tooltip("The number of cards shown to the player.\nPLEASE KEEP AT 3")]
        [SerializeField] private int m_numberOfCardToGive = 0;
        public int GetNumberOfCardsToGive => m_numberOfCardToGive;
        [SerializeField] private float m_cardTimeOut = 10f;
        public float GetCardTimeOut => m_cardTimeOut;
        [Tooltip("RNG table for cards. The Chances get combined into an average.")]
        [SerializeField] private CardData[] m_cardData = new CardData[2];
        public CardData[] GetCardData => m_cardData;
        [SerializeField] private bool m_allowDoubleUps = false;
        public bool DoAllowDoubleUps => m_allowDoubleUps;

        //[Header("Player")]
        [SerializeField] private float m_playerHealth = 15f;
        public float GetPlayerHealth => m_playerHealth;
        [SerializeField] private float m_iframes = 1f;
        public float GetiFrameDuration => m_iframes;
        [SerializeField] private float m_playerSpeed = 10f;
        public float GetPlayerSpeed => m_playerSpeed;
        [SerializeField] private BulletPatternObject m_playerShootingPattern;
        public BulletPatternObject GetPlayerShootingPattern => m_playerShootingPattern;

        //Unseen/Singleplayer AI
        [SerializeField] private bool m_useUnseen;
        public bool isUsingUnseenAI => m_useUnseen;
        [SerializeField] private CardData[] m_unseenCards = new CardData[0];
        public CardData[] GetUnseenCards => m_unseenCards;
        [SerializeField] private float m_unseenRate = 10;
        public float GetUnseenCardRate => m_unseenRate;

        //knockback
        [SerializeField] private float m_knockbackWindow = 0.1f;
        public float GetKnockbackWindow => m_knockbackWindow;
        [SerializeField] private Vector2 m_knockbackStrength = new(10, 1);
        public Vector2 GetKnockbackStrength => m_knockbackStrength;
        [SerializeField] private float m_knockbackRadius = 10f;
        public float GetKnockbackRadius => m_knockbackRadius;
        [SerializeField] private float m_knockbackStunDuration;
        public float GetKnockbackStunDuration => m_knockbackStunDuration;
        //[Header("Enemy")]
        [SerializeField] private EnemyPrefabs[] m_enemyGroups;
        public EnemyPrefabs[] GetEnemyGroups => m_enemyGroups;

        [SerializeField] private float m_spawnRangeMin = 10f;
        public float GetSpawnRangeMin => m_spawnRangeMin;
        [SerializeField] private float m_spawnRangeMax = 10f;
        public float GetSpawnRangeMax => m_spawnRangeMax;
        public Vector2 GetSpawnRange => new Vector2(m_spawnRangeMin, m_spawnRangeMax);
        [SerializeField] private AnimationCurve m_spawnTime;
        public AnimationCurve GetSpawnTime => m_spawnTime;
        [SerializeField] private AnimationCurve m_spawnCap;
        public AnimationCurve GetSpawnCap => m_spawnCap;
        [System.Serializable]
        public struct ModListEntry
        {
            public EnemyModifier Modifier;
            public float Chance;
        }
        [SerializeField] private ModListEntry[] m_modList = new ModListEntry[0];
        public ModListEntry[] GetModList => m_modList;
        [SerializeField] private AnimationCurve m_modChanceOverTime;
        public float GetModChance => m_modChanceOverTime.Evaluate(GameManager.Instance.PercentToMaxDiff);
        [SerializeField] private AnimationCurve m_modCountOverTime;
        public float GetMaxModCount => m_modCountOverTime.Evaluate(GameManager.Instance.PercentToMaxDiff);
        [System.Serializable]
        public struct ColorPrefType
        {
            public string m_key;
            public Color m_color;

            public ColorPrefType(string key, Color color)
            {
                m_key = key;
                m_color = color;
            }

            public void setup()
            {
                Debug.Log($"Initializing color with key {m_key} and value {m_color.ToString()}");
                ColorPref.Set(m_key, m_color);
            }
        }
        //[Header("Color")]
        [SerializeField] private ColorPrefType[] m_prefColors = { new("Important Color", Color.white),
                                                                    new("Buff color", Color.white),
                                                                    new("Debuff color", Color.white),
                                                                    new("Hazard color", Color.white),
                                                                    new("Summon color", Color.white) };

        public GameSettings()
        {

        }
        public void Assign(bool Override = false)
        {
            if (Current == null || Override)
            {
                Debug.Log("Assigning new settings.");
                Current = this;
                InitalizePrefs();
                foreach (var player in FindObjectsOfType<PlayerControls>())
                {
                    player.ChangeWeapon(GetPlayerShootingPattern);
                }
            }
        }
        public static void Unassign(){
            Debug.Log("Removing applied settings");
            Current = null;
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