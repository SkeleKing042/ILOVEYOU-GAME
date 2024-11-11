using ILOVEYOU.Hazards;
using ILOVEYOU.Management;
using ILOVEYOU.Player;
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
        public class BuffDataSystem : MonoBehaviour
        {
            [Serializable]
            public class BuffData
            {
                //Generic
                [SerializeField] private string m_name = "";
                [SerializeField] private int m_buffType = 0; //0 = stats change, 1 = bullet change, 2 = Unity Event
                [SerializeField] private int m_buffID = 0; //this is for grouping specific buffs (good for effects you don't want to stack)
                [SerializeField] private int m_isStackable = 0; //determines if a buff will stack with others with the same ID (0 = don't stack, 1 = don't stack but extend timer, 2 = stack)
                [SerializeField] private bool m_isPermanent = false; //will apply the buff permanently (won't be added to remove timer)
                [SerializeField] private float m_time = 1f; //how long the buff will last
                [SerializeField] private GameObject[] m_particleEffect = new GameObject[0];

                //Values for stat change
                [SerializeField] private float m_maxHealthValue = 0f;
                [SerializeField] private float m_moveSpeedValue = 0f;
                [SerializeField] private float m_shootSpeedValue = 0f;
                [SerializeField] private float m_damageValue = 0f;

                //values for bullet change
                //[SerializeField] private BulletPatternObject m_pattern;

                //Events for Event Type
                [SerializeField] private UnityEvent m_onActivate;
                [SerializeField] private UnityEvent m_onExpire;

                public string GetName { get { return m_name; } }
                public int GetBuffID { get { return m_buffID; } }
                public int GetIsStackable { get { return m_isStackable; } }
                public int GetBuffType { get { return m_buffType; } }
                public bool GetPermanent { get { return m_isPermanent; } }
                public float GetTime { get { return m_time; } }

                public float GetMaxHealth { get { return m_maxHealthValue; } }
                public float GetMoveSpeed { get { return m_moveSpeedValue; } }
                public float GetShootSpeed { get { return m_shootSpeedValue; } }
                public float GetDamage { get { return m_damageValue; } }

                public GameObject[] GetParticleEffect { get { return m_particleEffect; } }
                //public BulletPatternObject GetPatternObject { get { return m_pattern; } }

                public void SetName(string name) => m_name = name;
                public void SetBuffID(int id) => m_buffID = id;
                public void SetIsStackable(int i) => m_isStackable = i;
                public void SetBuffType(int type) => m_buffType = type;
                public void SetPermanent(bool set) => m_isPermanent = set;
                public void SetTime(float time) => m_time = time;

                public void SetMaxHealth(float health) => m_maxHealthValue = health;
                public void SetMoveSpeed(float speed) => m_moveSpeedValue = speed;
                public void SetShootSpeed(float speed) => m_shootSpeedValue = speed;
                public void SetDamage(float damage) => m_damageValue = damage;

                public void InvokeActivate() => m_onActivate.Invoke();
                public void InvokeExpire() => m_onExpire.Invoke();

                //can only make custom buffs that influence player stats
                public BuffData(string name, int buffID, int canStack, bool isPermanent, float time, float maxHealth, float moveSpeed, float shootSpeed, float damage)
                {
                    m_name = name;
                    m_buffID = buffID;
                    m_isStackable = canStack;
                    m_isPermanent = isPermanent;
                    m_time = time;
                    m_maxHealthValue = maxHealth;
                    m_moveSpeedValue = moveSpeed;
                    m_shootSpeedValue = shootSpeed;
                    m_damageValue = damage;
                }

            }

            public class ActiveBuff
            {
                private BuffData m_data;
                private float m_currentTime;
                private GameObject[] m_buffParticleEffect = new GameObject[0];
                //private bulle

                public BuffData GetData { get { return m_data; } }
                public GameObject[] GetParticles { get { return m_buffParticleEffect; } }
                public float CurrentTime { get { return m_currentTime; } }
                /// <summary>
                /// subtracts time according to deltatime
                /// </summary>
                /// <param name="time">time is multiplied by deltatime</param>
                public void SubtractTime(float time) { m_currentTime -= time * Time.deltaTime; }
                public void AddTime(float time) { m_currentTime += time; }
                public void SetBuffParticles(GameObject[] obj) { m_buffParticleEffect = obj; }
                public void DestroyBuffParticles() { foreach(var part in m_buffParticleEffect) Destroy(part); }

                public ActiveBuff(BuffData data, float time)
                {
                    m_data = data;
                    m_currentTime = time;
                }
            }

            [SerializeField] private BuffData[] m_buffData = new BuffData[0];

            public BuffData[] GetData { get { return m_buffData; } }

            private List<ActiveBuff> m_activeBuffs;

            public List<ActiveBuff> GetActiveBuffs { get { return m_activeBuffs; } }

            private PlayerControls m_playerControls;

            public void GiveBuff(int buffID)
            {
                BuffData dataClone = m_buffData[buffID];

                bool spawnParticle = true;


                //if conflicting data IDs in already applied buffs
                if (_CheckID(dataClone.GetBuffID, out int conflictPos))
                {
                    switch (dataClone.GetIsStackable)
                    {
                        case 0:
                            //if it can't stack stop adding buff
                            return;
                        case 1:
                            //if extending timer add time to already existing buff
                            _AddTime(dataClone.GetBuffID, dataClone.GetTime);
                            return;
                        case 2:
                            //if the buff is meant to override, remove the current one
                            RemoveBuff(conflictPos, true);
                            m_activeBuffs[conflictPos] = null;
                            _CleanList();
                            break;
                        default:
                            //do nothing lol
                            //prevents particles from stacking
                            spawnParticle = false;
                            break;
                    }
                }

                ActiveBuff buff = new ActiveBuff(dataClone, dataClone.GetTime);

                _ActivateBuff(dataClone);

                if (dataClone.GetParticleEffect.Length > 0 && spawnParticle) buff.SetBuffParticles(ParticleSpawner.SpawnParticles(dataClone.GetParticleEffect, transform));

                m_activeBuffs.Add(buff);
            }

            public void GiveBuff(int buffID, float time)
            {
                BuffData dataClone = m_buffData[buffID];

                bool spawnParticle = true;


                //if conflicting data IDs in already applied buffs
                if (_CheckID(dataClone.GetBuffID, out int conflictPos))
                {
                    switch (dataClone.GetIsStackable)
                    {
                        case 0:
                            //if it can't stack stop adding buff
                            return;
                        case 1:
                            //if extending timer add time to already existing buff
                            _AddTime(dataClone.GetBuffID, time);
                            return;
                        case 2:
                            //if the buff is meant to override, remove the current one
                            RemoveBuff(conflictPos, true);
                            m_activeBuffs[conflictPos] = null;
                            _CleanList();
                            break;
                        default:
                            //do nothing lol
                            //prevents particles from stacking
                            spawnParticle = false;
                            break;
                    }
                }

                ActiveBuff buff = new ActiveBuff(dataClone, time);

                _ActivateBuff(dataClone);

                if (dataClone.GetParticleEffect.Length > 0 && spawnParticle) buff.SetBuffParticles(ParticleSpawner.SpawnParticles(dataClone.GetParticleEffect, transform));

                m_activeBuffs.Add(buff);
            }

            public void GiveBuff(BuffData customData)
            {
                bool spawnParticle = true;

                //if conflicting data IDs in already applied buffs
                if (_CheckID(customData.GetBuffID, out int conflictPos))
                {
                    switch (customData.GetIsStackable)
                    {
                        case 0:
                            //if it can't stack stop adding buff
                            return;
                        case 1:
                            //if extending timer add time to already existing buff
                            _AddTime(customData.GetBuffID, customData.GetTime);
                            return;
                        case 2:
                            //if the buff is meant to override, remove the current one
                            RemoveBuff(conflictPos, true);
                            m_activeBuffs[conflictPos] = null;
                            _CleanList();
                            break;
                        default:
                            //do nothing lol
                            //prevents particles from stacking
                            spawnParticle = false;
                            break;
                    }
                }

                ActiveBuff buff = new ActiveBuff(customData, customData.GetTime);

                _ActivateBuff(customData);

                if (customData.GetParticleEffect.Length > 0 && spawnParticle) buff.SetBuffParticles(ParticleSpawner.SpawnParticles(customData.GetParticleEffect, transform));

                m_activeBuffs.Add(buff);
            }

            public void RemoveBuff(int listCount, bool deactivate)
            {
                m_activeBuffs[listCount].DestroyBuffParticles();

                if (deactivate) _DeactivateBuff(m_activeBuffs[listCount].GetData);
            }
            private void _ActivateBuff(BuffData data)
            {
                switch (data.GetBuffType)
                {
                    case 0:
                        m_playerControls.ChangeStat(0, data.GetMoveSpeed);
                        m_playerControls.ChangeStat(1, data.GetMaxHealth);
                        m_playerControls.ChangeStat(2, data.GetDamage);
                        m_playerControls.ChangeStat(3, data.GetShootSpeed);

                        break;
                    case 1:
                        data.InvokeActivate();
                        break;
                }

                

            }
            private void _DeactivateBuff(BuffData data)
            {
                switch (data.GetBuffType)
                {
                    case 0:
                        m_playerControls.ChangeStat(0, -data.GetMoveSpeed);
                        m_playerControls.ChangeStat(1, -data.GetMaxHealth);
                        m_playerControls.ChangeStat(2, -data.GetDamage);
                        m_playerControls.ChangeStat(3, -data.GetShootSpeed);
                        break;
                    case 1:
                        data.InvokeExpire();
                        break;
                }
            }

            private void _AddTime(int ID, float time)
            {
                foreach(ActiveBuff activeBuff in m_activeBuffs)
                {
                    if (activeBuff.GetData.GetBuffID == ID) activeBuff.AddTime(time);
                }
            }

            private bool _CheckID(int ID)
            {
                foreach(ActiveBuff activeBuff in m_activeBuffs)
                {
                    //buff with same ID found
                    if(activeBuff.GetData.GetBuffID == ID)
                    { 
                        return true;
                    }
                }
                return false;
            }

            private bool _CheckID(int ID, out int pos)
            {
                int count = 0;
                pos = 0;

                foreach (ActiveBuff activeBuff in m_activeBuffs)
                {
                    //buff with same ID found
                    if (activeBuff.GetData.GetBuffID == ID)
                    {
                         
                        return true;
                    }

                    pos = count;
                    count++;

                }
                return false;
            }

            /// <summary>
            /// Clears all buffs, including permanent buffs
            /// </summary>
            /// <param name="deactivate">if the deactivate function should also be called</param>
            public void ClearAllBuffs(bool deactivate)
            {
                if (deactivate)
                {
                    if (m_activeBuffs.Count == 0) return;

                    bool markForDeletion = false;

                    for (int i = 0; i < m_activeBuffs.Count; i++)
                    {

                            RemoveBuff(i, true);

                            m_activeBuffs[i] = null;

                            markForDeletion = true;
                    }

                    if (markForDeletion) _CleanList();
                }

                m_activeBuffs.Clear();
            }
            /// <summary>
            /// Clears all non-permanent buffs
            /// </summary>
            /// <param name="deactivate">if the deactivate function should also be called</param>
            public void ClearBuffs(bool deactivate)
            {
                if (m_activeBuffs.Count == 0) return;

                bool markForDeletion = false;

                for (int i = 0; i < m_activeBuffs.Count; i++)
                {
                    if (m_activeBuffs[i].GetData.GetPermanent) continue;

                    RemoveBuff(i, true);

                    m_activeBuffs[i] = null;

                    markForDeletion = true;
                }

                if (markForDeletion) _CleanList();
            }

            private void Awake()
            {
                m_playerControls = GetComponent<PlayerControls>();
                m_activeBuffs = new();

                //GiveBuff(0);
                //GiveBuff(1);
                //ClearAllBuffs(true);

            }

            private void Update()
            {
                if (m_activeBuffs.Count == 0) return;
                
                bool markForDeletion = false;

                for (int i = 0; i < m_activeBuffs.Count; i++)
                {
                    if (m_activeBuffs[i].GetData.GetPermanent) continue;

                    //Debug.Log(m_activeBuffs[i].GetData.GetName + " Buff (" + i + ") Time Remaining: " + m_activeBuffs[i].CurrentTime);

                    //if buff time has ran out;
                    if (m_activeBuffs[i].CurrentTime <= 0) 
                    {
                        //_DeactivateBuff(m_activeBuffs[i].GetData);

                        //Debug.Log(m_activeBuffs[i].GetData.GetName + " Buff (" + i + ") Finished");

                        RemoveBuff(i, true);

                        m_activeBuffs[i] = null;

                        markForDeletion = true;

                        continue; 
                    }

                    m_activeBuffs[i].SubtractTime(1f);
                }

                if(markForDeletion) _CleanList();
            }

            /// <summary>
            /// Clears out empty values from the list
            /// </summary>
            private void _CleanList()
            {
                List<ActiveBuff> tempList = new();

                foreach (ActiveBuff activeBuff in m_activeBuffs)
                {
                    if (activeBuff != null) tempList.Add(activeBuff);
                }

                m_activeBuffs = tempList;
            }

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

