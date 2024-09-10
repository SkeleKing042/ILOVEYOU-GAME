using ILOVEYOU.ProjectileSystem;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace ILOVEYOU
{
    namespace BuffSystem
    {
        public class BuffSystem : MonoBehaviour
        {
            [Serializable]
            public class BuffData
            {
                //Generic
                [SerializeField] private string m_name = "";
                [SerializeField] private int m_buffType = 0; //0 = stats change, 1 = bullet change, 2 = Unity Event
                [SerializeField] private bool m_isPermanent = false;
                [SerializeField] private float m_time = 1f;
                [SerializeField] private GameObject m_particleEffect;

                //Values for stat change
                [SerializeField] private float m_maxHealthValue = 0f;
                [SerializeField] private float m_moveSpeedValue = 0f;
                [SerializeField] private float m_shootSpeedValue = 0f;
                [SerializeField] private float m_damageValue = 0f;
                
                //values for bullet change
                [SerializeField] private BulletPattern m_pattern;

                //Events for Event Type
                [SerializeField] private UnityEvent m_onActivate;
                [SerializeField] private UnityEvent m_onExpire;

                public string GetName { get { return m_name; } }
                public int GetBuffType { get { return m_buffType; } }
                public bool GetPermanent { get { return m_isPermanent; } }
                public float GetTime { get { return m_time; } }

                public float GetMaxHealth { get { return m_maxHealthValue; } }
                public float GetMoveSpeed { get { return m_moveSpeedValue; } }
                public float GetShootSpeed { get { return m_shootSpeedValue; } }
                public float GetDamage { get { return m_damageValue; } }



                public void SetName(string name) => m_name = name;
                public void SetBuffType(int type) => m_buffType = type;
                public void SetPermanent(bool set) => m_isPermanent = set;
                public void SetTime(float time) => m_time = time;

                public void SetMaxHealth(float health) => m_maxHealthValue = health;
                public void SetMoveSpeed(float speed) => m_moveSpeedValue = speed;
                public void SetShootSpeed(float speed) => m_shootSpeedValue = speed;
                public void SetDamage(float damage) => m_damageValue = damage;

            }

            [SerializeField] private BuffData[] m_buffData = new BuffData[0];

            public BuffData[] GetData { get { return m_buffData; } }

            #region DataEditing
#if UNITY_EDITOR
            public void ChangeData(BuffData[] data)
            {
                m_buffData = data;
            }

            public void CreateData()
            {
                Array.Resize(ref m_buffData, m_buffData.Length + 1);
            }
#endif
            #endregion
        }

    }
}

