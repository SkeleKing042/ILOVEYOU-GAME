using ILOVEYOU.EnemySystem;
using ILOVEYOU.Player;
using UnityEngine;

namespace ILOVEYOU.Cards
{
    public class StunCard : DisruptCard
    {
            [SerializeField] private float m_stunDuration;
            public override void ExecuteEvents(PlayerManager caller)
            {
                base.ExecuteEvents(caller);

                foreach(var enemy in caller.GetLevelManager.GetSpawner.GetEnemies)
                {
                    Enemy brain = enemy.GetComponent<Enemy>();
                    brain.GetStunned(m_stunDuration);
                }
            }
        }
}
