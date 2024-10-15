using ILOVEYOU.Management;
using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ILOVEYOU 
{ 
    namespace Cards
    {
        public class EnemyCard : MonoBehaviour
        {

            [Tooltip("Which group id to spawn from")] [SerializeField] private int m_enemyGroup = 0;
            [Tooltip("How many enemies to spawn")] [SerializeField] private int m_enemyCount = 0;
            [Tooltip("If Spawning Should scale")][SerializeField] private bool m_scale = true;
            [Tooltip("If spawning should ignore the enemy cap")][SerializeField] private bool m_ignoreCap = false;
            public void ExecuteEvents(object[] data)
            {
                //get required data
                PlayerManager player = (PlayerManager)data[1];


                foreach (PlayerManager target in GameManager.Instance.GetOtherPlayers(player))
                {
                    if (m_scale) target.GetLevelManager.GetSpawner.SpawnRandomNumberOfEnemiesFromGroup(m_enemyGroup, m_enemyCount * (int)Mathf.Ceil(GameManager.Instance.PercentToMaxDiff), m_ignoreCap);
                    else target.GetLevelManager.GetSpawner.SpawnRandomNumberOfEnemiesFromGroup(m_enemyGroup, m_enemyCount, m_ignoreCap);
                    //target.GetLevelManager.GetSpawner.SpawnEnemyWave();
                }
            }
        }
    }

}