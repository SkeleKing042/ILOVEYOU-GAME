using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace ILOVEYOU
{
    namespace BuffSystem
    {
        [CustomEditor(typeof(BuffSystem))]
        public class BuffSystemEditor : Editor
        {
            private readonly string[] m_effectNames = new string[] { "Stat Change", "Bullet Change", "Unity Event", "Blah blah blah" };
            private bool[] m_foldOuts;
            private BuffSystem m_system;


            public override void OnInspectorGUI()
            {
                m_system = target as BuffSystem;
                Array.Resize(ref m_foldOuts, m_system.GetData.Length);

                for (int i = 0; i < m_system.GetData.Length; i++)
                {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();

                    //EditorGUILayout.LabelField("Status Effect " + i + ":");

                    string title = (m_system.GetData[i].GetName == "") ? "Status Effect " + i + ":" : m_system.GetData[i].GetName + ":";

                    m_foldOuts[i] = EditorGUILayout.Foldout(m_foldOuts[i], title);

                    if (GUILayout.Button("Remove"))
                    {
                        _Delete(i);

                        break;
                    }

                    EditorGUILayout.EndHorizontal();

                    if (m_foldOuts[i])
                    {
                        EditorGUILayout.Space();

                        EditorGUI.BeginChangeCheck();

                        BuffSystem.BuffData current = m_system.GetData[i];

                        string name = current.GetName;
                        int buffType = current.GetBuffType;
                        bool isPermanent = current.GetPermanent;
                        float time = current.GetTime;
                        SerializedProperty particleEffect = serializedObject.FindProperty("m_buffData").GetArrayElementAtIndex(i).FindPropertyRelative("m_particleEffect"); ;

                        float maxHealthValue = current.GetMaxHealth;
                        float moveSpeedValue = current.GetMoveSpeed;
                        float shootSpeedValue = current.GetShootSpeed;
                        float damageValue = current.GetDamage;

                        name = EditorGUILayout.TextField("Status Name: ", name);
                        buffType = EditorGUILayout.Popup("Buff Type:", buffType, m_effectNames);
                        isPermanent = EditorGUILayout.Toggle("Is Permanent:", isPermanent);
                        if (!isPermanent) time = EditorGUILayout.FloatField("Buff Time:", time);
                        EditorGUILayout.PropertyField(particleEffect, new GUIContent("Particle Effect: "));

                        

                        EditorGUILayout.Space();

                        switch (buffType)
                        {
                            case 0://Stat Change
                                maxHealthValue = EditorGUILayout.FloatField("Max Health Add: ", maxHealthValue);
                                moveSpeedValue = EditorGUILayout.FloatField("Move speed Add: ", moveSpeedValue);
                                shootSpeedValue = EditorGUILayout.FloatField("Shoot Speed Multiplier Add: ", shootSpeedValue);
                                damageValue = EditorGUILayout.FloatField("Damage Multiplier Add: ", damageValue);
                                break;
                            case 1://Bullet change
                                SerializedProperty bulletObject = serializedObject.FindProperty("m_buffData").GetArrayElementAtIndex(i).FindPropertyRelative("m_pattern");

                                EditorGUILayout.PropertyField(bulletObject, new GUIContent("New Bullet Pattern: "));
                                break;

                            case 2://unity event

                                SerializedProperty onActivate = serializedObject.FindProperty("m_buffData").GetArrayElementAtIndex(i).FindPropertyRelative("m_onActivate");
                                SerializedProperty onExpire = serializedObject.FindProperty("m_buffData").GetArrayElementAtIndex(i).FindPropertyRelative("m_onExpire");

                                EditorGUILayout.PropertyField(onActivate);
                                if (!isPermanent) EditorGUILayout.PropertyField(onExpire);
                                break;
                            default:
                                Debug.LogError("Idk how but you borked the Buff System");
                                break;
                        }

                        

                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(target, "Changed Status Effect " + i);

                            current.SetName(name);
                            current.SetBuffType(buffType);
                            current.SetPermanent(isPermanent);
                            if (!isPermanent) current.SetTime(time);

                            if(current.GetBuffType == 0)
                            {
                                current.SetMaxHealth(maxHealthValue);
                                current.SetMoveSpeed(moveSpeedValue);
                                current.SetShootSpeed(shootSpeedValue);
                                current.SetDamage(damageValue);
                            }

                            serializedObject.ApplyModifiedProperties();
                        }
                    }

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }


                if (GUILayout.Button("Create new buff"))
                {
                    Undo.RecordObject(target, "Added new Status Effect");
                    m_system.CreateData();
                }

            }


            private void _Delete(int dataToRemove)
            {

                BuffSystem.BuffData[] newData = new BuffSystem.BuffData[m_system.GetData.Length - 1];
                int count = 0;

                for (int i = 0; i < m_system.GetData.Length; i++)
                {
                    if (dataToRemove == i) continue;

                    newData[count] = m_system.GetData[i];
                    count++;
                }

                Undo.RecordObject(target, "Removed Status Effect " + dataToRemove);
                m_system.ChangeData(newData);

            }

            private void OnEnable()
            {

            }
        }

    }
}

