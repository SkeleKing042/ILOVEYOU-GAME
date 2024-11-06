using UnityEngine;
using UnityEngine.UI;

namespace ILOVEYOU.EnemySystem
{
    public class EnemyModifier : ScriptableObject
    {
        [SerializeField] private Image m_ModifierIcon;
        public Image GetIcon => m_ModifierIcon;
        [SerializeField] private bool m_stackable = false;
        public bool IsStackable => m_stackable;
        public virtual bool ApplyModifications(Enemy target)
        {
            return true;
        }
    }
}
