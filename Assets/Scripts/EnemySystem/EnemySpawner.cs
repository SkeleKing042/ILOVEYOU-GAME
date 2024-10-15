using ILOVEYOU.Management;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
                //[SerializeField][Tooltip("When difficulty/time is below this value, use this spawn group, values with 0 are ignored in the wave spawner"), Min(-1)] private Vector2 m_threshold;
                [Tooltip("When the enemy group spawns in. The X axis is the difficulty and Y is the likelyness of the enemy group spawning.")]
                [SerializeField] private AnimationCurve m_spawnRate;
                [SerializeField] private GameObject[] m_enemyPrefabs;

                public GameObject EnemyPrefab(int i) { return m_enemyPrefabs[i]; }
                public GameObject RandomEnemyPrefab() { return m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Length)]; }
                public AnimationCurve Threshold() { return m_spawnRate; }

            }

            [SerializeField] private EnemyPrefabs[] m_enemyGroups;
            [SerializeField] private float m_spawnRangeMin;
            [SerializeField] private float m_spawnRangeMax;
            private float m_spawnRange { get { return Random.Range(m_spawnRangeMin, m_spawnRangeMax); } }
            [SerializeField] private LayerMask m_spawnMask;

            [SerializeField] private AnimationCurve m_enemyCap;
            private List<GameObject> m_enemyObjects = new();

            [Header("Events")]
            [SerializeField] private UnityEvent m_onSpawnEnemy;

            /// <summary>
            /// called by GameManager to initialize the m_manager variable
            /// </summary>
            public bool Initialize()
            {
                return true;
            }
            /// <summary>
            /// spawns a group of enemies around the player
            /// </summary>
            public void SpawnEnemyWave()
            {   
                //goes through each prefab list
                for (int i = 0; i < m_enemyGroups.Length; i++)
                {
                    float rnd = Random.Range(0.0f, 1.0f);
                    //ignores list if threshold is 0 or the current difficulty is larger than the threshold assigned to the group
                    if (m_enemyGroups[i].Threshold().Evaluate(Mathf.Clamp(GameManager.Instance.PercentToMaxDiff, 0 , 1)) <= rnd) continue;

                    if (_SpawnEnemy(m_enemyGroups[i].RandomEnemyPrefab())) m_onSpawnEnemy.Invoke();
                }
            }
            /// <summary>
            /// Creates enemies in a circle surrounding the player. Number of enemies depends on the current difficulty.
            /// </summary>
            /// <param name="groupNumber">enemy group to spawn from</param>
            public void SpawnRandomEnemiesFromGroup(int groupNumber)
            {
                //TODO: formula for enemy count and game difficulty
                float enemyCount = GameManager.Instance.GetDifficulty + 1;

                for (int i = 0; i < enemyCount; i++)
                {

                    if(_SpawnEnemy(m_enemyGroups[groupNumber].RandomEnemyPrefab())) m_onSpawnEnemy.Invoke();
                }
            }
            /// <summary>
            /// Creates enemies in a circle surrounding the player. Number of enemies depends on the what is inputted.
            /// </summary>
            /// <param name="groupNumber">enemy group to spawn from</param>
            /// <param name="enemyCount">number of enemies</param>
            public void SpawnRandomNumberOfEnemiesFromGroup(int groupNumber, int enemyCount)
            {

                for (int i = 0; i < enemyCount; i++)
                {
                    if (_SpawnEnemy(m_enemyGroups[groupNumber].RandomEnemyPrefab())) m_onSpawnEnemy.Invoke();
                }
            }
            /// <summary>
            /// Spawns a singular random enemy from a group
            /// </summary>
            /// <param name="groupNumber">enemy group to spawn from</param>
            public void SpawnRandomEnemyFromGroup(int groupNumber)
            {
                if (_SpawnEnemy(m_enemyGroups[groupNumber].RandomEnemyPrefab())) m_onSpawnEnemy.Invoke();
            }
            /// <summary>
            /// spawns a singular specified enemy from a group
            /// </summary>
            /// <param name="groupNumber">enemy group to spawn from</param>
            /// <param name="prefabIndex">which enemy from the array to spawn</param>
            public void SpawnEnemyFromGroup(int groupNumber, int prefabIndex)
            {
                if(_SpawnEnemy(m_enemyGroups[groupNumber].EnemyPrefab(prefabIndex))) m_onSpawnEnemy.Invoke();
            }

            public void OnDrawGizmosSelected()
            {
                //this is just to make it easier to visualise where enemies will spawn
                if (transform) Gizmos.DrawWireSphere(transform.position, m_spawnRangeMin);
                if (transform) Gizmos.DrawWireSphere(transform.position, m_spawnRangeMax);
            }

            private bool _SpawnEnemy(GameObject prefab)
            {
                if (m_enemyObjects.Count >= m_enemyCap.Evaluate(GameManager.Instance.PercentToMaxDiff))
                {
                    Debug.Log("Max number of enemies reached!");
                    return false;
                }
                //creates enemy from given prefab
                GameObject enemy = Instantiate(prefab);
                //attempts 100 times to spawn an enemy
                for (int i = 0; i < 100; i++)
                {
                    float angle = Random.Range(0f, 1f) * Mathf.PI * 2f;
                    //sets position around circle
                    enemy.transform.position = new(transform.position.x + (Mathf.Cos(angle) * m_spawnRange), transform.position.y,
                    transform.position.z + (Mathf.Sin(angle) * m_spawnRange));
                    
                    //checks if the enemy is colliding with anything
                    if(!Physics.CheckSphere(enemy.transform.position, 1f, m_spawnMask))
                    {
                        //intializes enemy script
                        enemy.GetComponent<Enemy>().Initialize(transform);
                        m_enemyObjects.Add(enemy);
                        return true;
                    }
                }

                //destroys if no attempts work
                Destroy(enemy);
                return false;
            }

            private void Update()
            {
                //empty out the list
                for(int i = 0; i < m_enemyObjects.Count; i++)
                {
                    if (!m_enemyObjects[i])
                    {
                        m_enemyObjects.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }
}