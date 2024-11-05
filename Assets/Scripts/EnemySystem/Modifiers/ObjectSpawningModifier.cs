using ILOVEYOU.EnemySystem;
using ILOVEYOU.Environment;
using UnityEngine;

namespace ILOVEYOU.EnemySystem
{
    [CreateAssetMenu(fileName = "New Object Spawing Modifier", menuName = "ILOVEYOU Objects/Enemy Modifiers/Object Spawner")]
    public class ObjectSpawningModifier : EnemyModifier
    {
        [SerializeField] private GameObject[] m_objectsToSpawn;
        [SerializeField] private ObjectSpawner.SpawnStyle m_spawnStyle;
        [SerializeField] private float m_timeBetweenSpawns;
        [SerializeField] private float m_objectLifetime;
        [SerializeField] private int m_indexOrCount;

        public override bool ApplyModifications(Enemy target)
        {
            return target.gameObject.AddComponent<ObjectSpawner>().Initialize(m_objectsToSpawn, m_spawnStyle, m_timeBetweenSpawns, m_objectLifetime, m_indexOrCount);
        }
    }
}