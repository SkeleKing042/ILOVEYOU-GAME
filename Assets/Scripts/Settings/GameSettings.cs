using ILOVEYOU.Cards;
using ILOVEYOU.EnemySystem;
using ILOVEYOU.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
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
        [Header("Color")]
        [SerializeField] private Color m_importantColor;
        public Color GetImportantColor => m_importantColor;

        public void Assign()
        {
            Debug.Log("Assigning new settings.");
            Current = this;
        }
    }
}