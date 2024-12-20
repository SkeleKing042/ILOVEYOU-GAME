using ILOVEYOU.Management;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;
namespace ILOVEYOU
{
    namespace EnemySystem
    {
        [Serializable]
        public class EnemyPrefabs
        {
            [SerializeField] private string m_groupName;
            //[SerializeField][Tooltip("When difficulty/time is below this value, use this spawn group, values with 0 are ignored in the wave spawner"), Min(-1)] private Vector2 m_threshold;
            [Tooltip("When the enemy group spawns in. The X axis is the difficulty and Y is the likelyness of the enemy group spawning.")]
            [SerializeField] private AnimationCurve m_spawnRate;
            [SerializeField] private GameObject[] m_enemyPrefabs;
            [SerializeField] private bool m_usingMods;
            public bool CanUseMods => m_usingMods;

            public GameObject EnemyPrefab(int i) { return m_enemyPrefabs[i]; }
            public GameObject RandomEnemyPrefab() { return m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Length)]; }
            public AnimationCurve Threshold() { return m_spawnRate; }

        }
        public class EnemySpawner : MonoBehaviour
        {
            private float m_spawnRange { get { return Random.Range(GameSettings.Current.GetSpawnRangeMin, GameSettings.Current.GetSpawnRangeMax); } }
            [SerializeField] private LayerMask m_spawnMask;
            [SerializeField] private GameObject m_spawnEffect;

            private List<GameObject> m_enemyObjects = new();
            public List<GameObject> GetEnemies => m_enemyObjects;
            
            public float PercentToMaxEnemies => m_enemyObjects.Count / GameSettings.Current.GetSpawnCap.Evaluate(GameManager.Instance.PercentToMaxDiff);

            [Header("Events")]
            [SerializeField] private UnityEvent m_onSpawnEnemy;

            /// <summary>
            /// called by GameManager to initialize the m_manager variable
            /// </summary>
            public bool Initialize()
            {
                Debug.Log("Initializing enemy spawner");
                
                return true;
            }
            /// <summary>
            /// kills all enemies
            /// </summary>
            public void KillAllEnemies()
            {
                foreach(GameObject obj in m_enemyObjects)
                {
                    obj.GetComponent<Enemy>().TakeDamage(999999999999); //woah reference!!!
                }
            }
            /// <summary>
            /// destroys all enemies
            /// </summary>
            public void DestroyAllEnemies()
            {
                for (int i = 0; i < m_enemyObjects.Count; i++)
                {
                    //possible todo: disintergrate enemies?
                    Destroy(m_enemyObjects[i]);
                }

                m_enemyObjects.Clear();
            }
            /// <summary>
            /// Disables all enemies
            /// </summary>
            public void DisableAllEnemies()
            {
                foreach (GameObject obj in m_enemyObjects)
                {
                    obj.GetComponent<Enemy>().enabled = false;
                    obj.GetComponent<Enemy>().StopAllCoroutines();
                    obj.GetComponent<NavMeshAgent>().enabled = false;
                    foreach (Collider col in obj.GetComponentsInChildren<Collider>()) col.enabled = false;
                }
            }
            /// <summary>
            /// spawns a group of enemies around the player
            /// </summary>
            public void SpawnEnemyWave()
            {   
                //goes through each prefab list
                for (int i = 0; i < GameSettings.Current.GetEnemyGroups.Length; i++)
                {
                    EnemyPrefabs current = GameSettings.Current.GetEnemyGroups[i];
                    float rnd = Random.Range(0.0f, 1.0f);
                    //ignores list if threshold is 0 or the current difficulty is larger than the threshold assigned to the group
                    if (current.Threshold().Evaluate(Mathf.Clamp(GameManager.Instance.PercentToMaxDiff, 0 , 1)) <= rnd) continue;

                    if (_SpawnEnemy(current.RandomEnemyPrefab(), false, current.CanUseMods)) m_onSpawnEnemy.Invoke();
                }
            }
            /// <summary>
            /// Creates enemies in a circle surrounding the player. Number of enemies depends on the current difficulty.
            /// </summary>
            /// <param name="groupNumber">enemy group to spawn from</param>
            public void SpawnRandomEnemiesFromGroup(int groupNumber)
            {
                float enemyCount = GameManager.Instance.GetCurrentDifficulty + 1;

                for (int i = 0; i < enemyCount; i++)
                {

                    if(_SpawnEnemy(GameSettings.Current.GetEnemyGroups[groupNumber].RandomEnemyPrefab(), false, GameSettings.Current.GetEnemyGroups[groupNumber].CanUseMods)) m_onSpawnEnemy.Invoke();
                }
            }
            /// <summary>
            /// Creates enemies in a circle surrounding the player. Number of enemies depends on the what is inputted.
            /// </summary>
            /// <param name="groupNumber">enemy group to spawn from</param>
            /// <param name="enemyCount">number of enemies</param>
            public void SpawnRandomNumberOfEnemiesFromGroup(int groupNumber, int enemyCount, bool ignoreCap)
            {

                for (int i = 0; i < enemyCount; i++)
                {
                    if (_SpawnEnemy(GameSettings.Current.GetEnemyGroups[groupNumber].RandomEnemyPrefab(), ignoreCap, GameSettings.Current.GetEnemyGroups[groupNumber].CanUseMods)) m_onSpawnEnemy.Invoke();
                }
            }
            /// <summary>
            /// Spawns a singular random enemy from a group
            /// </summary>
            /// <param name="groupNumber">enemy group to spawn from</param>
            public void SpawnRandomEnemyFromGroup(int groupNumber)
            {
                if (_SpawnEnemy(GameSettings.Current.GetEnemyGroups[groupNumber].RandomEnemyPrefab(), false, GameSettings.Current.GetEnemyGroups[groupNumber].CanUseMods)) m_onSpawnEnemy.Invoke();
            }
            /// <summary>
            /// spawns a singular specified enemy from a group
            /// </summary>
            /// <param name="groupNumber">enemy group to spawn from</param>
            /// <param name="prefabIndex">which enemy from the array to spawn</param>
            public void SpawnEnemyFromGroup(int groupNumber, int prefabIndex)
            {
                if(_SpawnEnemy(GameSettings.Current.GetEnemyGroups[groupNumber].EnemyPrefab(prefabIndex), false, GameSettings.Current.GetEnemyGroups[groupNumber].CanUseMods)) m_onSpawnEnemy.Invoke();
            }

            public void OnDrawGizmosSelected()
            {
                //this is just to make it easier to visualise where enemies will spawn
                if (transform) Gizmos.DrawWireSphere(transform.position, GameSettings.Current.GetSpawnRangeMin);
                if (transform) Gizmos.DrawWireSphere(transform.position, GameSettings.Current.GetSpawnRangeMax);
            }

            private bool _SpawnEnemy(GameObject prefab, bool ignoreCap, bool useMods = true)
            {
                if (!ignoreCap && m_enemyObjects.Count >= GameSettings.Current.GetSpawnCap.Evaluate(GameManager.Instance.PercentToMaxDiff))
                {
                    Debug.Log("Max number of enemies reached!");
                    return false;
                }

                //try for a modifier
                GameSettings.ModListEntry[] entries = GameSettings.Current.GetModList;
                List<EnemyModifier> mods = new();
                if (entries.Length > 0 && useMods)
                {
                    int rolls = Mathf.FloorToInt(GameSettings.Current.GetMaxModCount);
                    for (int i = 0; i < rolls; i++)
                    {
                        int rndMod = Random.Range(0, entries.Length);
                        float modChance = Random.Range(0f, 1f);
                        if (entries[rndMod].Modifier.IsStackable || !mods.Contains(entries[rndMod].Modifier))
                            if (modChance <= entries[rndMod].Chance)
                                mods.Add(entries[rndMod].Modifier);
                    }
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
                        enemy.GetComponent<Enemy>().Initialize(transform, mods.ToArray());
                        m_enemyObjects.Add(enemy);

                        //spawns in spawn object
                        GameObject spawnObject = Instantiate(m_spawnEffect);
                        spawnObject.transform.position = enemy.transform.position;
                        Destroy(spawnObject, 1f);

                        return true;
                    }
                }

                //destroys if no attempts work
                Destroy(enemy);
                return false;
            }
            public bool RespawnEnemy(GameObject enemy)
            {
                if (!m_enemyObjects.Contains(enemy))
                {
                    Debug.LogWarning($"{enemy} isn't spawned and cannot be respawned.");
                    return false;
                }

                _SpawnEnemy(enemy, true);
                Destroy(enemy);
                return true;
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