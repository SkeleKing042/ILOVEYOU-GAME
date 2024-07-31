using ILOVEYOU.Management;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
namespace ILOVEYOU
{
    namespace EnemySystem
    {
        public class EnemySpawner : MonoBehaviour
        {
            [Serializable]
            class EnemyPrefabs
            {
                [SerializeField] private string m_groupName;
                [SerializeField][Tooltip("When difficulty/time is below this value, use this spawn group, values with 0 are ignored in the wave spawner")] private float m_threshold;
                [SerializeField] private GameObject[] m_enemyPrefabs;

                public GameObject EnemyPrefab(int i) { return m_enemyPrefabs[i]; }
                public GameObject RandomEnemyPrefab() { return m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Length)]; }
                public float Threshold() { return m_threshold; }

            }

            [SerializeField] private EnemyPrefabs[] m_enemyGroups;
            [SerializeField] private float m_spawnRange;

            private GameManager m_manager;

            //[SerializeField] private Transform TEMPPLAYERPOS;



            public void Initialize(GameManager manager)
            {
                m_manager = manager;
            }

            public void SpawnEnemyWave()
            {   

                for (int i = 0; i < m_enemyGroups.Length; i++)
                {
                    if (m_manager.GetDifficulty > m_enemyGroups[i].Threshold() || m_enemyGroups[i].Threshold() == 0) continue;

                    Debug.Log("Spawning Group " + i);

                    SpawnRandomEnemiesFromGroup(i);

                    return;
                }
            }

            public void SpawnRandomEnemiesFromGroup(int groupNumber)
            {
                //TODO: formula for enemy count and game difficulty
                float enemyCount = m_manager.GetDifficulty;
                //offset to make the enemy positions more random
                float offset = Random.Range(0f, 1f);

                for (int i = 0; i < enemyCount; i++)
                {
                    float angle = (i / enemyCount + offset) * Mathf.PI * 2f;

                    GameObject enemy = Instantiate(m_enemyGroups[groupNumber].RandomEnemyPrefab());
                    enemy.GetComponent<Enemy>().Initialize(transform);

                    enemy.transform.position = new(transform.position.x + (Mathf.Cos(angle) * m_spawnRange), 0f,
                        transform.position.z + (Mathf.Sin(angle) * m_spawnRange));
                }
            }

            public void SpawnRandomEnemyFromGroup(int groupNumber)
            {
                float angle = Random.Range(0f,1f) * Mathf.PI * 2f;

                GameObject enemy = Instantiate(m_enemyGroups[groupNumber].RandomEnemyPrefab());
                enemy.GetComponent<Enemy>().Initialize(transform);

                enemy.transform.position = new(transform.position.x + (Mathf.Cos(angle) * m_spawnRange), 0f,
                    transform.position.z + (Mathf.Sin(angle) * m_spawnRange));
            }

            public void SpawnEnemyFromGroup(int groupNumber, int prefabIndex)
            {
                float angle = Random.Range(0f, 1f) * Mathf.PI * 2f;

                GameObject enemy = Instantiate(m_enemyGroups[groupNumber].EnemyPrefab(prefabIndex));

                enemy.transform.position = new(transform.position.x + (Mathf.Cos(angle) * m_spawnRange), 0f,
                    transform.position.z + (Mathf.Sin(angle) * m_spawnRange));
            }

            public void OnDrawGizmos()
            {
                if (transform) Gizmos.DrawWireSphere(transform.position, m_spawnRange);
            }
        }
    }
}