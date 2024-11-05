using ILOVEYOU.Management;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace ILOVEYOU.EnemySystem
{
    [CreateAssetMenu(fileName = "New Simple Modifier", menuName = "ILOVEYOU Objects/Enemy Modifiers/Simple")]
    public class SimpleEnemyModifier : EnemyModifier
    {
        [Flags]
        private enum ModifierType
        {
            None,
            Damage  = 1,
            Health  = 2,
            Speed   = 4,
            Size    = 8
        };
        [SerializeField] private ModifierType m_modType;
        private enum ModifierOperation
        {
            Add,
            Sub,
            Mul,
            Div,
            Set
        }
        [SerializeField] private ModifierOperation m_operation;
        [SerializeField] private AnimationCurve m_value;
        public override bool ApplyModifications(Enemy target)
        {
            if(m_modType.HasFlag(ModifierType.Damage))
            {
                Debug.Log("using damage");
                target.GetSetDamage = _applyValue(target.GetSetDamage, m_value.Evaluate(GameManager.Instance.GetCurrentDifficulty));

            }
            if(m_modType.HasFlag(ModifierType.Health))
            {
                Debug.Log("using health");
                target.GetSetMaxHealth = _applyValue(target.GetSetMaxHealth, m_value.Evaluate(GameManager.Instance.PercentToMaxDiff));
            }
            if (m_modType.HasFlag(ModifierType.Speed))
            {
                NavMeshAgent agent = target.GetComponent<NavMeshAgent>();
                agent.speed = _applyValue(agent.speed, m_value.Evaluate(GameManager.Instance.GetCurrentDifficulty));

            }
            if (m_modType.HasFlag(ModifierType.Size))
            {
                target.transform.localScale = _applyValue(target.transform.localScale, m_value.Evaluate(GameManager.Instance.PercentToMaxDiff));
            }
            return false;
        }
        private float _applyValue(float variable, float v)
        {
            switch (m_operation)
            {
                case ModifierOperation.Add:
                    variable += v;
                    break;
                case ModifierOperation.Sub:
                    variable -= v;
                    break;
                case ModifierOperation.Mul:
                    variable *= v;
                    break;
                case ModifierOperation.Div:
                    variable /= v;
                    break;
                case ModifierOperation.Set:
                    variable = v;
                    break;
            }
            return variable;
        }
        private Vector3 _applyValue(Vector3 variable, float v)
        {
            switch (m_operation)
            {
                case ModifierOperation.Add:
                    variable += new Vector3(v, v, v);
                    break;
                case ModifierOperation.Sub:
                    variable -= new Vector3(v, v, v);
                    break;
                case ModifierOperation.Mul:
                    variable *= v;
                    break;
                case ModifierOperation.Div:
                    variable /= v;
                    break;
                case ModifierOperation.Set:
                    variable = new Vector3(v, v, v);
                    break;
            }
            return variable;
        }
    }
}
