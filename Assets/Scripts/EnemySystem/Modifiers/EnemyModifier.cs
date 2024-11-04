using UnityEngine;
using UnityEngine.UI;

namespace ILOVEYOU.EnemySystem
{
    public class EnemyModifier : ScriptableObject
    {
        [SerializeField] private Image m_ModifierIcon;
        public Image GetIcon => m_ModifierIcon;
        public virtual bool ApplyModifications(Enemy target)
        {
            return false;
        }
    }
}
