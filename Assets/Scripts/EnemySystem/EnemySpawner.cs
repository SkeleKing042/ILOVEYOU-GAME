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


            [SerializeField] private Transform TEMPPLAYERPOS;

            private void Start()
            {
                SpawnEnemyWave();
            }

            public void SpawnEnemyWave()
            {
                float tempTimeValue = 2f;
                

                for (int i = 0; i < m_enemyGroups.Length; i++)
                {
                    if (tempTimeValue > m_enemyGroups[i].Threshold() || m_enemyGroups[i].Threshold() == 0) continue;

                    Debug.Log("Spawning Group " + i);

                    SpawnRandomEnemiesFromGroup(i);
                }
            }

            public void SpawnRandomEnemiesFromGroup(int groupNumber)
            {
                int tempEnemyCount = 10;

                for (int i = 0; i < tempEnemyCount; i++)
                {
                    float angle = i / (float)tempEnemyCount * Mathf.PI * 2f;

                    GameObject enemy = Instantiate(m_enemyGroups[groupNumber].RandomEnemyPrefab());
                    enemy.GetComponent<Enemy>().Initialize(TEMPPLAYERPOS);

                    enemy.transform.position = new(TEMPPLAYERPOS.position.x + (Mathf.Cos(angle) * m_spawnRange), 0f,
                        TEMPPLAYERPOS.position.z + (Mathf.Sin(angle) * m_spawnRange));
                }
            }

            public void SpawnRandomEnemyFromGroup(int groupNumber)
            {
                float angle = Random.Range(0f,1f) * Mathf.PI * 2f;

                GameObject enemy = Instantiate(m_enemyGroups[groupNumber].RandomEnemyPrefab());
                enemy.GetComponent<Enemy>().Initialize(TEMPPLAYERPOS);

                enemy.transform.position = new(TEMPPLAYERPOS.position.x + (Mathf.Cos(angle) * m_spawnRange), 0f,
                    TEMPPLAYERPOS.position.z + (Mathf.Sin(angle) * m_spawnRange));
            }

            public void SpawnEnemyFromGroup(int groupNumber, int prefabIndex)
            {
                float angle = Random.Range(0f, 1f) * Mathf.PI * 2f;

                GameObject enemy = Instantiate(m_enemyGroups[groupNumber].EnemyPrefab(prefabIndex));

                enemy.transform.position = new(TEMPPLAYERPOS.position.x + (Mathf.Cos(angle) * m_spawnRange), 0f,
                    TEMPPLAYERPOS.position.z + (Mathf.Sin(angle) * m_spawnRange));
            }

            public void OnDrawGizmos()
            {
                if (TEMPPLAYERPOS) Gizmos.DrawWireSphere(TEMPPLAYERPOS.position, m_spawnRange);
            }
        }
    }
}