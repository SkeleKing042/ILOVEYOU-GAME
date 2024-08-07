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

            /// <summary>
            /// called by GameManager to initialize the m_manager variable
            /// </summary>
            public void Initialize(GameManager manager)
            {
                m_manager = manager;
            }
            /// <summary>
            /// spawns a group of enemies around the player
            /// </summary>
            public void SpawnEnemyWave()
            {   
                //goes through each prefab list
                for (int i = 0; i < m_enemyGroups.Length; i++)
                {
                    //ignores list if threshold is 0 or the current difficulty is larger than the threshold assigned to the group
                    if (m_manager.GetDifficulty > m_enemyGroups[i].Threshold() || m_enemyGroups[i].Threshold() == 0) continue;

                    SpawnRandomEnemiesFromGroup(i);

                    //terminates function to prevent further enemy spawn functions from being called
                    return;
                }
            }
            /// <summary>
            /// Creates enemies in a circle surrounding the player. Number of enemies depends on the current difficulty.
            /// </summary>
            /// <param name="groupNumber">enemy group to spawn from</param>
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
            /// <summary>
            /// Spawns a singular random enemy from a group
            /// </summary>
            /// <param name="groupNumber">enemy group to spawn from</param>
            public void SpawnRandomEnemyFromGroup(int groupNumber)
            {
                float angle = Random.Range(0f,1f) * Mathf.PI * 2f;

                GameObject enemy = Instantiate(m_enemyGroups[groupNumber].RandomEnemyPrefab());
                enemy.GetComponent<Enemy>().Initialize(transform);

                enemy.transform.position = new(transform.position.x + (Mathf.Cos(angle) * m_spawnRange), 0f,
                    transform.position.z + (Mathf.Sin(angle) * m_spawnRange));
            }
            /// <summary>
            /// spawns a singular specified enemy from a group
            /// </summary>
            /// <param name="groupNumber">enemy group to spawn from</param>
            /// <param name="prefabIndex">which enemy from the array to spawn</param>
            public void SpawnEnemyFromGroup(int groupNumber, int prefabIndex)
            {
                float angle = Random.Range(0f, 1f) * Mathf.PI * 2f;

                GameObject enemy = Instantiate(m_enemyGroups[groupNumber].EnemyPrefab(prefabIndex));

                enemy.transform.position = new(transform.position.x + (Mathf.Cos(angle) * m_spawnRange), 0f,
                    transform.position.z + (Mathf.Sin(angle) * m_spawnRange));
            }

            public void OnDrawGizmos()
            {
                //this is just to make it easier to visualise where enemies will spawn
                if (transform) Gizmos.DrawWireSphere(transform.position, m_spawnRange);
            }
        }
    }
}